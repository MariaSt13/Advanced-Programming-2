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
    public class Server
    {
        private int port;
        private TcpListener listener;
        private IClientHandler ch;
        private List<TcpClient> clients;
        public event EventHandler<MessageRecievedEventArgs> newConnection;
        public Server(int port, IClientHandler ch)
        {
            this.port = port;
            this.ch = ch;
            this.clients = new List<TcpClient>();
        }
        public void Start()
        {
            IPEndPoint ep = new
            IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);

            listener.Start();
            Console.WriteLine("Waiting for connections...");
            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        newConnection?.Invoke(this,new MessageRecievedEventArgs("new connection",MessageTypeEnum.INFO));
                        Console.WriteLine("Got new connection");
                        this.clients.Add(client);
                        ch.HandleClient(client);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            task.Start();
            
        }

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

        public void Stop()
        {
            listener.Stop();
        }

    }
}
