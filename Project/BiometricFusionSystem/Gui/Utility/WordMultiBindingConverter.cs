using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Gui.Utility
{
    class WordMultiBindingConverter : System.Windows.Data.IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string wordToAdd = values[0] as string;
            string wordFromListbox = (values[1] as string);
            if(!string.IsNullOrEmpty(wordToAdd))
            {
                return wordToAdd;
            }
            else if(!string.IsNullOrEmpty(wordFromListbox))
            {
                return wordFromListbox;
            }
            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { value };
        }
    }
}
