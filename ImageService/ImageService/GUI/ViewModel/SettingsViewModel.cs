using GUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModel
{
    public class SettingsViewModel: INotifyPropertyChanged
    {
        private ISettingsModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsViewModel(ISettingsModel model)
        {
            this.model = model;
        }

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


        public string OutputDirectory
        {
        get { return model.OutputDirectory; }
            set
            {
                model.OutputDirectory = value;
                NotifyPropertyChanged("OutputDirectory");
            }
        }

        public string SourceName
        {
            get { return model.SourceName; }
            set
            {
                model.SourceName = value;
                NotifyPropertyChanged("SourceName");
            }
        }



        public string LogName
        {
            get { return model.LogName; }
            set
            {
                model.LogName = value;
                NotifyPropertyChanged("LogName");
            }
        }

        public int ThumbnailSize
        {
            get { return model.ThumbnailSize; }
            set
            {
                model.ThumbnailSize = value;
                NotifyPropertyChanged("ThumbnailSize");
            }
        }

    }
}
