using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Modal;
using ImageService.Commands;
using ImageService.Logging;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        // The Modal Object
        private IImageServiceModal m_modal;
        private Dictionary<int, ICommand> commands;
        private ILoggingService logger;

        public ImageController(IImageServiceModal modal, ILoggingService logging)
        {
      
            // Storing the Modal Of The System
            m_modal = modal;
            this.logger = logging;
            commands = new Dictionary<int, ICommand>()
            {
                { 1, new NewFileCommand(m_modal, logger) }
            };
            this.logger.Log("Controller : constractor", Logging.Modal.MessageTypeEnum.INFO);
        }
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            this.logger.Log("Controller : executeCommand", Logging.Modal.MessageTypeEnum.INFO);
            ICommand command = this.commands[commandID];
            return command.Execute(args, out resultSuccesful);
        }
    }
}
