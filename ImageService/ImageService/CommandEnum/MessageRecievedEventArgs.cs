using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// args for message recieved event
    /// </summary>
    public class MessageRecievedEventArgs : EventArgs
    {
        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }

        public MessageRecievedEventArgs(string message, MessageTypeEnum status)
        {
            this.Status = status;
            this.Message = message;
        }
    }
}
