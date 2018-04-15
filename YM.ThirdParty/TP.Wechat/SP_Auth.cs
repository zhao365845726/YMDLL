using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMDLL.Class;

namespace ML.ThirdParty.Wechat
{
    /// <summary>
    /// 小程序授权
    /// </summary>
    public class SP_Auth
    {
        #region ----变量声明区域----
        /// <summary>
        /// 声明操作网页的对象
        /// </summary>
        public CS_OperaWeb ow = new CS_OperaWeb();
        #endregion

        /// <summary>
        /// 换取身份信息的方法
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public string ExchageStatus(string Code)
        {
            /// <summary>
            /// 换取身份的Url
            /// </summary>
            string strExchageUrl = "https://api.weixin.qq.com/sns/jscode2session?appid=wx133b351ac060a310&secret=a95d0d14156875c6d708c440a92ebbbe&js_code=" + Code + "&grant_type=authorization_code";
            string strResult = ow.HttpPostData(strExchageUrl,"");
            return strResult;
        }
    }
}
