using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GUI.Converters
{
    /// <summary>
    /// convertor of color , connect to server.
    /// </summary>
    public class ConnectedToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;
            //not connected
            if (boolValue == false)
            {
                return "gray";
            }
            //connected
            else
            {
                return "white";
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((string)value)
            {
                case "gray":
                    return false;
                case "white":
                    return true;
            }
            return false;
        }
    }
}
