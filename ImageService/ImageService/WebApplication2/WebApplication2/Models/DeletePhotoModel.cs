﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class DeletePhotoModel
    {
        private Photo _photo;

        /// <summary>
        /// delete the original photo and the thumbnail
        /// </summary>
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

        /// <summary>
        /// photo get and set.
        /// </summary>
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