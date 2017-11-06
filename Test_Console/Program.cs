using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            //获取北京快乐8的数据
            CS_OperaWeb ow = new CS_OperaWeb();
            //获取北京快乐8的数据
            //string strResult = ow.GetWebCode("http://www.bwlc.net/bulletin/keno.html", Encoding.UTF8);
            //获取国外彩种数据
            //string strResult = ow.GetWebCode("http://lotto.bclc.com/winning-numbers/keno-and-keno-bonus.html", Encoding.UTF8);


            WebClient myWebClient = new WebClient();
            byte[] myDataBuffer = myWebClient.DownloadData("http://lotto.bclc.com/winning-numbers/keno-and-keno-bonus.html");
            string strResult = Encoding.UTF8.GetString(myDataBuffer);
            //SecretHelper sh = new SecretHelper();
            //strResult = sh.EncryptToSHA1("0791zh");
            Console.WriteLine(strResult);
            Console.Read();
        }
    }
}
