using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dafujian.Common
{
    public class Log
    {
        public static void WriteLogByError(string msg, Exception ex = null)
        {
            var fileName = "Error" + DateTime.Now.Year.ToString().Trim() + DateTime.Now.Month.ToString().Trim() + DateTime.Now.Day.ToString().Trim();
            WriteLog(fileName, msg, ex);
        }

        public static void WriteLog(string fileName, string msg, Exception ex = null)
        {
            if (string.IsNullOrEmpty(msg))
                return;

            var text = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + DateTime.Now.ToString("yyyy-MM-dd");
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "Log" + DateTime.Now.Year.ToString().Trim() + DateTime.Now.Month.ToString().Trim() + DateTime.Now.Day.ToString().Trim();
            }
            fileName += ".txt";

            var path = text + "\\" + fileName;
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(path, true))
                {
                    streamWriter.WriteLine("记录时间：" + DateTime.Now.ToString());
                    streamWriter.Write(msg);
                    if (ex != null)
                    {
                        streamWriter.WriteLine();
                        streamWriter.WriteLine();
                        streamWriter.Write("系统异常：" + ex.ToString());
                    }
                    streamWriter.WriteLine();
                    streamWriter.WriteLine("*****************************************************");
                    streamWriter.Close();
                }
                return;
            }
            catch
            {
                return;
            }
        }
    }
}
