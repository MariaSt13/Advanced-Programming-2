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

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="imageController">the controller</param>
        /// <param name="loggingService">the logger</param>
        public DirectoyHandler(IImageController imageController, ILoggingService loggingService)
        {
            this.m_controller = imageController;
            this.m_logging = loggingService;
        }

        /// <summary>
        /// starts to handle a given directory
        /// </summary>
        /// <param name="dirPath">the path to the directoryto start handling </param>
        public void StartHandleDirectory(string dirPath)
        {
            this.m_path = dirPath;
            // create a file ststem watcher
            this.m_dirWatcher = new FileSystemWatcher();
            this.m_dirWatcher.Path = this.m_path;
            this.m_dirWatcher.Filter = "*";
            this.m_dirWatcher.Created += new FileSystemEventHandler(OnCreated);
            // Begin watching.
            this.m_dirWatcher.EnableRaisingEvents = true;
        }


        /// <summary>
        /// Define the event handlers. 
        /// </summary>
        /// <param name="source">sender</param>
        /// <param name="e">args</param>
        private void OnCreated(object source, FileSystemEventArgs e)
        {
            bool result;
            string path = e.FullPath;
            string[] args = { path };
            string fileExtension = Path.GetExtension(path).ToLower();
            string[] legalExtensions = {".gif", ".png", ".jpg", ".bmp" };

            //check if file extension is legal
            if (legalExtensions.Contains(fileExtension))
            {
                string strResult = this.m_controller.ExecuteCommand((int)CommandEnum.CommandEnum.NewFileCommand, args, out result);
                if (result == true)
                {
                    this.m_logging.Log(strResult, Logging.Modal.MessageTypeEnum.INFO);
                }
                else
                {
                    this.m_logging.Log(strResult, Logging.Modal.MessageTypeEnum.FAIL);
                }
           
            }
        }

        /// <summary>
        /// function happnes when on command recived event is invoked
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            // check which command is recieved
            switch (e.CommandID)
            {
                //close command
                case ((int)CommandEnum.CommandEnum.CloseCommand):
                    this.OnClose();
                    break;

                //add new file command
                case ((int)CommandEnum.CommandEnum.NewFileCommand):
                    break;
            }
        }

        /// <summary>
        /// when close command recieved
        /// </summary>
        public void OnClose()
        {
            // unsubscribe to event
            this.m_dirWatcher.Created -= OnCreated;
            // envoke DirectoryClose event
            DirectoryClose?.Invoke(this,new DirectoryCloseEventArgs(m_path,"close directory"));
        }

    }
}
