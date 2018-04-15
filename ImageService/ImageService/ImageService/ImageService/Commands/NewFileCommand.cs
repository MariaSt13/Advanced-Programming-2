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

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="modal">IImageServiceModal object</param>
        /// <param name="logging">ILoggingService object</param>
        public NewFileCommand(IImageServiceModal modal, ILoggingService logging)
        {
            m_modal = modal;            // Storing the Modal
            logger = logging;
        }

        /// <summary>
        //  Executes the command by calling a function in IImageServiceModal class.
        /// </summary>
        /// <param name="args">Function arguments</param>
        /// <param name="result">If the value is true then the process is successfully completed</param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            // The String Will Return the New Path if result = true, and will return the error message
            System.Threading.Thread.Sleep(500);
            return m_modal.AddFile(args[0], out result);
        }
    }
}
