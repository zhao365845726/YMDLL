using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using ML.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ML.ThirdParty
{
    /// <summary>
    /// 阿里云短信
    /// </summary>
    public class AliCloudCommunication
    {
        #region ----公共变量----
        /// <summary>
        /// 应用Key
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        /// 应用密钥
        /// </summary>
        public string AccessKeySecret { get; set; }
        #endregion

        #region ----统一发送接口----
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="Tel"></param>
        /// <param name="strVerCode"></param>
        /// <param name="SignName"></param>
        /// <param name="TemplateCode"></param>
        /// <returns></returns>
        private string SendSMS(string Tel, string strVerCode, string SignName, string TemplateCode)
        {
            //发送信息给手机号码
            String product = "Dysmsapi";//短信API产品名称（短信产品名固定，无需修改）
            String domain = "dysmsapi.aliyuncs.com";//短信API产品域名（接口地址固定，无需修改）
            //String accessKeyId = "LTAI6EX4sCzeA203";//你的accessKeyId，参考本文档步骤2
            //String accessKeySecret = "8J1UsfrNY4jGVTMv17495PE4hAiuha";//你的accessKeySecret，参考本文档步骤2
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", AccessKeyId, AccessKeySecret);
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
                return e.ErrorMessage;
            }
            catch (ClientException e)
            {
                return e.ErrorMessage;
            }
        }
        #endregion


        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="tel">手机号码</param>
        /// <returns></returns>
        public string SendVerificationCode(SendSMSParam ssp,out string strRandResult)
        {
            RandomHelper rh = new RandomHelper();
            strRandResult = rh.GetRandomInt(100000, 999999).ToString();
            string strResult = SendSMS(ssp.Mobile, strRandResult, ssp.SignName, ssp.TemplateCode);
            return strResult;
        }

    }

    #region ----请求参数类----
    /// <summary>
    /// 发送短信的基本参数(建议在配置文件中配置)
    /// </summary>
    public class SendSMSBaseParam
    {
        /// <summary>
        /// 短信签名
        /// </summary>
        public string SignName { get; set; }

        /// <summary>
        /// 模版代码
        /// </summary>
        public string TemplateCode { get; set; }
    }

    /// <summary>
    /// 发送短信参数
    /// </summary>
    public class SendSMSParam : SendSMSBaseParam
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
    }
    #endregion


}
