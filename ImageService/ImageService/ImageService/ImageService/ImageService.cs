using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ImageService.Logging;
using ImageService.Server;
using ImageService.Logging.Modal;
using ImageService.Modal;
using ImageService.Controller;
using System.Configuration;

namespace ImageService
{
    public partial class ImageService : ServiceBase

    {
        private System.Diagnostics.EventLog eventLog1;
        private ImageServer m_imageServer;          
        private IImageServiceModal modal;
        private IImageController controller;
        private ILoggingService logging;

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="args"></param>
        public ImageService(string[] args)
        {
            InitializeComponent();
            // get source name and log name from app config
            string eventSourceName = ConfigurationManager.AppSettings["SourceName"];
            string logName = ConfigurationManager.AppSettings["LogName"];
            if (args.Count() > 0)
            {
                eventSourceName = args[0];
            }
            if (args.Count() > 1)
            {
                logName = args[1];
            }
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
        }

        /// <summary>
        /// Funtion happens when service starts
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog1.WriteEntry("In OnStart");

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // get OutputDir path and Thumbnail size from app config
            string outputFolder = ConfigurationManager.AppSettings["OutputDir"];
            int size = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);

            
            //create logging modal and add to event
            this.logging = new LoggingService();
            logging.MessageRecieved += onMessage;
            
            // create image modal, controller and server
            this.modal = new ImageServiceModal(outputFolder, size,logging);
            this.controller = new ImageController(modal, logging);
            this.m_imageServer = new ImageServer(this.logging, this.controller);

        }

        /// <summary>
        /// Function happens when event MessageRecieved invokes.
        /// </summary>
        /// <param name="sender">the object that invoked the event </param>
        /// <param name="args">argumnets </param>
        public void onMessage(object sender, MessageRecievedEventArgs args)
        {
            eventLog1.WriteEntry(args.Message);
        }

        /// <summary>
        /// function happens when service stops
        /// </summary>
        protected override void OnStop()
        {
            // Update the service state to stop Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog1.WriteEntry("In onStop.");
            this.m_imageServer.Close();

            // Update the service state to stopped.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        /// <summary>
        /// when service continues
        /// </summary>
        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
        }
    }
}
