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

        public ImageServer(ILoggingService logging, IImageController imageController)
        {
            m_logging = logging;
            m_controller = imageController;
            readPath();
        }
        public void readPath()
        {
            string paths = ConfigurationManager.AppSettings["Handler"];
            string[] pathArray = paths.Split(';');
            for ( int i =0; i < pathArray.Length; i++)
            {
                IDirectoyHandler handle = new DirectoyHandler(this.m_controller,this.m_logging);
                CommandRecieved += handle.OnCommandRecieved;
                handle.DirectoryClose += OnDirctoryClose;
            }
        }
        public void OnDirctoryClose(object sender, DirectoryCloseEventArgs args)
        {
            IDirectoyHandler handler = (IDirectoyHandler)sender;
            CommandRecieved -= handler.OnCommandRecieved;
        }

    }
}
