using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YMDLL.Interface;

namespace YMDLL.Class.Web
{
    public class YM_InterceptWebInfo_51job : IInterceptWebInfo
    {
        /// <summary>
        /// 获取URL链接
        /// </summary>
        /// <param name="m_Url">当前的URL</param>
        /// <param name="m_Keyword">搜索的关键字</param>
        public string GetUrl(Uri uri,string m_Keyword)
        {
            return "";
        }

        



        #region IInterceptWebInfo 成员

        public string ErrorMsg
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public InterceptDirect AInterceptDirect
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public InterceptOrder AInterceptOrder
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public CharacterType ACharacterType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string InterceptSpecificedInfo(string m_Source, string m_Start, string m_End)
        {
            throw new NotImplementedException();
        }

        public string InterceptSpecificedInfo(string m_Source, string m_DestChar)
        {
            throw new NotImplementedException();
        }

        public string RemoveCharacters(string m_Source)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
