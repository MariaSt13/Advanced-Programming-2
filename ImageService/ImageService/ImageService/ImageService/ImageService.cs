﻿using System;
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
using ImageService.Modal;
using ImageService.Controller;
using System.Configuration;
using Infrastructure;
using Communication;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageService
{
    public partial class ImageService : ServiceBase

    {
        private IClientHandler clientHandler;
        private System.Diagnostics.EventLog eventLog1;
        private ImageServer m_imageServer;          
        private IImageServiceModal modal;
        private IImageController controller;
        private ILoggingService logging;
        private Communication.Server server;

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


            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // get OutputDir path and Thumbnail size from app config
            string outputFolder = ConfigurationManager.AppSettings["OutputDir"];
            int size = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);

            
            //create logging modal and add to event
            this.logging = new LoggingService();
            logging.MessageRecieved += onMessage;
            
            //create server and client handler
            this.clientHandler = new ByteClinetHandler();
            this.server = new Communication.Server(8000, clientHandler);
            // create image modal, controller and image server
            this.modal = new ImageServiceModal(outputFolder, size,logging);
            this.controller = new ImageController(modal, logging, clientHandler);
            this.m_imageServer = new ImageServer(this.logging, this.controller);

            //sign to events
            this.clientHandler.ClientHandlerCommandRecieved += ClientHandlerCommandRecievedHandle;
            this.clientHandler.ClientHandlerCommandRecievedByte += ClientHandlerCommandRecievedHandleByte;
            server.newConnection += newConnectionHandler;

            logging.Log("In OnStart", MessageTypeEnum.INFO);
            this.logging.Log(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(), MessageTypeEnum.INFO);
            //start server
            server.Start();

        }

        /// <summary>
        /// Happens when there is a new message wrom client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">args</param>
        public void ClientHandlerCommandRecievedHandle(object sender, CommandRecievedEventArgs e)
        {
            bool result;
            string message;  
            //close command
            if (e.CommandID == (int)CommandEnum.CommandEnum.CloseCommand)
            {
                message = this.controller.ExecuteCommand(e.CommandID, e.Args, out result);
                //write to all clients
                this.server.writeAll(message);

                //log command
            } else if(e.CommandID == (int)CommandEnum.CommandEnum.LogCommand) {
                //get all the logs from start service
                string[] logs = this.logging.GetLogs();
                message = this.controller.ExecuteCommand(e.CommandID, logs, out result);
                this.clientHandler.write(message);
            }
            
            //other commands
            else
            {
                message = this.controller.ExecuteCommand(e.CommandID, e.Args, out result);
                this.clientHandler.write(message);
            }
        }

        /// <summary>
        /// command recived with byte array
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ClientHandlerCommandRecievedHandleByte(object sender, CommandRecievedEventArgsByte e)
        {
            byte[] byteArray = e.Args;
            try
            {
                using (MemoryStream inputStream = new MemoryStream(byteArray))
                {
                    using (var image = Image.FromStream(inputStream))
                    {
                        string path = appConfigManager.Instance.Handlers.Split(';')[0];
                        string imagePath = path + "\\" + DateTime.Now.Date.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".jpg";
                        Image bitmap = new Bitmap(image);
                        bitmap.Save(imagePath, ImageFormat.Jpeg);
                        clientHandler.write("finish");
                    }
                }
            }catch (Exception ex)
            {
                this.logging.Log(ex.ToString(), MessageTypeEnum.INFO);
            }

        }

        /// <summary>
        /// happens when there is a new connection in the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void newConnectionHandler(object sender, MessageRecievedEventArgs e)
        {
            logging.Log("start a new connection", MessageTypeEnum.INFO);
        }

        /// <summary>
        /// Function happens when event MessageRecieved invokes.
        /// </summary>
        /// <param name="sender">the object that invoked the event </param>
        /// <param name="args">argumnets </param>
        public void onMessage(object sender, MessageRecievedEventArgs args)
        {
            MessageTypeEnum status= args.Status;
            string message = args.Message;

            switch (status)
            {
                case (MessageTypeEnum.INFO):
                    eventLog1.WriteEntry(message, EventLogEntryType.Information);
                    break;
                case (MessageTypeEnum.FAIL):
                    eventLog1.WriteEntry(message, EventLogEntryType.FailureAudit);
                    break;
                case (MessageTypeEnum.WARNING):
                    eventLog1.WriteEntry(message, EventLogEntryType.Warning);
                    break;
            }
            string[] argv = {status.ToString(),message};
            bool result;
            string messagee = this.controller.ExecuteCommand((int)CommandEnum.CommandEnum.LogCommand, argv, out result);
            //write log to all clients.
          //  this.server.writeAll(messagee);
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

            logging.Log("In onStop.", MessageTypeEnum.INFO);

            this.m_imageServer.Close();

            // Update the service state to stopped.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            //stop server
            this.server.Stop();
        }

        /// <summary>
        /// when service continues
        /// </summary>
        protected override void OnContinue()
        {
            logging.Log("In OnContinue.", MessageTypeEnum.INFO);
        }
    }
}
