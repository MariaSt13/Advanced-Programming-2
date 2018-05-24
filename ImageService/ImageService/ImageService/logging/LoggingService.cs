using System;
using System.Collections.Generic;
using Infrastructure;
using System.Collections;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        ArrayList logs;
        /// <summary>
        /// event for gitting a message.
        /// </summary>
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        public LoggingService()
        {
            this.logs = new ArrayList();
        }

        public string [] GetLogs()
        {
            return (String[])logs.ToArray(typeof(string));
        }

        /// <summary>
        /// Logging the Message
        /// </summary>
        /// <param name="message">The message to write in the log</param>
        /// <param name="type"> type of message </param>
        public void Log(string message, MessageTypeEnum type)
        {
            this.logs.Add(type.ToString());
            this.logs.Add(message);
            MessageRecieved.Invoke(this, new MessageRecievedEventArgs(message, type));
        }
    }
}
