using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMDLL.Class;

namespace Test_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            CS_OperaWeb ow = new CS_OperaWeb();
            string strResult = ow.GetWebClient();
            Console.WriteLine(strResult);
            Console.Read();
        }
    }
}
