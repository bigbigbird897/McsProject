using System.Globalization;
using System.Windows.Data;

namespace CoreWpfLib.Converters
{
    public class SubjectStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string data = "";
            int d = (int)value;
            switch (d)
            {
                case 0:
                    data = "准备就绪";
                    break;

                case 1:
                    data = "进行中";
                    break;

                case 2:
                    data = "任务成功";
                    break;

                case 3:
                    data = "任务失败";
                    break;

                default:
                    break;
            }
            return data;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}