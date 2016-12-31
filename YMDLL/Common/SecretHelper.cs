//=============================================================
//  Copyright (C) 2016-2100
//  CLR版本:                	$var$
//  机器名称:                  ZHHLWLJS_SYSTEM
//  命名空间名称/文件名:        YMDLL.Common/$projectname$ 
//  创建人:			   	    MartyZane     
//  创建时间:     		  	   2016/12/14 18:22:59
//  网站：				  		http://www.chuxinm.com
//==============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace YMDLL.Common
{
    public class SecretHelper
    {
        #region 获取由SHA1加密的字符串
        public string EncryptToSHA1(string str)
        {
            byte[] temp1 = Encoding.UTF8.GetBytes(str);
            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            byte[] temp2 = sha.ComputeHash(temp1);
            sha.Clear();

            string output = BitConverter.ToString(temp2);
            output = output.Replace("-", "");
            output = output.ToLower();
            return output;
        }
        #endregion
    }
}
