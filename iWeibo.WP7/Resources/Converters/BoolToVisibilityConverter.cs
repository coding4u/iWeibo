﻿using System;
using System.Windows;
using System.Windows.Data;


namespace iWeibo.WP7.Resources.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                bool result;
                if (bool.TryParse(value.ToString(),out result))
                {
                    if (result)
                        return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
