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

        /// <summary>
        /// constructor
        /// </summary>
        public LogViewModel() {
            this.model = new LogModel();
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            { NotifyPropertyChanged("VM_" + e.PropertyName); };
            Communication.CommunicationSingleton.Instance.connectServer += connectServerHandle;
            Communication.CommunicationSingleton.Connect();
        }

        /// <summary>
        /// server conneceted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">args</param>
        private void connectServerHandle(object sender, MessageRecievedEventArgs e)
        {
            //get log
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.LogCommand, null, null);
            string str = JsonConvert.SerializeObject(command);
            Communication.CommunicationSingleton.Write(str);
        }

        /// <summary>
        /// PropertyChanged
        /// </summary>
        /// <param name="propName"></param>
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
