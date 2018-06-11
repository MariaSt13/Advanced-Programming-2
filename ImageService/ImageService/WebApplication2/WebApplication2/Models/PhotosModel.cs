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
        string outputDirectory;

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="outputDirectory">path to output directory</param>
        public PhotosModel(string outputDirectory)
        {
            this.outputDirectory = outputDirectory;
            this._photos = readPhotosInfo();
        }

        /// <summary>
        /// this function create list of photos.
        /// </summary>
        /// <returns>list of photos</returns>
        private List<Photo> readPhotosInfo()
        {
            string[] photosPaths;
            string path = outputDirectory + "/Thumbnails";
            try
            {
                photosPaths = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            }
            catch (Exception)
            {
                //return empty list
                return new List<Photo>();
            }

            List<Photo> list = new List<Photo>();
           
            for (int i = 0; i < photosPaths.Length; i++)
            {
                //get detaile of each photo
                string photoName = Path.GetFileName(photosPaths[i]);
                string month = Path.GetFileName(Path.GetDirectoryName(photosPaths[i]));
                string year = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(photosPaths[i])));
                string photoPath = "../PhotosOutput/Thumbnails/" + year + "/" + month + "/" + photoName;

                //create photo and add to list
                Photo photo = new Photo(photoPath, photoName, "Year:" + year, "Month:"+month);
                list.Add(photo);
            }
            return list;
        }


        /// <summary>
        /// photos get and set.
        /// </summary>
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