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
        }
        return list;
    }
}