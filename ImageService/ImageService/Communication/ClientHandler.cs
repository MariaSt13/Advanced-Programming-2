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
    /// <summary>
    /// Handles a client.
    /// </summary>
    public class ClientHandler : IClientHandler
    {
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer = null;
        public event EventHandler<CommandRecievedEventArgs> ClientHandlerCommandRecieved;
        //Helper for Thread Safety
        private Mutex mutex = new Mutex();
        /// <summary>
        /// constructor. 
        /// </summary>
        public ClientHandler() { }

        /// <summary>
        /// Handles a client.
        /// </summary>
        /// <param name="client"> the tcp client to handle </param>
        public void HandleClient(TcpClient client)
        {
            // get streams
            this.stream = client.GetStream();
            this.reader = new BinaryReader(stream);
            this.writer = new BinaryWriter(stream);

            // try to read while client connected
            new Task(() =>
            {
                while (true)
                {
                    this.read();
                }
            }).Start();
        }

        /// <summary>
        /// read a message and deserialize it to CommandRecievedEventArgs object.
        /// </summary>
        public void read()
        {
            string output = this.reader.ReadString();
            CommandRecievedEventArgs deserializedProduct = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(output);
            ClientHandlerCommandRecieved?.Invoke(this, deserializedProduct);
        }

        /// <summary>
        /// write a message.
        /// </summary>
        /// <param name="message"> the message to write</param>
        public void write(string message)
        {
          
            if (writer != null)
            {
                this.mutex.WaitOne();
                this.writer.Write(message);
                this.mutex.ReleaseMutex();
            }
            
        }

        /// <summary>
        /// write to specific client.
        /// </summary>
        /// <param name="message"> the message to write </param>
        /// <param name="client">the tcp client to write to</param>
        public void writeToClient(string message, TcpClient client)
        {
            // get client stream
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
