using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Photo
    {
        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="path">path of photo</param>
        /// <param name="name">name of photo</param>
        /// <param name="year">the yeat the photo was taken</param>
        /// <param name="month">the month the photo was taken</param>
        public Photo(string path, string name, string year, string month)
        {
            this.Path = path;
            this.Name = name;
            this.Year = year;
            this.Month = month;
        }

        [DataType(DataType.Text)]
        [Display(Name = "Path")]
        public string Path { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Year")]
        public string Year { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Month")]
        public string Month { get; set; }
    }
}