using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public interface IImageController
    {
        /// <summary>
        /// Executing the Command Requet
        /// </summary>
        /// <param name="commandID"> The id of the command</param>
        /// <param name="args"> args to command </param>
        /// <param name="result"> bool that says if action was succes or fail</param>
        /// <returns></returns>
        string ExecuteCommand(int commandID, string[] args, out bool result);
    }
}
