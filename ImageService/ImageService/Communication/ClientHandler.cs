using Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Communication
{
    public class ClientHandler : IClientHandler
    {
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        public event EventHandler<CommandRecievedEventArgs> ClientHandlerCommandRecieved;

        public ClientHandler()
        {

        }

        public void HandleClient(TcpClient client)
        {
            this.stream = client.GetStream();
            this.reader = new BinaryReader(stream);
            this.writer = new BinaryWriter(stream);

            new Task(() =>
            {
                while (true)
                {
                    this.read();
                }
            }).Start();
        }

        public void read()
        {
            string output = this.reader.ReadString();
            CommandRecievedEventArgs deserializedProduct = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(output);
            ClientHandlerCommandRecieved?.Invoke(this, deserializedProduct);
        }
        public void write(string message)
        {
            this.writer.Write(message);
        }
    }
}
