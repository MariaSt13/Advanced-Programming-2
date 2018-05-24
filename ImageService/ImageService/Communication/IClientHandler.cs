using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Infrastructure;

namespace Communication
{
    public interface IClientHandler
    {
        event EventHandler<CommandRecievedEventArgs> ClientHandlerCommandRecieved;
        void HandleClient(TcpClient client);
        void write(string message);
    }
}
