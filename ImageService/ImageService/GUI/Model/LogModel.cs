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

        private ObservableCollection<MessageRecievedEventArgs> _MessageList = new ObservableCollection<MessageRecievedEventArgs> { };

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
                    string type;
                    MessageTypeEnum typeEnum = MessageTypeEnum.FAIL;
                    string message;
                    for (int i = 0; i < arg.Length; i += 2)
                    {
                        type =arg[i];
                        switch (type)
                        {
                            case "INFO":
                                typeEnum = MessageTypeEnum.INFO;
                                break;
                            case "WARNING":
                                typeEnum = MessageTypeEnum.WARNING;
                                break;
                            case "FAIL":
                                typeEnum = MessageTypeEnum.FAIL;
                                break;

                        }
                        message = arg[i + 1];
                        App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                        {
                            MessageList.Add(new MessageRecievedEventArgs(message, typeEnum));
                        });
                      
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
