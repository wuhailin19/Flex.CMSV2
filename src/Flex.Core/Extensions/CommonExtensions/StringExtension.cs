﻿using Flex.Core.Config;
using Flex.Core.Framework.Enum;
using Flex.Core.Helper;
using Flex.Core.Timing;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace Flex.Core.Extensions
{
    ///<summary>
    /// 字符串通用扩展类
    ///</summary>
    public static class StringExtension
    {
        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        /// <summary>
        /// 获取包含参数的个数
        /// </summary>
        /// <param name="content"></param>
        /// <param name="startChar"></param>
        /// <returns></returns>
        public static int GetStartCount(this string content, char startChar)
        {
            if (content == null)
            {
                return 0;
            }
            var count = 0;

            foreach (var theChar in content)
            {
                if (theChar == startChar)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }
        public static string GetCurrentBaseField(this string filed) {
            if (DataBaseConfig.dataBase != DataBaseType.PgSql)
                return filed;
            var list = filed.ToList();
            string filedstr = string.Empty;
            foreach (var item in list)
            {
                if (filedstr.IsNullOrEmpty())
                    filedstr = $@"{item} as ""{item}""";
                else
                    filedstr += $@",{item} as ""{item}""";
            }
            return filedstr;
        }
        public static DateTime ToUtcTime(this string timeString) {
            DateTime localTime;
            if (DateTime.TryParseExact(timeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out localTime))
            {
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                DateTime utcTime = DateTime.SpecifyKind(localTime, DateTimeKind.Utc);
                DateTime resTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
                return resTime;
            }
            else
            {
                return localTime;
            }
        }
        public static List<string> ToList(this string strs, string splitstr = ",")
        {
            if (string.IsNullOrEmpty(strs))
                return new List<string>();
            string[] ids = strs.Split(new string[] { splitstr }, StringSplitOptions.RemoveEmptyEntries);
            return ids.ToList();
        }
        public static List<T> ToList<T>(this string strs, string splitstr = ",")
        {
            if (string.IsNullOrEmpty(strs))
                return new List<T>();
            string[] ids = strs.Split(new string[] { splitstr }, StringSplitOptions.RemoveEmptyEntries);
            List<T> list =  new List<T>();
            foreach (var item in ids)
            {
                list.Add(item.CastTo<T>());
            }
            return list;
        }
        public static string ListToString(this List<object> strs, string splitstr = ",")
        {
            string result = string.Empty;
            foreach (var item in strs)
            {
                if (result.IsNullOrEmpty())
                    result += item.ToString();
                else
                    result += splitstr + item.ToString();
            }
            return result;
        }
        /// <summary>
        /// 倒置字符串，输入"abcd123"，返回"321dcba"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringReverse(this string str)
        {
            char[] input = str.ToCharArray();
            var output = new char[str.Length];
            for (int i = 0; i < input.Length; i++)
                output[input.Length - 1 - i] = input[i];
            return new string(output);
        }
        public static int ToInt(this object str)
        {
            try
            {
                if (str != null)
                    return Convert.ToInt32(str);
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }
        public static int ToIntAndThrowException(this object str)
        {
            try
            {
                if (str != null)
                    return Convert.ToInt32(str);
                else
                    return 0;
            }
            catch
            {
                throw;
            }
        }
        public static long ToLong(this object str)
        {
            try
            {
                if (str != null)
                    return Convert.ToInt64(str);
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 判断是否不为空
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 字符串格式化
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="arg0">参数0</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatWith(this string str, object arg0)
        {
            return string.Format(str, arg0);
        }

        /// <summary>
        /// 字符串格式化
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="arg0">参数0</param>
        /// <param name="arg1">参数1</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatWith(this string str, object arg0, object arg1)
        {
            return string.Format(str, arg0, arg1);
        }

        /// <summary>
        /// 字符串格式化
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="arg0">参数0</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatWith(this string str, object arg0, object arg1, object arg2)
        {
            return string.Format(str, arg0, arg1, arg2);
        }

        /// <summary>
        /// 字符串格式化
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args">参数集</param>
        /// <returns></returns>
        public static string FormatWith(this string str, params object[] args)
        {
            return string.Format(str, args);
        }

        /// <summary>
        /// 倒置字符串，输入"abcd123"，返回"321dcba"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Reverse(this string str)
        {
            char[] input = str.ToCharArray();
            var output = new char[str.Length];
            for (int i = 0; i < input.Length; i++)
                output[input.Length - 1 - i] = input[i];
            return new string(output);
        }

        /// <summary>
        /// 截断字符扩展
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="start">起始位置</param>
        /// <param name="len">长度</param>
        /// <param name="v">省略符</param>
        /// <returns></returns>
        public static string Sub(this string str, int start, int len, string v)
        {
            //(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
            var reg = "[\u4e00-\u9fa5]".ToRegex(RegexOptions.Compiled);
            var chars = str.ToCharArray();
            var result = string.Empty;
            int index = 0;
            foreach (char t in chars)
            {
                if (index >= start && index < (start + len))
                    result += t;
                else if (index >= (start + len))
                {
                    result += v;
                    break;
                }
                index += (reg.IsMatch(t.ToString(CultureInfo.InvariantCulture)) ? 2 : 1);
            }
            return result;
        }

        /// <summary>
        /// 截断字符扩展
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string Sub(this string str, int len, string v)
        {
            return str.Sub(0, len, v);
        }

        /// <summary>
        /// 截断字符扩展
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Sub(this string str, int len)
        {
            return str.Sub(0, len, "...");
        }

        /// <summary>
        /// 对传递的参数字符串进行处理，防止注入式攻击
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertSql(this string str)
        {
            str = str.Trim();
            str = str.Replace("'", "''");
            str = str.Replace(";--", "");
            str = str.Replace("=", "");
            str = str.Replace(" or ", "");
            str = str.Replace(" and ", "");
            return str;
        }

        /// <summary>
        /// 获取该值的MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md5(this string str)
        {
            return str.IsNullOrEmpty() ? str : EncryptHelper.Hash(str, EncryptHelper.HashFormat.MD532);
        }

        /// <summary> 读取配置文件 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T Config<T>(this string configName, T def = default(T))
        {
            return ConfigHelper.Instance.Get(def, supressKey: configName);
        }

        /// <summary> 小驼峰命名法 </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            if (!char.IsUpper(s[0]))
                return s;

            var chars = s.ToCharArray();

            for (var i = 0; i < chars.Length; i++)
            {
                var hasNext = (i + 1 < chars.Length);
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                    break;
                chars[i] = char.ToLower(chars[i], CultureInfo.InvariantCulture);
            }

            return new string(chars);
        }

        /// <summary> url命名法 </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToUrlCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;
            var chars = s.ToCharArray();
            var str = new StringBuilder();
            for (var i = 0; i < chars.Length; i++)
            {
                if (char.IsUpper(chars[i]))
                {
                    if (i > 0 && !char.IsUpper(chars[i - 1]))
                        str.Append("_");
                    str.Append(char.ToLower(chars[i], CultureInfo.InvariantCulture));
                }
                else
                {
                    str.Append(chars[i]);
                }
            }
            return str.ToString();
        }

        public static string GetPath(this string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = AppDomain.CurrentDomain.BaseDirectory;
            else
            {
                if (Path.IsPathRooted(path))
                    return path;
                path = path.Replace("/", "\\");
                if (path.StartsWith("\\"))
                {
                    path = path.Substring(path.IndexOf('\\', 1)).TrimStart('\\');
                }
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
            return path;
        }
        /// <summary>
        /// Html编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// Html解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string UrlEncode(this string str, Encoding encoding)
        {
            return HttpUtility.UrlEncode(str, encoding);
        }

        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(this string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// Url解码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string UrlDecode(this string str, Encoding encoding)
        {
            return HttpUtility.UrlDecode(str, encoding);
        }

        /// <summary>
        /// Url解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlDecode(this string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        /// <summary> 获取该字符串的QueryString值 </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="str">字符串</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static T Query<T>(this string str, T def)
        {
            try
            {
                var c = AcbHttpContext.Current;
                var qs = c.Request.Query[str].ToString();
                return qs.CastTo(def);
            }
            catch
            {
                return def;
            }
        }

        /// <summary> 获取该字符串的Form值 </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="str">字符串</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static T Form<T>(this string str, T def)
        {
            try
            {
                var c = AcbHttpContext.Current;
                var qs = c.Request.Form[str].ToString();
                return qs.CastTo(def);
            }
            catch
            {
                return def;
            }
        }

        /// <summary> 获取该字符串QueryString或Form值 </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="str"></param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static T QueryOrForm<T>(this string str, T def)
        {
            try
            {
                var c = AcbHttpContext.Current;
                var qs = c.Request.Query.ContainsKey(str) ? c.Request.Query[str].ToString() : c.Request.Form[str].ToString();
                return qs.CastTo(def);
            }
            catch
            {
                return def;
            }
        }

        /// <summary> 设置参数 </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="url">url</param>
        /// <returns></returns>
        public static string SetQuery(this string key, object value, string url = null)
        {
            if (url.IsNullOrEmpty())
            {
                url = Utils.RawUrl();
            }
            if (string.IsNullOrWhiteSpace(url) || key.IsNullOrEmpty())
                return url;
            value = value ?? string.Empty;
            var qs = url.Split('?');
            var list = new NameValueCollection();
            if (qs.Length < 2)
            {
                list.Add(key, UrlEncode(value.ToString()));
            }
            else
            {
                foreach (var query in qs[1].Split('&'))
                {
                    var item = query.Split('=');
                    if (item.Length == 2)
                        list.Add(item[0], item[1]);
                }
                list[key] = UrlEncode(value.ToString());
            }
            var search = string.Empty;
            for (var i = 0; i < list.AllKeys.Length; i++)
            {
                search += list.AllKeys[i] + "=" + list[i];
                if (i < list.Count - 1)
                    search += "&";
            }
            return qs[0] + "?" + search;
        }
    }
}
