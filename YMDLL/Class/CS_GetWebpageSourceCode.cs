using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Net;
using System.IO;
using YMDLL.Interface;

namespace YMDLL.Class
{
    /// <summary>
    /// 获取网页源代码实现
    /// </summary>
    public class CS_GetWebpageSourceCode:IGetWebpageSourceCode
    {
        /// <summary>
        /// 网页压缩加密状态
        /// </summary>
        private bool m_IsZip;
        /// <summary>
        /// 错误信息
        /// </summary>
        private string m_ErrorMsg;

        /// <summary>
        /// 源代码是否压缩加密
        /// </summary>
        public bool IsZip
        {
            get
            {
                return m_IsZip;
            }
            set
            {
                m_IsZip = value;
            }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg
        {
            get
            {
                return m_ErrorMsg;
            }
            set
            {
                m_ErrorMsg = value;
            }
        }

        /// <summary>
        /// 获取搜房网源代码
        /// </summary>
        /// <param name="m_URL">网页URL</param>
        public string Get_SouFang_SourceCode(string m_URL)
        {
            string pageCode;
            try
            {
                HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(m_URL);
                webRequest.Timeout = 30000;
                webRequest.Method = "GET";
                webRequest.UserAgent = "Mozilla/4.0";
                webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
                if (webResponse.ContentEncoding.ToLower() == "gzip")
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (var zipStream =
                            new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                        {
                            using (StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.Default))
                            {
                                pageCode = sr.ReadToEnd();
                            }
                        }
                    }
                }
                else
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(streamReceive, Encoding.Default))
                        {
                            pageCode = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                pageCode = ex.Message;
            }
            return pageCode;
        }
    }
}
