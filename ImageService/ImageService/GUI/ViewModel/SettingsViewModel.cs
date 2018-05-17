using GUI.Model;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI.ViewModel
{
    public class SettingsViewModel: INotifyPropertyChanged
    { 
        private ISettingsModel model;
        public ICommand RemoveCommand { get; private set; }
        private object _selectedItem;

        public event PropertyChangedEventHandler PropertyChanged;
        public event SelectionChangedEventHandler SelectionChanged;

        public SettingsViewModel()
        {
            this.model = new SettingsModel();
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            { NotifyPropertyChanged(e.PropertyName); };
        }


        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void OnRemove(object obj)
        {

        }

        private bool CanRemove(object obj)
        {
            if (this._selectedItem != null)
            {
                return true;
            }
            return false;
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

        public object selectedItem
        {
            get { return this._selectedItem; }
            set
            {
               this._selectedItem = value;
            }
        }

    }
}
