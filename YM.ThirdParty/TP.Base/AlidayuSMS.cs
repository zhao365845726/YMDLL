using ML.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Aliyun.Acs.Core.Exceptions;

namespace ML.ThirdParty.Base
{
    /// <summary>
    /// 阿里大鱼短信第三方对接
    /// </summary>
    public class AlidayuSMS
    {
        /// <summary>
        /// 正式环境地址
        /// </summary>
        public string ServerUrl = "http://gw.api.taobao.com/router/rest";
        /// <summary>
        /// 应用Key
        /// </summary>
        public string AppKey = "LTAI6EX4sCzeA203";
        /// <summary>
        /// 应用密钥
        /// </summary>
        public string Secret = "8J1UsfrNY4jGVTMv17495PE4hAiuha";


        /// <summary>
        /// 发送短信
        /// </summary>
        /// <returns></returns>
        public string SendSMS(AppInfo ai, SMSInfo si)
        {
            ITopClient client = new DefaultTopClient(ai.ServerUrl, ai.AppKey, ai.Secret);
            AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            req.Extend = si.Extend;
            req.SmsType = si.SmsType;
            req.SmsFreeSignName = si.SmsFreeSignName;
            req.SmsParam = si.SmsParam;
            req.RecNum = si.RecNum;
            req.SmsTemplateCode = si.SmsTemplateCode;
            AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
            return rsp.Body;
        }

        /// <summary>
        /// 新发送短信
        /// </summary>
        /// <param name="Tel"></param>
        /// <param name="strVerCode"></param>
        /// <param name="SignName"></param>
        /// <param name="TemplateCode"></param>
        /// <returns></returns>
        public string NewSendSMS(string Tel,string strVerCode,string SignName,string TemplateCode)
        {
            //发送信息给手机号码
            String product = "Dysmsapi";//短信API产品名称（短信产品名固定，无需修改）
            String domain = "dysmsapi.aliyuncs.com";//短信API产品域名（接口地址固定，无需修改）
            String accessKeyId = "LTAI6EX4sCzeA203";//你的accessKeyId，参考本文档步骤2
            String accessKeySecret = "8J1UsfrNY4jGVTMv17495PE4hAiuha";//你的accessKeySecret，参考本文档步骤2
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            //IAcsClient client = new DefaultAcsClient(profile);
            // SingleSendSmsRequest request = new SingleSendSmsRequest();
            //初始化ascClient,暂时不支持多region（请勿修改）
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为20个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = Tel;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = SignName;
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = TemplateCode;
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = "{\"code\":\"" + strVerCode + "\"}";
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = "yourOutId";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                return sendSmsResponse.Message;
            }
            catch (ServerException e)
            {
                //System.Console.WriteLine("Hello World!");
                return e.ErrorMessage;
            }
            catch (ClientException e)
            {
                //System.Console.WriteLine("Hello World!");
                return e.ErrorMessage;
            }
        }

        /// <summary>
        /// 发送绑定手机验证码
        /// </summary>
        /// <param name="vci">验证码信息参数</param>
        /// <returns></returns>
        public string SendBindPhoneVerificationCode(VerificationCodeInfo vci)
        {
            RandomHelper rh = new RandomHelper();
            string strRand = rh.GetRandomInt(100000, 999999).ToString();
            ITopClient client = new DefaultTopClient(ServerUrl, AppKey, Secret);
            AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            req.Extend = "";
            req.SmsType = "normal";
            req.SmsFreeSignName = "金钻团队";
            req.SmsParam = "{\"code\":\"" + strRand + "\",\"product\":\"金钻娱乐\"}";
            req.RecNum = vci.Phone_Number;
            req.SmsTemplateCode = "SMS_70180517";
            AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
            string strInsResult = "";
            return rsp.Body;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="tel">手机号码</param>
        /// <returns></returns>
        public string SendVerificationCode(string tel)
        {
            RandomHelper rh = new RandomHelper();
            string strRand = rh.GetRandomInt(100000, 999999).ToString();
            string strResult = NewSendSMS(tel, strRand, "钻石天下", "SMS_119910594");

            return strResult;
        }

        /// <summary>
        /// 发送修改密码验证码
        /// </summary>
        /// <param name="tel">手机号码</param>
        /// <returns></returns>
        public string SendForgetPwdVerificationCode(string tel)
        {
            RandomHelper rh = new RandomHelper();
            string strRand = rh.GetRandomInt(100000, 999999).ToString();
            //ITopClient client = new DefaultTopClient(ServerUrl, AppKey, Secret);
            //AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            //req.Extend = "";
            //req.SmsType = "normal";
            //req.SmsFreeSignName = "钻石天下";
            //req.SmsParam = "{\"code\":\"" + strRand + "\",\"product\":\"钻石天下\"}";
            //req.RecNum = tel;
            //req.SmsTemplateCode = "SMS_119920610";
            //AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
            string strResult = NewSendSMS(tel, strRand, "钻石天下", "SMS_119920610");
            return strResult;
        }

        /// <summary>
        /// 查询验证码
        /// </summary>
        /// <param name="qvci">查询验证码信息参数</param>
        /// <returns></returns>
        public string QueryVerificationCode(QueryVerificationCodeInfo qvci)
        {
            ITopClient client = new DefaultTopClient(ServerUrl, AppKey, Secret,"json");
            AlibabaAliqinFcSmsNumQueryRequest req = new AlibabaAliqinFcSmsNumQueryRequest();
            //req.BizId = "";
            req.RecNum = qvci.Phone_Number;
            req.QueryDate = DateTime.Now.ToString("yyyyMMdd");
            req.CurrentPage = 1L;
            req.PageSize = 1L;
            AlibabaAliqinFcSmsNumQueryResponse rsp = client.Execute(req);
            return rsp.Body;
        }
    }

    /// <summary>
    /// 阿里大鱼公共请求参数
    /// </summary>
    public class AlidayuCommonParam
    {
        /// <summary>
        /// [必填]---API接口名称
        /// </summary>
        public string method { get; set; }
        /// <summary>
        /// [必填]---TOP分配给应用的AppKey
        /// </summary>
        public string app_key { get; set; }
        /// <summary>
        /// 被调用的目标AppKey，仅当被调用的API为第三方ISV提供时有效
        /// </summary>
        public string target_app_key { get; set; }
        /// <summary>
        /// [必填]---签名的摘要算法，可选值为：hmac，md5
        /// </summary>
        public string sign_method { get; set; }
        /// <summary>
        /// [必填]---API输入参数签名结果
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 用户登录授权成功后，TOP颁发给应用的授权信息，详细介绍请点击这里。当此API的标签上注明：“需要授权”，则此参数必传；“不需要授权”，则此参数不需要传；“可选授权”，则此参数为可选。
        /// </summary>
        public string session { get; set; }
        /// <summary>
        /// [必填]---时间戳，格式为yyyy-MM-dd HH:mm:ss，时区为GMT+8，例如：2015-01-01 12:00:00。淘宝API服务端允许客户端请求最大时间误差为10分钟
        /// </summary>
        public string timestamp { get; set; }
        /// <summary>
        /// 响应格式。默认为xml格式，可选值：xml，json
        /// </summary>
        public string format { get; set; }
        /// <summary>
        /// [必填]---API协议版本，可选值：2.0。
        /// </summary>
        public string v { get; set; }
        /// <summary>
        /// 合作伙伴身份标识。
        /// </summary>
        public string partner_id { get; set; }
        /// <summary>
        /// 是否采用精简JSON返回格式，仅当format=json时有效，默认值为：false
        /// </summary>
        public string simplify { get; set; }
        /// <summary>
        /// 短信发送请求参数
        /// </summary>
        public AlidayuSendRequestParam asrp { get; set; }
    }

    /// <summary>
    /// 短信发送请求参数
    /// </summary>
    public class AlidayuSendRequestParam
    {
        /// <summary>
        /// 公共回传参数，在“消息返回”中会透传回该参数；举例：用户可以传入自己下级的会员ID，在消息返回时，该会员ID会包含在内，用户可以根据该会员ID识别是哪位会员使用了你的应用
        /// </summary>
        public string extend { get; set; }
        /// <summary>
        /// [必填]---短信类型，传入值请填写normal
        /// </summary>
        public string sms_type { get; set; }
        /// <summary>
        /// [必填]---短信签名，传入的短信签名必须是在阿里大于“管理中心-验证码/短信通知/推广短信-配置短信签名”中的可用签名。如“阿里大于”已在短信签名管理中通过审核，则可传入”阿里大于“（传参时去掉引号）作为短信签名。短信效果示例：【阿里大于】欢迎使用阿里大于服务
        /// </summary>
        public string sms_free_sign_name { get; set; }
        /// <summary>
        /// 短信模板变量，传参规则{"key":"value"}，key的名字须和申请模板中的变量名一致，多个变量之间以逗号隔开。示例：针对模板“验证码${code}，您正在进行${product}身份验证，打死不要告诉别人哦！”，传参时需传入{"code":"1234","product":"alidayu"}
        /// </summary>
        public string sms_param { get; set; }
        /// <summary>
        /// [必填]---短信接收号码。支持单个或多个手机号码，传入号码为11位手机号码，不能加0或+86。群发短信需传入多个号码，以英文逗号分隔，一次调用最多传入200个号码。示例：18600000000,13911111111,13322222222
        /// </summary>
        public string rec_num { get; set; }
        /// <summary>
        /// [必填]---短信模板ID，传入的模板必须是在阿里大于“管理中心-短信模板管理”中的可用模板。示例：SMS_585014
        /// </summary>
        public string sms_template_code { get; set; }
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

    /// <summary>
    /// 验证码信息
    /// </summary>
    public class VerificationCodeInfo
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone_Number { get; set; }
        ///// <summary>
        ///// 验证码模版参数
        ///// </summary>
        //public VerificationCodeTemplate VCT { get; set; }
    }

    /// <summary>
    /// 查询验证码参数
    /// </summary>
    public class QueryVerificationCodeInfo
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone_Number { get; set; }
        ///// <summary>
        ///// 查询日期
        ///// </summary>
        //public string Date { get; set; }
        ///// <summary>
        ///// 页码
        ///// </summary>
        //public long PageIndex { get; set; }
        ///// <summary>
        ///// 页大小
        ///// </summary>
        //public long PageSize { get; set; }
    }
}
