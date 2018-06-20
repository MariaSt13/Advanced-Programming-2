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
            List<byte> b = new List<byte>();
            Byte[] temp;
            Byte[] data = new byte[6790];
            int i = 0;

            do
            {
                i = stream.Read(data, 0, data.Length);
                temp = new byte[i];
                for (int n = 0; n < i; n++)
                {
                    temp[n] = data[n];
                    b.Add(temp[n]);
                }
                System.Threading.Thread.Sleep(300);
            } while (stream.DataAvailable || i == data.Length);
            if (i != 0)
            {
                byte[] byteArrayIn = b.ToArray();
                ClientHandlerCommandRecievedByte?.Invoke(this, new CommandRecievedEventArgsByte( byteArrayIn));
            }
        }


        public void write(string message)
        {
            this.writer.Write(message);
        }

        public void writeToClient(string message, TcpClient client)
        {
            throw new NotImplementedException();
        }
    }
}
