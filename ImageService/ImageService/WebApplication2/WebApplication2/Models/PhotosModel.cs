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
            string path = HttpContext.Current.Server.MapPath("PhotosOutput/Thumbnails");
            string[] photosPaths = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            List<Photo> list = new List<Photo>();
           
            for (int i = 0; i < photosPaths.Length; i++)
            {
                string photoName = Path.GetFileName(photosPaths[i]);
                string month = Path.GetFileName(Path.GetDirectoryName(photosPaths[i]));
                string year = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(photosPaths[i])));
                string photoPath = "../PhotosOutput/Thumbnails/" + year + "/" + month + "/" + photoName;
                Photo photo = new Photo(photoPath, photoName, "Year:" + year, "Month:"+month);
                list.Add(photo);
            }
            return list;
        }

        public List<Photo> photos
        {
            get
            {
                return this._photos;
            }
            set
            {
                this._photos = value;
            }
        }
    }

  
}