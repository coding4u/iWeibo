using System;
using System.Windows;
using System.Windows.Data;

namespace iWeibo.WP7.Resources.Converters
{
    public class IntToVisibilityConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int i = 0;
            if (value != null)
                i = (int)value;

            if (i == 1)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
