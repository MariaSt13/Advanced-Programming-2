using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;
        private ILoggingService logger;

        public NewFileCommand(IImageServiceModal modal, ILoggingService logging)
        {
            m_modal = modal;            // Storing the Modal
            logger = logging;
            this.logger.Log("NewFileCommand : constractor", Logging.Modal.MessageTypeEnum.INFO);
        }

        public string Execute(string[] args, out bool result)
        {
            this.logger.Log("NewFileCommand : execute", Logging.Modal.MessageTypeEnum.INFO);
            // The String Will Return the New Path if result = true, and will return the error message
            return m_modal.AddFile(args[0], out result);
        }
    }
}
