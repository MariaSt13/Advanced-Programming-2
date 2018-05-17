using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Communication
{
    class CommunicationSingleton
    {
        private static CommunicationSingleton instance;

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
            NetworkStream stream = client.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            BinaryWriter writer = new BinaryWriter(stream);
            
                // Send data to server
                int num = int.Parse(Console.ReadLine());
                writer.Write(num);
                // Get result from server
                int result = reader.ReadInt32();
            
            client.Close();

        }
    }
}
