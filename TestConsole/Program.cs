using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ML.ThirdParty;
using ML.ThirdParty.Wechat;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            PayTest();
            Console.ReadLine();
        }

        /// <summary>
        /// 阿里云通讯测试
        /// </summary>
        public static void SendSMSTest()
        {
            AliCloudCommunication acc = new AliCloudCommunication();
            acc.AccessKeyId = ConfigurationSettings.AppSettings["AccessKeyId"].ToString();
            acc.AccessKeySecret = ConfigurationSettings.AppSettings["AccessKeySecret"].ToString();

            SendSMSParam ssp = new SendSMSParam();
            ssp.Mobile = "17803565206";
            ssp.SignName = ConfigurationSettings.AppSettings["SignName"].ToString();
            ssp.TemplateCode = ConfigurationSettings.AppSettings["TemplateCode_ForgetPwd"].ToString();

            string strRandNumber = string.Empty;
            string strSendResult = acc.SendVerificationCode(ssp, out strRandNumber);
            Console.WriteLine("发送结果：" + strSendResult + ",验证码：" + strRandNumber);
        }

        public static void PayTest()
        {
            WechatPayHelper wph = new WechatPayHelper();
            WechatPayParam wpp = new WechatPayParam();
            wpp.openid = "onQpExOXpRJMd8FJdHKTbCa91BW0";
            wpp.total_fee = "1";
            wpp.notify_url = "http://ml.pay.ymstudio.xyz:90/api/wechat/paynotice";
            string strResult = wph.H5Pay(wpp);
            Console.WriteLine(strResult);
        }
    }
}
