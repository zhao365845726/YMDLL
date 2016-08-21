using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Windows.Forms;
using YMDLL;
using YMDLL.Class.Web;

namespace Test_WindowsForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String strUrl = "https://mylogin.51job.com/46586496056978054010/my/My_Pmc.php";
            String strAccount = "xxxxxxxx";
            String strPassword = "*************";
            YM_WebAutoLogin.PostLogin(strUrl, strAccount, strPassword);
            YM_WebAutoLogin.GetPage();
            richTextBox1.Text = YM_WebAutoLogin.ResultHtml;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = GetHtml("https://mail.qq.com/");
        }

        //简单的直接获取网页源代码，不用Post数据
        public static string GetHtml(string URL)
        {
            WebRequest wrt;
            wrt = WebRequest.Create(URL);
            wrt.Credentials = CredentialCache.DefaultCredentials;
            WebResponse wrp;
            wrp = wrt.GetResponse();
            return new StreamReader(wrp.GetResponseStream(), Encoding.Default).ReadToEnd();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //提交的post数据   
            string postData = "";// = string.Format("account={0}&reaccount={0}&cardcode={1}&cardpassword={2}", txtLoginId.Text, txtCardNo.Text, txtPass.Text);
            postData += string.Format("&__EVENTTARGET={0}&__EVENTARGUMENT=&__VIEWSTATE={1}", "nextStep", "YToxOntzOjExOiJjdXJyZW50VXNlciI7YjowO30=");
            HttpWebResponse response;
            HttpWebRequest request;
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(postData);
            request = (HttpWebRequest)WebRequest.Create("url");
            data = encoding.GetBytes(postData);
            request.Method = "POST";
            request.ProtocolVersion = HttpVersion.Version10;
            request.UserAgent = "Mozilla/4.0";

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            string html = string.Empty;
            try
            {
                //获取服务器返回的资源   
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                richTextBox1.Text = html;
            }
            catch (Exception ex)
            {
                richTextBox1.Text = ex.Message;
            }

        }



        /// <summary>   
        /// 通过get方式请求页面，传递一个实例化的cookieContainer   
        /// </summary>   
        /// <param name="postUrl"></param>   
        /// <param name="cookie"></param>   
        /// <returns></returns>   
        public static ArrayList GetHtmlData(string postUrl, CookieContainer cookie)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            ArrayList list = new ArrayList();
            request = WebRequest.Create(postUrl) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = "Mozilla/4.0";
            request.CookieContainer = cookie;
            request.KeepAlive = true;

            request.CookieContainer = cookie;
            try
            {
                //获取服务器返回的资源   
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default))
                    {
                        cookie.Add(response.Cookies);
                        //保存Cookies   
                        list.Add(cookie);
                        list.Add(reader.ReadToEnd());
                        list.Add(Guid.NewGuid().ToString());//图片名   
                    }
                }
            }
            catch (WebException ex)
            {
                list.Clear();
                list.Add("发生异常/n/r");
                WebResponse wr = ex.Response;
                using (Stream st = wr.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(st, System.Text.Encoding.Default))
                    {
                        list.Add(sr.ReadToEnd());
                    }
                }
            }
            catch (Exception ex)
            {
                list.Clear();
                list.Add("5");
                list.Add("发生异常：" + ex.Message);
            }
            return list;
        }

        /// <summary>   
        /// 下载验证码图片并保存到本地   
        /// </summary>   
        /// <param name="Url">验证码URL</param>   
        /// <param name="cookCon">Cookies值</param>   
        /// <param name="savePath">保存位置/文件名</param>   
        public static bool DowloadCheckImg(string Url, CookieContainer cookCon, string savePath)
        {
            bool bol = true;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Url);
            //属性配置   
            webRequest.AllowWriteStreamBuffering = true;
            webRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
            webRequest.MaximumResponseHeadersLength = -1;
            webRequest.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322)";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "GET";
            webRequest.Headers.Add("Accept-Language", "zh-cn");
            webRequest.Headers.Add("Accept-Encoding", "gzip,deflate");
            webRequest.KeepAlive = true;
            webRequest.CookieContainer = cookCon;
            try
            {
                //获取服务器返回的资源   
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (Stream sream = webResponse.GetResponseStream())
                    {
                        List<byte> list = new List<byte>();
                        while (true)
                        {
                            int data = sream.ReadByte();
                            if (data == -1)
                                break;
                            list.Add((byte)data);
                        }
                        File.WriteAllBytes(savePath, list.ToArray());
                    }
                }
            }
            catch (WebException ex)
            {
                bol = false;
            }
            catch (Exception ex)
            {
                bol = false;
            }
            return bol;
        }


        /// <summary>   
        /// 发送相关数据至页面   
        /// 进行登录操作   
        /// 并保存cookie   
        /// </summary>   
        /// <param name="postData"></param>   
        /// <param name="postUrl"></param>   
        /// <param name="cookie"></param>   
        /// <returns></returns>   
        public static ArrayList PostData(string postData, string postUrl, CookieContainer cookie)
        {
            ArrayList list = new ArrayList();
            HttpWebRequest request;
            HttpWebResponse response;
            ASCIIEncoding encoding = new ASCIIEncoding();
            request = WebRequest.Create(postUrl) as HttpWebRequest;
            byte[] b = encoding.GetBytes(postData);
            request.UserAgent = "Mozilla/4.0";
            request.Method = "POST";
            request.CookieContainer = cookie;
            request.ContentLength = b.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(b, 0, b.Length);
            }

            try
            {
                //获取服务器返回的资源   
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        if (response.Cookies.Count > 0)
                            cookie.Add(response.Cookies);
                        list.Add(cookie);
                        list.Add(reader.ReadToEnd());
                    }
                }
            }
            catch (WebException wex)
            {
                WebResponse wr = wex.Response;
                using (Stream st = wr.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(st, System.Text.Encoding.Default))
                    {
                        list.Add(sr.ReadToEnd());
                    }
                }
            }
            catch (Exception ex)
            {
                list.Add("发生异常/n/r" + ex.Message);
            }
            return list;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string strCookie = "";
            //string strUrl = "https://mail.qq.com/cgi-bin/login?vt=passport&vm=wpt&ft=loginpage&target=&account=669874884&qqmailkey=7fb69b9dcaffe0896cb70c5893a9148089f8b29bf820f66e6e8e7b1a22202869";
            string strUrl = "https://mail.qq.com/cgi-bin/login?vt=passport&vm=wpt&ft=loginpage&target=&account=669874884&qqmailkey=f5083311625b2ced4e696c2b1ff07b5992d962cef2c64097aec1a424b6626f67";
            YM_Http.GetHtml(strUrl, out strCookie);
            CookieContainer cc = null;
            ArrayList alTemp = GetHtmlData(strUrl, cc);

            /*
             
             */
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<String> lStr = new List<string>();
            List<int> iValue = new List<int>();
            lStr.Add(richTextBox1.Text);
            String strTemp = "";
            iValue.Add(lStr[0].IndexOf("简历管理"));
            lStr.Add(lStr[0].Substring(0, iValue[0]));
            iValue.Add(lStr[1].LastIndexOf("href=")+6);
            lStr.Add(lStr[1].Substring(iValue[1], lStr[1].Length - iValue[1]));
            lStr.Add(lStr[2].Substring(0, lStr[2].IndexOf("\"")));
            richTextBox1.Text = lStr[3];
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //string cookie;
            string strHeader;
            //String strTemp = YM_Http.GetHtml(richTextBox1.Text.Trim(),out cookie);
            
            //String strTemp = YM_Http.GetHtml(richTextBox1.Text.Trim(), YM_WebAutoLogin.CookiesString,out strHeader);

            YM_WebAutoLogin.GetPage(richTextBox1.Text.Trim());
            richTextBox1.Text = YM_WebAutoLogin.ResultHtml;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            List<String> lStr = new List<string>();
            List<int> iValue = new List<int>();
            //获取简历管理中间的列表数据
            lStr.Add(richTextBox1.Text);
            iValue.Add(richTextBox1.Text.IndexOf("resumelist"));
            lStr.Add(lStr[0].Substring(iValue[0], lStr[0].Length - iValue[0]));
            iValue.Add(lStr[1].IndexOf("<table"));
            lStr.Add(lStr[1].Substring(iValue[1], lStr[1].Length - iValue[1]));
            iValue.Add(lStr[2].IndexOf("</table>",7)+8);
            lStr.Add(lStr[2].Substring(0, iValue[2]));
            //获取tbody的数据
            //iValue.Add(lStr[3].IndexOf("ReSumeID"));
            //lStr.Add(lStr[3].Substring(iValue[3], lStr[3].Length - iValue[3]));
            //lStr.Add(lStr[4].Substring(0, lStr[4].IndexOf("&")));
            iValue.Add(lStr[3].IndexOf("修改简历"));
            lStr.Add(lStr[3].Substring(0, iValue[3]));
            iValue.Add(lStr[4].LastIndexOf("href=")+6);
            lStr.Add(lStr[4].Substring(iValue[4], lStr[4].Length - iValue[4]));
            lStr.Add(lStr[5].Substring(0, lStr[5].IndexOf("\"")));

            richTextBox1.Text = lStr[6];
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //string strUrl = "http://my.51job.com/sc/applyjob/preview_resume.php?";
            //strUrl += richTextBox1.Text.Trim() + "&AccountID=53722312";
            YM_WebAutoLogin.GetPage(richTextBox1.Text.Trim());
            richTextBox1.Text = YM_WebAutoLogin.ResultHtml;
        }
    }
}
