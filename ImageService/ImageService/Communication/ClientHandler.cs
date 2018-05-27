using Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;

namespace Communication
{
    public class ClientHandler : IClientHandler
    {
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer = null;
        public event EventHandler<CommandRecievedEventArgs> ClientHandlerCommandRecieved;
        //Helper for Thread Safety
        private Mutex mutex = new Mutex();

        public ClientHandler() { }

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
          
            if (writer != null)
            {
                this.mutex.WaitOne();
                this.writer.Write(message);
                this.mutex.ReleaseMutex();
            }
            
        }

        public void writeToClient(string message, TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            this.mutex.WaitOne();
            try 
            {
                writer.Write(message);
            }
            catch (Exception)
            {
                writer.Dispose();
            }
            this.mutex.ReleaseMutex();

        }
    }
}
