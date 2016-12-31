using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

/// <summary>
/// 导数据到商学院的数据库中
/// </summary>
namespace PilotData_ToOBS
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            WriteLog("\n开始监听...");
            import import = new import();
            //读取配置文件信息
            import.ExecCommand((TableType)Enum.Parse(typeof(TableType), ReadAppSetting("EnumType"), false));
            WriteLog(import.strResult);
            watch.Stop();
            WriteLog("\n-----------------------\n导入完毕，监听结束\n总运行时间为:" + watch.Elapsed.ToString() + "秒.");
            Console.Read();
        }

        /// <summary>
        /// 读取设置
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string ReadAppSetting(string strKey)
        {
            return ConfigurationSettings.AppSettings[strKey].ToString();
        }

        /// <summary>
        /// 输出日志信息
        /// </summary>
        /// <param name="m_Content"></param>
        public static void WriteLog(string m_Content)
        {
            Console.WriteLine(m_Content);
        }
    }
}
