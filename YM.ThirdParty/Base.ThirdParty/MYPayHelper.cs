using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YMDLL.Class;
using YMDLL.Common;

namespace ML.ThirdParty
{
    public class MYPayHelper
    {
        public static CS_OperaWeb ow = new CS_OperaWeb();
        public static CS_CalcDateTime cdt = new CS_CalcDateTime();
        public static string MYPay_URL = "http://www.51mayun.com/";
        public static string SH_Number = "1499847684150";
        public static string SH_Secret = "8a82439ac83734cd20856a67fe181894";
        public static Dictionary<string, string> dic_MYRequestUrl = new Dictionary<string, string>();
        public static Dictionary<string, object> dicMYPay = new Dictionary<string, object>();

        /// <summary>
        /// 初始化参数
        /// </summary>
        public static void InitParam()
        {
            ////支付api
            //dic_MYRequestUrl.Add("openPay", MYPay_URL + "pay/gateway/openPay.do");
            ////查询订单api
            //dic_MYRequestUrl.Add("orderQuery", MYPay_URL + "pay/gateway/orderQuery.do");
            ////申请退款api
            //dic_MYRequestUrl.Add("refund", MYPay_URL + "refundOrder/refund.do");
            ////查询退款api
            //dic_MYRequestUrl.Add("refundQuery", MYPay_URL + "refundOrder/query.do");
            ////关闭订单api
            //dic_MYRequestUrl.Add("closeOrder", MYPay_URL + "pay/gateway/closeOrder.do");

            dicMYPay.Clear();
            string strNonce = Guid.NewGuid().ToString().Replace("-", "").Substring(1,30);
            dicMYPay.Add("version", "1.0");
            dicMYPay.Add("charset", "UTF-8");
            dicMYPay.Add("signType", "MD5");
            dicMYPay.Add("nonceStr", strNonce);
        }

        /// <summary>
        /// 自动生成签名
        /// </summary>
        /// <param name="dicParam">所有参数的集合</param>
        /// <param name="strSecret">商户秘钥</param>
        /// <returns></returns>
        public static string GeneralKeyGen(Dictionary<string,object> dicParam,string strSecret)
        {
            string strUrlParam = "";
            List<String> lst_Param = new List<string>();
            foreach (KeyValuePair<string, object> kv in dicParam.OrderBy(p => p.Key))
            {
                //如果字符为空或者null直接去掉这个参数
                if (kv.Value == null || kv.Value.ToString() == "" || kv.Key == "sign")
                {
                    continue;
                }
                lst_Param.Add(kv.Key + "=" + kv.Value);
            }
            strUrlParam = string.Join("&", lst_Param.ToArray());
            return Md5.GetMD5String(strUrlParam + "&key=" + strSecret).ToUpper();
        }

        /// <summary>
        /// 随机字符集
        /// </summary>
        private static char[] constant =
          {
            '0','1','2','3','4','5','6','7','8','9',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
          };

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GenerateRandomNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 生成退款订单号
        /// </summary>
        /// <returns></returns>
        public static string SetRefundOrder()
        {
            return "R" + SetCommonOrder();
        }

        /// <summary>
        /// 生成支付订单号
        /// </summary>
        /// <returns></returns>
        public static string SetPayOrder()
        {
            return "P" + SetCommonOrder();
        }

        /// <summary>
        /// 生成公共的订单号
        /// </summary>
        /// <returns></returns>
        public static string SetCommonOrder()
        {
            string strRand = GenerateRandomNumber(5);
            CS_CalcDateTime cdt = new CS_CalcDateTime();
            int strCurTime = cdt.DateTimeToStamp(DateTime.Now);
            return strRand + strCurTime.ToString();
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
                //如果字符为空或者null直接去掉这个参数
                if(dicItem.Value == null || dicItem.Value.ToString() == "")
                {
                    i++;
                    continue;
                }
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

        /// <summary>
        /// 微信/支付宝支付
        /// </summary>
        /// <param name="opp"></param>
        /// <returns></returns>
        public static string OpenPay(OpenPayParam opp)
        {
            string result = string.Empty;
            InitParam();
            //基本信息
            dicMYPay.Add("service", opp.service);
            dicMYPay.Add("mchCode", opp.mchCode);
            dicMYPay.Add("outTradeNo", SetPayOrder());
            dicMYPay.Add("body", opp.body);
            dicMYPay.Add("totalFee", opp.totalFee);
            dicMYPay.Add("mchCreateIp", opp.mchCreateIp);
            dicMYPay.Add("notifyUrl", opp.notifyUrl);
            dicMYPay.Add("callbackUrl", opp.callbackUrl);
            dicMYPay.Add("limitCreditPay", opp.limitCreditPay);
            dicMYPay.Add("deviceInfo", opp.deviceInfo);
            dicMYPay.Add("attach", opp.attach);
            dicMYPay.Add("timeStart", opp.timeStart);
            dicMYPay.Add("timeExpire", opp.timeExpire);
            dicMYPay.Add("opUserId", opp.opUserId);
            dicMYPay.Add("goodsTag", opp.goodsTag);
            dicMYPay.Add("productId", opp.productId);
            dicMYPay.Add("sign", GeneralKeyGen(dicMYPay,SH_Secret));

            result = ow.HttpPostData(MYPay_URL + "pay/gateway/openPay.do", FormatParam(dicMYPay));

            return result;
        }

        /// <summary>
        /// 微信公众号支付
        /// </summary>
        /// <param name="wpsp">码云微信公众号支付参数</param>
        /// <returns></returns>
        public static string WechatPublicSignPay(WechatPublicSignPayParam wpsp)
        {
            string result = string.Empty;
            InitParam();
            dicMYPay.Add("service", wpsp.service);
            dicMYPay.Add("mchCode", wpsp.mchCode);
            dicMYPay.Add("outTradeNo", SetPayOrder());
            dicMYPay.Add("body", wpsp.body);
            dicMYPay.Add("totalFee", wpsp.totalFee);
            dicMYPay.Add("mchCreateIp", wpsp.mchCreateIp);
            dicMYPay.Add("notifyUrl", wpsp.notifyUrl);
            dicMYPay.Add("limitCreditPay", wpsp.limitCreditPay);
            dicMYPay.Add("callbackUrl", wpsp.callbackUrl);
            dicMYPay.Add("deviceInfo", wpsp.deviceInfo);
            dicMYPay.Add("attach", wpsp.attach);
            dicMYPay.Add("timeStart", wpsp.timeStart);
            dicMYPay.Add("timeExpire", wpsp.timeExpire);
            dicMYPay.Add("goodsTag", wpsp.goodsTag);
            dicMYPay.Add("sign", GeneralKeyGen(dicMYPay, SH_Secret));
            dicMYPay.Add("isRaw", wpsp.isRaw);
            dicMYPay.Add("isMinipg", wpsp.isMinipg);
            dicMYPay.Add("openId", wpsp.openId);
            dicMYPay.Add("appId", wpsp.appId);

            result = ow.HttpPostData(MYPay_URL + "pay/gateway/openPay.do", FormatParam(dicMYPay));

            return result;
        }

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="oqp">码云查询订单参数</param>
        /// <returns></returns>
        public static string OrderQuery(OrderQueryParam oqp)
        {
            string result = string.Empty;
            InitParam();
            dicMYPay.Add("service", "unified.trade.query");
            dicMYPay.Add("mchCode", oqp.mchCode);
            dicMYPay.Add("outTradeNo", oqp.outTradeNo);
            //dicMYPay.Add("tradeNo", oqp.tradeNo);
            dicMYPay.Add("sign", GeneralKeyGen(dicMYPay, SH_Secret));

            result = ow.HttpPostData(MYPay_URL + "pay/gateway/orderQuery.do", FormatParam(dicMYPay));

            return result;
        }

        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="rp">码云申请退款参数</param>
        /// <returns></returns>
        public static string Refund(RefundParam rp)
        {
            string result = string.Empty;
            InitParam();
            dicMYPay.Add("mchCode", rp.mchCode);
            dicMYPay.Add("outTradeNo", rp.outTradeNo);
            dicMYPay.Add("outRefundNo", SetRefundOrder());
            dicMYPay.Add("totalFee", rp.totalFee);
            dicMYPay.Add("refundFee", rp.refundFee);
            dicMYPay.Add("sign", GeneralKeyGen(dicMYPay, SH_Secret));

            result = ow.HttpPostData(MYPay_URL + "refundOrder/refund.do", FormatParam(dicMYPay));

            return result;
        }

        /// <summary>
        /// 查询退款api
        /// </summary>
        /// <param name="rqp">码云查询退款参数</param>
        /// <returns></returns>
        public static string RefundQuery(RefundQueryParam rqp)
        {
            string result = string.Empty;
            //InitParam();
            dicMYPay.Clear();
            string strNonce = Guid.NewGuid().ToString().Replace("-", "").Substring(1, 30);
            dicMYPay.Add("version", "2.0");
            dicMYPay.Add("charset", "UTF-8");
            dicMYPay.Add("signType", "MD5");
            dicMYPay.Add("nonceStr", strNonce);
            dicMYPay.Add("mchCode", rqp.mchCode);
            dicMYPay.Add("outTradeNo", rqp.outTradeNo);
            dicMYPay.Add("tradeNo", rqp.tradeNo);
            dicMYPay.Add("outRefundNo", rqp.outRefundNo);
            dicMYPay.Add("refundNo", rqp.refundNo);
            dicMYPay.Add("sign", GeneralKeyGen(dicMYPay, SH_Secret));

            result = ow.HttpPostData(MYPay_URL + "refundOrder/query.do", FormatParam(dicMYPay));

            return result;
        }

        /// <summary>
        /// 关闭订单api
        /// </summary>
        /// <param name="cop">码云关闭订单参数</param>
        /// <returns></returns>
        public static string CloseOrder(CloseOrderParam cop)
        {
            string result = string.Empty;
            InitParam();
            dicMYPay.Add("service", "unified.trade.close");
            dicMYPay.Add("mchCode", cop.mchCode);
            dicMYPay.Add("outTradeNo", cop.outTradeNo);
            dicMYPay.Add("sign", GeneralKeyGen(dicMYPay, SH_Secret));

            result = ow.HttpPostData(MYPay_URL + "pay/gateway/closeOrder.do", FormatParam(dicMYPay));

            return result;
        }
    }

    /// <summary>
    /// 微信/支付宝支付参数
    /// </summary>
    public class OpenPayParam
    {
        /// <summary>
        /// 接口类型
        /// </summary>
        public string service { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mchCode { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        //public string outTradeNo { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public string totalFee { get; set; }
        /// <summary>
        /// 终端IP
        /// </summary>
        public string mchCreateIp { get; set; }
        /// <summary>
        /// 通知地址
        /// </summary>
        public string notifyUrl { get; set; }
        /// <summary>
        /// 跳转地址
        /// </summary>
        public string callbackUrl { get; set; }
        /// <summary>
        /// 限制信用卡
        /// </summary>
        public string limitCreditPay { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        //public string nonceStr { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        //public string version { get; set; }
        /// <summary>
        /// 字符集
        /// </summary>
        //public string charset { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        //public string signType { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string deviceInfo { get; set; }
        /// <summary>
        /// 附加信息
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 订单生成时间
        /// </summary>
        public string timeStart { get; set; }
        /// <summary>
        /// 订单超时时间
        /// </summary>
        public string timeExpire { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string opUserId { get; set; }
        /// <summary>
        /// 商品标记
        /// </summary>
        public string goodsTag { get; set; }
        /// <summary>
        /// 商品Id
        /// </summary>
        public string productId { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        //public string sign { get; set; }
    }

    /// <summary>
    /// 码云微信公众号支付参数
    /// </summary>
    public class WechatPublicSignPayParam
    {
        /// <summary>
        /// 接口类型
        /// </summary>
        public string service { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mchCode { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string outTradeNo { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public string totalFee { get; set; }
        /// <summary>
        /// 终端IP
        /// </summary>
        public string mchCreateIp { get; set; }
        /// <summary>
        /// 通知地址
        /// </summary>
        public string notifyUrl { get; set; }
        /// <summary>
        /// 限制信用卡
        /// </summary>
        public string limitCreditPay { get; set; }
        /// <summary>
        /// 跳转地址
        /// </summary>
        public string callbackUrl { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        //public string nonceStr { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        //public string version { get; set; }
        /// <summary>
        /// 字符集
        /// </summary>
        //public string charset { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        //public string signType { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string deviceInfo { get; set; }
        /// <summary>
        /// 附加信息
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 订单生成时间
        /// </summary>
        public string timeStart { get; set; }
        /// <summary>
        /// 订单超时时间
        /// </summary>
        public string timeExpire { get; set; }
        /// <summary>
        /// 商品标记
        /// </summary>
        public string goodsTag { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        //public string sign { get; set; }
        /// <summary>
        /// 是否原生态
        /// </summary>
        public string isRaw { get; set; }
        /// <summary>
        /// 是否小程序支付
        /// </summary>
        public string isMinipg { get; set; }
        /// <summary>
        /// 用户Openid
        /// </summary>
        public string openId { get; set; }
        /// <summary>
        /// 公众号或小程序ID
        /// </summary>
        public string appId { get; set; }
    }

    /// <summary>
    /// 码云查询订单参数
    /// </summary>
    public class OrderQueryParam
    {
        /// <summary>
        /// 接口类型
        /// </summary>
        //public string service { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        //public string version { get; set; }
        /// <summary>
        /// 字符集
        /// </summary>
        //public string charset { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        //public string signType { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mchCode { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string outTradeNo { get; set; }
        /// <summary>
        /// 平台订单号
        /// </summary>
        //public string tradeNo { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        //public string nonceStr { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        //public string sign { get; set; }
    }

    /// <summary>
    /// 码云申请退款参数
    /// </summary>
    public class RefundParam
    {
        /// <summary>
        /// 版本号
        /// </summary>
        //public string version { get; set; }
        /// <summary>
        /// 字符集
        /// </summary>
        //public string charset { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        //public string signType { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mchCode { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string outTradeNo { get; set; }
        /// <summary>
        /// 平台订单号
        /// </summary>
        public string tradeNo { get; set; }
        /// <summary>
        /// 商户退款单号
        /// </summary>
        //public string outRefundNo { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public string totalFee { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public string refundFee { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        //public string opUserId { get; set; }
        /// <summary>
        /// 退款渠道
        /// </summary>
        //public string refundChannel { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        //public string nonceStr { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        //public string sign { get; set; }
    }

    /// <summary>
    /// 码云查询退款参数
    /// </summary>
    public class RefundQueryParam
    {
        /// <summary>
        /// 版本号
        /// </summary>
        //public string version { get; set; }
        /// <summary>
        /// 字符集
        /// </summary>
        //public string charset { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        //public string signType { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mchCode { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string outTradeNo { get; set; }
        /// <summary>
        /// 平台订单号
        /// </summary>
        public string tradeNo { get; set; }
        /// <summary>
        /// 商户退款单号
        /// </summary>
        public string outRefundNo { get; set; }
        /// <summary>
        /// 平台退款单号
        /// </summary>
        public string refundNo { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        //public string nonceStr { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        //public string sign { get; set; }
    }

    /// <summary>
    /// 码云关闭订单参数
    /// </summary>
    public class CloseOrderParam
    {
        /// <summary>
        /// 接口类型
        /// </summary>
        //public string service { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        //public string version { get; set; }
        /// <summary>
        /// 字符集
        /// </summary>
        //public string charset { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        //public string signType { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mchCode { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string outTradeNo { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        //public string nonceStr { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        //public string sign { get; set; }
    }

    /// <summary>
    /// 码云支付基本信息
    /// </summary>
    public class MYPayBaseInfo
    {
        /// <summary>
        /// 接口类型
        /// </summary>
        public string service { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mchCode { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string outTradeNo { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public string totalFee { get; set; }
        /// <summary>
        /// 终端IP
        /// </summary>
        public string mchCreateIp { get; set; }
        /// <summary>
        /// 通知地址
        /// </summary>
        public string notifyUrl { get; set; }
        /// <summary>
        /// 限制信用卡
        /// </summary>
        public string limitCreditPay { get; set; }
        /// <summary>
        /// 跳转地址
        /// </summary>
        public string callbackUrl { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        //public string nonceStr { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        //public string version { get; set; }
        /// <summary>
        /// 字符集
        /// </summary>
        //public string charset { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        //public string signType { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string deviceInfo { get; set; }
        /// <summary>
        /// 附加信息
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 订单生成时间
        /// </summary>
        public string timeStart { get; set; }
        /// <summary>
        /// 订单超时时间
        /// </summary>
        public string timeExpire { get; set; }
        /// <summary>
        /// 商品标记
        /// </summary>
        public string goodsTag { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        //public string sign { get; set; }
        /// <summary>
        /// 是否原生态
        /// </summary>
        public string isRaw { get; set; }
        /// <summary>
        /// 是否小程序支付
        /// </summary>
        public string isMinipg { get; set; }
        /// <summary>
        /// 用户Openid
        /// </summary>
        public string openId { get; set; }
        /// <summary>
        /// 公众号或小程序ID
        /// </summary>
        public string appId { get; set; }
        /// <summary>
        /// 平台订单号
        /// </summary>
        public string tradeNo { get; set; }
        /// <summary>
        /// 平台订单号
        /// </summary>
        public string transactionId { get; set; }
        /// <summary>
        /// 商户退款单号
        /// </summary>
        public string outRefundNo { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public string refundFee { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string opUserId { get; set; }
        /// <summary>
        /// 退款渠道
        /// </summary>
        public string refundChannel { get; set; }
        /// <summary>
        /// 平台退款单号
        /// </summary>
        public string refundId { get; set; }
    }
}
