using iWeibo.WP7.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace iWeibo.WP7.Resources.Converters
{
    public class TimeStampConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            string text = value.ToString();
            DateTime TimestampLocalZero = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
            long timeStamp = System.Convert.ToInt64(text);
            DateTime dt = TimestampLocalZero.AddSeconds(timeStamp);

            return DateTimeHelper.GetDateString(dt);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
