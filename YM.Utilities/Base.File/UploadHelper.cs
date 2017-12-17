//=====================================================================================
// All Rights Reserved , Copyright © YMStudio 2016
//=====================================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;
using System.Web;
using System.Net;

namespace ML.Utilities
{
    /// <summary>
    /// 文件上传帮助类
    /// </summary>
    public class UploadHelper
    {
        /// <summary>
        /// 附件上传 成功：succeed、失败：error、文件太大：-1、
        /// </summary>
        /// <param name="file">单独文件的访问</param>
        /// <param name="path">存储路径</param>
        /// <param name="filename">输出文件名</param>
        /// <returns></returns>
        public static string FileUpload(HttpPostedFileBase file, string path, string FileName)
        {
            if (Directory.Exists(path) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(path);
            }
            //取得文件的扩展名,并转换成小写
            string Extension = System.IO.Path.GetExtension(file.FileName).ToLower();
            //取得文件大小
            string filesize = SizeHelper.CountSize(file.ContentLength);
            try
            {
                int Size = file.ContentLength / 1024 / 1024;
                if (Size > 10)
                {
                    return "-1";
                }
                else
                {
                    file.SaveAs(path + FileName);
                    return "succeed";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary> 
        /// 上传图片文件 
        /// </summary> 
        /// <param name="url">提交的地址</param> 
        /// <param name="poststr">发送的文本串   比如：user=eking&pass=123456  </param> 
        /// <param name="fileformname">文本域的名称  比如：name="file"，那么fileformname=file  </param> 
        /// <param name="filepath">上传的文件路径  比如： c:\12.jpg </param> 
        /// <param name="cookie">cookie数据</param> 
        /// <param name="refre">头部的跳转地址</param> 
        /// <returns></returns> 
        public string HttpUploadFile(string url, string poststr, string fileformname, string filepath, string cookie, string refre)
        {
            // 这个可以是改变的，也可以是下面这个固定的字符串 
            string boundary = "—————————7d930d1a850658";
            // 创建request对象 
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webrequest.Method = "POST";
            webrequest.Headers.Add("Cookie: " + cookie);
            webrequest.Referer = refre;
            // 构造发送数据
            StringBuilder sb = new StringBuilder();
            // 文本域的数据，将user=eking&pass=123456  格式的文本域拆分 ，然后构造 
            foreach (string c in poststr.Split('&'))
            {
                string[] item = c.Split('=');
                if (item.Length != 2)
                {
                    break;
                }
                string name = item[0];
                string value = item[1];
                sb.Append("–" + boundary);
                sb.Append("\r\n");
                sb.Append("Content-Disposition: form-data; name=\"" + name + "\"");
                sb.Append("\r\n\r\n");
                sb.Append(value);
                sb.Append("\r\n");
            }
            // 文件域的数据
            sb.Append("–" + boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"icon\";filename=\"" + filepath + "\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append("image/pjpeg");
            sb.Append("\r\n\r\n");
            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);
            //构造尾部数据 
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n–" + boundary + "–\r\n");
            FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            long length = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length;
            webrequest.ContentLength = length;
            Stream requestStream = webrequest.GetRequestStream();
            // 输入头部数据 
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            // 输入文件流数据 
            byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);
            // 输入尾部数据 
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            // 返回数据流(源码) 
            return sr.ReadToEnd();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string FileUpLoad(HttpPostedFile file)
        {
            string fileName, fileExtension;//文件名，文件类型
            fileName = System.IO.Path.GetFileName(file.FileName);
            fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
            int FileLen = file.ContentLength;
            Byte[] FileData = new Byte[FileLen];
            Stream sr = file.InputStream;//创建数据流对象 
            sr.Read(FileData, 0, FileLen);
            sr.Close();
            return HeadPhotoTemp(FileData, "E:\\", ".txt");
        }

        /// <summary>
        /// 头像处理
        /// </summary>
        /// <param name="FileByteArray"></param>
        /// <param name="savpath"></param>
        /// <param name="houzui"></param>
        /// <returns></returns>
        public string HeadPhotoTemp(Byte[] FileByteArray, string savpath, string houzui)
        {
            string filepath = System.Configuration.ConfigurationManager.AppSettings["filepath"].ToString();
            string str = "";
            try
            {
                string path = filepath + savpath;
                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(path));
                }
                string name = System.DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + DateTime.Now.Ticks + houzui;// ".jpg";
                string savepath = System.Web.HttpContext.Current.Request.MapPath(path) + "/" + name;
                FileStream fs = new FileStream(savepath, FileMode.OpenOrCreate);
                fs.Write(FileByteArray, 0, FileByteArray.Length);
                fs.Close();
                str = path + "/" + name;
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="strFileType"></param>
        /// <param name="iFileLength"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Path"></param>
        /// <param name="strInfo"></param>
        /// <returns></returns>
        public bool UploadFTP(HttpPostedFile file, string strFileType, int iFileLength, int Width, int Height, string Path, ref string strInfo)
        {
            HttpPostedFileBase hpfb = new HttpPostedFileWrapper(file) as HttpPostedFileBase;
            return UploadFTP(hpfb, strFileType, iFileLength, Width, Height, Path, ref strInfo);
        }

        private bool UploadFTP(HttpPostedFileBase hpfb, string strFileType, int iFileLength, int width, int height, string path, ref string strInfo)
        {
            throw new NotImplementedException();
        }
    }
}
