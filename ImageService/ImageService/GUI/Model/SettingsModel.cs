using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Model
{

    public class SettingsModel : ISettingsModel
    {
        //INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        //properties
        private string _LogName = "_LogName text";
        private string _OutputDirectory = "_OutputDirectory text";
        private string _SourceName = "_SourceName text";
        private int _ThumbnailSize = 80;
        private object _SelectedItem;
        private ObservableCollection<object> _Handlerslist = new ObservableCollection<object> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public SettingsModel()
        {
            Communication.CommunicationSingleton.Instance.SingletonCommandRecieved += CommandRecieved;
            //Communication.CommunicationSingleton.Write(new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.GetConfigCommand, null, null));
        }

        private void CommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            switch (e.CommandID)
            {
                //GetConfigCommand command
                case ((int)CommandEnum.CommandEnum.GetConfigCommand):
                    string[] args = e.Args;
                    this.OutputDirectory = args[0];
                    this.SourceName = args[1];
                    this.LogName = args[2];
                    this.ThumbnailSize = int.Parse(args[3]);
                    string[] pathArray = args[4].Split(';');
                    foreach( string path in pathArray) {
                        this.Handlerslist.Add(path);
                    }
                    break;

            }
        }

        public void NotifyPropertyChanged(string propName)
        {
            this?.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        // the properties implementatio

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

        public object SelectedItem
        {
            get
            {
                return this._SelectedItem;
            }

            set
            {
                this._SelectedItem = value;
                NotifyPropertyChanged("SelectedItem");
            }
        }

        public ObservableCollection<object> Handlerslist
        {
            get
            {
                return this._Handlerslist;
            }

            set
            {
                this._Handlerslist = value;
                NotifyPropertyChanged("Handlelist");
            }
        }
    }
}
