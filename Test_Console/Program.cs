using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMDLL.Class;
using YMDLL.Common;

namespace Test_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //CS_OperaWeb ow = new CS_OperaWeb();
            //string strResult = ow.GetWebClient();

            //SecretHelper sh = new SecretHelper();
            //strResult = sh.EncryptToSHA1("0791zh");
            //Console.WriteLine(strResult);
            //Console.Read();

            CS_CalcDateTime cdt = new CS_CalcDateTime();
            Console.WriteLine(cdt.GetTimeStamp());
            Console.Read();
        }
    }
}
