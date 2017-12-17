using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YMDLL.Class;
using YMDLL.Common;

namespace ML.ThirdParty
{
    public class WechatPayHelper
    {
        public static CS_OperaWeb ow = new CS_OperaWeb();
        public static CS_CalcDateTime cdt = new CS_CalcDateTime();
        public static string WechatPay_URL = "https://api.mch.weixin.qq.com/";
        public static string SH_Number = "1499847684150";
        public static string SH_Secret = "8a82439ac83734cd20856a67fe181894";
        public static Dictionary<string, string> dic_WechatRequestUrl = new Dictionary<string, string>();
        public static Dictionary<string, object> dicWechatPay = new Dictionary<string, object>();

        /// <summary>
        /// 初始化参数
        /// </summary>
        public static void InitParam()
        {
            dicWechatPay.Clear();
            //string strNonce = Guid.NewGuid().ToString().Replace("-", "").Substring(1, 30);
            //dicWechatPay.Add("version", "1.0");
            //dicWechatPay.Add("charset", "UTF-8");
            //dicWechatPay.Add("signType", "MD5");
            //dicWechatPay.Add("nonceStr", strNonce);
        }

        /// <summary>
        /// 自动生成签名
        /// </summary>
        /// <param name="dicParam">所有参数的集合</param>
        /// <param name="strSecret">商户秘钥</param>
        /// <returns></returns>
        public static string GeneralKeyGen(Dictionary<string, object> dicParam, string strSecret)
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
                if (dicItem.Value == null || dicItem.Value.ToString() == "")
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
        /// 微信支付
        /// </summary>
        /// <param name="wpp">微信支付参数</param>
        /// <returns></returns>
        public static string Pay(WechatPayParam wpp)
        {
            string result = string.Empty;
            //InitParam();
            //基本信息
            dicWechatPay.Add("appid", wpp.appid);
            dicWechatPay.Add("mch_id", wpp.mch_id);
            dicWechatPay.Add("device_info", wpp.device_info);
            dicWechatPay.Add("nonce_str", wpp.nonce_str);
            dicWechatPay.Add("sign", wpp.sign);
            dicWechatPay.Add("sign_type", wpp.sign_type);
            dicWechatPay.Add("body", wpp.body);
            dicWechatPay.Add("detail", wpp.detail);
            dicWechatPay.Add("attach", wpp.attach);
            dicWechatPay.Add("out_trade_no", wpp.out_trade_no);
            dicWechatPay.Add("fee_type", wpp.fee_type);
            dicWechatPay.Add("total_fee", wpp.total_fee);
            dicWechatPay.Add("spbill_create_ip", wpp.spbill_create_ip);
            dicWechatPay.Add("time_start", wpp.time_start);
            dicWechatPay.Add("time_expire", wpp.time_expire);
            dicWechatPay.Add("goods_tag", wpp.goods_tag);
            dicWechatPay.Add("notify_url", wpp.notify_url);
            dicWechatPay.Add("trade_type", wpp.trade_type);
            dicWechatPay.Add("limit_pay", wpp.limit_pay);
            dicWechatPay.Add("scene_info", wpp.scene_info);

            result = ow.HttpPostData(WechatPay_URL + "pay/unifiedorder", FormatParam(dicWechatPay));

            return result;
        }

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="woqp">微信查询订单参数</param>
        /// <returns></returns>
        public static string OrderQuery(WechatOrderQueryParam woqp)
        {
            string result = string.Empty;
            //InitParam();
            //基本信息
            dicWechatPay.Add("appid", woqp.appid);
            dicWechatPay.Add("mch_id", woqp.mch_id);
            dicWechatPay.Add("transaction_id", woqp.transaction_id);
            dicWechatPay.Add("out_trade_no", woqp.out_trade_no);
            dicWechatPay.Add("nonce_str", woqp.nonce_str);
            dicWechatPay.Add("sign", woqp.sign);

            result = ow.HttpPostData(WechatPay_URL + "pay/orderquery", FormatParam(dicWechatPay));

            return result;
        }

        /// <summary>
        /// 关闭订单
        /// </summary>
        /// <param name="wcop">微信关闭订单参数</param>
        /// <returns></returns>
        public static string CloseOrder(WechatCloseOrderParam wcop)
        {
            string result = string.Empty;
            //InitParam();
            //基本信息
            dicWechatPay.Add("appid", wcop.appid);
            dicWechatPay.Add("mch_id", wcop.mch_id);
            dicWechatPay.Add("out_trade_no", wcop.out_trade_no);
            dicWechatPay.Add("nonce_str", wcop.nonce_str);
            dicWechatPay.Add("sign", wcop.sign);
            
            result = ow.HttpPostData(WechatPay_URL + "pay/closeorder", FormatParam(dicWechatPay));

            return result;
        }

        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="wrp">微信申请退款参数</param>
        /// <returns></returns>
        public static string Refund(WechatRefundParam wrp)
        {
            string result = string.Empty;
            //InitParam();
            //基本信息
            dicWechatPay.Add("appid", wrp.appid);
            dicWechatPay.Add("mch_id", wrp.mch_id);
            dicWechatPay.Add("out_trade_no", wrp.out_trade_no);
            dicWechatPay.Add("nonce_str", wrp.nonce_str);
            dicWechatPay.Add("sign", wrp.sign);

            result = ow.HttpPostData(WechatPay_URL + "secapi/pay/refund", FormatParam(dicWechatPay));

            return result;
        }

        /// <summary>
        /// 关闭订单
        /// </summary>
        /// <param name="wrqp">微信关闭订单参数</param>
        /// <returns></returns>
        public static string RefundQuery(WechatRefundQueryParam wrqp)
        {
            string result = string.Empty;
            //InitParam();
            //基本信息
            dicWechatPay.Add("appid", wrqp.appid);
            dicWechatPay.Add("mch_id", wrqp.mch_id);
            dicWechatPay.Add("nonce_str", wrqp.nonce_str);
            dicWechatPay.Add("sign", wrqp.sign);
            dicWechatPay.Add("transaction_id", wrqp.transaction_id);
            dicWechatPay.Add("out_trade_no", wrqp.out_trade_no);
            dicWechatPay.Add("out_refund_no", wrqp.out_refund_no);
            dicWechatPay.Add("refund_id", wrqp.refund_id);

            result = ow.HttpPostData(WechatPay_URL + "pay/refundquery", FormatParam(dicWechatPay));

            return result;
        }

        /// <summary>
        /// 下载对账单
        /// </summary>
        /// <param name="wdbp">微信下载对账单参数</param>
        /// <returns></returns>
        public static string DownloadBill(WechatDownloadBillParam wdbp)
        {
            string result = string.Empty;
            //InitParam();
            //基本信息
            dicWechatPay.Add("appid", wdbp.appid);
            dicWechatPay.Add("mch_id", wdbp.mch_id);
            dicWechatPay.Add("device_info", wdbp.mch_id);
            dicWechatPay.Add("nonce_str", wdbp.nonce_str);
            dicWechatPay.Add("sign", wdbp.sign);
            dicWechatPay.Add("sign_type", wdbp.sign_type);
            dicWechatPay.Add("bill_date", wdbp.bill_date);
            dicWechatPay.Add("bill_type", wdbp.bill_type);
            dicWechatPay.Add("tar_type", wdbp.tar_type);

            result = ow.HttpPostData(WechatPay_URL + "pay/downloadbill", FormatParam(dicWechatPay));

            return result;
        }

        /// <summary>
        /// 交易保障
        /// </summary>
        /// <param name="wrep">微信交易保障参数</param>
        /// <returns></returns>
        public static string Report(WechatReportParam wrep)
        {
            string result = string.Empty;
            //InitParam();
            //基本信息
            dicWechatPay.Add("appid", wrep.appid);
            dicWechatPay.Add("mch_id", wrep.mch_id);
            dicWechatPay.Add("device_info", wrep.mch_id);
            dicWechatPay.Add("nonce_str", wrep.nonce_str);
            dicWechatPay.Add("interface_url", wrep.interface_url);
            dicWechatPay.Add("execute_time_", wrep.execute_time_);
            dicWechatPay.Add("return_code", wrep.return_code);
            dicWechatPay.Add("return_msg", wrep.return_msg);
            dicWechatPay.Add("err_code", wrep.err_code);
            dicWechatPay.Add("err_code_des", wrep.err_code_des);
            dicWechatPay.Add("out_trade_no", wrep.out_trade_no);
            dicWechatPay.Add("user_ip", wrep.user_ip);
            dicWechatPay.Add("time", wrep.time);

            result = ow.HttpPostData(WechatPay_URL + "payitil/report", FormatParam(dicWechatPay));

            return result;
        }
    }

    /// <summary>
    ///  交易保障
    /// </summary>
    public class WechatReportParam
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string device_info { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 接口URL
        /// </summary>
        public string interface_url { get; set; }
        /// <summary>
        /// 接口耗时
        /// </summary>
        public string execute_time_ { get; set; }
        /// <summary>
        /// 返回状态码
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string return_msg { get; set; }
        /// <summary>
        /// 业务结果
        /// </summary>
        public string result_code { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public string err_code { get; set; }
        /// <summary>
        /// 错误代码描述
        /// </summary>
        public string err_code_des { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 访问接口IP
        /// </summary>
        public string user_ip { get; set; }
        /// <summary>
        /// 商户上报时间
        /// </summary>
        public string time { get; set; }
    }

    /// <summary>
    /// 下载微信对账单
    /// </summary>
    public class WechatDownloadBillParam
    {
        /// <summary>
        /// 公众帐号ID
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string device_info { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 签名类型
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>
        /// 对账单日期
        /// </summary>
        public string bill_date { get; set; }
        /// <summary>
        /// 账单类型
        /// </summary>
        public string bill_type { get; set; }
        /// <summary>
        /// 压缩账单
        /// </summary>
        public string tar_type { get; set; }
    }

    /// <summary>
    /// 查询退款
    /// </summary>
    public class WechatRefundQueryParam
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 微信订单号
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 商户退款单号
        /// </summary>
        public string out_refund_no { get; set; }
        /// <summary>
        /// 微信退款单号
        /// </summary>
        public string refund_id { get; set; }
    }

    /// <summary>
    /// 申请退款
    /// </summary>
    public class WechatRefundParam
    {
        /// <summary>
        /// 公众帐号Id
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 签名类型
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>
        /// 微信订单号
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 商户退款单号
        /// </summary>
        public string out_refund_no { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public string total_fee { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public string refund_fee { get; set; }
        /// <summary>
        /// 货币种类
        /// </summary>
        public string refund_fee_type { get; set; }
        /// <summary>
        /// 退款原因
        /// </summary>
        public string refund_desc { get; set; }
        /// <summary>
        /// 退款资金来源
        /// </summary>
        public string refund_account { get; set; }
    }

    /// <summary>
    /// 关闭订单
    /// </summary>
    public class WechatCloseOrderParam
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
    }

    /// <summary>
    /// 查询订单
    /// </summary>
    public class WechatOrderQueryParam
    {
        /// <summary>
        /// 应用APPID
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 微信订单号
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
    }

    /// <summary>
    /// 微信支付参数
    /// </summary>
    public class WechatPayParam
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 设备号(非必填)
        /// </summary>
        public string device_info { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 签名类型(非必填)
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 商品详情(非必填)
        /// </summary>
        public string detail { get; set; }
        /// <summary>
        /// 附加数据(非必填)
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 货币类型(非必填)
        /// </summary>
        public string fee_type { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public string total_fee { get; set; }
        /// <summary>
        /// 终端IP
        /// </summary>
        public string spbill_create_ip { get; set; }
        /// <summary>
        /// 交易起始时间(非必填)
        /// </summary>
        public string time_start { get; set; }
        /// <summary>
        /// 交易结束时间(非必填)
        /// </summary>
        public string time_expire { get; set; }
        /// <summary>
        /// 订单优惠标记(非必填)
        /// </summary>
        public string goods_tag { get; set; }
        /// <summary>
        /// 通知地址
        /// </summary>
        public string notify_url { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 指定支付方式(非必填)
        /// </summary>
        public string limit_pay { get; set; }
        /// <summary>
        /// 场景信息(非必填)
        /// </summary>
        public string scene_info { get; set; }
    }
}
