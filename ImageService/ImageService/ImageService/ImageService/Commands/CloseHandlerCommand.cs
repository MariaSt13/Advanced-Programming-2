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
        public string Execute(string[] args, out bool result)
        {
            
            result = true;
            appConfigManager.Instance.removeHandler(args[0]);
            CommandRecievedEventArgs command =
               new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.CloseCommand, args, null);
            return JsonConvert.SerializeObject(command);
        }
    }
}
