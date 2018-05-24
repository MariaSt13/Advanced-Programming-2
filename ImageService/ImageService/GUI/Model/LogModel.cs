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
    public class LogModel : ILogModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<MessageRecievedEventArgs> _MessageList = new ObservableCollection<MessageRecievedEventArgs> {
            new MessageRecievedEventArgs("vmfkvf\n\n",MessageTypeEnum.INFO), new MessageRecievedEventArgs("vmfkvf\n\n", MessageTypeEnum.WARNING), new MessageRecievedEventArgs("vmfkvf\n\n", MessageTypeEnum.FAIL) };

        public LogModel()
        {
            Communication.CommunicationSingleton.Instance.SingletonCommandRecieved += CommandRecieved;
        }

        private void CommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            switch (e.CommandID)
            {
                //GetConfigCommand command
                case ((int)CommandEnum.CommandEnum.LogCommand):
                    string[] arg = e.Args;
                    int type;
                    string message;
                    for (int i = 0; i < arg.Length; i += 2)
                    {
                        type = int.Parse(arg[i]);
                        message = arg[i + 1];
                        MessageList.Add(new MessageRecievedEventArgs(message,(MessageTypeEnum)type));
                    }
                    break;

            }
        }

        public void NotifyPropertyChanged(string propName)
        {
            this?.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public ObservableCollection<MessageRecievedEventArgs> MessageList
        {
            get
            {
                return this._MessageList;
            }

            set
            {
                this._MessageList = value;
                NotifyPropertyChanged("Handlelist");
            }
        }
    }
}
