using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class ViewPhotoModel
    {
        private Photo _photo;

        public ViewPhotoModel(Photo p)
        {
            _photo = p;
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