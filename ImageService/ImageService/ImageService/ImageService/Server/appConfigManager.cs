using Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class appConfigManager
    {
        private static appConfigManager instance;
        private string _OutputDir;
        private string _SourceName;
        private string _LogName;
        private int _ThumbnailSize;
        private List<string> _Handlers;
        public event EventHandler<CommandRecievedEventArgs> clickRemove;

        private appConfigManager()
        {
            this._Handlers = new List<string>();
            this.OutputDir = ConfigurationManager.AppSettings["OutputDir"];
            this.SourceName = ConfigurationManager.AppSettings["SourceName"];
            this.LogName = ConfigurationManager.AppSettings["LogName"];
            this.ThumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
            string[] pathArray = ConfigurationManager.AppSettings["Handler"].Split(';');
            foreach (string path in pathArray)
            {
                this.addHandler(path);
            }
        }
        public static appConfigManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new appConfigManager();
                }
                return instance;
            }
        }
        public string OutputDir
        {
            get
            {
                return _OutputDir;
            }

            set
            {
                _OutputDir = value;
            }
        }

        public string SourceName
        {
            get
            {
                return _SourceName;
            }

            set
            {
                _SourceName = value;
            }
        }

        public string LogName
        {
            get
            {
                return _LogName;
            }

            set
            {
                _LogName = value;
            }
        }

        public int ThumbnailSize
        {
            get
            {
                return _ThumbnailSize;
            }

            set
            {
                _ThumbnailSize = value;
            }
        }

        public string Handlers
        {
            get
            {
                return string.Join(";",this._Handlers);
            }
        }
        private void addHandler(string handler)
        {
            this._Handlers.Add(handler);
        }
        public void removeHandler(string handler)
        {
            string[] arg = { handler };
            clickRemove?.Invoke(this, new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.CloseCommand, arg, null));
            for (int i = 0; i < _Handlers.Count; i++)
            {
                if (_Handlers[i].Equals(handler))
                {
                    _Handlers.RemoveAt(i);
                }
            }
        }
    }
}
