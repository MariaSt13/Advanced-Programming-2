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
    public class TypeToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string returnVal = "";
            switch ((MessageTypeEnum)value)
            {
                case MessageTypeEnum.FAIL:
                    returnVal = "red";
                    break;
                case MessageTypeEnum.INFO:
                    returnVal = "green";
                    break;
                case MessageTypeEnum.WARNING:
                    returnVal = "yellow";
                    break;
            }

            return returnVal;
    }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MessageTypeEnum e = 0;
            switch ((string)value)
            {
                case "red":
                    e = MessageTypeEnum.FAIL;
                    break;
                case "green":
                    e = MessageTypeEnum.INFO;
                    break;
                case "yellow":
                    e = MessageTypeEnum.WARNING;
                    break;
            }
            return e;
        }
    }
}
