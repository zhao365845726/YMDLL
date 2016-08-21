using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YMDLL.Interface;

namespace YMDLL.Class
{
    /// <summary>
    /// 获取城市代码实现类
    /// </summary>
    public class CS_CityCode : ICityCode
    {
        /// <summary>
        /// 源代码
        /// </summary>
        private string m_SourceCode;
        /// <summary>
        /// 错误信息
        /// </summary>
        private string m_ErrorMsg;
        /// <summary>
        /// 源代码
        /// </summary>
        public string SourceCode
        {
            get
            {
                return m_SourceCode;
            }
            set
            {
                m_SourceCode = value;
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
        /// 获取搜房网城市首页地址
        /// </summary>
        /// <param name="m_CityName">城市名称</param>
        public string Get_SouFang_CityHome(string m_CityName)
        {
            CS_GetWebpageSourceCode gwsc = new CS_GetWebpageSourceCode();
            SourceCode = gwsc.Get_SouFang_SourceCode("http://www.soufun.com/");

            return SourceCode;
        }
    }
}
