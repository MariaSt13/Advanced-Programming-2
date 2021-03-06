﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Communication;
using Infrastructure;
using Newtonsoft.Json;
using System.ComponentModel;

namespace GUI.Communication
{
    /// <summary>
    /// Communication singlton
    /// </summary>
    public class CommunicationSingleton:INotifyPropertyChanged
    {
        private static CommunicationSingleton instance;
        public event EventHandler<MessageRecievedEventArgs> connectServer;
        public event EventHandler<CommandRecievedEventArgs> SingletonCommandRecieved;
        public event PropertyChangedEventHandler PropertyChanged;

        private  IClientHandler clientHandler;
        private bool _isConnected;

        //Helper for Thread Safety
        private static object m_lock = new object();


        private CommunicationSingleton() {
            
        }

        /// <summary>
        /// get instance of singlton.
        /// </summary>
        public static CommunicationSingleton Instance
        {
            get
            {

                lock (m_lock)
                {
                    if (instance == null)
                    {
                        instance = new CommunicationSingleton();
                        instance._isConnected = false;
                        Instance.clientHandler = new ClientHandler();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// connect to server
        /// </summary>
        public static void Connect()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            TcpClient client = new TcpClient();
            //try to connect
            try
            {
                client.Connect(ep);
            }
            //connection failed
            catch (Exception)
            {
                return;
            }
            //connection success
            instance.isConnected = true;

            //HandleClient
            Instance.clientHandler.ClientHandlerCommandRecieved += ClientHandlerCommandRecievedHandle;
            Instance.clientHandler.HandleClient(client);

            //invoke event connectServer
            Instance.connectServer?.Invoke(Instance,
                new MessageRecievedEventArgs("connect",MessageTypeEnum.INFO));
        }

        /// <summary>
        /// recieve command from client handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">args</param>
        private static void ClientHandlerCommandRecievedHandle(object sender, CommandRecievedEventArgs e)
        {
            Instance.SingletonCommandRecieved?.Invoke(Instance, e);
        }

        /// <summary>
        /// write a message to server
        /// </summary>
        /// <param name="command"></param>
        public static void Write(string command)
        {
            Instance.clientHandler.write(command);
        }

        // Properties
        public bool isConnected
        {
            get
            {
                return _isConnected;
            }
            set
            {
               _isConnected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("isConnected"));
            }
        }

    }
}
