using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YMDLL
{
    public class YM_WebAutoLogin
    {
        #region 属性
        /// <summary>
        /// 登陆后返回的Html
        /// </summary>
        public static string ResultHtml
        {
            get;
            set;
        }
        /// <summary>
        /// 下一次请求的Url
        /// </summary>
        public static string NextRequestUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 若要从远程调用中获取COOKIE一定要为request设定一个CookieContainer用来装载返回的cookies
        /// </summary>
        public static CookieContainer CookieContainer
        {
            get;
            set;
        }
        /// <summary>
        /// Cookies 字符创
        /// </summary>
        public static string CookiesString
        {
            get;
            set;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 用户登陆指定的网站
        /// </summary>
        /// <param name="loginUrl"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public static void PostLogin(string loginUrl, string account, string password)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                //模拟请求数据，数据样式可以用FireBug插件得到。
                //string postdata = "email=" + account + "&password=" + password + "&origURL=" + "http://www.renren.com/home" + "&domain=renren.com";
                //string postdata = "https://mylogin.51job.com/41963504570322063434/my/My_Pmc.php?login_verify=&url=&username=zhao365845726&userpwd=mz19881023&x=56&y=20";
                string postdata = loginUrl + "?login_verify=&url=&username=" + account + "&userpwd=" + password + "&x=56&y=20";
                // string LoginUrl = "http://www.renren.com/PLogin.do";
                request = (HttpWebRequest)WebRequest.Create(loginUrl);//实例化web访问类  
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "POST";//数据提交方式为POST  
                request.ContentType = "application/x-www-form-urlencoded";    //模拟头  
                request.AllowAutoRedirect = false;   // 不用需自动跳转
                //Uri uri = new Uri(request.Address.ToString());

                //必须设置CookieContainer存储请求返回的Cookies
                if (CookieContainer != null)
                {
                    request.CookieContainer = CookieContainer;
                }
                else
                {
                    request.CookieContainer = new CookieContainer();
                    CookieContainer = request.CookieContainer;
                }
                request.KeepAlive = true;
                //提交请求  
                byte[] postdatabytes = Encoding.UTF8.GetBytes(postdata);
                request.ContentLength = postdatabytes.Length;
                Stream stream;
                stream = request.GetRequestStream();
                //设置POST 数据
                stream.Write(postdatabytes, 0, postdatabytes.Length);
                stream.Close();
                //接收响应  
                response = (HttpWebResponse)request.GetResponse();
                NextRequestUrl = response.GetResponseHeader("Location");
                //string[] strKeys = response.Headers.AllKeys;
                //NextRequestUrl = strKeys[3];
                //保存返回cookie  
                response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
                CookieCollection cook = response.Cookies;
                string strcrook = request.CookieContainer.GetCookieHeader(request.RequestUri);
                CookiesString = strcrook;
                
                //取下一次GET跳转地址  
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string content = sr.ReadToEnd();
                sr.Close();
                request.Abort();
                response.Close();
                //依据登陆成功后返回的Page信息，求出下次请求的url
                //每个网站登陆后加载的Url和顺序不尽相同，以下两步需根据实际情况做特殊处理，从而得到下次请求的URL
                //string[] substr = content.Split(new char[] { '"' });
                //NextRequestUrl = substr[1];
            }
            catch (WebException ex)
            {
                MessageBox.Show(string.Format("登陆时出错，详细信息：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 获取用户登陆后下一次请求返回的内容
        /// </summary>
        public static void GetPage()
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(NextRequestUrl);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "GET";
                request.KeepAlive = true;
                request.Headers.Add("Cookie:" + CookiesString);
                request.CookieContainer = CookieContainer;
                request.AllowAutoRedirect = false;
                response = (HttpWebResponse)request.GetResponse();
                //设置cookie  
                CookiesString = request.CookieContainer.GetCookieHeader(request.RequestUri);
                //取再次跳转链接  
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));
                string ss = sr.ReadToEnd();
                sr.Close();
                request.Abort();
                response.Close();
                //依据登陆成功后返回的Page信息，求出下次请求的url
                //每个网站登陆后加载的Url和顺序不尽相同，以下两步需根据实际情况做特殊处理，从而得到下次请求的URL
                string[] substr = ss.Split(new char[] { '"' });
                NextRequestUrl = substr[1];
                ResultHtml = ss;
            }
            catch (WebException ex)
            {
                MessageBox.Show(string.Format("获取页面HTML信息出错，详细信息：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 获取指定页面请求返回的内容
        /// </summary>
        public static string GetPage(string m_Url)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(m_Url);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "GET";
                request.KeepAlive = true;
                request.Headers.Add("Cookie:" + CookiesString);
                request.CookieContainer = CookieContainer;
                request.AllowAutoRedirect = false;
                response = (HttpWebResponse)request.GetResponse();
                //设置cookie  
                CookiesString = request.CookieContainer.GetCookieHeader(request.RequestUri);
                //取再次跳转链接  
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));
                string ss = sr.ReadToEnd();
                sr.Close();
                request.Abort();
                response.Close();
                return ss;
                //依据登陆成功后返回的Page信息，求出下次请求的url
                //每个网站登陆后加载的Url和顺序不尽相同，以下两步需根据实际情况做特殊处理，从而得到下次请求的URL
                //string[] substr = ss.Split(new char[] { '"' });
                //m_Url = substr[1];
                //ResultHtml = ss;
            }
            catch (WebException ex)
            {
                //MessageBox.Show(string.Format("获取页面HTML信息出错，详细信息：{0}", ex.Message));
                return ex.Message;
            }
        }
        #endregion

    }

}
