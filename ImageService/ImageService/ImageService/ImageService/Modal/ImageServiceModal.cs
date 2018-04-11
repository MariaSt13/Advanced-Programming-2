﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size

        #endregion
        public ImageServiceModal(string outputFolder, int size)
        {
            this.m_OutputFolder = outputFolder;
            this.m_thumbnailSize = size;
        }

        public string AddFile(string path, out bool result) 
        {
            result = true;
            string strReturn = "file added successfully";
            if (!File.Exists(path))
            {
                result = false;
                return "error: file doesnt exist";
            }
            //check if outputDir doesnt exists - create it.
            if (!Directory.Exists(m_OutputFolder))
            {
                // Try to create the directory.
                Directory.CreateDirectory(m_OutputFolder);
            }

            string thumbnailsPath = m_OutputFolder + "\\Thumbnails";

            //check if thumbnailsPath doesnt exists - create it.
            if (!Directory.Exists(thumbnailsPath))
            {
                // Try to create the directory.
                Directory.CreateDirectory(thumbnailsPath);
            }

            DateTime picDate = getDate(path);
            string folderPathByDate = createFolder(picDate,this.m_OutputFolder);
            string fileName = Path.GetFileName(path);
            if (File.Exists(folderPathByDate + "\\" + fileName))
            {
                fileName += "1";
                string directoryName = Path.GetDirectoryName(path);
                path = "";
                path = directoryName + "\\" + fileName;
                strReturn = "error: file name was changed";
            }
            string thumbnailsPathByDate = createFolder(picDate, thumbnailsPath);
            createThumbnails(path, thumbnailsPathByDate);
            File.Move(path, folderPathByDate);
            return strReturn;
        }

        private DateTime getDate(string path)
        {
            Regex r = new Regex(":");
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }

        private string createFolder(DateTime dateTime, string path)
        {
            int year =  dateTime.Year;
            int month = dateTime.Month;
            string newpath = path + "\\" + year;
            if (!Directory.Exists(newpath))
            {
                // Try to create the directory.
                Directory.CreateDirectory(newpath);
            }
            path += "/" + month;
            if (!Directory.Exists(newpath))
            {
                // Try to create the directory.
                Directory.CreateDirectory(newpath);
            }
            return newpath;
        }

        
        private void createThumbnails(string path,string thumbnailsPathByDate) {
            Image image = Image.FromFile(path);
            Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
            thumb.Save(Path.ChangeExtension(thumbnailsPathByDate, Path.GetFileName(path)));
        }

    }
}