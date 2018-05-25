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
    class CommunicationSingleton
    {
        private static CommunicationSingleton instance;
        public event EventHandler<CommandRecievedEventArgs> SingletonCommandRecieved;
        private  IClientHandler clientHandler;

        private CommunicationSingleton() {

        }

        public static CommunicationSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommunicationSingleton();
                    Instance.clientHandler = new ClientHandler();
                }
                return instance;
            }
        }

        public static void Connect()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            TcpClient client = new TcpClient();
            client.Connect(ep);
            //Console.WriteLine("You are connected");
            Instance.clientHandler.ClientHandlerCommandRecieved += ClientHandlerCommandRecievedHandle;
            Instance.clientHandler.HandleClient(client);
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.GetConfigCommand, null, null);
            string str = JsonConvert.SerializeObject(command);
            Write(str);
            // client.Close();

        }

        private static void ClientHandlerCommandRecievedHandle(object sender, CommandRecievedEventArgs e)
        {
            Instance.SingletonCommandRecieved?.Invoke(Instance, e);
        }

        public static void Write(string command)
        {
            Instance.clientHandler.write(command);
        }

    }
}
