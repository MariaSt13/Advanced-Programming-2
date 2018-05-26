using System;
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

namespace GUI.Communication
{
    public class CommunicationSingleton
    {
        private static CommunicationSingleton instance;
        public event EventHandler<MessageRecievedEventArgs> connectServer;
        public event EventHandler<CommandRecievedEventArgs> SingletonCommandRecieved;
        private  IClientHandler clientHandler;
        private bool _isConnected;
        //Helper for Thread Safety
        private static object m_lock = new object();


        private CommunicationSingleton() {
            
        }

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

        public static void Connect()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(ep);
            }
            catch (Exception e)
            {

            }
            instance._isConnected = true;
            //HandleClient
            Instance.clientHandler.ClientHandlerCommandRecieved += ClientHandlerCommandRecievedHandle;
            Instance.clientHandler.HandleClient(client);

            //invoke event connectServer
            Instance.connectServer?.Invoke(Instance, new MessageRecievedEventArgs("connect",MessageTypeEnum.INFO));
        }

        private static void ClientHandlerCommandRecievedHandle(object sender, CommandRecievedEventArgs e)
        {
            Instance.SingletonCommandRecieved?.Invoke(Instance, e);
        }

        public static void Write(string command)
        {
            Instance.clientHandler.write(command);
        }

        // Properties
        public bool isConnected
        {
            get { return Instance._isConnected; }
            set
            {
                Instance._isConnected = value;
            }
        }

    }
}
