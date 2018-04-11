using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;

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

        }
    }
}
