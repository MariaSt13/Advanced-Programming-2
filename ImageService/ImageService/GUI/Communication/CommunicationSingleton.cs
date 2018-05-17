using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Communication;

namespace GUI.Communication
{
    class CommunicationSingleton
    {
        private static CommunicationSingleton instance;
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
            instance.clientHandler = new ClientHandler();
            instance.clientHandler.HandleClient(client);
           // client.Close();

        }
    }
}
