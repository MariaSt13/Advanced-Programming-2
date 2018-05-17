using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Model
{

    public class SettingsModel : ISettingsModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _LogName;
        private string _OutputDirectory;
        private string _SourceName;
        private int _ThumbnailSize;

        public void NotifyPropertyChanged(string propName)
        {
            this?.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public string LogName
        {
            get
            {
                return this._LogName;
            }

            set
            {
                this._LogName = value;
                NotifyPropertyChanged("LogName");
            }
        }

        public string OutputDirectory
        {
            get
            {
                return this._OutputDirectory;
            }

            set
            {
                this._OutputDirectory = value;
                NotifyPropertyChanged("OutputDirectory");
            }
        }

        public string SourceName
        {
            get
            {
                return this._SourceName;
            }

            set
            {
                this._SourceName = value;
                NotifyPropertyChanged("_SourceName");
            }
        }

        public int ThumbnailSize
        {
            get
            {
                return this._ThumbnailSize;
            }

            set
            {
                this._ThumbnailSize = value;
                NotifyPropertyChanged("_ThumbnailSize");
            }
        }
    }
}
