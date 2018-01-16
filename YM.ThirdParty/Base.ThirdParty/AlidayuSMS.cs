using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace ML.ThirdParty
{
    /// <summary>
    /// 阿里大鱼短信第三方对接（2018-01-17失效）
    /// </summary>
    public class AlidayuSMS
    {
        ///// <summary>
        ///// 发送短信
        ///// </summary>
        ///// <returns></returns>
        //public string SendSMS(AppInfo ai,SMSInfo si)
        //{
        //    ITopClient client = new DefaultTopClient(ai.ServerUrl, ai.AppKey, ai.Secret);
        //    AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
        //    req.Extend = si.Extend;
        //    req.SmsType = si.SmsType;
        //    req.SmsFreeSignName = si.SmsFreeSignName;
        //    req.SmsParam = si.SmsParam;
        //    req.RecNum = si.RecNum;
        //    req.SmsTemplateCode = si.SmsTemplateCode;
        //    AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
        //    return rsp.Body;
        //}
    }

    /// <summary>
    /// 应用信息
    /// </summary>
    public class AppInfo
    {
        /// <summary>
        /// 请求的URL
        /// </summary>
        public string ServerUrl { get; set; }
        /// <summary>
        /// 应用的APPID
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 应用的私钥
        /// </summary>
        public string Secret { get; set; }
    }

    /// <summary>
    /// 短信信息
    /// </summary>
    public class SMSInfo
    {
        /// <summary>
        /// 公共回传参数
        /// </summary>
        public string Extend { get; set; }
        /// <summary>
        /// 短信类型(请填写normal)
        /// </summary>
        public string SmsType { get; set; }
        /// <summary>
        /// 短信签名
        /// </summary>
        public string SmsFreeSignName { get; set; }
        /// <summary>
        /// 短信模板变量
        /// </summary>
        public string SmsParam { get; set; }
        /// <summary>
        /// 短信接收号码
        /// </summary>
        public string RecNum { get; set; }
        /// <summary>
        /// 短信模板ID
        /// </summary>
        public string SmsTemplateCode { get; set; }
    }
}
