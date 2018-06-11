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

        /// <summary>
        /// constructor.
        /// </summary>
        public ImageWebModel()
        {
            communicationModel.Connect();
            this.students = readStudentsInfo();
            this._connect = communicationModel.Instance.isConnected;
        }

        /// <summary>
        /// if server is connected
        /// </summary>
        public string connect
        {
            get {
                if (this._connect == true)
                {
                    return "Connceted";
                } else
                {
                    return "Not Connected";
                }
            }
        }

        /// <summary>
        /// numOfPictures get and set.
        /// </summary>
        public int numOfPictures
        {
            get { return _numOfPictures; }
            set { _numOfPictures = value; }
        }

        /// <summary>
        /// students get and set.
        /// </summary>
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

        /// <summary>
        /// read the students info from file and return list of students.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// count number of pictures in output dirctory
        /// </summary>
        /// <param name="path">path to output dirctory</param>
        public void CountNumOfPictures(string path)
        {
            path += "/Thumbnails";
            int fileCount;
            try
            {
                 fileCount = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Length;
            } catch(Exception)
            {
                fileCount = 0;
            }
            this.numOfPictures =  fileCount;
        }
    }
}