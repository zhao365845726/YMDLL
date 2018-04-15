using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using YMDLL.Class;
using YMDLL.Common;
using ML.ThirdParty.AliPay;
using System.Reflection;

namespace ML.ThirdParty.Base
{
    /// <summary>
    /// 支付宝帮助类
    /// </summary>
    public class AliPayHelper
    {
        /// <summary>
        /// 网页操作类
        /// </summary>
        public static CS_OperaWeb ow = new CS_OperaWeb();
        /// <summary>
        /// 时间操作类
        /// </summary>
        public static CS_CalcDateTime cdt = new CS_CalcDateTime();
        /// <summary>
        /// 支付宝正式环境地址
        /// </summary>
        public static string AliPay_ZS_URL = "https://openapi.alipay.com/gateway.do";
        /// <summary>
        /// 支付宝沙箱环境地址
        /// </summary>
        public static string AliPay_CS_URL = "https://openapi.alipaydev.com/gateway.do";
        /// <summary>
        /// 支付宝正式环境公钥
        /// </summary>
        public static string AliPay_Public_Secret = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA1/g9IekMBU1W5ouwIJFFfuitPEGybUrCBQ9kJrET1oiT0znOLVpFi5zb2vOmOq9wNJO4HRf6xBRyyXR0MVbGEkMxSJzzBOa+n+Ugu4aAjWUbHp8szF0IUg7zdwsy1riL1UtYTVBolDuGuheQevX4Khr6kiXfvPnY0S0guH0lJCjxqW4Swlo59XQUjhr5pTb1mHcQdh/ID5olgE1h9Nch4owu6W7kGzbjA8TMCGWO53FWP/6t1OyTK1WU8exas4rC2/Y5eNzLU+g8NAVYRJvDMdBxM+xI1zUoEk4HtmGrGTYuzsueM8jYLxj9JInulj5psVCcTRi6jTohsV3k9yUNVwIDAQAB";
        /// <summary>
        /// 支付宝正式环境私钥
        /// </summary>
        public static string AliPay_Private_Secret = "MIIEpAIBAAKCAQEAteNKoQxjCJevWcu8qbogKII+EHXBNWldRhZvmh4D1dFWZFW61o1XMa8lENtXk0loeGhgeRonGfBLy9H87PzC+01JmXHCGu+Tj26ucNG5j+RBwB9hIrbwGaU0O/uLq/tRYDFPY4ZBe24oi96mwuF1SaH64XQeY69Oc8HldBxHfOBdoYgvepqXEOoYlgj+ed+HPsGrhuAOVPsOC4lU5DdyPf0BO/sTzgFe+U1/ffA/VieuXE2xbWViR2nkQ0ECsmvWXQ+Aes1UzCKL+OxZ8BF17ebK5BTVKao78yr/q6Fn/p2jDYkJ6TrmX8onp+6O0H7iE+01BSAGnfatXqYDZLVvhQIDAQABAoIBAD4g/3L3ejSLK5huoWkQbL+pjW2t62tFLFkufLyWGBVHRk1Lg2CaMviDrROO+OEUqBzI+nsjVcvkat+aQNzqkeepRMSnOPhecXcDBY7/9HSUMWlgzps/BdCcv7TlfZYnaGqFLWr/hwqUXEE6spfKz3dxXvL36RF/5jpgNvJsXnbm0KgzLsdWL0eVQpX+jdtoy7RytEbDSbYUTX+4dMMal4jZlai6oAr9GAzlwhZhbVRzQGKjRTjhS/GZF+Rmc3AuI3tFnGnjgAagpcCnnRe15LjqLMevEFN5FwQ2F2BiCSOy2+80KP3ZqOWarFbxljbGNBXTfDIZXzw5p2avfHlOClkCgYEA7sl2h5KH8OJdWqhF0WO1aHD8SMPfEBSXxswOsXMYFhhdF+w8AA4CMe+nuBWSnQjGV3UuaF4AQ0tviZHi7PvpuDilVIkcx/PRlCc+8fTvA3EaOcpPt8NGbDmsybkqkbMfIUZjq8dSxv4rddEgDIyxxobTWAuk8GZeg4LLn4AOBl8CgYEAwv/SRJfCnWy7JWhc9yIea5eD62EBN2oa6vv8Z8xXV4iH4/qXwJgHAkqDXnjQ3RVQ0RVTkjOGpCGuW7pURlpuxbpeyQfmF9sq4fwccyxAShwz50gnsMzcUMAXFo/lIO1siMDDhX57nKgXerkqh7/3KIq0VizPK3gmeU9vaECo7JsCgYEAgLGRsUWXAItvluGkJ1LevXhPcAQo32jJ2Dm4HYkTTS06Vx9ZwwnJwxIPyZXO29WT2CTuqw8mgx5P8cvvipvGfiQBbwsquNU+eeOg+BRzEN7PLKBN0JNtC/0Vk+6486efOnbOVxFpGph34dqgtELLuVyG4Sf9cBcsDyLaDY+df28CgYEAsscCS1rr1ZLbXad6HCXOyAvIqaNtDxYbibbvKWcB6MHG/LBCMxPu0R3ppVKVAyzIxHiN/yt4VMCAb568JyhACM4emqbAQLzyau/OUZWPd6K1v2S65vBbSTljsY7Jnk+uww4f9CMLR4wgwqZDJORNwICyZ5N7eLoWY//8/xLRDOMCgYBKg6CEvm7bNEAt4l2jobvrtec7T4FCqzBBT/gEEHpl5MsRNuiypmqk/1KtDnEYVAjzjprHGq7jlwvM4DFT5tlTpUV2MDYVPo5UA3PoJfkTL2am9OIhAyvkdZqEm9zlwPmaVcN/7BSGV5QrkIGXn/TOc+nxUW6FM8Rz6r/xewXorA==";
        /// <summary>
        /// 支付宝沙箱环境公钥
        /// </summary>
        public static string AliPay_CS_Public_Secret = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlThl5lrzozvElN4IfopgUe9lXS34H6ZXQyImpFgqzNprw1wOG+cqcuYkIVM3APvRi7n8CuJkOKDuFF9pT9uHx6bDA1OWFriKjeFcCNiROi/EuOGdsvhNtglpB4m0MbR1z8tnz43FNrsUAC6g5/cQmUYbvnRLarITeJvta9BROfF5bfxYrN7YSYBpsxVY1TzreHnzJckqI6DGgmrxD4ast/0NWbijxCh2H7zeaU5IIWteIvgxpwXn974e9ZWbpEgjaa/siMfBwjSlYcThEh+NbLOnZQPtXg/F4gOIq7je9UVRzXV98Ot7DWDZbDBeurna3FoXfJ74RKfFiRE4rBNNFQIDAQAB";
        /// <summary>
        /// 支付宝沙箱环境私钥
        /// </summary>
        public static string AliPay_CS_Private_Secret = "MIIEpAIBAAKCAQEAteNKoQxjCJevWcu8qbogKII+EHXBNWldRhZvmh4D1dFWZFW61o1XMa8lENtXk0loeGhgeRonGfBLy9H87PzC+01JmXHCGu+Tj26ucNG5j+RBwB9hIrbwGaU0O/uLq/tRYDFPY4ZBe24oi96mwuF1SaH64XQeY69Oc8HldBxHfOBdoYgvepqXEOoYlgj+ed+HPsGrhuAOVPsOC4lU5DdyPf0BO/sTzgFe+U1/ffA/VieuXE2xbWViR2nkQ0ECsmvWXQ+Aes1UzCKL+OxZ8BF17ebK5BTVKao78yr/q6Fn/p2jDYkJ6TrmX8onp+6O0H7iE+01BSAGnfatXqYDZLVvhQIDAQABAoIBAD4g/3L3ejSLK5huoWkQbL+pjW2t62tFLFkufLyWGBVHRk1Lg2CaMviDrROO+OEUqBzI+nsjVcvkat+aQNzqkeepRMSnOPhecXcDBY7/9HSUMWlgzps/BdCcv7TlfZYnaGqFLWr/hwqUXEE6spfKz3dxXvL36RF/5jpgNvJsXnbm0KgzLsdWL0eVQpX+jdtoy7RytEbDSbYUTX+4dMMal4jZlai6oAr9GAzlwhZhbVRzQGKjRTjhS/GZF+Rmc3AuI3tFnGnjgAagpcCnnRe15LjqLMevEFN5FwQ2F2BiCSOy2+80KP3ZqOWarFbxljbGNBXTfDIZXzw5p2avfHlOClkCgYEA7sl2h5KH8OJdWqhF0WO1aHD8SMPfEBSXxswOsXMYFhhdF+w8AA4CMe+nuBWSnQjGV3UuaF4AQ0tviZHi7PvpuDilVIkcx/PRlCc+8fTvA3EaOcpPt8NGbDmsybkqkbMfIUZjq8dSxv4rddEgDIyxxobTWAuk8GZeg4LLn4AOBl8CgYEAwv/SRJfCnWy7JWhc9yIea5eD62EBN2oa6vv8Z8xXV4iH4/qXwJgHAkqDXnjQ3RVQ0RVTkjOGpCGuW7pURlpuxbpeyQfmF9sq4fwccyxAShwz50gnsMzcUMAXFo/lIO1siMDDhX57nKgXerkqh7/3KIq0VizPK3gmeU9vaECo7JsCgYEAgLGRsUWXAItvluGkJ1LevXhPcAQo32jJ2Dm4HYkTTS06Vx9ZwwnJwxIPyZXO29WT2CTuqw8mgx5P8cvvipvGfiQBbwsquNU+eeOg+BRzEN7PLKBN0JNtC/0Vk+6486efOnbOVxFpGph34dqgtELLuVyG4Sf9cBcsDyLaDY+df28CgYEAsscCS1rr1ZLbXad6HCXOyAvIqaNtDxYbibbvKWcB6MHG/LBCMxPu0R3ppVKVAyzIxHiN/yt4VMCAb568JyhACM4emqbAQLzyau/OUZWPd6K1v2S65vBbSTljsY7Jnk+uww4f9CMLR4wgwqZDJORNwICyZ5N7eLoWY//8/xLRDOMCgYBKg6CEvm7bNEAt4l2jobvrtec7T4FCqzBBT/gEEHpl5MsRNuiypmqk/1KtDnEYVAjzjprHGq7jlwvM4DFT5tlTpUV2MDYVPo5UA3PoJfkTL2am9OIhAyvkdZqEm9zlwPmaVcN/7BSGV5QrkIGXn/TOc+nxUW6FM8Rz6r/xewXorA==";
        /// <summary>
        /// 正式应用ID
        /// </summary>
        public static string APPID = "2017072007825548";
        /// <summary>
        /// 沙箱应用ID
        /// </summary>
        public static string CS_APPID = "2016080500175871";
        /// <summary>
        /// 字符编码
        /// </summary>
        public static string CHARSET = "utf-8";
        /// <summary>
        /// 支付宝参数字典
        /// </summary>
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
        /// 演示
        /// </summary>
        /// <returns></returns>
        public static string Demo()
        {
            //IAopClient client = new DefaultAopClient("https://openapi.alipaydev.com/gateway.do", CS_APPID, AliPay_CS_Private_Secret, "json", "1.0", "RSA2", AliPay_CS_Public_Secret, CHARSET, false);
            ////实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.open.public.template.message.industry.modify 
            //AlipayOpenPublicTemplateMessageIndustryModifyRequest request = new AlipayOpenPublicTemplateMessageIndustryModifyRequest();
            ////SDK已经封装掉了公共参数，这里只需要传入业务参数
            ////此次只是参数展示，未进行字符串转义，实际情况下请转义
            //request.BizContent = "{" +
            //"    \"out_trade_no\":\"10001/20102\"," +
            //"    \"scene\":\"bar_code\"," +
            //"    \"auth_code\":\"28763443825664394\"" +
            //"    \"subject\":\"Iphone6 16G\"" +
            //"  }";
            //AlipayOpenPublicTemplateMessageIndustryModifyResponse response = client.Execute(request);
            ////调用成功，则处理业务逻辑
            //if (response.isSuccess())
            //{
            //    //.....
            //}


            ////当面付
            //IAopClient client = new DefaultAopClient("https://openapi.alipaydev.com/gateway.do", 
            //    "2016080500175871", 
            //    "MIIEpAIBAAKCAQEAteNKoQxjCJevWcu8qbogKII+EHXBNWldRhZvmh4D1dFWZFW61o1XMa8lENtXk0loeGhgeRonGfBLy9H87PzC+01JmXHCGu+Tj26ucNG5j+RBwB9hIrbwGaU0O/uLq/tRYDFPY4ZBe24oi96mwuF1SaH64XQeY69Oc8HldBxHfOBdoYgvepqXEOoYlgj+ed+HPsGrhuAOVPsOC4lU5DdyPf0BO/sTzgFe+U1/ffA/VieuXE2xbWViR2nkQ0ECsmvWXQ+Aes1UzCKL+OxZ8BF17ebK5BTVKao78yr/q6Fn/p2jDYkJ6TrmX8onp+6O0H7iE+01BSAGnfatXqYDZLVvhQIDAQABAoIBAD4g/3L3ejSLK5huoWkQbL+pjW2t62tFLFkufLyWGBVHRk1Lg2CaMviDrROO+OEUqBzI+nsjVcvkat+aQNzqkeepRMSnOPhecXcDBY7/9HSUMWlgzps/BdCcv7TlfZYnaGqFLWr/hwqUXEE6spfKz3dxXvL36RF/5jpgNvJsXnbm0KgzLsdWL0eVQpX+jdtoy7RytEbDSbYUTX+4dMMal4jZlai6oAr9GAzlwhZhbVRzQGKjRTjhS/GZF+Rmc3AuI3tFnGnjgAagpcCnnRe15LjqLMevEFN5FwQ2F2BiCSOy2+80KP3ZqOWarFbxljbGNBXTfDIZXzw5p2avfHlOClkCgYEA7sl2h5KH8OJdWqhF0WO1aHD8SMPfEBSXxswOsXMYFhhdF+w8AA4CMe+nuBWSnQjGV3UuaF4AQ0tviZHi7PvpuDilVIkcx/PRlCc+8fTvA3EaOcpPt8NGbDmsybkqkbMfIUZjq8dSxv4rddEgDIyxxobTWAuk8GZeg4LLn4AOBl8CgYEAwv/SRJfCnWy7JWhc9yIea5eD62EBN2oa6vv8Z8xXV4iH4/qXwJgHAkqDXnjQ3RVQ0RVTkjOGpCGuW7pURlpuxbpeyQfmF9sq4fwccyxAShwz50gnsMzcUMAXFo/lIO1siMDDhX57nKgXerkqh7/3KIq0VizPK3gmeU9vaECo7JsCgYEAgLGRsUWXAItvluGkJ1LevXhPcAQo32jJ2Dm4HYkTTS06Vx9ZwwnJwxIPyZXO29WT2CTuqw8mgx5P8cvvipvGfiQBbwsquNU+eeOg+BRzEN7PLKBN0JNtC/0Vk+6486efOnbOVxFpGph34dqgtELLuVyG4Sf9cBcsDyLaDY+df28CgYEAsscCS1rr1ZLbXad6HCXOyAvIqaNtDxYbibbvKWcB6MHG/LBCMxPu0R3ppVKVAyzIxHiN/yt4VMCAb568JyhACM4emqbAQLzyau/OUZWPd6K1v2S65vBbSTljsY7Jnk+uww4f9CMLR4wgwqZDJORNwICyZ5N7eLoWY//8/xLRDOMCgYBKg6CEvm7bNEAt4l2jobvrtec7T4FCqzBBT/gEEHpl5MsRNuiypmqk/1KtDnEYVAjzjprHGq7jlwvM4DFT5tlTpUV2MDYVPo5UA3PoJfkTL2am9OIhAyvkdZqEm9zlwPmaVcN/7BSGV5QrkIGXn/TOc+nxUW6FM8Rz6r/xewXorA==", 
            //    "json", 
            //    "1.0", 
            //    "RSA2", 
            //    "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlThl5lrzozvElN4IfopgUe9lXS34H6ZXQyImpFgqzNprw1wOG+cqcuYkIVM3APvRi7n8CuJkOKDuFF9pT9uHx6bDA1OWFriKjeFcCNiROi/EuOGdsvhNtglpB4m0MbR1z8tnz43FNrsUAC6g5/cQmUYbvnRLarITeJvta9BROfF5bfxYrN7YSYBpsxVY1TzreHnzJckqI6DGgmrxD4ast/0NWbijxCh2H7zeaU5IIWteIvgxpwXn974e9ZWbpEgjaa/siMfBwjSlYcThEh+NbLOnZQPtXg/F4gOIq7je9UVRzXV98Ot7DWDZbDBeurna3FoXfJ74RKfFiRE4rBNNFQIDAQAB", 
            //    "utf-8", false);
            //AlipayTradePayRequest request = new AlipayTradePayRequest();
            //request.SetNotifyUrl("http://www.ymstudio.xyz:81/Api/AliPay/PayNotice");
            //request.BizContent = "{" +
            //"\"out_trade_no\":\"20170729010101201\"," +
            //"\"scene\":\"bar_code\"," +
            //"\"auth_code\":\"28763443825664394\"," +
            //"\"subject\":\"Iphone6 16G\"" +
            //"  }";
            //AlipayTradePayResponse response = client.Execute(request);

            //return response.Body;

            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", CS_APPID, AliPay_CS_Private_Secret, "json", "1.0", "RSA2", AliPay_CS_Public_Secret, CHARSET, false);
            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            //SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
            AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
            model.Body = "我是测试数据";
            model.Subject = "App支付测试DoNet";
            model.TotalAmount = "0.01";
            model.ProductCode = "QUICK_MSECURITY_PAY";
            model.OutTradeNo = Guid.NewGuid().ToString().Replace("-", "");
            model.TimeoutExpress = "30m";
            request.SetBizModel(model);
            request.SetNotifyUrl("http://www.ymstudio.xyz:81/Api/AliPay/PayNotice");
            //这里和普通的接口调用不同，使用的是sdkExecute
            AlipayTradeAppPayResponse response = client.SdkExecute(request);
            //HttpUtility.HtmlEncode是为了输出到页面时防止被浏览器将关键参数html转义，实际打印到日志以及http传输不会有这个问题
            //Response.Write(HttpUtility.HtmlEncode(response.Body));
            //页面输出的response.Body就是orderString 可以直接给客户端请求，无需再做处理。
            return HttpUtility.HtmlEncode(response.Body);
        }

        /// <summary>
        /// app支付前请求服务器
        /// </summary>
        /// <param name="aarp"></param>
        /// <returns></returns>
        public static string AppRequest(AliAppRequestParam aarp)
        {
            //正式
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", APPID, AliPay_Private_Secret, "json", "1.0", "RSA2", AliPay_Public_Secret, CHARSET, false);
            //测试
            //IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", CS_APPID, AliPay_CS_Private_Secret, "json", "1.0", "RSA2", AliPay_CS_Public_Secret, CHARSET, false);
            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            //SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
            AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
            model.Body = aarp.Body;
            model.Subject = aarp.Subject;
            model.TotalAmount = aarp.TotalAmount;
            model.ProductCode = Guid.NewGuid().ToString().Replace("-", "");
            model.OutTradeNo = Guid.NewGuid().ToString().Replace("-", "");
            model.TimeoutExpress = aarp.TimeoutExpress;
            request.SetBizModel(model);
            //调用支付宝通知商户验签的接口
            request.SetNotifyUrl("http://www.ymstudio.xyz:81/Api/AliPay/VerificationParam");
            //这里和普通的接口调用不同，使用的是sdkExecute
            AlipayTradeAppPayResponse response = client.SdkExecute(request);
            //HttpUtility.HtmlEncode是为了输出到页面时防止被浏览器将关键参数html转义，实际打印到日志以及http传输不会有这个问题
            //Response.Write(HttpUtility.HtmlEncode(response.Body));
            //页面输出的response.Body就是orderString 可以直接给客户端请求，无需再做处理。
            //return HttpUtility.HtmlEncode(response.Body);
            return response.Body;
        }

        /// <summary>
        /// 验证参数
        /// </summary>
        /// <returns></returns>
        public static string VerificationParam(AsyncNotifyParam anp)
        {
            SortedDictionary<string, string> sPara = new SortedDictionary<string, string>();
            Type t = anp.GetType();
            foreach(PropertyInfo pi in t.GetProperties())
            {
                sPara.Add(pi.Name, pi.GetValue(anp, null).ToString());
            }
            Notify aliNotify = new Notify();
            bool verifyResult = aliNotify.Verify(sPara, anp.notify_id, anp.sign);
            if (verifyResult)
            {
                return "success";
            }
            else
            {
                return "fail";
            }
        }

        /// <summary>
        /// 验证参数
        /// </summary>
        /// <returns></returns>
        public static string VerificationParamByForm(HttpRequestBase request)
        {
            SortedDictionary<string, string> sPara = GetRequestPost(request); 
            if(sPara.Count > 0)
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, request.Form["notify_id"], request.Form["sign"]);
                if (verifyResult)
                {
                    return "success";
                }
                else
                {
                    return "fail";
                }
            }
            else
            {
                return "无通知参数";
            }
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public static SortedDictionary<string, string> GetRequestPost(HttpRequestBase request)
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], request.Form[requestItem[i]]);
            }

            return sArray;
        }

        ///// <summary>
        ///// request回来的信息组成的数组
        ///// </summary>
        ///// <returns></returns>        
        //public static IDictionary<string, string> GetRequestPost()
        //{
        //    int i = 0;
        //    IDictionary<string, string> sArray = new IDictionary<string,string>();
        //    //NameValueCollection coll;
        //    ////Load Form variables into NameValueCollection variable. 
        //    //coll = Request.Form;
        //    //// Get names of all forms into a string array.
        //    //String[] requestItem = coll.AllKeys;

        //    //for (i = 0; i < requestItem.Length; i++)
        //    //{
        //    //    sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
        //    //}

        //    return sArray;
        //}

        ///// <summary>
        ///// 支付宝App支付
        ///// </summary>
        ///// <param name="app">支付请求参数</param>
        ///// <returns></returns>
        //public static string Pay(AliPayParam app)
        //{
        //    string strResult = string.Empty;
        //    IAopClient client = new DefaultAopClient(AliPay_CS_URL, CS_APPID, AliPay_CS_Private_Secret, "json", "1.0", "RSA2", AliPay_CS_Public_Secret, "utf-8", false);
        //    AlipayTradePayRequest request = new AlipayTradePayRequest();
        //    //request.SetNotifyUrl("http://www.ymstudio.xyz:81/Api/AliPay/PayNotice");

        //    dicAliPay.Clear();
        //    dicAliPay.Add("out_trade_no", Guid.NewGuid().ToString().Replace("-", ""));
        //    dicAliPay.Add("scene", app.scene);
        //    dicAliPay.Add("auth_code", app.auth_code);
        //    dicAliPay.Add("product_code", app.product_code);
        //    dicAliPay.Add("subject", app.subject);
        //    dicAliPay.Add("buyer_id", app.buyer_id);
        //    dicAliPay.Add("seller_id", app.seller_id);
        //    dicAliPay.Add("total_amount", app.total_amount.ToString());
        //    dicAliPay.Add("discountable_amount", app.discountable_amount.ToString());
        //    dicAliPay.Add("body", app.body);
        //    dicAliPay.Add("operator_id", app.operator_id);
        //    dicAliPay.Add("store_id", app.store_id);
        //    dicAliPay.Add("terminal_id", app.terminal_id);
        //    dicAliPay.Add("timeout_express", app.timeout_express);
        //    dicAliPay.Add("auth_no", app.auth_no);

        //    request.BizContent = FormatToJson(dicAliPay);
        //    AlipayTradePayResponse response = client.Execute(request);
        //    strResult = response.Body;

        //    return strResult;
        //}

        /// <summary>
        /// 交易关闭
        /// </summary>
        /// <returns></returns>
        public static string Close(CloseParam cp)
        {
            string strResult = string.Empty;
            IAopClient client = new DefaultAopClient(AliPay_CS_URL, CS_APPID, AliPay_CS_Private_Secret, "json", "1.0", "RSA2", AliPay_CS_Public_Secret, "UTF-8", false);
            AlipayTradeCloseRequest request = new AlipayTradeCloseRequest();

            dicAliPay.Clear();
            dicAliPay.Add("trade_no", cp.trade_no);
            dicAliPay.Add("out_trade_no", cp.out_trade_no);
            dicAliPay.Add("operator_id", cp.operator_id);

            request.BizContent = FormatToJson(dicAliPay);
            AlipayTradeCloseResponse response = client.Execute(request);
            strResult = response.Body;

            return strResult;
        }

        /// <summary>
        /// 交易状态查询
        /// </summary>
        /// <returns></returns>
        public static string Query(QueryParam qp)
        {
            string strResult = string.Empty;
            IAopClient client = new DefaultAopClient(AliPay_CS_URL + "gateway.do", CS_APPID, AliPay_CS_Private_Secret, "json", "1.0", "RSA2", AliPay_CS_Public_Secret, "UTF-8", false);
            AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();

            dicAliPay.Clear();
            dicAliPay.Add("trade_no", qp.trade_no);
            dicAliPay.Add("out_trade_no", qp.out_trade_no);

            request.BizContent = FormatToJson(dicAliPay);
            AlipayTradeQueryResponse response = client.Execute(request);
            strResult = response.Body;

            return strResult;
        }

        /// <summary>
        /// 交易退款
        /// </summary>
        /// <returns></returns>
        public static string Refund(AliRefundParam arp)
        {
            string strResult = string.Empty;
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
            strResult = response.Body;

            return strResult;
        }

        /// <summary>
        /// 退款查询
        /// </summary>
        /// <returns></returns>
        public static string RefundQuery(AliRefundQueryParam arqp)
        {
            string strResult = string.Empty;
            IAopClient client = new DefaultAopClient(AliPay_CS_URL + "gateway.do", CS_APPID, AliPay_CS_Private_Secret, "json", "1.0", "RSA2", AliPay_CS_Public_Secret, "UTF-8", false);
            AlipayTradeFastpayRefundQueryRequest request = new AlipayTradeFastpayRefundQueryRequest();
            dicAliPay.Clear();
            dicAliPay.Add("trade_no", arqp.trade_no);
            dicAliPay.Add("out_trade_no", arqp.out_trade_no);
            dicAliPay.Add("out_request_no", arqp.out_request_no);

            request.BizContent = FormatToJson(dicAliPay);
            AlipayTradeFastpayRefundQueryResponse response = client.Execute(request);
            strResult = response.Body;

            return strResult;
        }

        /// <summary>
        /// 账单查询
        /// </summary>
        /// <returns></returns>
        public static string DownloadUrlQuery()
        {
            string strResult = string.Empty;
            IAopClient client = new DefaultAopClient(AliPay_CS_URL + "gateway.do", CS_APPID, AliPay_CS_Private_Secret, "json", "1.0", "RSA2", AliPay_CS_Public_Secret, "UTF-8", false);
            AlipayDataBillDownloadurlGetRequest request = new AlipayDataBillDownloadurlGetRequest();
            //request.BizContent = "{" +
            //"\"bill_type\":\"20150320010101001\"," +
            //"\"bill_date\":\"20150320010101001\"," +
            //"  }";
            AlipayDataBillDownloadurlGetResponse response = client.Execute(request);
            strResult = response.Body;

            return strResult;
        }

        /// <summary>
        /// 支付宝请求统一入口[测试使用]
        /// </summary>
        /// <param name="dicParam">请求参数</param>
        /// <returns></returns>
        public static string AliRequest(Dictionary<string, string> dicParam)
        {
            string result = string.Empty;
            IAopClient client = new DefaultAopClient(AliPay_CS_URL, CS_APPID, AliPay_CS_Private_Secret, "json", "1.0", "RSA2", AliPay_CS_Public_Secret, "utf-8", false);
            AlipayTradePayRequest request = new AlipayTradePayRequest();
            request.SetNotifyUrl("http://www.ymstudio.xyz:81/Api/AliPay/PayNotice");
            request.BizContent = FormatToJson(dicParam);
            //request.BizContent = strParam;
            AlipayTradePayResponse response = client.Execute(request);

            return response.Body;
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

    /// <summary>
    /// 异步通知参数
    /// </summary>
    public class AsyncNotifyParam
    {
        /// <summary>
        /// 通知时间
        /// </summary>
        public string notify_time { get; set; }
        /// <summary>
        /// 通知类型
        /// </summary>
        public string notify_type { get; set; }
        /// <summary>
        /// 通知校验ID
        /// </summary>
        public string notify_id { get; set; }
        /// <summary>
        /// 支付宝分配给开发者的应用Id
        /// </summary>
        public string app_id { get; set; }
        /// <summary>
        /// 编码格式
        /// </summary>
        public string charset { get; set; }
        /// <summary>
        /// 接口版本
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 签名类型
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string trade_no { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 商户业务号
        /// </summary>
        public string out_biz_no { get; set; }
        /// <summary>
        /// 买家支付宝用户号
        /// </summary>
        public string buyer_id { get; set; }
        /// <summary>
        /// 买家支付宝账号
        /// </summary>
        public string buyer_logon_id { get; set; }
        /// <summary>
        /// 卖家支付宝用户号
        /// </summary>
        public string seller_id { get; set; }
        /// <summary>
        /// 卖家支付宝账号
        /// </summary>
        public string seller_email { get; set; }
        /// <summary>
        /// 交易状态
        /// WAIT_BUYER_PAY----交易创建，等待买家付款
        /// TRADE_CLOSED----未付款交易超时关闭，或支付完成后全额退款
        /// TRADE_SUCCESS----交易支付成功
        /// TRADE_FINISHED----交易结束，不可退款
        /// </summary>
        public string trade_status { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public string total_amount { get; set; }
        /// <summary>
        /// 实收金额
        /// </summary>
        public string receipt_amount { get; set; }
        /// <summary>
        /// 开票金额
        /// </summary>
        public string invoice_amount { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public string buyer_pay_amount { get; set; }
        /// <summary>
        /// 集分宝金额
        /// </summary>
        public string point_amount { get; set; }
        /// <summary>
        /// 总退款金额
        /// </summary>
        public string refund_fee { get; set; }
        /// <summary>
        /// 订单标题
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 交易创建时间
        /// </summary>
        public string gmt_create { get; set; }
        /// <summary>
        /// 交易付款时间
        /// </summary>
        public string gmt_payment { get; set; }
        /// <summary>
        /// 交易退款时间
        /// </summary>
        public string gmt_refund { get; set; }
        /// <summary>
        /// 交易结束时间
        /// </summary>
        public string gmt_close { get; set; }
        /// <summary>
        /// 支付金额信息
        /// </summary>
        public FundBillListParam fund_bill_list { get; set; }
        /// <summary>
        /// 回传参数
        /// </summary>
        public string passback_params { get; set; }
        /// <summary>
        /// 优惠券信息
        /// </summary>
        public VoucherDetailListParam voucher_detail_list { get; set; }
    }

    /// <summary>
    /// 资金明细信息说明
    /// </summary>
    public class FundBillListParam
    {
        /// <summary>
        /// 支付渠道
        /// COUPON----支付宝红包
        /// ALIPAYACCOUNT----支付宝余额
        /// POINT----集分宝
        /// DISCOUNT----折扣券
        /// PCARD----预付卡
        /// FINANCEACCOUNT----余额宝
        /// MCARD----商家储值卡
        /// MDISCOUNT----商户优惠券
        /// MCOUPON----商户红包
        /// PCREDIT----蚂蚁花呗
        /// </summary>
        public string fundChannel { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public string amount { get; set; }
    }

    /// <summary>
    /// 优惠券信息说明
    /// </summary>
    public class VoucherDetailListParam
    {
        /// <summary>
        /// 券名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 券类型
        /// ALIPAY_FIX_VOUCHER----全场代金券
        /// ALIPAY_DISCOUNT_VOUCHER----折扣券
        /// ALIPAY_ITEM_VOUCHER----单品优惠
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 优惠券面额
        /// </summary>
        public string amount { get; set; }
        /// <summary>
        /// 商家出资
        /// </summary>
        public string merchant_contribute { get; set; }
        /// <summary>
        /// 其他出资方出资金额
        /// </summary>
        public string other_contribute { get; set; }
        /// <summary>
        /// 优惠券备注信息
        /// </summary>
        public string memo { get; set; }
    }

    /// <summary>
    /// 支付宝APP支付的请求参数
    /// </summary>
    public class AliAppRequestParam
    {
        /// <summary>
        /// 订单描述
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 订单标题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000] 如果同时传入【可打折金额】和【不可打折金额】，该参数可以不用传入； 如果同时传入了【可打折金额】，【不可打折金额】，【订单总金额】三者，则必须满足如下条件：【订单总金额】=【可打折金额】+【不可打折金额】
        /// </summary>
        public string TotalAmount { get; set; }
        ///// <summary>
        ///// 销售产品码
        ///// </summary>
        //public string ProductCode { get; set; }
        ///// <summary>
        ///// 商户订单号,64个字符以内、可包含字母、数字、下划线；需保证在商户端不重复
        ///// </summary>
        //public string OutTradeNo { get; set; }
        /// <summary>
        /// 该笔订单允许的最晚付款时间，逾期将关闭交易。取值范围：1m～15d。m-分钟，h-小时，d-天，1c-当天（1c-当天的情况下，无论交易何时创建，都在0点关闭）。 该参数数值不接受小数点， 如 1.5h，可转换为 90m
        /// </summary>
        public string TimeoutExpress { get; set; }
        ///// <summary>
        ///// 通知地址
        ///// </summary>
        //public string NotifyUrl { get; set; }
    }

    /// <summary>
    /// 请求参数
    /// </summary>
    public class AliPayParam
    {
        ///// <summary>
        ///// [必填参]----商户订单号,64个字符以内、可包含字母、数字、下划线；需保证在商户端不重复
        ///// </summary>
        //public string out_trade_no { get; set; }
        /// <summary>
        /// [必填参]----支付场景  条码支付，取值：bar_code  声波支付，取值：wave_code
        /// </summary>
        public string scene { get; set; }
        /// <summary>
        /// [必填参]----支付授权码，25~30开头的长度为16~24位的数字，实际字符串长度以开发者获取的付款码长度为准
        /// </summary>
        public string auth_code { get; set; }
        /// <summary>
        /// 销售产品码
        /// </summary>
        public string product_code { get; set; }
        /// <summary>
        /// [必填参]----订单标题
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// 买家的支付宝用户id，如果为空，会从传入了码值信息中获取买家ID
        /// </summary>
        public string buyer_id { get; set; }
        /// <summary>
        /// 如果该值为空，则默认为商户签约账号对应的支付宝用户ID
        /// </summary>
        public string seller_id { get; set; }
        /// <summary>
        /// 订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000] 如果同时传入【可打折金额】和【不可打折金额】，该参数可以不用传入； 如果同时传入了【可打折金额】，【不可打折金额】，【订单总金额】三者，则必须满足如下条件：【订单总金额】=【可打折金额】+【不可打折金额】
        /// </summary>
        public double total_amount { get; set; }
        /// <summary>
        /// 参与优惠计算的金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]。 如果该值未传入，但传入了【订单总金额】和【不可打折金额】，则该值默认为【订单总金额】-【不可打折金额】
        /// </summary>
        public double discountable_amount { get; set; }
        /// <summary>
        /// 订单描述
        /// </summary>
        public string body { get; set; }
        ///// <summary>
        ///// 订单包含的商品列表信息，Json格式，其它说明详见商品明细说明
        ///// </summary>
        //public List<Goods_detailItem> goods_detail { get; set; }
        /// <summary>
        /// 商户操作员编号
        /// </summary>
        public string operator_id { get; set; }
        /// <summary>
        /// 商户门店编号
        /// </summary>
        public string store_id { get; set; }
        /// <summary>
        /// 商户机具终端编号
        /// </summary>
        public string terminal_id { get; set; }
        ///// <summary>
        ///// 业务扩展参数
        ///// </summary>
        //public Extend_params extend_params { get; set; }
        /// <summary>
        /// 该笔订单允许的最晚付款时间，逾期将关闭交易。取值范围：1m～15d。m-分钟，h-小时，d-天，1c-当天（1c-当天的情况下，无论交易何时创建，都在0点关闭）。 该参数数值不接受小数点， 如 1.5h，可转换为 90m
        /// </summary>
        public string timeout_express { get; set; }
        ///// <summary>
        ///// 描述分账信息，Json格式，其它说明详见分账说明
        ///// </summary>
        //public Royalty_info royalty_info { get; set; }
        ///// <summary>
        ///// 间连受理商户信息体，当前只对特殊银行机构特定场景下使用此字段
        ///// </summary>
        //public Sub_merchant sub_merchant { get; set; }
        ///// <summary>
        ///// 外部指定买家
        ///// </summary>
        //public Ext_user_info ext_user_info { get; set; }
        /// <summary>
        /// 预授权号，预授权转交易请求中传入，适用于预授权转交易业务使用，目前只支持FUND_TRADE_FAST_PAY（资金订单即时到帐交易）、境外预授权产品（OVERSEAS_AUTH_PAY）两个产品。
        /// </summary>
        public string auth_no { get; set; }
    }

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
