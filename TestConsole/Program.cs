using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ML.ThirdParty.Wechat;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            WechatPayHelper wph = new WechatPayHelper();
            WechatPayParam wpp = new WechatPayParam();
            wpp.openid = "onQpExOXpRJMd8FJdHKTbCa91BW0";
            wpp.total_fee = "1";
            wpp.notify_url = "http://ml.pay.ymstudio.xyz:90/api/wechat/paynotice";
            string strResult = wph.H5Pay(wpp);
            Console.WriteLine(strResult);
            Console.ReadLine();
        }
    }
}
