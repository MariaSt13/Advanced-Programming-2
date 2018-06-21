using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Communication
{
    public class ByteClinetHandler : IClientHandler
    {
        public event EventHandler<CommandRecievedEventArgs> ClientHandlerCommandRecieved;
        public event EventHandler<CommandRecievedEventArgsByte> ClientHandlerCommandRecievedByte;
        private BinaryReader reader;
        private BinaryWriter writer;
        private NetworkStream stream;

        /// <summary>
        /// contructor.
        /// </summary>
        public ByteClinetHandler(){}


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
        /// read a message 
        /// </summary>
        public void read()
        {
            int i = 0;
            List<byte> b = new List<byte>();
            Byte[] temp;
            Byte[] data = new byte[6790];

            do
            {
                i = stream.Read(data, 0, data.Length);
                temp = new byte[i];
                for (int j = 0; j < i; j++)
                {
                    temp[j] = data[j];
                    b.Add(temp[j]);
                }
                System.Threading.Thread.Sleep(300);
            } while (stream.DataAvailable || i == data.Length);

            stream.Flush();

            if (i != 0)
            {
                byte[] byteArrayIn = b.ToArray();
                ClientHandlerCommandRecievedByte?.Invoke(this, new CommandRecievedEventArgsByte( byteArrayIn));
            }
        }

        /// <summary>
        /// write a message.
        /// </summary>
        /// <param name="message"> the message to write</param>
        public void write(string message)
        {
            this.writer.Write(message);
        }

        /// <summary>
        /// write to specific client.
        /// </summary>
        /// <param name="message"> the message to write </param>
        /// <param name="client">the tcp client to write to</param>
        public void writeToClient(string message, TcpClient client)
        {
            throw new NotImplementedException();
        }
    }
}
