using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Modal;
using ImageService.Logging;
using ImageService.Commands;
using Communication;
using Infrastructure;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;
        private Dictionary<int, ICommand> commands;
        private ILoggingService logger;
        private IClientHandler clientHandler;

        /// <summary>
        /// constractor. 
        /// </summary>
        /// <param name="modal">The modal object</param>
        /// <param name="logging"> The logger </param>
        public ImageController(IImageServiceModal modal, ILoggingService logging,
            IClientHandler clientHandler)
        {
            // Storing the Modal Of The System
            m_modal = modal;
            this.logger = logging;
            this.clientHandler = clientHandler;
            this.clientHandler.ClientHandlerCommandRecieved += ClientHandlerCommandRecievedHandle; 

            //Dictinpry of commands
            commands = new Dictionary<int, ICommand>()
            {
                {(int)CommandEnum.CommandEnum.NewFileCommand, new NewFileCommand(m_modal, logger) },
                { (int)CommandEnum.CommandEnum.GetConfigCommand, new GetConfigCommand()},
                {(int)CommandEnum.CommandEnum.LogCommand, new LogCommand() }
            };
            
        }

        private void ClientHandlerCommandRecievedHandle(object sender, CommandRecievedEventArgs e)
        {
            bool result;
            this.ExecuteCommand(e.CommandID, e.Args, out result);
        }
        /// <summary>
        /// Exucte sent command
        /// </summary>
        /// <param name="commandID"> the id of command</param>
        /// <param name="args"> the args for the command</param>
        /// <param name="resultSuccesful"> if action ended succesfully</param>
        /// <returns></returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            // check if command in dictionary
            if (!this.commands.ContainsKey(commandID))
            {
                resultSuccesful = false;
                return "no such command";
            }

            ICommand command = this.commands[commandID];
            // execute the command
            return command.Execute(args, out resultSuccesful);
        }
    }
}
