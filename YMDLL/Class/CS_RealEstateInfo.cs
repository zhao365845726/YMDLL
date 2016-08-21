using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YMDLL.Interface;

namespace YMDLL.Class
{
    /// <summary>
    /// 楼盘信息实现类
    /// </summary>
    public class CS_RealEstateInfo :IRealEstateInfo
    {
        #region 字段集合
        /// <summary>
        /// 网址名称
        /// </summary>
        private string m_WebName;
        /// <summary>
        /// 网址域名
        /// </summary>
        private string m_WebURL;

        /// <summary>
        /// 网页城市
        /// </summary>
        private string m_WebCity = "南昌";
        #endregion
        /// <summary>
        /// 错误信息
        /// </summary>
        private string m_ErrorMsg;

        #region 属性集合
        /// <summary>
        /// 网址名称
        /// </summary>
        public string WebName
        {
            get
            {
                return m_WebName;
            }
            set
            {
                m_WebName = value;
            }
        }

        /// <summary>
        /// 网址域名
        /// </summary>
        public string WebURL
        {
            get
            {
                return m_WebURL;
            }
            set
            {
                m_WebURL = value;
            }
        }


        /// <summary>
        /// 网页城市
        /// </summary>
        /// <value>南昌</value>
        public string WebCity
        {
            get
            {
                return m_WebCity;
            }
            set
            {
                m_WebCity = value;
            }
        }
        #endregion

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


        #region 方法集合
        /// <summary>
        /// 显示信息
        /// </summary>
        public void ShowMsg()
        {
            MessageBox.Show(ErrorMsg);
        }
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="m_Message">输入信息参数</param>
        public void ShowMsg(string m_Message)
        {
            MessageBox.Show(m_Message);
        }

        /// <summary>
        /// 获取首页网址
        /// </summary>
        public bool GetHomeURL()
        {
            try
            {
                if (WebName.IndexOf("搜房") != -1)
                {
                    CS_CityCode sfcc = new CS_CityCode();
                    //WebURL = "http://" + sfcc.Get_SouFang_CityCode(WebCity) + ".soufun.com/";
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                ErrorMsg = ex.Message;
                return false;
            }
            
        }
#endregion

        
        
    }
}
