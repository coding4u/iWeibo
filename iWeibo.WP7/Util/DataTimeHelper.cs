using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace iWeibo.WP7.Util
{
    public class DateTimeHelper
    {
        private static CultureInfo cInfo = new CultureInfo("en-US");
        public static string GetTime(string sinaFormat)
        {
            if (string.IsNullOrEmpty(sinaFormat))
                return string.Empty;
            string[] array = sinaFormat.Split(' ');
            array[4] = array[4].Substring(0, 3) + ":" + array[4].Substring(3, 2);
            string text = string.Join(" ", array);
            DateTime d = DateTime.ParseExact(text, "ddd MMM dd HH:mm:ff zzz yyyy", DateTimeHelper.cInfo);
            return DateTimeHelper.GetDateString(d);
        }
        public static string GetDateString(DateTime d)
        {
            if (null == d)
                return string.Empty;
            TimeSpan ts = DateTime.Now.Subtract(d);
            return DateTimeHelper.GetDateString(ts, d);
        }
        public static string GetDateString(string datestring)
        {
            if (string.IsNullOrEmpty(datestring))
                return string.Empty;
            DateTime dateTime = DateTime.ParseExact(datestring, "ddd, dd MMM yyyy HH:mm:ss zzz", DateTimeHelper.cInfo);
            TimeSpan ts = DateTime.Now.Subtract(dateTime);
            return DateTimeHelper.GetDateString(ts, dateTime);
        }
        private static string GetDateString(TimeSpan ts, DateTime d)
        {
            string result;
            if (ts.Days > 1)
            {
                result = string.Format(d.ToString("MM-dd"), new object[0]);
            }
            else
            {
                if (ts.Days == 1)
                {
                    result = string.Format("昨天{0}", d.ToString("HH:mm"));
                }
                else
                {
                    if (ts.Hours > 1)
                    {
                        result = string.Format("{0} 小时前", ts.Hours);
                    }
                    else
                    {
                        if (ts.Hours == 1)
                        {
                            result = "1 小时前";
                        }
                        else
                        {
                            if (ts.Minutes >= 1)
                            {
                                result = string.Format("{0} 分钟前", ts.Minutes.ToString("D"));
                            }
                            else
                            {
                                if (ts.Seconds > 5)
                                {
                                    result = string.Format("{0} 秒前", ts.Seconds);
                                }
                                else
                                {
                                    result = "5秒前";
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
