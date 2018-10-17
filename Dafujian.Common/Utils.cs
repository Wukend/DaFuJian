using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dafujian.Common
{
    public class Utils
    {


        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        public static Guid GetGuid()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// 根据今天或昨天返回当前时间的天数
        /// </summary>
        /// <param name="dayString"></param>
        /// <returns></returns>
        public static int GetDayByString(string dayString)
        {
            var date = DateTime.Now;
            if (dayString.IndexOf("昨天") >= 0)
                return date.AddDays(-1).Day;
            else if (dayString.IndexOf("今天") >= 0)
                return date.Day;
            else if (dayString.IndexOf("明天") >= 0)
                return date.AddDays(1).Day;
            else if (dayString.IndexOf("后天") >= 0)
                return date.AddDays(2).Day;
            else if (dayString.IndexOf("号") >= 0)
                return TypeParse.StrToInt(dayString.Replace("号", ""), date.Day);
            return date.Day;
        }

        /// <summary>
        /// 根据过去和现在的两个DateTime时间差计算出评论大概时间
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string GetTimeSpan(DateTime dt)
        {
            TimeSpan ts = DateTime.Now - dt;
            if (Math.Floor(ts.TotalDays) > 365)
            {
                return (Math.Floor(ts.TotalDays) / 365).ToString("0") + "年前";
            }
            else if (Math.Floor(ts.TotalDays) > 30)
            {
                return (Math.Floor(ts.TotalDays) / 30).ToString("0") + "月前";
            }
            else if (Math.Floor(ts.TotalDays) > 1)
            {
                return Math.Floor(ts.TotalDays).ToString("0") + "天前";
            }
            else if (Math.Floor(ts.TotalHours) > 1)
            {
                return Math.Floor(ts.TotalHours).ToString("0") + "小时前";
            }
            else if (Math.Floor(ts.TotalMinutes) > 1)
            {
                return Math.Floor(ts.TotalMinutes).ToString("0") + "分钟前";
            }
            else
            {
                return Math.Floor(ts.TotalSeconds).ToString("0") + "秒前";
            }

        }

        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="length"></param>
        /// <param name="constant"></param>
        /// <returns></returns>
        public static string GenerateRandomNumber(int length, char[] constant)
        {
            if (constant == null)
                constant = new char[]
            {
                '0','1','2','3','4','5','6','7','8','9',
                'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
            };
            var newRandom = new StringBuilder(constant.Length);
            var rd = new Random();
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(constant[rd.Next(constant.Length)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        ///  防SQL注入，过掉非法字符
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string PotString(string strText)
        {
            if (!string.IsNullOrEmpty(strText))
            {
                return strText.Replace("'", "").Trim().TrimEnd();
            }
            return "";
        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            str = PotString(str);
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }


                if (startIndex > str.Length)
                {
                    return "";
                }


            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            return CutString(str, startIndex, str.Length);
        }

        /// <summary>
        /// 密码哈希加密
        /// </summary>
        /// <param name="password">原始密码</param>
        /// <param name="salt">盐值</param>
        /// <returns></returns>
        public static string PasswordHashing(string password, string salt)
        {
            byte[] passwordAndSaltBytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hashBytes = new SHA256Managed().ComputeHash(passwordAndSaltBytes);
            string hashString = Convert.ToBase64String(hashBytes);
            return hashString;
        }

        #region 读取或写入cookie
        ///
        /// 写cookie值
        ///
        /// 名称
        /// 值
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = HttpUtility.UrlEncode(strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        ///
        /// 写cookie值
        ///
        /// 名称
        /// 值
        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = HttpUtility.UrlEncode(strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        ///
        /// 写cookie值
        ///
        /// 名称
        /// 值
        public static void WriteCookie(string strName, string key, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = HttpUtility.UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        ///
        /// 写cookie值
        ///
        /// 名称
        /// 值
        /// 过期时间(分钟)
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = HttpUtility.UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        ///
        /// 读cookie值
        ///
        /// 名称
        /// cookie值
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
                return HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[strName].Value.ToString());
            return "";
        }

        ///
        /// 读cookie值
        ///
        /// 名称
        /// cookie值
        public static string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null && HttpContext.Current.Request.Cookies[strName][key] != null)
                return HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[strName][key].ToString());

            return "";
        }
        #endregion
    }
}
