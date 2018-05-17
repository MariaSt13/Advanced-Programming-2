using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public class ClientHandler : IClientHandler
    {
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;

        public void HandleClient(TcpClient client)
        {
            this.stream = client.GetStream();
            this.reader = new StreamReader(stream);
            this.writer = new StreamWriter(stream);

            new Task(() =>
            {
                while (client.Connected)
                {
                    this.read();
                }
            }).Start();
        }

        public void read()
        {
            string command = this.reader.ReadLine();
        }
        public void write(string message)
        {
            this.writer.Write(message);
        }
    }
}
