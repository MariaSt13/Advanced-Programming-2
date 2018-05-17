using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            result = true;
            JObject configObj = new JObject();
            configObj["OutputDir"] = ConfigurationManager.AppSettings["OutputDir"];
            configObj["SourceName"] = ConfigurationManager.AppSettings["SourceName"];
            configObj["LogName"] = ConfigurationManager.AppSettings["LogName"];
            configObj["ThumbnailSize"] = ConfigurationManager.AppSettings["ThumbnailSize"];
            string paths = ConfigurationManager.AppSettings["Handler"];
            string[] pathArray = paths.Split(';');
            JArray arr = new JArray(pathArray);
            configObj["Handlers"] = arr;
            return configObj.ToString();
        }
    }
}
