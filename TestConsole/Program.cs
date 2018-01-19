using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ML.ThirdParty;
using ML.ThirdParty.Wechat;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            QryBankCardBy4Element();
            Console.ReadLine();
        }

        /// <summary>
        /// 阿里四要素验证
        /// </summary>
        public static void QryBankCardBy4Element()
        {
            BankCardBy4ElementRequestParam bcberp = new BankCardBy4ElementRequestParam();
            bcberp.appcode = "412d4d16760245bb9d1d269f69f1bdcc";
            bcberp.accountNo = "6214837910347495";
            bcberp.bankPreMobile = "15879052605";
            bcberp.idCardCode = "140581198810231111";
            bcberp.name = "赵铭哲";
            string strResult = AliService.QryBankCardBy4Element(bcberp);
            Console.WriteLine(strResult);
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
