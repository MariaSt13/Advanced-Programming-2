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
            this.m_logging.Log("handler : constractor", Logging.Modal.MessageTypeEnum.INFO);
        }

        public void StartHandleDirectory(string dirPath)
        {
            this.m_path = dirPath;
            this.m_dirWatcher = new FileSystemWatcher();
            this.m_dirWatcher.Path = this.m_path;
            this.m_dirWatcher.Filter = "*";
            this.m_dirWatcher.Created += new FileSystemEventHandler(OnCreated);
            // Begin watching.
            this.m_dirWatcher.EnableRaisingEvents = true;
            this.m_logging.Log("handler : StartHandleDirectory*", Logging.Modal.MessageTypeEnum.INFO);
        }


        // Define the event handlers.
        private void OnCreated(object source, FileSystemEventArgs e)
        {
            this.m_logging.Log("handler : OnCreated", Logging.Modal.MessageTypeEnum.INFO);
            string[] args = {e.FullPath};
            bool result;
            string strResult = this.m_controller.ExecuteCommand((int)CommandEnum.NewFileCommand, args, out result);
            this.m_logging.Log(strResult, Logging.Modal.MessageTypeEnum.INFO);
        }

        //for the close only !!!!!!!!!!!
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            this.m_logging.Log("handler : OnCommandRecieved", Logging.Modal.MessageTypeEnum.INFO);
            switch (e.CommandID)
            {
                //close command
                case ((int)CommandEnum.CloseCommand):
                    this.m_logging.Log("close directory +" + this.m_path , Logging.Modal.MessageTypeEnum.INFO);
                    this.OnClose();
                    break;

                //add new file command
                case ((int)CommandEnum.NewFileCommand):
                    this.m_logging.Log("add file to directory +" + this.m_path, Logging.Modal.MessageTypeEnum.INFO);
                    break;
            }
        }

        public void OnClose()
        {
            this.m_logging.Log("handler : OnClose", Logging.Modal.MessageTypeEnum.INFO);
            this.m_dirWatcher.Created -= OnCreated;
            DirectoryClose?.Invoke(this,new DirectoryCloseEventArgs(m_path,"close directory"));
        }

    }
}
