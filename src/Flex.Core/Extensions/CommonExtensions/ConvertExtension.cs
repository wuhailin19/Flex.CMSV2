using Flex.Core.Helper;

namespace Flex.Core.Extensions
{
    public static class ConvertExtension
    {
       
        public static bool ToBool(this string @this)
        {
            try
            {
                return Convert.ToBoolean(@this);
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// 默认时间转字符串格式 2020-01-01
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string ToDefaultDateTimeStr(this DateTime c)
        {
            return c.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 默认时间转字符串格式 2020-01-01 00:00:00
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string ToDefaultDateTimeLongStr(this DateTime c)
        {
            return c.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// EqualsIgnoreCase
        /// </summary>
        /// <param name="s1">string1</param>
        /// <param name="s2">string2</param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string s1, string s2)
            => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
        public static DateTime ToDateTime(this string c)
        {
            return ConvertHelper.StrToDateTime(c);
        }
    }
}
