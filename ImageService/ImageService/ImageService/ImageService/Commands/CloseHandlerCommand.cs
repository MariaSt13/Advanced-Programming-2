using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Server;
using Infrastructure;
using Newtonsoft.Json;

namespace ImageService.Commands
{
    public class CloseHandlerCommand : ICommand
    {
        /// <summary>
        /// Execute the close handler command. closes a directory handler
        /// </summary>
        /// <param name="args">args[0] is the directory path</param>
        /// <param name="result">boll result</param>
        /// <returns>a serialize CommandRecievedEventArgs object</returns>
        public string Execute(string[] args, out bool result)
        {
            result = true;

            //remove from appConfigManager
            appConfigManager.Instance.removeHandler(args[0]);

            CommandRecievedEventArgs command =
               new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.CloseCommand, args, null);
            return JsonConvert.SerializeObject(command);
        }
    }
}
