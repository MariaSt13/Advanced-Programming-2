using Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    /// <summary>
    /// TCP server 
    /// </summary>
    public class Server
    {
        private int port;
        private TcpListener listener;
        private IClientHandler ch;
        private List<TcpClient> clients;
        public event EventHandler<MessageRecievedEventArgs> newConnection;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="port">port to connect</param>
        /// <param name="ch">client handle object</param>
        public Server(int port, IClientHandler ch)
        {
            this.port = port;
            this.ch = ch;
            this.clients = new List<TcpClient>();
        }

        /// <summary>
        /// start running client
        /// </summary>
        public void Start()
        {
            IPEndPoint ep = new
            IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);

            //start listening
            listener.Start();
            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        newConnection?.Invoke(this,new MessageRecievedEventArgs("new connection",MessageTypeEnum.INFO));

                        //add client to list
                        this.clients.Add(client);
                        //handle client
                        ch.HandleClient(client);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
            });
            task.Start();
            
        }

        /// <summary>
        /// write a message to all clients.
        /// </summary>
        /// <param name="message"></param>
        public void writeAll(string message)
        {
            foreach (TcpClient client in clients)
            {
                if (client.Connected)
                {
                    ch.writeToClient(message, client);
                }
            }
        }

        /// <summary>
        /// stop server.
        /// </summary>
        public void Stop()
        {
            listener.Stop();
        }

    }
}
