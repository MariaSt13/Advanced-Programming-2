using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    interface ICommand
    {
        // The Function That will Execute The command
        string Execute(string[] args, out bool result);         
    }
}
