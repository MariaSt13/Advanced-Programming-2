using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        /// <summary>
        /// event for gitting a message.
        /// </summary>
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /// <summary>
        /// Logging the Message
        /// </summary>
        /// <param name="message">The message to write in the log</param>
        /// <param name="type"> type of message </param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecieved.Invoke(this, new MessageRecievedEventArgs(message, type));
        }
    }
}
