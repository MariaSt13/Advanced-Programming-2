using Infrastructure;
using Newtonsoft.Json;
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
        /// <summary>
        /// Execute log command. gets a log a return it in serialize CommandRecievedEventArgs object.
        /// </summary>
        /// <param name="args">the log args - message and type</param>
        /// <param name="result"></param>
        /// <returns>a serialize CommandRecievedEventArgs object</returns>
        public string Execute(string[] args, out bool result)
        {
            result = true;
            CommandRecievedEventArgs command =
               new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.LogCommand, args, null);
            return JsonConvert.SerializeObject(command);

        }
    }
}
