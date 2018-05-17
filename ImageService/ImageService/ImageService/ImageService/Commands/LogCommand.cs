using ImageService.Logging.Modal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class LogCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            result = true;
            JObject logMessage = new JObject();
            JArray argArray = new JArray(args);
            logMessage["log"] = argArray;
            return logMessage.ToString();

        }
    }
}
