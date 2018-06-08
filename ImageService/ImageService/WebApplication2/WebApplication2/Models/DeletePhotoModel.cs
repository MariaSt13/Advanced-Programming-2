using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class DeletePhotoModel
    {
        private Photo _photo;


        public void Delete()
        {
            string yearNum = _photo.Year.Split(':')[1];
            string monthNum = _photo.Month.Split(':')[1];
            string path = "../PhotosOutput" + "/" + yearNum + "/" + monthNum + "/" + _photo.Name;
            string thumnialPhotoPath = HttpContext.Current.Server.MapPath(_photo.Path);
            string orignaPhotolPath = HttpContext.Current.Server.MapPath(path);
            File.Delete(thumnialPhotoPath);
            File.Delete(orignaPhotolPath);
        }

        public Photo photo
        {
            get
            {
                return this._photo;
            }
            set
            {
                this._photo = value;
            }
        }

    }
}