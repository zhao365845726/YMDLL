using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ML.ThirdParty
{
    public class AliService
    {
        private const String host = "http://aliyuncardby4element.haoservice.com";
        private const String path = "/creditop/BankCardQuery/QryBankCardBy4Element";
        private const String method = "GET";
        private static String appcode = "";

        /// <summary>
        /// 阿里四要素验证
        /// </summary>
        public static string QryBankCardBy4Element(BankCardBy4ElementRequestParam bcberp)
        {
            string strResult = string.Empty;
            appcode = bcberp.appcode;
            String querys = "accountNo=" + bcberp.accountNo + "&bankPreMobile=" + bcberp.bankPreMobile + "&idCardCode=" + bcberp.idCardCode + "&name=" + bcberp.name;
            String bodys = "";
            String url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (host.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpRequest.Method = method;
            httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            //Console.WriteLine(httpResponse.StatusCode);
            //Console.WriteLine(httpResponse.Method);
            //Console.WriteLine(httpResponse.Headers);
            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            strResult = reader.ReadToEnd();
            return strResult;
            //Console.WriteLine(reader.ReadToEnd());
            //Console.WriteLine("\n");
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }

    /// <summary>
    /// 四要素请求参数
    /// </summary>
    public class BankCardBy4ElementRequestParam
    {
        /// <summary>
        /// appCode
        /// </summary>
        public string appcode { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        public string accountNo { get; set; }

        /// <summary>
        /// 银行预留的手机号
        /// </summary>
        public string bankPreMobile { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string idCardCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
    }
}
