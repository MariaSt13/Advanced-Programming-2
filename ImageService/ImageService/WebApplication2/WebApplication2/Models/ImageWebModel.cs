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
        private bool _connect;
        public ImageWebModel()
        {
            this.students = readStudentsInfo();
            this.connect = communicationModel.Instance.isConnected;
        }

        public bool connect
        {
            get { return _connect; }
            set { _connect = value; }
        }
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

        private List<Student> readStudentsInfo()
        {
            List<Student> list = new List<Student>();
            string[] lines = File.ReadAllLines(HttpContext.Current.Server.MapPath("/App_Data/student_info.txt"));
            for (int i = 0; i < lines.Length; i++)
            {
                string[] info = lines[i].Split(' ');
                Student s = new Student(info[0], info[1], info[2]);
                list.Add(s);
            }
            return list;
        }

        public void CountNumOfPictures()
        {
            string path = HttpContext.Current.Server.MapPath("../PhotosOutput");
            int fileCount = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Length;
            this.numOfPictures =  fileCount;
        }
    }
}