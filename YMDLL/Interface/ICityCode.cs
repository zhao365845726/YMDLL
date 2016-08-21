using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMDLL.Interface
{
    /// <summary>
    /// 获取城市代码接口
    /// </summary>
    public interface ICityCode
    {
        /// <summary>
        /// 源代码
        /// </summary>
        string SourceCode
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
        /// 获取搜房网城市首页地址
        /// </summary>
        /// <param name="m_CityName">城市名称</param>
        string Get_SouFang_CityHome(string m_CityName);
    }
}
