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

namespace GUI.Communication
{
    class CommunicationSingleton
    {
        private static CommunicationSingleton instance;
        public event EventHandler<CommandRecievedEventArgs> SingletonCommandRecieved;
        private  IClientHandler clientHandler;

        private CommunicationSingleton() { }

        public static CommunicationSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommunicationSingleton();
                }
                return instance;
            }
        }

        public static void Connect()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            TcpClient client = new TcpClient();
            client.Connect(ep);
            Console.WriteLine("You are connected");
            Instance.clientHandler = new ClientHandler();
            Instance.clientHandler.ClientHandlerCommandRecieved += ClientHandlerCommandRecievedHandle;
            Instance.clientHandler.HandleClient(client);
           // client.Close();

        }

        private static void ClientHandlerCommandRecievedHandle(object sender, CommandRecievedEventArgs e)
        {
            Instance.SingletonCommandRecieved?.Invoke(Instance, e);
        }

        public static void Write(CommandRecievedEventArgs command)
        {
            Instance.clientHandler.write(command);
        }

    }
}
