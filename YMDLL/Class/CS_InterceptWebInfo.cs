using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using YMDLL.Interface;

namespace YMDLL.Class
{
    /// <summary>
    /// 截取网页信息实现
    /// </summary>
    public class CS_InterceptWebInfo : IInterceptWebInfo
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        private string m_ErrorMsg;
        /// <summary>
        /// 截取方向,默认截取方向是左边
        /// </summary>
        private InterceptDirect m_InterceptDirect=InterceptDirect.Intercept_LEFT;
        /// <summary>
        /// 截取顺序
        /// </summary>
        private InterceptOrder m_InterceptOrder = InterceptOrder.InterceptOrder_FTB;
        /// <summary>
        /// 字符类型
        /// </summary>
        private CharacterType m_CharacterType=CharacterType.SpecificSymbols;
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
        /// 截取方向
        /// </summary>
        public InterceptDirect AInterceptDirect
        {
            get
            {
                return m_InterceptDirect;
            }
            set
            {
                m_InterceptDirect = value;
            }
        }

        /// <summary>
        /// 截取顺序
        /// </summary>
        public InterceptOrder AInterceptOrder
        {
            get
            {
                return m_InterceptOrder;
            }
            set
            {
                m_InterceptOrder = value;
            }
        }

        /// <summary>
        /// 字符类型
        /// </summary>
        public CharacterType ACharacterType
        {
            get
            {
                return m_CharacterType;
            }
            set
            {
                m_CharacterType = value;
            }
        }
        /// <summary>
        /// 截取指定信息
        /// </summary>
        /// <param name="m_Source">源数据</param>
        /// <param name="m_Start">开始的字符串</param>
        /// <param name="m_End">结束的字符串</param>
        public string InterceptSpecificedInfo(string m_Source, string m_Start, string m_End)
        {
            //取出俩个字符间的字符串信息
            int ipos0, ipos1;
            String strResult="";
            try
            {
                switch (AInterceptOrder)
                {
                    case InterceptOrder.InterceptOrder_FTB:
                        {
                            //从前往后截取m_Start到m_End之间的内容
                            ipos0 = m_Source.IndexOf(m_Start);
                            ipos1 = m_Source.IndexOf(m_End, ipos0);
                            strResult = m_Source.Substring(ipos0 + m_Start.Length, ipos1 - ipos0 - m_Start.Length);
                            break;
                        }
                    case InterceptOrder.InterceptOrder_BTF:
                        {
                            //从后往前截取m_Start到m_End之间的内容
                            ipos0 = m_Source.LastIndexOf(m_Start);
                            ipos1 = m_Source.LastIndexOf(m_End, ipos0);
                            strResult = m_Source.Substring(ipos0 + m_Start.Length, ipos1 - ipos0 - m_Start.Length);
                            break;
                        }
                }
                //返回结果
                return strResult;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                return ErrorMsg;
            }
        }

        /// <summary>
        /// 截取指定信息
        /// </summary>
        /// <param name="m_Source">源数据</param>
        /// <param name="m_DestChar">目标字符串</param>
        public string InterceptSpecificedInfo(string m_Source,string m_DestChar)
        {
            //取出俩个字符间的字符串信息
            int ipos0;
            String strResult="";
            try
            {
                switch (AInterceptOrder)
                {
                    case InterceptOrder.InterceptOrder_FTB:
                        {
                            //截取顺序从前往后截取
                            switch (AInterceptDirect)
                            {
                                case InterceptDirect.Intercept_LEFT:
                                    {
                                        //截取左边的内容
                                        ipos0 = m_Source.IndexOf(m_DestChar);
                                        strResult = m_Source.Substring(0, ipos0);
                                        break;
                                    }
                                case InterceptDirect.Intercept_RIGHT:
                                    {
                                        //截取右边的内容
                                        ipos0 = m_Source.IndexOf(m_DestChar);
                                        strResult = m_Source.Substring(ipos0 + 1, m_Source.Length - ipos0 - 1);
                                        break;
                                    }
                            }
                            break;
                        }
                    case InterceptOrder.InterceptOrder_BTF:
                        {
                            //截取顺序从后往前截取
                            switch (AInterceptDirect)
                            {
                                case InterceptDirect.Intercept_LEFT:
                                    {
                                        //截取左边的内容
                                        ipos0 = m_Source.LastIndexOf(m_DestChar);
                                        strResult = m_Source.Substring(0, ipos0);
                                        break;
                                    }
                                case InterceptDirect.Intercept_RIGHT:
                                    {
                                        //截取右边的内容
                                        ipos0 = m_Source.LastIndexOf(m_DestChar);
                                        strResult = m_Source.Substring(ipos0 + 1, m_Source.Length - ipos0 - 1);
                                        break;
                                    }
                            }
                            break;
                        }
                }
                //返回结果
                return strResult;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                return ErrorMsg;
            }
        }

        /// <summary>
        /// 去除不需要的字符
        /// </summary>
        /// <param name="m_Source">源数据</param>
        public string RemoveCharacters(string m_Source)
        {
            String strResult="";
            switch (ACharacterType)
            {
                case CharacterType.NumericCharacter:
                    {
                        //去除数字
                        strResult = Regex.Replace(m_Source,"\\d","");
                        break;
                    }
                case CharacterType.LowercaseCharacter:
                    {
                        //去除小写字母
                        strResult = Regex.Replace(m_Source, "[a-z]", "");
                        break;
                    }
                case CharacterType.UppercaseCharacter:
                    {
                        //去除大写字母
                        strResult = Regex.Replace(m_Source, "[A-Z]", "");
                        break;
                    }
                case CharacterType.SpecificSymbols:
                    {
                        //去除特殊符号
                        strResult = Regex.Replace(m_Source, "[\"\'\\s]", "");
                        break;
                    }  
            }
            return strResult;
        }
    }


}
