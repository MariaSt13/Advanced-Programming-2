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
        /// <summary>
        /// constructor.
        /// </summary>
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

        /// <summary>
        /// get config from server.
        /// </summary>
        public void getConfig()
        {
            // create new get config command
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.GetConfigCommand, null, null);
            string str = JsonConvert.SerializeObject(command);
            //write requast
            communicationModel.Write(str);
            // read answer
            string output = communicationModel.read();
            CommandRecievedEventArgs deserializedProduct = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(output);
            string[] args = deserializedProduct.Args;
            //get config 
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

        /// <summary>
        /// send delete hendler requast to server
        /// </summary>
        /// <param name="name">the name of hendler to remove</param>
        public void deleteHendler(string name)
        {
            string[] args = { name };
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.CloseCommand, args, null);
            string str = JsonConvert.SerializeObject(command);
            //write requast
            communicationModel.Write(str);
            //get confirm that the hendler is deleted
            string output = communicationModel.read();
        }
    }
}