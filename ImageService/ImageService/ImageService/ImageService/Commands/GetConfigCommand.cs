using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Newtonsoft.Json;
using ImageService.Server;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        /// <summary>
        /// Execute get config command. return paramets from app config.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns>a serialize CommandRecievedEventArgs object</returns>
        public string Execute(string[] args, out bool result)
        {
            result = true;
            string[] arr = new string[5];
            arr[0] = appConfigManager.Instance.OutputDir;
            arr[1] = appConfigManager.Instance.SourceName;
            arr[2] = appConfigManager.Instance.LogName;
            arr[3] = appConfigManager.Instance.ThumbnailSize.ToString();
            arr[4] = appConfigManager.Instance.Handlers;
            CommandRecievedEventArgs command =
                new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.GetConfigCommand, arr, null);
            return JsonConvert.SerializeObject(command);
        }
    }
}
