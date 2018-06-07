using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class PhotosModel
    {
        private List<Photo> _photos;

        public PhotosModel()
        {
            this._photos = readPhotosInfo();
        }

        private List<Photo> readPhotosInfo()
        {
            string path = HttpContext.Current.Server.MapPath("../PhotosOutput");
            string[] photosPaths = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);


            List<Photo> list = new List<Photo>();
            string[] lines = File.ReadAllLines(HttpContext.Current.Server.MapPath("/App_Data/student_info.txt"));
            for (int i = 0; i < photosPaths.Length; i++)
            {
                string photoName = Path.GetFileName(photosPaths[i]);
                string photoPath = photosPaths[i];
                string month = Path.GetFileName(Path.GetDirectoryName(photoPath));
                string year = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(photoPath)));
                string photoDate = "Year:" + year + "\n" + "Month" + month;
                Photo photo = new Photo(photoPath, photoName, photoDate);
                list.Add(photo);
            }
            return list;
        }
    }

  
}