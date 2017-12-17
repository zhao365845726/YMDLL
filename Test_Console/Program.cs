using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using YMDLL.Class;
using YMDLL.Common;

namespace Test_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            CS_OperaWeb ow = new CS_OperaWeb();
            //string strResult = ow.GetWebCode("http://www.qixin.com/company/534472fd-7d53-4958-8132-d6a6242423d8", Encoding.UTF8);
            //新浪微博
            //string strResult = ow.GetWebCode("http://weibo.com/login.php", Encoding.UTF8);
            //腾讯微博
            string strResult = ow.GetWebCode("http://t.qq.com/joechenchiaoen/", Encoding.UTF8);
            //公众号文章
            //string strResult = ow.GetWebCode("http://mp.weixin.qq.com/mp/homepage?__biz=MjM5NTE4Njc4NQ==&hid=11&sn=91a76beee4c038af4b10ca335a0ad6c5#wechat_redirect", Encoding.UTF8);
            //SecretHelper sh = new SecretHelper();
            //strResult = sh.EncryptToSHA1("0791zh");

            CS_InterceptWebInfo iw = new CS_InterceptWebInfo();
            //string strTemp = iw.InterceptSpecificedInfo(strResult, "company-name-now", "\"conpany-history-name\"");
            //string strTemp = iw.InterceptSpecificedInfo(strResult, "今天凌晨", "被震醒！");
            string strTemp = "";
            for(int i = 0; i < 1000; i++)
            {
                strTemp += "这是第" + i.ToString() + "次抓取" + iw.InterceptSpecificedInfo(strResult, "我沒有要錄節目", "麻煩幫我點播一") + "\n";
            }
            
            //string strTemp = iw.InterceptSpecificedInfo(strResult, "微信扫一扫", "摩拜率先接入小程序新能力");

            Console.WriteLine(strTemp);


            Console.WriteLine(SaveAsWebImg("http://img5.imgtn.bdimg.com/it/u=2447714267,1037621802&fm=23&gp=0.jpg"));
            Console.Read();


        }


        //public void DownloadFile()
        //{
        //    try
        //    {
        //        string FullFileName = Server.MapPath(@"/images/imgname.jpg"); //FileName--要下载的文件名 
        //        System.IO.FileInfo DownloadFile = new System.IO.FileInfo(FullFileName);
        //        if (DownloadFile.Exists)
        //        {
        //            Response.Clear();
        //            Response.ClearHeaders();
        //            Response.Buffer = false;
        //            Response.ContentType = "application/octet-stream";
        //            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFile.Name, System.Text.Encoding.ASCII));
        //            Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
        //            Response.WriteFile(DownloadFile.FullName);
        //            Response.Flush();
        //            Response.End();
        //        }
        //        else
        //        {
        //            //文件不存在 
        //        }
        //    }
        //    catch
        //    {
        //        //文件不存在
        //    }
        //}

        /// <summary>  
        /// 下载网站图片  
        /// </summary>  
        /// <param name="picUrl"></param>  
        /// <returns></returns>  
        public static string SaveAsWebImg(string picUrl)
        {
            string result = "";
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"/File/";  //目录  
            try
            {
                if (!String.IsNullOrEmpty(picUrl))
                {
                    Random rd = new Random();
                    DateTime nowTime = DateTime.Now;
                    string fileName = nowTime.Month.ToString() + nowTime.Day.ToString() + nowTime.Hour.ToString() + nowTime.Minute.ToString() + nowTime.Second.ToString() + rd.Next(1000, 1000000) + ".jpeg";
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(picUrl, path + fileName);
                    result = fileName;
                }
            }
            catch { }
            return result;
        }
    }
}
