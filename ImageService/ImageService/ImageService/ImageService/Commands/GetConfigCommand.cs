using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Newtonsoft.Json;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            result = true;
            string[] arr = new string[5];
            arr[0] = ConfigurationManager.AppSettings["OutputDir"];
            arr[1] = ConfigurationManager.AppSettings["SourceName"];
            arr[2] = ConfigurationManager.AppSettings["LogName"];
            arr[3] = ConfigurationManager.AppSettings["ThumbnailSize"];
            arr[4] = ConfigurationManager.AppSettings["Handler"];
            CommandRecievedEventArgs command =
                new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.GetConfigCommand, arr, null);
            return JsonConvert.SerializeObject(command);
        }
    }
}
