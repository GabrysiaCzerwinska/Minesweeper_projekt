using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Minesweeper.Converters
{
    public class ValueToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((int)value)
            {
                case 0: return Brushes.MidnightBlue;
                case 1: return Brushes.Orchid;
                case 2: return Brushes.MediumOrchid;
                case 3: return Brushes.DarkOrchid;
                case 1000: return Brushes.Crimson; // bomba
                case 1001: return Brushes.Aquamarine; // pytajnik
                default: return Brushes.DarkMagenta;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (SolidColorBrush)value;
            if (val == Brushes.MidnightBlue)
                return 0;
            else if (val == Brushes.Orchid)
                return 1;
            else if (val == Brushes.MediumOrchid)
                return 2;
            else if (val == Brushes.DarkOrchid)
                return 3;
            else if (val == Brushes.DarkMagenta)
                return 4;
            else
                return 5;
        }
    }
}
