using GUI.Model;
using Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModel
{
    class LogViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private LogModel model;

        public LogViewModel() {
            this.model = new LogModel();
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            { NotifyPropertyChanged("VM_" + e.PropertyName); };
            Communication.CommunicationSingleton.Instance.connectServer += connectServerHandle;
            Communication.CommunicationSingleton.Connect();
        }

        private void connectServerHandle(object sender, MessageRecievedEventArgs e)
        {
            //get log
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.LogCommand, null, null);
            string str = JsonConvert.SerializeObject(command);
            Communication.CommunicationSingleton.Write(str);
        }

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public ObservableCollection<MessageRecievedEventArgs> VM_MessageList
        {
            get { return model.MessageList; }
            set
            {
                model.MessageList = value;
            }
        }
    }
}
