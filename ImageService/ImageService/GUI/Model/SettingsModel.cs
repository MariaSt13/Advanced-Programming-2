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
        private string _LogName;
        private string _OutputDirectory;
        private string _SourceName;
        private int _ThumbnailSize;
        private object _SelectedItem;
        private ObservableCollection<object> _Handlerslist = new ObservableCollection<object> { };

        /// <summary>
        /// constructor.
        /// </summary>
        public SettingsModel()
        {
            Communication.CommunicationSingleton.Instance.SingletonCommandRecieved += CommandRecieved;
        }

        /// <summary>
        /// recieved command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">args </param>
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
                    //get handler split by ';'
                    string[] pathArray = args[4].Split(';');
                    //add to list
                    foreach( string path in pathArray) {
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            this.Handlerslist.Add(path);
                        });
                    }
                    break;

                //CloseCommand command
                case ((int)CommandEnum.CommandEnum.CloseCommand):
                    //close a command
                    string handlerPath = e.Args[0];
                    for (int i = 0; i < _Handlerslist.Count; i++)
                    {
                        if (_Handlerslist[i].Equals(handlerPath))
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                _Handlerslist.RemoveAt(i);
                            });
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// PropertyChanged
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName)
        {
            this?.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        // the properties implementation

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
                NotifyPropertyChanged("SourceName");
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
                NotifyPropertyChanged("ThumbnailSize");
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
