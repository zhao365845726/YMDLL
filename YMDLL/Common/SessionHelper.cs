//=============================================================
//  Copyright (C) 2016-2100
//  CLR版本:                	$var$
//  机器名称:                  ZHHLWLJS_SYSTEM
//  命名空间名称/文件名:        YMDLL.Common/$projectname$ 
//  创建人:			   	    MartyZane     
//  创建时间:     		  	   2016/12/13 17:50:23
//  网站：				  		http://www.chuxinm.com
//==============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace YMDLL.Common
{
    /// <summary>
    /// Session助手
    /// </summary>
    public class SessionHelper
    {
        /// <summary>
        /// 声明Session的实例
        /// </summary>
        private static HttpSessionState _session = HttpContext.Current.Session;
        /// <summary>
        /// 设置一个Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetSession(string key,object value)
        {
            _session[key] = value;
        }
        /// <summary>
        /// 获取Session的Number
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetSessionNumber(string key)
        {
            int result = 0;
            if (_session[key] != null)
            {
                int.TryParse(_session[key].ToString(), out result);
            }
            return result;
        }
        /// <summary>
        /// 获取Session的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSessionString(string key)
        {
            string result = "";
            if (_session[key] != null)
            {
                result = _session[key].ToString();
            }
            return result;
        }
        /// <summary>
        /// 清除Session
        /// </summary>
        public static void Clear()
        {
            _session.Clear();
        }
    }
}
