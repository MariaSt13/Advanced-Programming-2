using Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace WebApplication2.Models
{
    public class communicationModel
    {
        private static communicationModel instance;
        private static object m_lock = new object();
        private IClientHandler clientHandler;
        private bool _isConnected;

        private communicationModel() { }

        /// <summary>
        /// get instance of singlton.
        /// </summary>
        public static communicationModel Instance
        {
            get
            {
                lock (m_lock)
                {
                    if (instance == null)
                    {
                        instance = new communicationModel();
                        instance._isConnected = false;
                        Instance.clientHandler = new ClientHandler();
                    }
                }
                return instance;
            }
        }

        public bool isConnected
        {
            get
            {
                return _isConnected;
            }
            set
            {
                _isConnected = value;
            }
        }
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
           Instance.isConnected = true;

            //HandleClient
            Instance.clientHandler.HandleClient(client);
        }

        /// <summary>
        /// write a message to server
        /// </summary>
        /// <param name="command"></param>
        public static void Write(string command)
        {
            Instance.clientHandler.write(command);
        }
    }
}