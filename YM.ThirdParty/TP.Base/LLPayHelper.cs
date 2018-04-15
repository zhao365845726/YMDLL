using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using YMDLL.Class;
using YMDLL.Common;
using com.lianlianpay.security.utils;
using ML.LLYTPay;

namespace ML.ThirdParty.Base
{
    /// <summary>
    /// 连连支付帮助类
    /// </summary>
    public class LLPayHelper
    {
        public static CS_OperaWeb ow = new CS_OperaWeb();
        /// <summary>
        /// 连连支付请求地址
        /// </summary>
        public static string LLPay_URL = "https://instantpay.lianlianpay.com/paymentapi/";
        /// <summary>
        /// 连连支付字典
        /// </summary>
        public static SortedDictionary<string, string> dicLLPay = new SortedDictionary<string, string>();
        /// <summary>
        /// 商户号
        /// </summary>
        public static string SH_Number = "201706121001811585";

        /// <summary>
        /// 格式化参数
        /// </summary>
        /// <param name="dicParam"></param>
        /// <returns></returns>
        public static string FormatParam(SortedDictionary<string,string> dicParam)
        {
            int i = 0;
            string str_Format = "";
            foreach (KeyValuePair<string, string> dicItem in dicParam)
            {
                if(i == dicParam.Count - 1)
                {
                    str_Format += dicItem.Key + "=" + dicItem.Value;
                }
                else
                {
                    str_Format += dicItem.Key + "=" + dicItem.Value + "&";
                }
                i++;
            }
            return str_Format;
        }

        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="SDParam">排序字典</param>
        /// <returns></returns>
        public static string ReturnResult(string RequestUrl,SortedDictionary<string, string> SDParam)
        {
            string reqJson = YinTongUtil.dictToJson(SDParam);

            var json = "";
            try
            {
                json = com.lianlianpay.security.utils.LianLianPaySecurity.encrypt(reqJson, PartnerConfig.YT_PUB_KEY);
            }
            catch (Exception ex)
            {
                //new Base_SysLogBll().InsertLog("异常信息--->" + ex.ToString());
            }

            var http = (HttpWebRequest)WebRequest.Create(new Uri(RequestUrl));
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = "POST";
            //new Base_SysLogBll().InsertLog("请求的参数--->" + reqJson);

            var pay = @"{""pay_load"":""" + json + @""",""oid_partner"":""201706121001811585""}";

            //new Base_SysLogBll().InsertLog("支付请求加密报文--->" + pay);

            byte[] bytes = Encoding.UTF8.GetBytes(pay);

            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();

            var response = http.GetResponse();

            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();

            //调用付款接口，同步返回0000，是指创建连连支付单成功，订单处于付款处理中状态，最终的付款状态由异步通知告知
            //出现1002，2005，4006，4007，4009，9999这6个返回码时或者没返回码，抛exception（或者对除了0000之后的code都查询一遍查询接口）调用付款结果查询接口，
            //明确订单状态，不能私自设置订单为失败状态，以免造成这笔订单在连连付款成功了，而商户设置为失败,用户重新发起付款请求,造成重复付款，商户资金损失
            //写入日志
            //new Base_SysLogBll().InsertLog("返回的结果---->" + content);
            //对连连响应报文内容需要用连连公钥验签
            return content;
        }

        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="SDParam">排序字典</param>
        /// <returns></returns>
        public static string ReturnResultNoPay(string RequestUrl, SortedDictionary<string, string> SDParam)
        {
            string reqJson = YinTongUtil.dictToJson(SDParam);

            var http = (HttpWebRequest)WebRequest.Create(new Uri(RequestUrl));
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = "POST";
            //new Base_SysLogBll().InsertLog("请求的参数--->" + reqJson);

            byte[] bytes = Encoding.UTF8.GetBytes(reqJson);

            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();

            var response = http.GetResponse();

            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();

            //调用付款接口，同步返回0000，是指创建连连支付单成功，订单处于付款处理中状态，最终的付款状态由异步通知告知
            //出现1002，2005，4006，4007，4009，9999这6个返回码时或者没返回码，抛exception（或者对除了0000之后的code都查询一遍查询接口）调用付款结果查询接口，
            //明确订单状态，不能私自设置订单为失败状态，以免造成这笔订单在连连付款成功了，而商户设置为失败,用户重新发起付款请求,造成重复付款，商户资金损失
            //写入日志
            //new Base_SysLogBll().InsertLog("返回的结果---->" + content);
            //对连连响应报文内容需要用连连公钥验签
            return content;
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        public static void InitParam()
        {
            dicLLPay.Clear();
            dicLLPay.Add("api_version", "1.0");
            dicLLPay.Add("sign_type", "RSA");
        }
        
        /// <summary>
        /// 付款交易
        /// </summary>
        public static string Payment(PaymentParam pp,string strAcctName,string strBankName,string strCardNo)
        {
            string result = string.Empty;
            InitParam();
            //付款交易参数
            dicLLPay.Add("acct_name", strAcctName);
            dicLLPay.Add("bank_name", strBankName);
            dicLLPay.Add("card_no", strCardNo);
            dicLLPay.Add("dt_order", DateTime.Now.ToString("yyyyMMddHHmmss"));
            dicLLPay.Add("flag_card", "0");
            dicLLPay.Add("info_order", "提现");//订单描述
            dicLLPay.Add("money_order", pp.money_order);
            dicLLPay.Add("no_order", Guid.NewGuid().ToString().Replace("-", ""));
            dicLLPay.Add("notify_url", "http://www.ymstudio.xyz:81/Api/LLPay/PayResultNotice");
            dicLLPay.Add("oid_partner", SH_Number);//商户编号
            dicLLPay.Add("platform", pp.platform);
            //签名
            string sign = YinTongUtil.addSign(dicLLPay, PartnerConfig.TRADER_PRI_KEY, string.Empty);
            dicLLPay["sign"] = sign;

            return ReturnResult(LLPay_URL + "payment.htm", dicLLPay);
        }

        /// <summary>
        /// 确认付款
        /// </summary>
        public static string ConfirmPayment(ConfirmPaymentParam cpp)
        {
            string result = string.Empty;
            InitParam();
            //基本信息
            dicLLPay.Add("oid_partner", cpp.oid_partner);
            dicLLPay.Add("platform", cpp.platform);
            //订单信息
            dicLLPay.Add("no_order", cpp.no_order);
            dicLLPay.Add("confirm_code", cpp.confirm_code);
            dicLLPay.Add("notify_url", cpp.notify_url);
            //签名
            string sign = YinTongUtil.addSign(dicLLPay, PartnerConfig.TRADER_PRI_KEY, string.Empty);
            dicLLPay["sign"] = sign;

            return ReturnResult(LLPay_URL + "confirmPayment.htm", dicLLPay);
        }

        /// <summary>
        /// 付款结果查询
        /// </summary>
        public static string QueryPayment(QueryPaymentParam qpp)
        {
            string result = string.Empty;
            dicLLPay.Add("api_version", "1.0");
            dicLLPay.Add("sign_type", "RSA");
            //基本信息
            dicLLPay.Add("oid_partner", qpp.oid_partner);
            dicLLPay.Add("platform", qpp.platform);
            //订单信息
            dicLLPay.Add("no_order", qpp.no_order);
            dicLLPay.Add("oid_paybill", qpp.oid_paybill);
            //签名
            string sign = YinTongUtil.addSign(dicLLPay, PartnerConfig.TRADER_PRI_KEY, string.Empty);
            dicLLPay["sign"] = sign;

            return ReturnResultNoPay("https://instantpay.lianlianpay.com/paymentapi/queryPayment.htm", dicLLPay);

            //result = ow.HttpPostData(LLPay_URL + "queryPayment.htm", FormatParam(dicLLPay));

            //return result;
        }
    }

    /// <summary>
    /// 付款交易参数
    /// </summary>
    public class PaymentParam
    {
        ///// <summary>
        ///// 商户编号
        ///// </summary>
        //public string oid_partner { get; set; }
        /// <summary>
        /// 平台来源
        /// </summary>
        public string platform { get; set; }
        ///// <summary>
        ///// 版本号
        ///// </summary>
        //public string api_version { get; set; }
        ///// <summary>
        ///// 签名方式
        ///// </summary>
        //public string sign_type { get; set; }
        ///// <summary>
        ///// 签名
        ///// </summary>
        //public string sign { get; set; }
        ///// <summary>
        ///// 商户付款流水号
        ///// </summary>
        //public string no_order { get; set; }
        ///// <summary>
        ///// 商户时间
        ///// </summary>
        //public string dt_order { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public string money_order { get; set; }
        ///// <summary>
        ///// 银行帐号
        ///// </summary>
        //public string card_no { get; set; }
        ///// <summary>
        ///// 收款人姓名
        ///// </summary>
        //public string acct_name { get; set; }
        ///// <summary>
        ///// 收款人银行名称
        ///// </summary>
        //public string bank_name { get; set; }
        ///// <summary>
        ///// 订单描述
        ///// </summary>
        //public string info_order { get; set; }
        ///// <summary>
        ///// 对公对私标志
        ///// </summary>
        //public string flag_card { get; set; }
        ///// <summary>
        ///// 服务器异步通知地址
        ///// </summary>
        //public string notify_url { get; set; }
        /// <summary>
        /// 收款备注
        /// </summary>
        public string memo { get; set; }
        ///// <summary>
        ///// 大额行号
        ///// </summary>
        //public string prcptcd { get; set; }
        ///// <summary>
        ///// 银行编码
        ///// </summary>
        //public string bank_code { get; set; }
        ///// <summary>
        ///// 开户行所在市编码
        ///// </summary>
        //public string city_code { get; set; }
        ///// <summary>
        ///// 开户支行名称
        ///// </summary>
        //public string brabank_name { get; set; }
    }

    /// <summary>
    /// 确认付款参数
    /// </summary>
    public class ConfirmPaymentParam
    {
        /// <summary>
        /// 商户编号
        /// </summary>
        public string oid_partner { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        //public string sign_type { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        //public string sign { get; set; }
        /// <summary>
        /// 平台来源
        /// </summary>
        public string platform { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        //public string api_version { get; set; }
        /// <summary>
        /// 商户付款流水号
        /// </summary>
        public string no_order { get; set; }
        /// <summary>
        /// 确认付款验证码
        /// </summary>
        public string confirm_code { get; set; }
        /// <summary>
        /// 服务器异步通知
        /// </summary>
        public string notify_url { get; set; }
    }

    /// <summary>
    /// 付款结果查询
    /// </summary>
    public class QueryPaymentParam
    {
        /// <summary>
        /// 商户编号
        /// </summary>
        public string oid_partner { get; set; }
        /// <summary>
        /// 平台来源
        /// </summary>
        public string platform { get; set; }
        ///// <summary>
        ///// 版本号
        ///// </summary>
        //public string api_version { get; set; }
        ///// <summary>
        ///// 签名方式
        ///// </summary>
        //public string sign_type { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        //public string sign { get; set; }
        /// <summary>
        /// 商户付款流水号
        /// </summary>
        public string no_order { get; set; }
        /// <summary>
        /// 连连支付支付单号
        /// </summary>
        public string oid_paybill { get; set; }
    }

    /// <summary>
    /// 连连支付基本信息
    /// </summary>
    public class LLPayBaseInfo<T>
    {
        /// <summary>
        /// 商户编号
        /// </summary>
        public string oid_partner { get; set; }
        /// <summary>
        /// 平台来源
        /// </summary>
        public string platform { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string api_version { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 其他类对象加载
        /// </summary>
        public T other { get; set; }
    }

    /// <summary>
    /// 连连支付订单信息
    /// </summary>
    public class LLPayOrderInfo
    {
        /// <summary>
        /// 商户付款流水号
        /// </summary>
        public string no_order { get; set; }
        /// <summary>
        /// 商户时间
        /// </summary>
        public string dt_order { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public string money_order { get; set; }
        /// <summary>
        /// 银行帐号
        /// </summary>
        public string card_no { get; set; }
        /// <summary>
        /// 收款人姓名
        /// </summary>
        public string acct_name { get; set; }
        /// <summary>
        /// 收款人银行名称
        /// </summary>
        public string bank_name { get; set; }
        /// <summary>
        /// 订单描述
        /// </summary>
        public string info_order { get; set; }
        /// <summary>
        /// 对公对私标志
        /// </summary>
        public string flag_card { get; set; }
        /// <summary>
        /// 服务器异步通知
        /// </summary>
        public string notify_url { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 收款备注
        /// </summary>
        public string memo { get; set; }
        /// <summary>
        /// 大额行号
        /// </summary>
        public string prcptcd { get; set; }
        /// <summary>
        /// 银行编码
        /// </summary>
        public string bank_code { get; set; }
        /// <summary>
        /// 开户行所在市编码
        /// </summary>
        public string city_code { get; set; }
        /// <summary>
        /// 开户支行名称
        /// </summary>
        public string brabank_name { get; set; }
        /// <summary>
        /// 确认付款验证码
        /// </summary>
        public string confirm_code { get; set; }
        /// <summary>
        /// 连连支付支付单号
        /// </summary>
        public string oid_paybill { get; set; }
    }
}

