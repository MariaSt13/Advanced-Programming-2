using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class ViewPhotoModel
    {
        private Photo _photo;

        /// <summary>
        /// constrcutor.
        /// </summary>
        /// <param name="p"></param>
        public ViewPhotoModel(Photo p)
        {
            _photo = p;
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