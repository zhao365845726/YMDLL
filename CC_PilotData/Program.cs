using System;
using System.Text;
using YMDLL.Class;

namespace CC_PilotData
{
    class Program
    {
        static void Main(string[] args)
        {
            CS_OperaWeb ow = new CS_OperaWeb();
            string strResult = ow.GetWebCode("https://www.caipiaokong.com/chart/bjklb/21.html", Encoding.UTF8);




            Console.WriteLine("Hello World!");
        }
    }
}