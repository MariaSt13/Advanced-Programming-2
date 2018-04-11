using ImageService.Controller;
using ImageService.Modal.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoyHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        #endregion

        // The Event That Notifies that the Directory is being closed
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;

        public DirectoyHandler(IImageController imageController, ILoggingService loggingService)
        {
            this.m_controller = imageController;
            this.m_logging = loggingService;
        }

        public void StartHandleDirectory(string dirPath)
        {
            this.m_path = dirPath;
            this.m_dirWatcher = new FileSystemWatcher(m_path, "*.jpg,*.png,*.gif,*.bmp");
            this.m_dirWatcher.Created += new FileSystemEventHandler(OnCreate); ;
        }


        // Define the event handlers.
        private void OnCreate(object source, FileSystemEventArgs e)
        {
            string[] args = {e.FullPath};
            bool result;
            this.m_controller.ExecuteCommand((int)CommandEnum.NewFileCommand, args, out result);
        }

        //for the close only !!!!!!!!!!!
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            
        }

        public void OnClose()
        {
           
        }

        // Implement Here!
    }
}
