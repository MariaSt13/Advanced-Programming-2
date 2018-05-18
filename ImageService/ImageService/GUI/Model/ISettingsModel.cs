using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Model
{
    public interface ISettingsModel : INotifyPropertyChanged
    {
        //properties
        string OutputDirectory { get; set; }
        string SourceName { get; set; }
        string LogName { get; set; }
        object SelectedItem { get; set; }
        ObservableCollection<object> Handlerslist { get; set; }
        int ThumbnailSize { get; set; }
    }
}
