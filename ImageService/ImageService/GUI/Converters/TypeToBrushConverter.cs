using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Converters
{
    class TypeToBrushConverter
    {
       public string convert(MessageTypeEnum type)
        {
            string returnVal = "";
            switch (type)
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
    }
}
