using Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class LogsModel
    {
        public LogsModel()
        {
            this.MessageList = new List<MessageRecievedEventArgs>();
        }
        [DataType(DataType.Text)]
        [Display(Name = "MessageList")]
        public List<MessageRecievedEventArgs> MessageList { get; set; }

        public void getLogs()
        {
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.LogCommand, null, null);
            string str = JsonConvert.SerializeObject(command);
            communicationModel.Write(str);
            string output = communicationModel.read();
            CommandRecievedEventArgs deserializedProduct = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(output);
            string[] arg = deserializedProduct.Args;
            string type;
            MessageTypeEnum typeEnum = MessageTypeEnum.FAIL;
            string message;
            MessageList.Clear();
            for (int i = 0; i < arg.Length; i += 2)
            {
                type = arg[i];
                switch (type)
                {
                    case "INFO":
                        typeEnum = MessageTypeEnum.INFO;
                        break;
                    case "WARNING":
                        typeEnum = MessageTypeEnum.WARNING;
                        break;
                    case "FAIL":
                        typeEnum = MessageTypeEnum.FAIL;
                        break;
                }
                message = arg[i + 1];
                //add message to list
                MessageList.Add(new MessageRecievedEventArgs(message, typeEnum));
            }
        }
    }
}