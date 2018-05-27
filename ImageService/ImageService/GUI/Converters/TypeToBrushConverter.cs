using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace GUI.Converters
{
    /// <summary>
    /// type of log message convert
    /// </summary>
    public class TypeToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string returnVal = "";
            switch ((MessageTypeEnum)value)
            {
                //error
                case MessageTypeEnum.FAIL:
                    returnVal = "IndianRed";
                    break;
                 // info
                case MessageTypeEnum.INFO:
                    returnVal = "LightGreen";
                    break;
                // warning
                case MessageTypeEnum.WARNING:
                    returnVal = "khaki";
                    break;
            }

            return returnVal;
    }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MessageTypeEnum e = 0;
            switch ((string)value)
            {
                case "IndianRed":
                    e = MessageTypeEnum.FAIL;
                    break;
                case "LightGreen":
                    e = MessageTypeEnum.INFO;
                    break;
                case "khaki":
                    e = MessageTypeEnum.WARNING;
                    break;
            }
            return e;
        }
    }
}
