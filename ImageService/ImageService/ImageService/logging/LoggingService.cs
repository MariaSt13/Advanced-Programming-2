﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecieved.Invoke(this, new MessageRecievedEventArgs(message, type));
        }
    }
}
