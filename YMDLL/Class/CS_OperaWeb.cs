using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using System.Data;
using System.Net;
using YMDLL.Interface;


namespace YMDLL.Class
{
    /// <summary>
    /// 操作网页类
    /// </summary>
    public class CS_OperaWeb : IOperaWeb
    {
        /// <summary>
        /// 当前日期时间
        /// </summary>
        private string m_ClientDateTime;
        /// <summary>
        /// 客户端IP
        /// </summary>
        private string m_ClientIPAddress;
        /// <summary>
        /// 客户端访问URL
        /// </summary>
        private string m_ClientAskURL;
        /// <summary>
        /// 客户端IPv4
        /// </summary>
        private string m_ClientIPv4;
        /// <summary>
        /// 客户端IPv6
        /// </summary>
        private string m_ClientIPv6;
        /// <summary>
        /// 当前日期时间
        /// </summary>
        public string ClientDateTime
        {
            get
            {
                return m_ClientDateTime;
            }
            set
            {
                m_ClientDateTime = value;
            }
        }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIPAddress
        {
            get
            {
                return m_ClientIPAddress;
            }
            set
            {
                m_ClientIPAddress = value;
            }
        }

        /// <summary>
        /// 客户端访问URL
        /// </summary>
        public string ClientAskURL
        {
            get
            {
                return m_ClientAskURL;
            }
            set
            {
                m_ClientAskURL = value;
            }
        }

        /// <summary>
        /// 客户端IPv6
        /// </summary>
        public string ClientIPv6
        {
            get
            {
                return m_ClientIPv6;
            }
            set
            {
                m_ClientIPv6 = value;
            }
        }

        /// <summary>
        /// 客户端IPv4
        /// </summary>
        public string ClientIPv4
        {
            get
            {
                return m_ClientIPv4;
            }
            set
            {
                m_ClientIPv4 = value;
            }
        }
        /// <summary>
        /// 获取客户端日期时间
        /// </summary>
        public DateTime GetClientDateTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        public string GetClientIPAddress()
        {
            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            //这样,如果没有安装IPV6协议,可以取得IP地址.  但是如果安装了IPV6,就取得的是IPV6的IP地址.
            m_ClientIPAddress = IpEntry.AddressList[0].ToString();
            if (m_ClientIPAddress == "")
            {
                //这样就在IPV6的情况下取得IPV4的IP地址.
                //但是,如果本机有很多块网卡, 如何得到IpEntry.AddressList[多少]才是本机的局网IPV4地址呢?
                m_ClientIPAddress = IpEntry.AddressList[1].ToString();
            }
            return m_ClientIPAddress;
        }

        /// <summary>
        /// 获取客户端当前访问的URL
        /// </summary>
        public string GetClientCurrentAskUrl()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 获取客户端IPv6
        /// </summary>
        public string GetClientIPv6()
        {
            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            //这样,如果没有安装IPV6协议,可以取得IP地址.  但是如果安装了IPV6,就取得的是IPV6的IP地址.
            m_ClientIPv6 = IpEntry.AddressList[0].ToString();
            return m_ClientIPv6;
        }

        /// <summary>
        /// 获取客户端IPv4
        /// </summary>
        public string GetClientIPv4()
        {
            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            //这样,如果没有安装IPV6协议,可以取得IP地址.  但是如果安装了IPV6,就取得的是IPV6的IP地址.
            m_ClientIPv4 = IpEntry.AddressList[1].ToString();
            return m_ClientIPv4;
        }

        /// <summary>
        /// 获取客户端用户名
        /// </summary>
        public string GetClientUseName()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 打开指定网页
        /// </summary>
        /// <param name="m_URL">网页地址</param>
        public bool OpenURL(string m_URL)
        {
            System.Diagnostics.Process.Start(m_URL);
            return true;
        }

        /// <summary>
        /// 模拟提交数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public string HttpPostData(string url, string param)
        {
            var result = string.Empty;
            //注意提交的编码 这边是需要改变的 这边默认的是Default：系统当前编码
            byte[] postData = Encoding.UTF8.GetBytes(param);

            // 设置提交的相关参数 
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            Encoding myEncoding = Encoding.UTF8;
            request.Method = "POST";
            request.KeepAlive = false;
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR  3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
            request.ContentLength = postData.Length;

            // 提交请求数据 
            System.IO.Stream outputStream = request.GetRequestStream();
            outputStream.Write(postData, 0, postData.Length);
            outputStream.Close();

            HttpWebResponse response;
            Stream responseStream;
            StreamReader reader;
            string srcString;
            response = request.GetResponse() as HttpWebResponse;
            responseStream = response.GetResponseStream();
            reader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
            srcString = reader.ReadToEnd();
            result = srcString;   //返回值赋值
            reader.Close();

            return result;
        }

        /// <summary>
        /// 获取不加密网页源码
        /// </summary>
        /// <param name="m_URL">网址</param>
        /// <param name="m_EncodeType">编码类型</param>
        public string GetWebCode(string m_URL, Encoding m_EncodeType)
        {
            //指定请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_URL);
            //得到返回
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //得到流
            Stream recStream = response.GetResponseStream();
            //编码方式
            //Encoding gb2312 = Encoding.GetEncoding("gb2312");
            //指定转换为gb2312编码
            StreamReader sr = new StreamReader(recStream, m_EncodeType);
            //以字符串方式得到网页内容
            String content = sr.ReadToEnd();
            //将网页内容显示在TextBox中
            return content;
        }

        /// <summary>
        /// 获取客户端网页代码
        /// </summary>
        public string GetWebClient()
        {
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                //Byte[] pageData = MyWebClient.DownloadData("http://www.163.com"); //从指定网站下载数据
                Byte[] pageData = MyWebClient.DownloadData("http://test.zh.com/housedeal/interfaceDealTransfers?contract_code=NF1610649"); //从指定网站下载数据
                //string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句            

                string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句
                //Console.WriteLine(pageHtml);//在控制台输入获取的内容
                //using (StreamWriter sw = new StreamWriter("c:\\test\\ouput.html"))//将获取的内容写入文本
                //{
                //    sw.Write(pageHtml);
                //}
                //Console.ReadLine(); //让控制台暂停,否则一闪而过了       
                return pageHtml;
            }
            catch (WebException webEx)
            {
                //Console.WriteLine(webEx.Message.ToString());
                return webEx.Message.ToString();
            }
        }

        ///// <summary>
        ///// 获取指定的内容
        ///// </summary>
        //private string querystring(string m_qs)
        //{
        //    String pageurl;
        //    string m_Result = "";
        //    string m_Temp;
        //    try
        //    {
        //        pageurl = Request.Url.ToString();
        //        if (pageurl.IndexOf("?") != -1)
        //        {
        //            m_Temp = pageurl.Replace("?", "?&");
        //            string[] saurl = m_Temp.Split('&');
        //            for (int i = 1; i < saurl.Count(); i++)
        //            {
        //                if (saurl[i].IndexOf(m_qs + "=") != -1)
        //                {
        //                    m_Result = saurl[i].Replace(m_qs + "=", "");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        m_Result = ex.Message;
        //    }
        //    return m_Result;
        //}
    }
}
