using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    interface ICommand
    {
        /// <summary>
        //  Executes the command by calling a function in IImageServiceModal class.
        /// </summary>
        /// <param name="args">Function arguments</param>
        /// <param name="result">If the value is true then the process is successfully completed</param>
        /// <returns></returns>
        string Execute(string[] args, out bool result);         
    }
}
