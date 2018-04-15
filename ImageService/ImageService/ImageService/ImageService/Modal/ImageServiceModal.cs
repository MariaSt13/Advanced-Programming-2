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
        private ILoggingService m_logging;
        private static Regex r = new Regex(":");

        #endregion
        /// <summary>
        /// constrcutor.
        /// </summary>
        /// <param name="outputFolder">output folder path</param>
        /// <param name="size">size of thumbnail</param>
        /// <param name="m_logging">logging object</param>
        public ImageServiceModal(string outputFolder, int size, ILoggingService m_logging)
        {
            this.m_logging = m_logging;
            this.m_OutputFolder = outputFolder;
            this.m_thumbnailSize = size;
            this.m_logging.Log("ImageModal: constructor", Logging.Modal.MessageTypeEnum.INFO);
        }

        /// <summary>
        /// This function creates "Thumbnails" and "Outpot" folders if they do not exist, 
        /// And transfers the new image that was added to the appropriate 
        /// location in Outpot folder a month and a year ago. 
        /// In addition creates thumbnail.
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="result">true if the process ended successfully</param>
        /// <returns></returns>
        public string AddFile(string path, out bool result)
        {
            result = true;
            string strReturn = "file added successfully";
            if (!File.Exists(@path))
            {
                this.m_logging.Log("ImageModal: AddFile. File not exist", Logging.Modal.MessageTypeEnum.INFO);
                result = false;
                return "error: file doesnt exist";
            }
            //check if outputDir doesnt exists - create it.
            if (!Directory.Exists(m_OutputFolder))
            {
                this.m_logging.Log("ImageModal: AddFile. outputDir not exist", Logging.Modal.MessageTypeEnum.INFO);
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(m_OutputFolder);
                //di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }

            string thumbnailsPath = m_OutputFolder + "\\Thumbnails";

            //check if thumbnailsPath doesnt exists - create it.
            if (!Directory.Exists(thumbnailsPath))
            {
                this.m_logging.Log("ImageModal: AddFile. thumbnailsPath not exist", Logging.Modal.MessageTypeEnum.INFO);
                // Try to create the directory.
                Directory.CreateDirectory(thumbnailsPath);
            }

            this.m_logging.Log("ImageModal: getDate " + path, Logging.Modal.MessageTypeEnum.INFO);
            DateTime picDate = new DateTime();
            try
            {
               picDate = GetDateTakenFromImage(path);
            }
            catch (Exception e)
            {
                this.m_logging.Log(e.Message, Logging.Modal.MessageTypeEnum.INFO);
            }
            
            this.m_logging.Log("ImageModal: getDate reutrn year" + picDate.Year, Logging.Modal.MessageTypeEnum.INFO);

            string folderPathByYear = m_OutputFolder + "\\" + picDate.Year;
            if (!Directory.Exists(folderPathByYear))
            {
                // Try to create the directory.
                Directory.CreateDirectory(folderPathByYear);
            }

            string folderPathByYearAndMonth = folderPathByYear + "\\" + picDate.Month;
            if (!Directory.Exists(folderPathByYearAndMonth))
            {
                // Try to create the directory.
                Directory.CreateDirectory(folderPathByYearAndMonth);
            }

            string newPath = path;
            string fileName = Path.GetFileName(path);
            while (File.Exists(folderPathByYearAndMonth + "\\" + fileName))
            {
                fileName = Path.GetFileNameWithoutExtension(newPath);
                string extension = Path.GetExtension(newPath);
                this.m_logging.Log("ImageModal: AddFile. file name was changed", Logging.Modal.MessageTypeEnum.INFO);
                fileName += "1";
                fileName = fileName + extension;
                string directoryName = Path.GetDirectoryName(newPath);
                newPath = "";
                newPath = directoryName + "\\" + fileName;
                this.m_logging.Log("ImageModal:" + newPath, Logging.Modal.MessageTypeEnum.INFO);
                strReturn = "error: file name was changed";
            }


            string thumbnailsPathByYear = thumbnailsPath + "\\" + picDate.Year;
            if (!Directory.Exists(thumbnailsPathByYear))
            {
                // Try to create the directory.
                Directory.CreateDirectory(thumbnailsPathByYear);
            }

            string thumbnailsPathByYearAndMonth = thumbnailsPathByYear + "\\" + picDate.Month;
            if (!Directory.Exists(thumbnailsPathByYearAndMonth))
            {
                // Try to create the directory.
                Directory.CreateDirectory(thumbnailsPathByYearAndMonth);
            }
            
            this.m_logging.Log("sorce::: " + path + " dest " + folderPathByYearAndMonth + "\\" + Path.GetFileName(newPath), Logging.Modal.MessageTypeEnum.INFO);
            try
            {
                File.Copy(path, folderPathByYearAndMonth + "\\" + Path.GetFileName(newPath));
                File.Delete(path);
            }
          
            catch (IOException ex)
            {
                this.m_logging.Log(ex.Message + Path.GetFileName(path), Logging.Modal.MessageTypeEnum.INFO);
            }
            createThumbnails(folderPathByYearAndMonth + "\\" + Path.GetFileName(newPath), thumbnailsPathByYearAndMonth + "\\");
            return strReturn;
        }


        //retrieves the datetime WITHOUT loading the whole image
        public DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))

            {
                PropertyItem propItem = null;
                try
                {
                    propItem = myImage.GetPropertyItem(36867);
                }
                catch (ArgumentException e)
                {
                    this.m_logging.Log(e.Message, Logging.Modal.MessageTypeEnum.INFO);
                }
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }

        private string createFolder(DateTime dateTime, string path)
        {
            this.m_logging.Log("ImageModal: createFolder", Logging.Modal.MessageTypeEnum.INFO);

            int year = dateTime.Year;
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


        private void createThumbnails(string path, string thumbnailsPathByDate)
        {
            this.m_logging.Log("ImageModal: createThumbnails", Logging.Modal.MessageTypeEnum.INFO);
            Image image = Image.FromFile(path);
            Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
            thumb.Save(Path.ChangeExtension(thumbnailsPathByDate, Path.GetFileNameWithoutExtension(path) + ".thumb"));
            thumb.Dispose();
            image.Dispose();
        }

    }
}