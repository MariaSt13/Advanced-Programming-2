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
        /// <summary>
        /// constructor.
        /// </summary>
        public LogsModel()
        {
            this.MessageList = new List<MessageRecievedEventArgs>();
        }

        [DataType(DataType.Text)]
        [Display(Name = "MessageList")]
        public List<MessageRecievedEventArgs> MessageList { get; set; }

        /// <summary>
        /// this function adds  logs to the list.
        /// </summary>
        public void getLogs()
        {
            //logs request
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.LogCommand, null, null);
            string str = JsonConvert.SerializeObject(command);
            communicationModel.Write(str);
    
            //get logs
            string output = communicationModel.read();
            CommandRecievedEventArgs deserializedProduct = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(output);
            string[] arg = deserializedProduct.Args;

            string type;
            string message;
            MessageTypeEnum typeEnum = MessageTypeEnum.FAIL;
            MessageList.Clear();

            //get type to each message
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