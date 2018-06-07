using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Photo
    {
        public Photo(string path, string name, string date)
        {
            this.Path = path;
            this.Name = name;
            this.Date = date;
        }
        [DataType(DataType.Text)]
        [Display(Name = "Path")]
        public string Path { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Date")]
        public string Date { get; set; }
    }
}