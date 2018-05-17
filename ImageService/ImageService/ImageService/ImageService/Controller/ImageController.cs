using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Modal;
using ImageService.Logging;
using ImageService.Commands;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;
        private Dictionary<int, ICommand> commands;
        private ILoggingService logger;

        /// <summary>
        /// constractor. 
        /// </summary>
        /// <param name="modal">The modal object</param>
        /// <param name="logging"> The logger </param>
        public ImageController(IImageServiceModal modal, ILoggingService logging)
        {
            // Storing the Modal Of The System
            m_modal = modal;
            this.logger = logging;
            
            //Dictinpry of commands
            commands = new Dictionary<int, ICommand>()
            {
                {0, new NewFileCommand(m_modal, logger) }
            };
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
