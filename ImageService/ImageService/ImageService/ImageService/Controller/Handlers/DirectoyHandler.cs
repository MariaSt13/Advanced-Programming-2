using ImageService.Controller;
using ImageService.Modal.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    class DirectoyHandler : IDirectoyHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        #endregion

        // The Event That Notifies that the Directory is being closed
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;

        public void StartHandleDirectory(string dirPath)
        {
            
        }

        //for the close only !!!!!!!!!!!
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            
        }

        public void OnClose()
        {
            throw new NotImplementedException();
        }

        // Implement Here!
    }
}
