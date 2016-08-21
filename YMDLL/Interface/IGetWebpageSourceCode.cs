using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMDLL.Interface
{
    /// <summary>
    /// 获取网页源代码接口
    /// </summary>
    public interface IGetWebpageSourceCode
    {
        /// <summary>
        /// 源代码是否压缩加密
        /// </summary>
        bool IsZip
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
        /// 获取搜房网源代码
        /// </summary>
        /// <param name="m_URL">网页URL</param>
        string Get_SouFang_SourceCode(string m_URL);
    }
}
