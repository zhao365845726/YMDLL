using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMDLL.Interface
{
    /// <summary>
    /// 楼盘信息接口
    /// </summary>
    public interface IRealEstateInfo
    {

        /// <summary>
        /// 网址域名
        /// </summary>
        String WebURL
        {
            get;
            set;
        }

        /// <summary>
        /// 网址名称
        /// </summary>
        String WebName
        {
            get;
            set;
        }

        /// <summary>
        /// 网页城市
        /// </summary>
        string WebCity
        {
            get;
            set;
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        string ErrorMsg
        {
            get;
            set;
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        void ShowMsg();
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="m_Message">输入信息参数</param>
        void ShowMsg(string m_Message);


        /// <summary>
        /// 获取首页网址
        /// </summary>
        bool GetHomeURL();
    }
}
