using System;
using System.Collections.Generic;
using Infrastructure;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        Dictionary<string, MessageTypeEnum> logs;
        /// <summary>
        /// event for gitting a message.
        /// </summary>
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        public LoggingService()
        {
            this.logs = new Dictionary<string, MessageTypeEnum>();
        }

        public Dictionary<string, MessageTypeEnum> GetLogs()
        {
            return this.logs;
        }

        /// <summary>
        /// Logging the Message
        /// </summary>
        /// <param name="message">The message to write in the log</param>
        /// <param name="type"> type of message </param>
        public void Log(string message, MessageTypeEnum type)
        {
            this.logs.Add(message, type);
            MessageRecieved.Invoke(this, new MessageRecievedEventArgs(message, type));
        }
    }
}
