using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class ImageWebModel
    {
        private List<Student> _students;
        private int _numOfPictures;

        public ImageWebModel()
        {
            //this.students = readStudentsInfo();
           // this.numOfPictures = CountNumOfPictures();

        }

        [Display(Name = "FirstName")]
        public int numOfPictures
        {
            get { return _numOfPictures; }
            set { _numOfPictures = value; }
        }
        public List<Student> students
        {
            get {
                return this._students;
            }
            set
            {
                this._students = value;
            }
        }

        public void readStudentsInfo()
        {
            List<Student> list = new List<Student>();
            string[] lines = System.IO.File.ReadAllLines(HttpContext.Current.Server.MapPath("../App_Data/student_info.txt"));
            for (int i = 0; i < lines.Length; i +=3)
            {
                Student s = new Student(lines[i], lines[i + 1], lines[i + 2]);
                list.Add(s);
            }
            this.students = list;
        }

        public void CountNumOfPictures()
        {
            string path = HttpContext.Current.Server.MapPath("../PhotosOutput");
            int fileCount = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Length;
            this.numOfPictures =  fileCount;
        }
    }
}