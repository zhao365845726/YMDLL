using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using YMDLL.Class;
using YMDLL.Common;
using com.lianlianpay.security.utils;

namespace ML.ThirdParty
{
    public class LLPayHelper
    {
        public static CS_OperaWeb ow = new CS_OperaWeb();
        public static CS_CalcDateTime cdt = new CS_CalcDateTime();
        public static string LLPay_URL = "https://instantpay.lianlianpay.com/paymentapi/";
        public static string SH_Secret = "MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBAMer1kZdlqdrxbjchHqMObRvfFYD0RDao/B0ev9HmPRcrlaz3O3eKcOF5NBOJslphTbgSG1saRVZVNRDYXie50TtNGbnClB0AW8c9Ixf4KAzmCr7OQ7q4fRjzXottZbbmwApuCBTcFwJHbl1aLFpSoX5NyVclr22xFTHc3HZ7TJHAgMBAAECgYBgSzjdWokrWMhULNUfHL0/jXyTJugOjsL9Vc9ziZ30SzYwzjE/7iKKDuqYovgFroP2QRfs3ZmYGCrW61/4gfnZ48ViR/ZjMGEWtmo9jEM6SZogvlx+LOOrCZykNFGCW7nmLgZyUckCT0ijU6LsPwPMWbf/+tbuxZ0dIG5/SLqGkQJBAObaLP8vTnUB9vPJby+wRusrP1Gln43GteialDuS6dI8HOlIGDo/p3WdvXExTXf5vR+OTbW8coDyjOn8pq2IeC0CQQDdbBwnD3PKgmq8SL49OR/ZX/VMEAlxFOT8LnYIQAH12SVQTto0FjfvaHESTtDV/JnkVQdif/PbvDaXkQG1AUjDAkAF/siIYAwjkcd+EU8n5+YPmXHthuWb4vs6bTlISspzwUfm7w5iBOEudshCtksSwJOezC1MePZoTuRF91/ExfSJAkB7RIWDxVl8IxDS01h9cwDlHkPMXZ00BCLate7l9uRgfswEInHdz4TCVo2kWJZwmtj9wcyDrKIQ8X4e8Q5XO2jLAkApyS9I0Nktd/SksJOcU64EQY+DGflI7/4+DZc+APMy2nleR1wDLjtCexPPkwkjtyOk96hI+1EO+jK2S4KIqN/J";
        public static Dictionary<string, string> dic_LLRequestUrl;
        public static SortedDictionary<string, string> dicLLPay = new SortedDictionary<string, string>();

        /// <summary>
        /// 连连支付助手的构造函数
        /// </summary>
        public LLPayHelper() {
            ////付款交易地址
            //dic_LLRequestUrl.Add("Payment", LLPay_URL + "payment.htm");
            ////确认付款地址
            //dic_LLRequestUrl.Add("ConfirmPayment", LLPay_URL + "confirmPayment.htm");
            ////付款结果查询地址
            //dic_LLRequestUrl.Add("QueryPayment", LLPay_URL + "queryPayment.htm");
        }

        /// <summary>
        /// 自动生成签名
        /// </summary>
        /// <param name="dicParam">所有参数的集合</param>
        /// <param name="strSecret">商户秘钥</param>
        /// <returns></returns>
        public static string GeneralKeyGen(SortedDictionary<string, string> dicParam, string strSecret)
        {
            string strUrlParam = "";
            List<String> lst_Param = new List<string>();
            foreach (KeyValuePair<string, string> kv in dicParam.OrderBy(p => p.Key))
            {
                //如果字符为空或者null直接去掉这个参数
                if (kv.Value == null || kv.Value.ToString() == "" || kv.Key == "sign")
                {
                    continue;
                }
                lst_Param.Add(kv.Key + "=" + kv.Value);
            }
            strUrlParam = string.Join("&", lst_Param.ToArray());
            return Md5.GetMD5String(strUrlParam + "&key=" + strSecret);
        }

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
        public static string Payment(PaymentParam pp)
        {
            string result = string.Empty;
            InitParam();
            //Dictionary<string, string> dicPayment = new Dictionary<string, string>();
            //付款交易参数
            dicLLPay.Add("oid_partner", pp.oid_partner);
            //dicLLPay.Add("platform", pp.platform);
            //订单参数
            dicLLPay.Add("no_order", DateTime.Now.ToString("yyyyMMddHHmmss"));
            dicLLPay.Add("dt_order", DateTime.Now.ToString("yyyyMMddHHmmss"));
            dicLLPay.Add("money_order", pp.money_order);
            dicLLPay.Add("card_no", pp.card_no);
            dicLLPay.Add("acct_name", pp.acct_name);
            //dicLLPay.Add("bank_name", pp.bank_name);
            //dicLLPay.Add("info_order", pp.info_order);
            dicLLPay.Add("flag_card", pp.flag_card);
            dicLLPay.Add("notify_url", pp.notify_url);
            //dicLLPay.Add("address", pp.address);
            //dicLLPay.Add("memo", pp.memo);
            //dicLLPay.Add("prcptcd", pp.prcptcd);
            //dicLLPay.Add("bank_code", pp.bank_code);
            //dicLLPay.Add("city_code", pp.city_code);
            //dicLLPay.Add("brabank_name", pp.brabank_name);
            //dicLLPay.Add("sign", YinTongUtil.addSign(dicLLPay, PartnerConfig.TRADER_PRI_KEY, string.Empty));

            //string reqJson = YinTongUtil.dictToJson(dicLLPay);
            //var json = "";

            //try
            //{
            //    json = LianLianPaySecurity.encrypt(reqJson, PartnerConfig.YT_PUB_KEY);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("异常信息：" + ex.ToString());
            //}

            //var http = (HttpWebRequest)WebRequest.Create(new Uri("https://instantpay.lianlianpay.com/paymentapi/payment.htm"));
            //http.Accept = "application/json";
            //http.ContentType = "application/json";
            //http.Method = "POST";
            //Console.WriteLine(reqJson);

            //var pay = @"{""pay_load"":""" + json + @""",""oid_partner"":""" + pp.oid_partner + @"""}";

            //byte[] bytes = Encoding.UTF8.GetBytes(pay);

            //Stream newStream = http.GetRequestStream();
            //newStream.Write(bytes, 0, bytes.Length);
            //newStream.Close();

            //var response = http.GetResponse();

            //var stream = response.GetResponseStream();
            //var sr = new StreamReader(stream);
            //var content = sr.ReadToEnd();

            //result = ow.HttpPostData(LLPay_URL + "payment.htm", FormatParam(dicLLPay),"application/json");

            return "";
        }

        /// <summary>
        /// 确认付款
        /// </summary>
        public static string ConfirmPayment(ConfirmPaymentParam cpp)
        {
            string result = string.Empty;
            InitParam();
            //Dictionary<string, string> dicConfirmPayment = new Dictionary<string, string>();
            //基本信息
            dicLLPay.Add("oid_partner", cpp.oid_partner);
            dicLLPay.Add("platform", cpp.platform);
            //dicLLPay.Add("api_version", cpp.api_version);
            //dicLLPay.Add("sign_type", cpp.sign_type);
            //订单信息
            dicLLPay.Add("no_order", cpp.no_order);
            dicLLPay.Add("confirm_code", cpp.confirm_code);
            dicLLPay.Add("notify_url", cpp.notify_url);
            dicLLPay.Add("sign", GeneralKeyGen(dicLLPay, SH_Secret));
            result = ow.HttpPostData(LLPay_URL + "confirmPayment.htm", FormatParam(dicLLPay));

            return result;
        }

        /// <summary>
        /// 付款结果查询
        /// </summary>
        public static string QueryPayment(QueryPaymentParam qpp)
        {
            string result = string.Empty;
            InitParam();
            //Dictionary<string, string> dicQueryPayment = new Dictionary<string, string>();
            //基本信息
            dicLLPay.Add("oid_partner", qpp.oid_partner);
            dicLLPay.Add("platform", qpp.platform);
            //dicLLPay.Add("api_version", qpp.api_version);
            //dicLLPay.Add("sign_type", qpp.sign_type);
            //订单信息
            dicLLPay.Add("no_order", qpp.no_order);
            dicLLPay.Add("oid_paybill", qpp.oid_paybill);
            dicLLPay.Add("sign", GeneralKeyGen(dicLLPay, SH_Secret));
            result = ow.HttpPostData(LLPay_URL + "queryPayment.htm", FormatParam(dicLLPay));

            return result;
        }
    }

    /// <summary>
    /// 付款交易参数
    /// </summary>
    public class PaymentParam
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
        public string sign { get; set; }
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
        public string sign { get; set; }
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

