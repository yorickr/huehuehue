using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace HueUWP.Converters
{
    public class BooleanToStringConverter : IValueConverter
    {
        //From bool to nullable
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool b = (bool)value;
            return b.ToString();
        }

        //From nullable to bool
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string s = (string)value;
            bool b = false;

            if (s == "true")
                b = true;

            return b;
        }
    }
}
