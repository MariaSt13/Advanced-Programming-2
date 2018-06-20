using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Infrastructure;

namespace Communication
{
    /// <summary>
    /// client hadler interface
    /// </summary>
    public interface IClientHandler
    {
        // message recieved
        event EventHandler<CommandRecievedEventArgs> ClientHandlerCommandRecieved;
        // message recieved
        event EventHandler<CommandRecievedEventArgsByte> ClientHandlerCommandRecievedByte;
        // handle client
        void HandleClient(TcpClient client);
        // write a message to client
        void write(string message);
        // write message to specific client
        void writeToClient(string message, TcpClient client);
    }
}
