using System;
using System.Globalization;
using System.Windows.Data;

namespace Repraicer.Converters
{
    public class DateToDaysCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var datePart = ((string) value).Split(' ')[0];
            var startDate = DateTime.Parse(datePart);
            return (DateTime.Now - startDate).Days;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
