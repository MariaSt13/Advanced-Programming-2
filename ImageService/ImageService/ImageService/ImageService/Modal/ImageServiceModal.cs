using System;
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
            //check if outputDir doesnt exists - create it.
            if (!Directory.Exists(m_OutputFolder))
            {
                // Try to create the directory.
                Directory.CreateDirectory(m_OutputFolder);
            }
            getDate(path);
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
    }
}
