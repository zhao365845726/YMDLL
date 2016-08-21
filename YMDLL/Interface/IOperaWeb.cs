using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMDLL.Interface
{
    /// <summary>
    /// 操作网页接口
    /// </summary>
    public interface IOperaWeb
    {
        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        string GetClientIPAddress();

        /// <summary>
        /// 获取客户端日期时间
        /// </summary>
        DateTime GetClientDateTime();

        /// <summary>
        /// 获取客户端当前访问URL
        /// </summary>
        string GetClientCurrentAskUrl();

        /// <summary>
        /// 获取IPv6
        /// </summary>
        string GetClientIPv6();

        /// <summary>
        /// 获取IPv4
        /// </summary>
        string GetClientIPv4();

        /// <summary>
        /// 获取客户端用户名
        /// </summary>
        string GetClientUseName();

        /// <summary>
        /// 打开指定网页
        /// </summary>
        /// <param name="m_URL">网页地址</param>
        bool OpenURL(string m_URL);

        /// <summary>
        /// 获取不加密网页源代码
        /// </summary>
        /// <param name="m_URL">网址</param>
        /// <param name="m_EncodeType">编码类型</param>
        string GetWebCode(string m_URL, Encoding m_EncodeType);
    }
}
