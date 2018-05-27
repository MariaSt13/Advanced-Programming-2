using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace ImageService
{
    /// <summary>
    /// Interface handle writing to logger.
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// event for gitting a message.
        /// </summary>
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        /// <summary>
        /// Logging the Message
        /// </summary>
        /// <param name="message">The message to write in the log</param>
        /// <param name="type"> type of message </param>
        void Log(string message, MessageTypeEnum type);
        //return all logs
        string[] GetLogs();
    }
}
