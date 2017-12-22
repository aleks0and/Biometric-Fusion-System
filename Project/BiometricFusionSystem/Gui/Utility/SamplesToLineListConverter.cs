using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Shapes;

namespace Gui.Utility
{
    [ValueConversion(typeof(List<short>), typeof(List<Line>))]
    public class SamplesToLineListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return null;
            }
            var lines = new List<Line>();
            var renderPos = 0;
            var samples = value as List<short>;
            for(int i = 0; i < samples.Count; i+=8)
            {
                var line = new Line()
                {
                    X1 = renderPos,
                    X2 = renderPos,
                    Y1 = Math.Abs((double)samples[i]),
                    Y2 = 2 * Math.Abs((double)samples[i])
                };
                lines.Add(line);
                renderPos++;
            }
            return lines;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
