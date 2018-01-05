using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using YMDLL.Class;
using YMDLL.Common;

namespace ML.ThirdParty
{
    public class AliPayHelper
    {
        public static CS_OperaWeb ow = new CS_OperaWeb();
        public static CS_CalcDateTime cdt = new CS_CalcDateTime();
        public static string AliPay_ZS_URL = "https://openapi.alipay.com/";
        public static string AliPay_CS_URL = "https://openapi.alipaydev.com/";
        public static string AliPay_Public_Secret = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA1/g9IekMBU1W5ouwIJFFfuitPEGybUrCBQ9kJrET1oiT0znOLVpFi5zb2vOmOq9wNJO4HRf6xBRyyXR0MVbGEkMxSJzzBOa+n+Ugu4aAjWUbHp8szF0IUg7zdwsy1riL1UtYTVBolDuGuheQevX4Khr6kiXfvPnY0S0guH0lJCjxqW4Swlo59XQUjhr5pTb1mHcQdh/ID5olgE1h9Nch4owu6W7kGzbjA8TMCGWO53FWP/6t1OyTK1WU8exas4rC2/Y5eNzLU+g8NAVYRJvDMdBxM+xI1zUoEk4HtmGrGTYuzsueM8jYLxj9JInulj5psVCcTRi6jTohsV3k9yUNVwIDAQAB";
        public static string AliPay_Private_Secret = "MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQC+LIn6RkyF5ixvxxxEpS9d4lL9MCsS73Kui2PN5ZbkkdnC3Qd7VDe5bajPwK4l8hKmEgWFNifc30/402o/V0ol/KLBnD1hyrTWv6i8SkQllE9uWAwDKkC5KJ8PtAPfbDmIuH60AitysnaN0ndcuBDttDr9n2M2sOpbA8VwTsPX4xWrmiY3UesAYn3rvzK3WGUsYkvSJtUBQYTATzLX/hQSJboLkFhABJ4wZ8ZdW54ZychjeoBVIMA8Sl6U6RrARS0FtL/NTTyiYne6GAyxQ+KuQpLFcvKGK7ujzlhzs+RO3EUoqFaYjJwWHkc1yB1bIKPokkiL7awpt0N2xuZhxwK/AgMBAAECggEANe63qqypIwgl7gVPtQALsNpworE4ZgRWR73sLgZAhs3+Lc81QfqeWw4IY/LG8Kf4NfTCcAbtwyvpqC184j3FH4TIO5QtrEAqjQSJu+xr6yRmamq57WPcOyaUBYYN2xz8LBP2iuJapAqMqKfZhggFdQQ5/pdnMal2rR0irzkHPYAB6rS8SrEUcNecUkqDrhJbW8yeoIG3D0k9lkvgzTGZOqzHlHVNoku/9CPLf9y+Ge3gBz8WOJt0sB/wllJDBqCixHLPy0gPqKAgn9nY30tPE/Jf8t5UtBPkWaVX8XEgWL5uJtq+f7B978faXTa+3cjJL8cpuOBvpU9w8sTrcBcXYQKBgQDnEalIzpY7+Q5HOUaB8qhcTfhmN7i1Y7SmGHXHUvC54c2acCsAzn/SwW1VsEZpB4QFq/xMt95zfZvpw9vd0A8aZf7KMwpUGE14f4BGHTFthsYc3nxCmGgqecZHlpJDjSq3vWdaRQJdcXNTz8lRDz0hKbr/uWG9sHfnrxfZggXeZwKBgQDSsVHftBWjfLx3IePZ44pbe2FZ69sww4jH2kTQIuwKmZqCfGSDjXzwre2Uk0uBEOU7/lSAefKIFpPHyVOhlcbtK/YkWX+4dZbsAD2TKKlUr5Q1QiRzICPsNyX8/bumS6Qt8uLPK3jeB61DRZWCi07gwfoAWBhjv1BL9vwgK8dR6QKBgQDFvzGWo2Wi0ZPMlFGo6Bf5VqjcIZerdDanZot1F5/4FCw2nQPQXui5XtvI12CKheoQSI1Uwo5XXAnQVtOU0nf6aYtRU7jlGx9BniwS1Oz2BL8K+cRx4ahToX90yIFH2knSkP8gG6rYeTzvXwW0n0v7U5E7RFVHVvxmhEvu0v1exQKBgBnSYZZpG73Nq8tt+97YZDySAnxiy5pOXJUhyw03OnaSHL2hQTBeRtG4f0WWR8qpp26S3Qhh6beEDNiPdBB5qseme6Q4085nmveRj/pZIWefnSpp0M3LQpvQpnc7IaRf4N9AxUteqJGNn05/WLOHH0OsgldIdLTE3bJxsrS+qM8BAoGASR03HFYepq+IHLDhO7xetUxLknBln3+FJoJ7D6U2UkLfGaAdlKhqR3KyngcELV94GSaPGFhi+QooEPvJ8XpDv/+FjgpPpTQ8ElT/lChHcwA89smEOSAz0tR09ZTvz2aTNe3H0UxAo26TKq2aOKMFncWPtnHIrg/qM3EgtsAmiNk=";
        public static string AliPay_CS_Public_Secret = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAuaQkCVnycOLQ/YOcsUES8MFmq9EUJd4BOhYENC9IOisdCNZ4EkoSAxsvo98vtZnKFo8JuEI8z3jWmHC7CU67qHCcpmKR/L6JMdCX7x5WqtkluU4g9jPdWLAeAK0IY77AoOda0IWyQjJ0Y30yIsz5BYXgQ6iP0b7FnyDdkEceLWoOeWrVC0TnoWtGY5ExINJ4khVDkUE5VZRxwixop522SGb5NtN9dFTmVlc/KSLV9ZMImWlsQZHuoDeK0vTqCcLHtpNggb7ZVmt0Z8vpFbkgixR9viNnknOb0eFiP/LZR+Ud2Ps+kNR2O5vuEx0wIUCbw119+T436hgCvsqmteNq7wIDAQAB";
        public static string AliPay_CS_Private_Secret = "MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQC5pCQJWfJw4tD9g5yxQRLwwWar0RQl3gE6FgQ0L0g6Kx0I1ngSShIDGy+j3y+1mcoWjwm4QjzPeNaYcLsJTruocJymYpH8vokx0JfvHlaq2SW5TiD2M91YsB4ArQhjvsCg51rQhbJCMnRjfTIizPkFheBDqI/RvsWfIN2QRx4tag55atULROeha0ZjkTEg0niSFUORQTlVlHHCLGinnbZIZvk20310VOZWVz8pItX1kwiZaWxBke6gN4rS9OoJwse2k2CBvtlWa3Rny+kVuSCLFH2+I2eSc5vR4WI/8tlH5R3Y+z6Q1HY7m+4THTAhQJvDXX35PjfqGAK+yqa142rvAgMBAAECggEAefVS023rL6xjHlnLpEubFN4KBJC8CtCZv75dDqeNbOGMFpVHlsRgpvfCSYdDwauCL2XTPeEOMEMzwDw3NDssX9FzqH+TZxcJ7NccKbSlvWIhPWcNkpGqNiyl2U3T+CuGQ28ao2liHckl24KRYHmGk9FK70gbC/GnxQ4DYVjHg49N9HJNEj9MPYuRMiW6WJvBdi4eLARPrmWInNXxIzYuUh5IYAg023alOfOEq/YEe1Dug+VHwqEO7UZ5B/0V8yzcFzBxerkxO7I03k3SUGxnNdOLa81hywrvjqHyx3YF7KXt2Lrrs7XXROSq3JBUYSWQL4kdhXkPYZ/WAY7H3W4rAQKBgQDcaPwD4BEBMscdaz/OxkXgqGXqzvRlqZV3TDgN2bIVPeurBt93jpfraceQd/7D83ZanBZJrGAPvFByBPWSeiepRw/1i6jyd83wWK+MleWcHx3SCoNqkhNvaIVsKMTknmBZ7iuUKjvgk6iT4zlfs+RZ9i8akBoOenSmzd4uxUTxgQKBgQDXnezwJL/PT0ELMFKYHs3qGmhzrNm13GZxT5IOudPQCRGjYMiGC4h/aaJqBqsXv/9O2DSwMkk/H1RWunJZARcWxuonGxnCzvgUh3CA+EwmR3miHWW+rJgGifITFhAX25oGW8pRVWjBi1DLFgxGt7tY+lsyN4gDDqpZBnkM6zC0bwKBgGFXvHW5r8jntCi6BSQ9TP87YEejvyxnCSv27YqwaoYXIs4V6vvSbiX6Qbj8RfgkvlCmPvGqv4IzmRPCPPLEIGASkmnPlH4Bi2JqyJ0+VnntBC6PlhqrCQpbDxIFr2+IwuT02ypEvM0iaYFnCR/LQz64C3WfzuI2Eu7YXRgq55gBAoGAYXxuxzjmXFXqA6QKg+rRkPn1pe9N09Ldonemgu/z24huKB83KpwGUA0zuJphOvC8/ynz0II76cVBfaD42FMM3Gh9L8cqA5FOSwHdlQIuc9Q3I1wFR4uDhNlT0NtV1WOdpShVNibWoUNa+Sdzz3l8xEMVfoPrA+7aiDvL+VJRCBMCgYEAoUcH4wZ5PRLb6VOLoKc74zNZ7z5gj0xxGxi+koDVQctSFbXw6U6JEkSao46ia+dxaKW+8VFs3KKAcPM9wETopAKqrfWO91nWJCyIB9c7QzLUT72mnDKw6kbLBvnAMCjQ8+JMjukY/NqpUnJv40+SRVFdDUUw6wag+pGDjStkFrE=";
        public static string APPID = "2017072007825548";
        public static string CS_APPID = "2016080500175871";
        public static string CHARSET = "utf-8";
        public static Dictionary<string, string> dicAliPay = new Dictionary<string, string>();

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
        /// 将字典格式化为Json格式字符串
        /// </summary>
        /// <param name="dicParam"></param>
        /// <returns></returns>
        public static string FormatToJson(Dictionary<string, string> dicParam)
        {
            int i = 0;
            string str_Format = "{";
            string strValue = "";
            foreach (KeyValuePair<string, string> dicItem in dicParam)
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
                    str_Format += "\"" + dicItem.Key + "\":\"" + strValue + "\"}";
                }
                else
                {
                    str_Format += "\"" + dicItem.Key + "\":\"" + strValue + "\",";
                }
                i++;
            }
            return str_Format;
        }

        /// <summary>
        /// 支付宝App支付
        /// </summary>
        /// <param name="crp">公共请求参数</param>
        /// <returns></returns>
        public static string Pay()
        {
            string result = string.Empty;
            IAopClient client = new DefaultAopClient(AliPay_CS_URL + "gateway.do", CS_APPID, AliPay_CS_Private_Secret, "json", "1.0", "RSA2", AliPay_CS_Public_Secret, "UTF-8", false);
            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
            model.Body = "我是测试数据";
            model.Subject = "App支付测试DoNet";
            model.TotalAmount = "0.01";
            model.ProductCode = "QUICK_MSECURITY_PAY";
            model.OutTradeNo = "20170216test01";
            model.TimeoutExpress = "30m";
            request.SetBizModel(model);
            request.SetNotifyUrl("外网商户可以访问的异步地址");
            //这里和普通的接口调用不同，使用的是sdkExecute
            AlipayTradeAppPayResponse response = client.SdkExecute(request);
            result = HttpUtility.HtmlEncode(response.Body);









            //request.BizContent = "{" +
            //"\"out_trade_no\":\"20150320010101001\"," +
            //"\"scene\":\"bar_code\"," +
            //"\"auth_code\":\"28763443825664394\"," +
            //"\"product_code\":\"FACE_TO_FACE_PAYMENT\"," +
            //"\"subject\":\"Iphone6 16G\"," +
            //"\"buyer_id\":\"2088202954065786\"," +
            //"\"seller_id\":\"2088102146225135\"," +
            //"\"total_amount\":88.88," +
            //"\"discountable_amount\":8.88," +
            //"\"undiscountable_amount\":80.00," +
            //"\"body\":\"Iphone6 16G\"," +
            //"      \"goods_detail\":[{" +
            //"        \"goods_id\":\"apple-01\"," +
            //"\"alipay_goods_id\":\"20010001\"," +
            //"\"goods_name\":\"ipad\"," +
            //"\"quantity\":1," +
            //"\"price\":2000," +
            //"\"goods_category\":\"34543238\"," +
            //"\"body\":\"特价手机\"," +
            //"\"show_url\":\"http://www.alipay.com/xxx.jpg\"" +
            //"        }]," +
            //"\"operator_id\":\"yx_001\"," +
            //"\"store_id\":\"NJ_001\"," +
            //"\"terminal_id\":\"NJ_T_001\"," +
            //"\"alipay_store_id\":\"2016041400077000000003314986\"," +
            //"\"extend_params\":{" +
            //"\"sys_service_provider_id\":\"2088511833207846\"," +
            //"\"hb_fq_num\":\"3\"," +
            //"\"hb_fq_seller_percent\":\"100\"" +
            //"    }," +
            //"\"timeout_express\":\"90m\"," +
            //"\"royalty_info\":{" +
            //"\"royalty_type\":\"ROYALTY\"," +
            //"        \"royalty_detail_infos\":[{" +
            //"          \"serial_no\":1," +
            //"\"trans_in_type\":\"userId\"," +
            //"\"batch_no\":\"123\"," +
            //"\"out_relation_id\":\"20131124001\"," +
            //"\"trans_out_type\":\"userId\"," +
            //"\"trans_out\":\"2088101126765726\"," +
            //"\"trans_in\":\"2088101126708402\"," +
            //"\"amount\":0.1," +
            //"\"desc\":\"分账测试1\"," +
            //"\"amount_percentage\":\"100\"" +
            //"          }]" +
            //"    }," +
            //"\"sub_merchant\":{" +
            //"\"merchant_id\":\"19023454\"" +
            //"    }," +
            //"\"disable_pay_channels\":\"credit_group\"," +
            //"\"ext_user_info\":{" +
            //"\"name\":\"李明\"," +
            //"\"mobile\":\"16587658765\"," +
            //"\"cert_type\":\"IDENTITY_CARD\"," +
            //"\"cert_no\":\"362334768769238881\"," +
            //"\"fix_buyer\":\"F\"" +
            //"    }" +
            //"  }";
            //AlipayTradePayResponse response = client.Execute(request);
            //result = request.ToString();
            //Console.WriteLine(response.Body);

            return result;
        }

        /// <summary>
        /// 交易关闭
        /// </summary>
        /// <returns></returns>
        public static string Close(CloseParam cp)
        {
            string result = string.Empty;
            IAopClient client = new DefaultAopClient(AliPay_CS_URL + "gateway.do", CS_APPID, AliPay_CS_Private_Secret, "json", "1.0", "RSA2", AliPay_CS_Public_Secret, "UTF-8", false);
            AlipayTradeCloseRequest request = new AlipayTradeCloseRequest();
            dicAliPay.Clear();
            dicAliPay.Add("trade_no", cp.trade_no);
            dicAliPay.Add("out_trade_no", cp.out_trade_no);
            dicAliPay.Add("operator_id", cp.operator_id);
            request.BizContent = FormatToJson(dicAliPay);

            AlipayTradeCloseResponse response = client.Execute(request);
            result = request.ToString();
            Console.WriteLine(response.Body);

            return result;
        }

        /// <summary>
        /// 交易状态查询
        /// </summary>
        /// <returns></returns>
        public static string Query(QueryParam qp)
        {
            string result = string.Empty;
            IAopClient client = new DefaultAopClient(AliPay_CS_URL + "gateway.do", CS_APPID, AliPay_CS_Private_Secret, "json", "1.0", "RSA2", AliPay_CS_Public_Secret, "UTF-8", false);
            AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
            dicAliPay.Clear();
            dicAliPay.Add("trade_no", qp.trade_no);
            dicAliPay.Add("out_trade_no", qp.out_trade_no);
            request.BizContent = FormatToJson(dicAliPay);

            AlipayTradeQueryResponse response = client.Execute(request);
            result = request.ToString();
            Console.WriteLine(response.Body);

            return result;
        }

        /// <summary>
        /// 交易退款
        /// </summary>
        /// <returns></returns>
        public static string Refund(AliRefundParam arp)
        {
            string result = string.Empty;
            IAopClient client = new DefaultAopClient(AliPay_CS_URL + "gateway.do", CS_APPID, AliPay_CS_Private_Secret, "json", "1.0", "RSA2", AliPay_CS_Public_Secret, "UTF-8", false);

            AlipayTradeRefundRequest request = new AlipayTradeRefundRequest();
            dicAliPay.Clear();
            dicAliPay.Add("out_trade_no", arp.out_trade_no);
            dicAliPay.Add("trade_no", arp.trade_no);
            dicAliPay.Add("refund_amount", arp.refund_amount);
            dicAliPay.Add("refund_reason", arp.refund_reason);
            dicAliPay.Add("out_request_no", arp.out_request_no);
            dicAliPay.Add("operator_id", arp.operator_id);
            dicAliPay.Add("store_id", arp.store_id);
            dicAliPay.Add("terminal_id", arp.terminal_id);
            request.BizContent = FormatToJson(dicAliPay);

            AlipayTradeRefundResponse response = client.Execute(request);
            result = request.ToString();
            Console.WriteLine(response.Body);

            return result;
        }

        /// <summary>
        /// 退款查询
        /// </summary>
        /// <returns></returns>
        public static string RefundQuery(AliRefundQueryParam arqp)
        {
            string result = string.Empty;
            IAopClient client = new DefaultAopClient(AliPay_CS_URL + "gateway.do", CS_APPID, AliPay_CS_Private_Secret, "json", "1.0", "RSA2", AliPay_CS_Public_Secret, "UTF-8", false);
            AlipayTradeFastpayRefundQueryRequest request = new AlipayTradeFastpayRefundQueryRequest();
            dicAliPay.Clear();
            dicAliPay.Add("trade_no", arqp.trade_no);
            dicAliPay.Add("out_trade_no", arqp.out_trade_no);
            dicAliPay.Add("out_request_no", arqp.out_request_no);
            request.BizContent = FormatToJson(dicAliPay);

            AlipayTradeFastpayRefundQueryResponse response = client.Execute(request);
            result = request.ToString();
            Console.WriteLine(response.Body);

            return result;
        }

        /// <summary>
        /// 账单查询
        /// </summary>
        /// <returns></returns>
        public static string DownloadUrlQuery()
        {
            string result = string.Empty;
            IAopClient client = new DefaultAopClient(AliPay_CS_URL + "gateway.do", CS_APPID, AliPay_CS_Private_Secret, "json", "1.0", "RSA2", AliPay_CS_Public_Secret, "UTF-8", false);
            AlipayDataBillDownloadurlGetRequest request = new AlipayDataBillDownloadurlGetRequest();
            //request.BizContent = "{" +
            //"\"bill_type\":\"20150320010101001\"," +
            //"\"bill_date\":\"20150320010101001\"," +
            //"  }";
            AlipayDataBillDownloadurlGetResponse response = client.Execute(request);
            result = request.ToString();
            Console.WriteLine(response.Body);

            return result;
        }
    }

    /// <summary>
    /// 交易关闭参数
    /// </summary>
    public class CloseParam
    {
        /// <summary>
        /// 该交易在支付宝系统中的交易流水号。最短 16 位，最长 64 位。和out_trade_no不能同时为空，如果同时传了 out_trade_no和 trade_no，则以 trade_no为准。
        /// </summary>
        public string trade_no { get; set; }
        /// <summary>
        /// 订单支付时传入的商户订单号,和支付宝交易号不能同时为空。 trade_no,out_trade_no如果同时存在优先取trade_no
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 卖家端自定义的的操作员 ID
        /// </summary>
        public string operator_id { get; set; }
    }

    /// <summary>
    /// 线下交易查询参数
    /// </summary>
    public class QueryParam
    {
        /// <summary>
        /// 	订单支付时传入的商户订单号,和支付宝交易号不能同时为空。 trade_no,out_trade_no如果同时存在优先取trade_no
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 支付宝交易号，和商户订单号不能同时为空
        /// </summary>
        public string trade_no { get; set; }
    }

    /// <summary>
    /// 交易退款参数
    /// </summary>
    public class AliRefundParam
    {
        /// <summary>
        /// 订单支付时传入的商户订单号,不能和 trade_no同时为空。
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 支付宝交易号，和商户订单号不能同时为空
        /// </summary>
        public string trade_no { get; set; }
        /// <summary>
        /// 需要退款的金额，该金额不能大于订单金额,单位为元，支持两位小数
        /// </summary>
        public string refund_amount { get; set; }
        /// <summary>
        /// 退款的原因说明
        /// </summary>
        public string refund_reason { get; set; }
        /// <summary>
        /// 标识一次退款请求，同一笔交易多次退款需要保证唯一，如需部分退款，则此参数必传。
        /// </summary>
        public string out_request_no { get; set; }
        /// <summary>
        /// 商户的操作员编号
        /// </summary>
        public string operator_id { get; set; }
        /// <summary>
        /// 商户的门店编号
        /// </summary>
        public string store_id { get; set; }
        /// <summary>
        /// 商户的终端编号
        /// </summary>
        public string terminal_id { get; set; }
    }

    /// <summary>
    /// 交易退款查询参数
    /// </summary>
    public class AliRefundQueryParam
    {
        /// <summary>
        /// 支付宝交易号，和商户订单号不能同时为空
        /// </summary>
        public string trade_no { get; set; }
        /// <summary>
        /// 订单支付时传入的商户订单号,和支付宝交易号不能同时为空。 trade_no,out_trade_no如果同时存在优先取trade_no
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 请求退款接口时，传入的退款请求号，如果在退款请求时未传入，则该值为创建交易时的外部交易号
        /// </summary>
        public string out_request_no { get; set; }
    }

    ///// <summary>
    ///// 公共请求参数
    ///// </summary>
    //public class CommonRequestParam
    //{
    //    /// <summary>
    //    /// 支付宝分配给开发者的应用ID
    //    /// </summary>
    //    //public string app_id { get; set; }
    //    /// <summary>
    //    /// 接口名称
    //    /// </summary>
    //    public string method { get; set; }
    //    /// <summary>
    //    /// 仅支持JSON
    //    /// </summary>
    //    //public string format { get; set; }
    //    /// <summary>
    //    /// 请求使用的编码格式，如utf-8,gbk,gb2312等
    //    /// </summary>
    //    //public string charset { get; set; }
    //    /// <summary>
    //    /// 商户生成签名字符串所使用的签名算法类型，目前支持RSA2和RSA，推荐使用RSA2
    //    /// </summary>
    //    //public string sign_type { get; set; }
    //    /// <summary>
    //    /// 商户请求参数的签名串
    //    /// </summary>
    //    public string sign { get; set; }
    //    /// <summary>
    //    /// 发送请求的时间，格式"yyyy-MM-dd HH:mm:ss"
    //    /// </summary>
    //    //public string timestamp { get; set; }
    //    /// <summary>
    //    /// 调用的接口版本，固定为：1.0
    //    /// </summary>
    //    //public string version { get; set; }
    //    /// <summary>
    //    /// 支付宝服务器主动通知商户服务器里指定的页面http/https路径。
    //    /// </summary>
    //    public string notify_url { get; set; }
    //    /// <summary>
    //    /// 应用授权token
    //    /// </summary>
    //    public string app_auth_token { get; set; }
    //    /// <summary>
    //    /// 请求参数的集合，最大长度不限，除公共参数外所有请求参数都必须放在这个参数中传递，具体参照各产品快速接入文档
    //    /// </summary>
    //    public Root biz_content { get; set; }
    //}

    ///// <summary>
    ///// 请求参数
    ///// </summary>
    //public class Root
    //{
    //    /// <summary>
    //    /// 商户订单号,64个字符以内、可包含字母、数字、下划线；需保证在商户端不重复
    //    /// </summary>
    //    public string out_trade_no { get; set; }
    //    /// <summary>
    //    /// 支付场景  条码支付，取值：bar_code  声波支付，取值：wave_code
    //    /// </summary>
    //    public string scene { get; set; }
    //    /// <summary>
    //    /// 支付授权码，25~30开头的长度为16~24位的数字，实际字符串长度以开发者获取的付款码长度为准
    //    /// </summary>
    //    public string auth_code { get; set; }
    //    /// <summary>
    //    /// 销售产品码
    //    /// </summary>
    //    public string product_code { get; set; }
    //    /// <summary>
    //    /// 订单标题
    //    /// </summary>
    //    public string subject { get; set; }
    //    /// <summary>
    //    /// 买家的支付宝用户id，如果为空，会从传入了码值信息中获取买家ID
    //    /// </summary>
    //    public string buyer_id { get; set; }
    //    /// <summary>
    //    /// 如果该值为空，则默认为商户签约账号对应的支付宝用户ID
    //    /// </summary>
    //    public string seller_id { get; set; }
    //    /// <summary>
    //    /// 订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000] 如果同时传入【可打折金额】和【不可打折金额】，该参数可以不用传入； 如果同时传入了【可打折金额】，【不可打折金额】，【订单总金额】三者，则必须满足如下条件：【订单总金额】=【可打折金额】+【不可打折金额】
    //    /// </summary>
    //    public double total_amount { get; set; }
    //    /// <summary>
    //    /// 参与优惠计算的金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]。 如果该值未传入，但传入了【订单总金额】和【不可打折金额】，则该值默认为【订单总金额】-【不可打折金额】
    //    /// </summary>
    //    public double discountable_amount { get; set; }
    //    /// <summary>
    //    /// 不参与优惠计算的金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]。如果该值未传入，但传入了【订单总金额】和【可打折金额】，则该值默认为【订单总金额】-【可打折金额】
    //    /// </summary>
    //    public int undiscountable_amount { get; set; }
    //    /// <summary>
    //    /// 订单描述
    //    /// </summary>
    //    public string body { get; set; }
    //    /// <summary>
    //    /// 订单包含的商品列表信息，Json格式，其它说明详见商品明细说明
    //    /// </summary>
    //    public List<Goods_detailItem> goods_detail { get; set; }
    //    /// <summary>
    //    /// 商户操作员编号
    //    /// </summary>
    //    public string operator_id { get; set; }
    //    /// <summary>
    //    /// 商户门店编号
    //    /// </summary>
    //    public string store_id { get; set; }
    //    /// <summary>
    //    /// 商户机具终端编号
    //    /// </summary>
    //    public string terminal_id { get; set; }
    //    /// <summary>
    //    /// 支付宝的店铺编号
    //    /// </summary>
    //    public string alipay_store_id { get; set; }
    //    /// <summary>
    //    /// 业务扩展参数
    //    /// </summary>
    //    public Extend_params extend_params { get; set; }
    //    /// <summary>
    //    /// 该笔订单允许的最晚付款时间，逾期将关闭交易。取值范围：1m～15d。m-分钟，h-小时，d-天，1c-当天（1c-当天的情况下，无论交易何时创建，都在0点关闭）。 该参数数值不接受小数点， 如 1.5h，可转换为 90m
    //    /// </summary>
    //    public string timeout_express { get; set; }
    //    /// <summary>
    //    /// 描述分账信息，Json格式，其它说明详见分账说明
    //    /// </summary>
    //    public Royalty_info royalty_info { get; set; }
    //    /// <summary>
    //    /// 间连受理商户信息体，当前只对特殊银行机构特定场景下使用此字段
    //    /// </summary>
    //    public Sub_merchant sub_merchant { get; set; }
    //    /// <summary>
    //    /// 禁用支付渠道,多个渠道以逗号分割，如同时禁用信用支付类型和积分，则disable_pay_channels="credit_group,point"
    //    /// </summary>
    //    public string disable_pay_channels { get; set; }
    //    /// <summary>
    //    /// 外部指定买家
    //    /// </summary>
    //    public Ext_user_info ext_user_info { get; set; }
    //}

    ///// <summary>
    ///// 商品列表信息
    ///// </summary>
    //public class Goods_detailItem
    //{
    //    /// <summary>
    //    /// 商品的编号
    //    /// </summary>
    //    public string goods_id { get; set; }
    //    /// <summary>
    //    /// 支付宝定义的统一商品编号
    //    /// </summary>
    //    public string alipay_goods_id { get; set; }
    //    /// <summary>
    //    /// 商品名称
    //    /// </summary>
    //    public string goods_name { get; set; }
    //    /// <summary>
    //    /// 商品数量
    //    /// </summary>
    //    public int quantity { get; set; }
    //    /// <summary>
    //    /// 商品单价，单位为元
    //    /// </summary>
    //    public int price { get; set; }
    //    /// <summary>
    //    /// 商品类目
    //    /// </summary>
    //    public string goods_category { get; set; }
    //    /// <summary>
    //    /// 商品描述信息
    //    /// </summary>
    //    public string body { get; set; }
    //    /// <summary>
    //    /// 商品的展示地址
    //    /// </summary>
    //    public string show_url { get; set; }
    //}

    ///// <summary>
    ///// 业务扩展参数
    ///// </summary>
    //public class Extend_params
    //{
    //    /// <summary>
    //    /// 系统商编号 该参数作为系统商返佣数据提取的依据，请填写系统商签约协议的PID
    //    /// </summary>
    //    public string sys_service_provider_id { get; set; }
    //    /// <summary>
    //    /// 使用花呗分期要进行的分期数
    //    /// </summary>
    //    public string hb_fq_num { get; set; }
    //    /// <summary>
    //    /// 使用花呗分期需要卖家承担的手续费比例的百分值，传入100代表100%
    //    /// </summary>
    //    public string hb_fq_seller_percent { get; set; }
    //}

    ///// <summary>
    ///// 分账明细的信息
    ///// </summary>
    //public class Royalty_detail_infosItem
    //{
    //    /// <summary>
    //    /// 分账序列号，表示分账执行的顺序，必须为正整数
    //    /// </summary>
    //    public int serial_no { get; set; }
    //    /// <summary>
    //    /// 接受分账金额的账户类型： userId：支付宝账号对应的支付宝唯一用户号。 bankIndex：分账到银行账户的银行编号。目前暂时只支持分账到一个银行编号。 storeId：分账到门店对应的银行卡编号。 默认值为userId。
    //    /// </summary>
    //    public string trans_in_type { get; set; }
    //    /// <summary>
    //    /// 分账批次号 分账批次号。 目前需要和转入账号类型为bankIndex配合使用。
    //    /// </summary>
    //    public string batch_no { get; set; }
    //    /// <summary>
    //    /// 商户分账的外部关联号，用于关联到每一笔分账信息，商户需保证其唯一性。 如果为空，该值则默认为“商户网站唯一订单号+分账序列号”
    //    /// </summary>
    //    public string out_relation_id { get; set; }
    //    /// <summary>
    //    /// 要分账的账户类型。  目前只支持userId：支付宝账号对应的支付宝唯一用户号。 默认值为userId。
    //    /// </summary>
    //    public string trans_out_type { get; set; }
    //    /// <summary>
    //    /// 如果转出账号类型为userId，本参数为要分账的支付宝账号对应的支付宝唯一用户号。以2088开头的纯16位数字。
    //    /// </summary>
    //    public string trans_out { get; set; }
    //    /// <summary>
    //    /// 如果转入账号类型为userId，本参数为接受分账金额的支付宝账号对应的支付宝唯一用户号。以2088开头的纯16位数字。 如果转入账号类型为bankIndex，本参数为28位的银行编号（商户和支付宝签约时确定）。 如果转入账号类型为storeId，本参数为商户的门店ID。
    //    /// </summary>
    //    public string trans_in { get; set; }
    //    /// <summary>
    //    /// 分账的金额，单位为元
    //    /// </summary>
    //    public double amount { get; set; }
    //    /// <summary>
    //    /// 分账描述信息
    //    /// </summary>
    //    public string desc { get; set; }
    //    /// <summary>
    //    /// 分账的比例，值为20代表按20%的比例分账
    //    /// </summary>
    //    public string amount_percentage { get; set; }
    //}

    ///// <summary>
    ///// 分账信息
    ///// </summary>
    //public class Royalty_info
    //{
    //    /// <summary>
    //    /// 分账类型 卖家的分账类型，目前只支持传入ROYALTY（普通分账类型）。
    //    /// </summary>
    //    public string royalty_type { get; set; }
    //    /// <summary>
    //    /// 分账明细的信息，可以描述多条分账指令，json数组。
    //    /// </summary>
    //    public List<Royalty_detail_infosItem> royalty_detail_infos { get; set; }
    //}

    ///// <summary>
    ///// 间连受理商户信息体，当前只对特殊银行机构特定场景下使用此字段
    ///// </summary>
    //public class Sub_merchant
    //{
    //    /// <summary>
    //    /// 间连受理商户的支付宝商户编号，通过间连商户入驻后得到。间连业务下必传，并且需要按规范传递受理商户编号。
    //    /// </summary>
    //    public string merchant_id { get; set; }
    //}

    ///// <summary>
    ///// 外部指定买家
    ///// </summary>
    //public class Ext_user_info
    //{
    //    /// <summary>
    //    /// 姓名
    //    /// </summary>
    //    public string name { get; set; }
    //    /// <summary>
    //    /// 手机号
    //    /// </summary>
    //    public string mobile { get; set; }
    //    /// <summary>
    //    /// 身份证：IDENTITY_CARD、护照：PASSPORT、军官证：OFFICER_CARD、士兵证：SOLDIER_CARD、户口本：HOKOU等。如有其它类型需要支持，请与蚂蚁金服工作人员联系。
    //    /// </summary>
    //    public string cert_type { get; set; }
    //    /// <summary>
    //    /// 证件号
    //    /// </summary>
    //    public string cert_no { get; set; }
    //    /// <summary>
    //    /// 是否强制校验付款人身份信息[统一接口定义，T:强制校验，F：不强制
    //    /// </summary>
    //    public string fix_buyer { get; set; }
    //}
}
