using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Student
    {

        public Student(string first, string last, string id)
        {
            this.FirstName = first;
            this.LastName = last;
            this.ID = id;  
        }
        [DataType(DataType.Text)]
        [Display(Name = "ID")]
        public string ID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
    }
}