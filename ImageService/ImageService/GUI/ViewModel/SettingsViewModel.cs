using GUI.Model;
using Infrastructure;
using Microsoft.Practices.Prism.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        //INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private ISettingsModel model;
        public ICommand RemoveCommand { get; private set; }

        public SettingsViewModel()
        {
            this.model = new SettingsModel();
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            { NotifyPropertyChanged("VM_" + e.PropertyName); };
            this.PropertyChanged += PropertyChangedd;
            Communication.CommunicationSingleton.Instance.connectServer += connectServerHandle;
            Communication.CommunicationSingleton.Connect();
        }

        private void connectServerHandle(object sender, MessageRecievedEventArgs e)
        {
            //get config
          /*  CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.GetConfigCommand, null, null);
            string str = JsonConvert.SerializeObject(command);
            Communication.CommunicationSingleton.Write(str);*/
        }

        private void PropertyChangedd(object sender, PropertyChangedEventArgs e)
        {
            var command = this.RemoveCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }

        protected void NotifyPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void OnRemove(object obj)
        {
           this.model.Handlerslist.Remove(this.model.SelectedItem);
        }

        private bool CanRemove(object obj)
        {
            if (this.model.SelectedItem == null)
            {
                return false;
            }
            return true;
        }


        // Properties
        public string VM_OutputDirectory
        {
        get { return model.OutputDirectory; }
            set
            {
                model.OutputDirectory = value;
            }
        }

        public string VM_SourceName
        {
            get { return model.SourceName; }
            set
            {
                model.SourceName = value;
            }
        }

        public string VM_LogName
        {
            get { return model.LogName; }
            set
            {
                model.LogName = value;
            }
        }

        public int VM_ThumbnailSize
        {
            get { return model.ThumbnailSize; }
            set
            {
                model.ThumbnailSize = value;
            }
        }

        public object VM_selectedItem
        {
            get { return model.SelectedItem; }
            set
            {
               model.SelectedItem = value;
            }
        }
        public ObservableCollection<object> VM_Handlerslist
        {
            get { return model.Handlerslist; }
            set
            {
                model.Handlerslist = value;
            }
        }
    }
}
