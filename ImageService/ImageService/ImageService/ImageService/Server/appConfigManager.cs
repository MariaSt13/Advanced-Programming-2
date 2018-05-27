using Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    /// <summary>
    /// Manages app config.
    /// </summary>
    public class appConfigManager
    {
        private static appConfigManager instance;
        private string _OutputDir;
        private string _SourceName;
        private string _LogName;
        private int _ThumbnailSize;
        private List<string> _Handlers;
        public event EventHandler<CommandRecievedEventArgs> clickRemove;
        private static object m_lock = new object();

        // initilize members from app config file.
        private appConfigManager()
        {
            this._Handlers = new List<string>();
            this.OutputDir = ConfigurationManager.AppSettings["OutputDir"];
            this.SourceName = ConfigurationManager.AppSettings["SourceName"];
            this.LogName = ConfigurationManager.AppSettings["LogName"];
            this.ThumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);

            //split the handler by ';'
            string[] pathArray = ConfigurationManager.AppSettings["Handler"].Split(';');
            foreach (string path in pathArray)
            {
                //add to list.
                this.addHandler(path);
            }
        }

        /// <summary>
        /// get singlton instance
        /// </summary>
        public static appConfigManager Instance
        {
            get
            {
                lock (m_lock)
                {
                    if (instance == null)
                    {
                        instance = new appConfigManager();
                    }
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
                //return the list in the original way "handler;handler"
                return string.Join(";",this._Handlers);
            }
        }
        /// <summary>
        /// add a new handler to the list.
        /// </summary>
        /// <param name="handler">the handler to add</param>
        private void addHandler(string handler)
        {
            this._Handlers.Add(handler);
        }
        /// <summary>
        /// remove a handler from the list
        /// </summary>
        /// <param name="handler">the handler to remove</param>
        public void removeHandler(string handler)
        {
            string[] arg = { handler };
            //invoke click remove event
            clickRemove?.Invoke(this, new CommandRecievedEventArgs((int)CommandEnum.CommandEnum.CloseCommand, arg, null));
            //serach the handler in the list and remove it
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
