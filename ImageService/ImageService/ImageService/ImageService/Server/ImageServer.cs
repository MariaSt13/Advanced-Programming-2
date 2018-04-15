using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ImageService.Modal.Event;

namespace ImageService.Server
{
    public class ImageServer
    {

        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;

        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        // The event that notifies about a new Command being recieved
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="logging">ILoggingService object</param>
        /// <param name="imageController">IImageController ocject</param>
        public ImageServer(ILoggingService logging, IImageController imageController)
        {
            m_logging = logging;
            m_controller = imageController;
            readPath();
           
        }

        /// <summary>
        /// This function reads all the paths from the Config file,
        /// and creates a handler for each path.
        /// </summary>
        public void readPath()
        {
            string paths = ConfigurationManager.AppSettings["Handler"];
            string[] pathArray = paths.Split(';');
            for ( int i =0; i < pathArray.Length; i++)
            {
                IDirectoyHandler handle = new DirectoyHandler(this.m_controller,this.m_logging);
                CommandRecieved += handle.OnCommandRecieved;
                handle.DirectoryClose += OnDirctoryClose;
                handle.StartHandleDirectory(pathArray[i]);
            }
        }

        /// <summary>
        /// This function is called when the Handler is closed.
        /// </summary>
        /// <param name="sender"> The Hendler that was closed</param>
        /// <param name="args"></param>
        public void OnDirctoryClose(object sender, DirectoryCloseEventArgs args)
        {
            this.m_logging.Log("Close hendler", Logging.Modal.MessageTypeEnum.INFO);
            IDirectoyHandler handler = (IDirectoyHandler)sender;
            CommandRecieved -= handler.OnCommandRecieved;
        }

        /// <summary>
        /// This function closes all handlers by invoke CommandRecieved event.
        /// </summary>
        public void Close()
        {
            CommandRecieved?.Invoke(this, new CommandRecievedEventArgs((int)CommandEnum.CloseCommand,null,null));
        }

    }
}
