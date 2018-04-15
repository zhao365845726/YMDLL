using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YMDLL.Class;
using YMDLL.Common;

namespace ML.ThirdParty.Base
{
    /// <summary>
    /// 融智付帮助类
    /// </summary>
    public class ZitoPayHelper
    {
        #region ----声明的变量----
        public static CS_OperaWeb ow = new CS_OperaWeb();
        public static CS_CalcDateTime cdt = new CS_CalcDateTime();
        public static string ZitoPay_DEBUG_URL = "https://pretech.zitopay.com";     //联调地址
        public static string ZitoPay_REALEASE_URL = "https://tech.zitopay.com";     //生产地址
        public static Dictionary<string, object> dicZitoPay = new Dictionary<string, object>();     //Zito支付的参数包
        public static ZitoSignParam zsp = new ZitoSignParam();      //签名参数
        #endregion

        /// <summary>
        /// 初始化参数
        /// </summary>
        public static void InitParam()
        {

        }

        /// <summary>
        /// 设置支付宝签名参数
        /// </summary>
        public static string SetAliSignParam(string TotalPrice,string OrderNum)
        {
            zsp.id = "RZF187570";
            zsp.appid = "RZF187504";
            zsp.orderidinf = OrderNum;
            zsp.totalPrice = TotalPrice;
            zsp.appkey = "9871d5a52c182d76c714d0d8f64825ef";
            return GeneralKeyGen(zsp);
        }

        /// <summary>
        /// 设置微信签名参数
        /// </summary>
        public static string SetWechatSignParam(string TotalPrice, string OrderNum)
        {
            zsp.id = "RZF187570";
            zsp.appid = "RZF187504";
            zsp.orderidinf = OrderNum;
            zsp.totalPrice = TotalPrice;
            zsp.appkey = "9871d5a52c182d76c714d0d8f64825ef";
            return GeneralKeyGen(zsp);
        }

        #region ----公共方法----
        /// <summary>
        /// 自动生成签名
        /// </summary>
        /// <param name="zsp">签名参数</param>
        /// <returns></returns>
        public static string GeneralKeyGen(ZitoSignParam zsp)
        {
            string result = string.Empty;
            result = zsp.id + zsp.appid + zsp.orderidinf + zsp.totalPrice + zsp.appkey;
            return Md5.GetMD5String(result);
        }

        /// <summary>
        /// 格式化参数
        /// </summary>
        /// <param name="dicParam"></param>
        /// <returns></returns>
        public static string FormatParam(Dictionary<string, object> dicParam)
        {
            int i = 0;
            string str_Format = "";
            string strValue = "";
            foreach (KeyValuePair<string, object> dicItem in dicParam)
            {
                ////如果字符为空或者null直接去掉这个参数
                //if (dicItem.Value == null || dicItem.Value.ToString() == "")
                //{
                //    i++;
                //    continue;
                //}
                //去掉所有Value的空字符
                strValue = dicItem.Value.ToString().Replace(" ", "");
                if (i == dicParam.Count - 1)
                {
                    str_Format += dicItem.Key + "=" + strValue;
                }
                else
                {
                    str_Format += dicItem.Key + "=" + strValue + "&";
                }
                i++;
            }
            return str_Format;
        }
        #endregion

        /// <summary>
        /// 融智付支付
        /// </summary>
        /// <param name="zpp">融智付参数</param>
        /// <param name="UserId">用户Id</param>
        /// <returns></returns>
        public static string ZitoPay(ZitoPayParam zpp,string UserId)
        {
            string result = string.Empty;
            string strOrderNum = Guid.NewGuid().ToString().Replace("-", "");
            string strSign = "";
            string gid = "";
            dicZitoPay.Clear();
            switch (zpp.PayType)
            {
                case "ALIPAY":
                    strSign = SetAliSignParam(zpp.totalPrice, strOrderNum);
                    gid = "77";
                    break;
                case "WECHATPAY":
                    strSign = SetWechatSignParam(zpp.totalPrice, strOrderNum);
                    gid = "91";
                    break;
            }
            dicZitoPay.Add("id", zsp.id);
            dicZitoPay.Add("appid", zsp.appid);
            dicZitoPay.Add("posttime", DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            dicZitoPay.Add("gid", gid);
            dicZitoPay.Add("orderidinf", zsp.orderidinf);
            dicZitoPay.Add("totalPrice", zpp.totalPrice);
            dicZitoPay.Add("ordertitle", "随便穿点");
            dicZitoPay.Add("goodsname", "随便穿点");
            dicZitoPay.Add("goodsdetail", "随便穿点");
            dicZitoPay.Add("bgRetUrl", "http://47.93.255.23:81/Api/ZitoPay/PayNotice");
            dicZitoPay.Add("returnUrl", "http://47.93.255.23:81/Api/ZitoPay/PayNotice");
            //dicZitoPay.Add("openid", "");
            dicZitoPay.Add("sign", strSign);

            result = ow.HttpPostData(ZitoPay_REALEASE_URL + "/service/api/controller/zitopay/topayByMc", FormatParam(dicZitoPay));
            //new Base_SysLogBll().InsertLog("提交给融智付支付的参数：" + FormatParam(dicZitoPay));
            //记录充值的订单信息
            //new Base_CashChangeDetailsBll().RechargeInsertCashDetailInfo(UserId, strOrderNum, Convert.ToDecimal(zpp.totalPrice));
            return result;
        }
    }

    /// <summary>
    /// 签名参数
    /// </summary>
    public class ZitoSignParam
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 应用Id
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string orderidinf { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public string totalPrice { get; set; }
        /// <summary>
        /// 应用key
        /// </summary>
        public string appkey { get; set; }
    }

    /// <summary>
    /// 支付参数
    /// </summary>
    public class ZitoPayParam
    {
        /// <summary>
        /// 支付类型（WECHATPAY:微信;ALIPAY:支付宝;）
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public string totalPrice { get; set; }
    }
}