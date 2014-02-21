using System;
using System.Windows.Data;

namespace iWeibo.WP7.Resources.Converters
{
    class HeadImageConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            string url = value.ToString();
            switch (parameter.ToString())
            {
                case "20":
                    return url + @"/20";
                case "30":
                    return url + @"/30";
                case "40":
                    return url + @"/40";
                case "50":
                    return url + @"/50";
                case "100":
                    return url + @"/100";
                default:
                    return url + @"/50";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
