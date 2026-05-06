using System.Globalization;
using System.Windows.Data;

namespace McsWpfCore.Converters
{
    public class DatetimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dt = (DateTime)value;
            var m = System.Convert.ToDateTime("1900-01-01 00:00:00");
            if (m == dt)
            {
                return "";
            }
            else
            {
                return dt;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}