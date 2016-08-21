using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PilotDataConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            WriteLog("\n开始监听...");
            import import = new import();
            //读取配置文件信息
            //import.ExecCommand((TableType)Enum.Parse(typeof(TableType), ReadAppSetting("EnumType"), false));
            import.ExecCommand(TableType.EMPLOYEE);
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
            //rtbConsole.ForeColor = Color.ForestGreen;
            //rtbConsole.Text = m_Content + "\n" + rtbConsole.Text;
            Console.WriteLine(m_Content);
        }
    }
}
