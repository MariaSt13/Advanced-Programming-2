using Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class ConfigModel
    {
        public ConfigModel()
        {
            this.Handlerslist = new List<string>();
        }
        [DataType(DataType.Text)]
        [Display(Name = "OutputDirectory")]
        public string OutputDirectory { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "LogName")]
        public string LogName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "ThumbnailSize")]
        public int ThumbnailSize { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Handlerslist")]
        public List<string> Handlerslist { get; set; }

        public void getConfig()
        {
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.GetConfigCommand, null, null);
            string str = JsonConvert.SerializeObject(command);
            communicationModel.Write(str);
            string output = communicationModel.read();
            CommandRecievedEventArgs deserializedProduct = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(output);
            string[] args = deserializedProduct.Args;
            this.OutputDirectory = args[0];
            this.SourceName = args[1];
            this.LogName = args[2];
            this.ThumbnailSize = int.Parse(args[3]);
            //get handler split by ';'
            string[] pathArray = args[4].Split(';');
            this.Handlerslist.Clear();
            //add to list
            foreach (string path in pathArray)
            {
                this.Handlerslist.Add(path);
            }
        }
    }
}