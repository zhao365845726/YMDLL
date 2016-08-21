using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMDLL.Interface
{
    /// <summary>
    /// 截取网页信息接口
    /// </summary>
    public interface IInterceptWebInfo
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        String ErrorMsg
        {
            get;
            set;
        }

        /// <summary>
        /// 截取方向
        /// </summary>
        InterceptDirect AInterceptDirect
        {
            get;
            set;
        }

        /// <summary>
        /// 截取顺序
        /// </summary>
        InterceptOrder AInterceptOrder
        {
            get;
            set;
        }

        /// <summary>
        /// 字符类型
        /// </summary>
        CharacterType ACharacterType
        {
            get;
            set;
        }
        /// <summary>
        /// 截取指定信息
        /// </summary>
        /// <param name="m_Source">源数据</param>
        /// <param name="m_Start">开始的字符串</param>
        /// <param name="m_End">结束的字符串</param>
        string InterceptSpecificedInfo(string m_Source, string m_Start, string m_End);

        /// <summary>
        /// 截取指定信息
        /// </summary>
        /// <param name="m_Source">源数据</param>
        /// <param name="m_DestChar">目标字符</param>
        string InterceptSpecificedInfo(string m_Source,string m_DestChar);

        /// <summary>
        /// 去除不需要的字符
        /// </summary>
        /// <param name="m_Source">源数据</param>
        string RemoveCharacters(string m_Source);
    }

    public enum InterceptDirect
    {
        /// <summary>
        /// 截取左边的信息
        /// </summary>
        Intercept_LEFT = 0,
        /// <summary>
        /// 截取右边的信息
        /// </summary>
        Intercept_RIGHT = 1,
    }

    public enum InterceptOrder
    {
        /// <summary>
        /// 截取顺序从前往后
        /// </summary>
        InterceptOrder_FTB = 0,
        /// <summary>
        /// 截取顺序从后往前
        /// </summary>
        InterceptOrder_BTF = 1,
    }

    public enum CharacterType
    {
        /// <summary>
        /// 数字字符
        /// </summary>
        NumericCharacter = 0,
        /// <summary>
        /// 小写字母字符
        /// </summary>
        LowercaseCharacter = 1,
        /// <summary>
        /// 大写字母字符
        /// </summary>
        UppercaseCharacter = 2,
        /// <summary>
        /// 特殊符号
        /// </summary>
        SpecificSymbols = 3,
    }
}
