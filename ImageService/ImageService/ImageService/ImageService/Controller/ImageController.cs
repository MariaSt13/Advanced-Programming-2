using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Modal;
using ImageService.Commands;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        // The Modal Object
        private IImageServiceModal m_modal;
        private Dictionary<int, ICommand> commands;

        public ImageController(IImageServiceModal modal)
        {
            // Storing the Modal Of The System
            m_modal = modal;                    
            commands = new Dictionary<int, ICommand>()
            {
                // For Now will contain NEW_FILE_COMMAND
            };
        }
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            // Write Code Here
        }
    }
}
