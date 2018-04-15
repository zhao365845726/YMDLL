using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using YMDLL.Class;

namespace ML.ThirdParty.Base
{
    /// <summary>
    /// 极光推送
    /// </summary>
    public class JGPush
    {
        /// <summary>
        /// 网页操作类
        /// </summary>
        public static CS_OperaWeb ow = new CS_OperaWeb();
        /// <summary>
        /// 连连支付请求地址
        /// </summary>
        public static string JGPush_URL = "https://api.jpush.cn/v3/push";

        public String TITLE = "Test from C# v3 sdk";
        public String ALERT = "Test from  C# v3 sdk - alert";
        public String MSG_CONTENT = "Test from C# v3 sdk - msgContent";
        public String REGISTRATION_ID = "0900e8d85ef";
        public String SMSMESSAGE = "Test from C# v3 sdk - SMSMESSAGE";
        public int DELAY_TIME = 1;
        public String TAG = "tag_api";
        /// <summary>
        /// 应用APPKey
        /// </summary>
        public static String app_key = "e78d2f6ef44db51e508436ca";
        /// <summary>
        /// 应用MasterSecret
        /// </summary>
        public static String master_secret = "518f490e4784c13cd46e8e52";

        /// <summary>
        /// 格式化参数
        /// </summary>
        /// <param name="dicParam"></param>
        /// <returns></returns>
        public static string FormatParam(Dictionary<string, string> dicParam)
        {
            int i = 0;
            string str_Format = "";
            foreach (KeyValuePair<string, string> dicItem in dicParam)
            {
                //如果字符为空或者null直接去掉这个参数
                if (dicItem.Value == null || dicItem.Value.ToString() == "")
                {
                    i++;
                    continue;
                }
                if (i == dicParam.Count - 1)
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

        //有序字典（最简单的有序字典）转为json
        public static string dictToJson(SortedDictionary<string, object> dict)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");
            foreach (KeyValuePair<string, object> temp in dict)
            {
                json.Append("\"" + temp.Key + "\"" + ":" + "\"" + temp.Value + "\"");
                json.Append(",");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("}");
            string content = json.ToString();
            return content;
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        public string PushMessage(JGPushObjectParam pop)
        {
            string result = string.Empty;
            string strTest = "{\"audience\":\"all\",\"message\":{\"msg_content\" : \"极光推送\"},\"notification\":{\"alert\" : \"" + pop.Message + "\"},\"platform\":\"all\"}";
            result = HttpPostDataByAuth(JGPush_URL, strTest);
            return result;

            //SortedDictionary<string, object> dicPush = new SortedDictionary<string, object>();
            //dicPush.Add("platform", "all");
            //dicPush.Add("audience", "all");
            //dicPush.Add("notification", pop.notification);
            //dicPush.Add("message", pop.message);
            //result = ow.HttpPostData(JGPush_URL, FastJSON.JSON.ToJSON(dicPush));

            //result = HttpPostDataByAuth(JGPush_URL, FastJSON.JSON.ToJSON(dicPush));
        }

        /// <summary>
        /// 模拟提交数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public string HttpPostDataByAuth(string url, string param, string contentType = "application/x-www-form-urlencoded")
        {
            var result = string.Empty;
            //注意提交的编码 这边是需要改变的 这边默认的是Default：系统当前编码
            byte[] postData = Encoding.UTF8.GetBytes(param);

            // 设置提交的相关参数 
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            Encoding myEncoding = Encoding.UTF8;
            request.Method = "POST";
            request.KeepAlive = false;
            request.AllowAutoRedirect = true;
            request.ContentType = contentType;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR  3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
            request.ContentLength = postData.Length;
            //获得用户名密码的Base64编码
            string code = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", app_key, master_secret)));

            //添加Authorization到HTTP头
            request.Headers.Add("Authorization", "Basic " + code);

            // 提交请求数据 
            System.IO.Stream outputStream = request.GetRequestStream();
            outputStream.Write(postData, 0, postData.Length);
            outputStream.Close();

            HttpWebResponse response;
            Stream responseStream;
            StreamReader reader;
            string srcString;
            response = request.GetResponse() as HttpWebResponse;
            responseStream = response.GetResponseStream();
            reader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
            srcString = reader.ReadToEnd();
            result = srcString;   //返回值赋值
            reader.Close();

            return result;
        }

        //public PushPayload PushObject_All_All_Alert()
        //{
        //    PushPayload pushPayload = new PushPayload();
        //    pushPayload.platform = Platform.all();
        //    pushPayload.audience = Audience.all();
        //    pushPayload.notification = new Notification().setAlert(ALERT);
        //    return pushPayload;
        //}
        //public PushPayload PushObject_all_alia_alert()
        //{

        //    PushPayload pushPayload_alias = new PushPayload();
        //    pushPayload_alias.platform = Platform.android();
        //    pushPayload_alias.audience = Audience.s_alias("alias1");
        //    pushPayload_alias.notification = new Notification().setAlert(ALERT);
        //    return pushPayload_alias;
        //}
        //public PushPayload PushObject_all_alias_alert()
        //{

        //    PushPayload pushPayload_alias = new PushPayload();
        //    pushPayload_alias.platform = Platform.android();
        //    string[] alias = new string[] { "alias1", "alias2", "alias3" };
        //    Console.WriteLine(alias);
        //    pushPayload_alias.audience = Audience.s_alias(alias);
        //    pushPayload_alias.notification = new Notification().setAlert(ALERT);
        //    return pushPayload_alias;
        //}
        //public PushPayload PushObject_Android_Tag_AlertWithTitle()
        //{
        //    PushPayload pushPayload = new PushPayload();

        //    pushPayload.platform = Platform.android();
        //    pushPayload.audience = Audience.s_tag("tag1");
        //    pushPayload.notification = Notification.android(ALERT, TITLE);
        //    return pushPayload;
        //}
        //public PushPayload PushObject_android_and_ios()
        //{
        //    PushPayload pushPayload = new PushPayload();
        //    pushPayload.platform = Platform.android_ios();
        //    var audience = Audience.s_tag("tag1");
        //    pushPayload.audience = audience;
        //    var notification = new Notification().setAlert("alert content");
        //    notification.AndroidNotification = new AndroidNotification().setTitle("Android Title");
        //    notification.IosNotification = new IosNotification();
        //    notification.IosNotification.incrBadge(1);
        //    notification.IosNotification.AddExtra("extra_key", "extra_value");
        //    pushPayload.notification = notification.Check();
        //    return pushPayload;
        //}
        //public PushPayload PushObject_android_with_options()
        //{
        //    PushPayload pushPayload = new PushPayload();
        //    pushPayload.platform = Platform.android_ios();
        //    var audience = Audience.all();
        //    pushPayload.audience = audience;
        //    var notification = new Notification().setAlert("alert content");
        //    AndroidNotification androidnotification = new AndroidNotification();
        //    androidnotification.setAlert("Push Object android with options");
        //    androidnotification.setBuilderID(3);
        //    androidnotification.setStyle(1);
        //    androidnotification.setBig_text("big text content");
        //    androidnotification.setInbox("JSONObject");
        //    androidnotification.setBig_pic_path("picture url");
        //    androidnotification.setPriority(0);
        //    androidnotification.setCategory("category str");
        //    notification.AndroidNotification = androidnotification;
        //    notification.IosNotification = new IosNotification();
        //    notification.IosNotification.incrBadge(1);
        //    notification.IosNotification.AddExtra("extra_key", "extra_value");
        //    pushPayload.notification = notification.Check();
        //    return pushPayload;
        //}
        //public PushPayload PushObject_ios_tagAnd_alertWithExtrasAndMessage()
        //{
        //    PushPayload pushPayload = new PushPayload();
        //    pushPayload.platform = Platform.android_ios();
        //    pushPayload.audience = Audience.s_tag_and("tag1", "tag_all");
        //    var notification = new Notification();
        //    notification.IosNotification = new IosNotification().setAlert(ALERT).setBadge(5).setSound("happy").AddExtra("from", "JPush");
        //    pushPayload.notification = notification;
        //    pushPayload.message = Message.content(MSG_CONTENT);
        //    return pushPayload;
        //}
        //public PushPayload PushObject_ios_alert_json()
        //{
        //    PushPayload pushPayload = new PushPayload();
        //    pushPayload.platform = Platform.all();
        //    pushPayload.audience = Audience.all();
        //    var notification = new Notification();
        //    Hashtable alert = new Hashtable();
        //    alert["title"] = "JPush Title";
        //    alert["subtitle"] = "JPush Subtitle";
        //    alert["body"] = "JPush Body";
        //    notification.IosNotification = new IosNotification().setAlert(alert).setBadge(5).setSound("happy").AddExtra("from", "JPush");
        //    pushPayload.notification = notification;
        //    pushPayload.message = Message.content(MSG_CONTENT);
        //    return pushPayload;
        //}
        //public PushPayload PushObject_ios_audienceMore_messageWithExtras()
        //{
        //    var pushPayload = new PushPayload();
        //    pushPayload.platform = Platform.android_ios();
        //    pushPayload.audience = Audience.s_tag("tag1", "tag2");
        //    pushPayload.message = Message.content(MSG_CONTENT).AddExtras("from", "JPush");
        //    return pushPayload;
        //}
        //public PushPayload PushObject_apns_production_options()
        //{
        //    var pushPayload = new PushPayload();
        //    pushPayload.platform = Platform.android_ios();
        //    pushPayload.audience = Audience.s_tag("tag1", "tag2");
        //    pushPayload.message = Message.content(MSG_CONTENT).AddExtras("from", "JPush");
        //    pushPayload.options.apns_production = false;
        //    return pushPayload;

        //}
        //public PushPayload PushSendSmsMessage()
        //{
        //    var pushPayload = new PushPayload();
        //    pushPayload.platform = Platform.all();
        //    pushPayload.audience = Audience.all();
        //    pushPayload.notification = new Notification().setAlert(ALERT);
        //    SmsMessage sms_message = new SmsMessage();
        //    sms_message.setContent(SMSMESSAGE);
        //    sms_message.setDelayTime(DELAY_TIME);
        //    pushPayload.sms_message = sms_message;
        //    return pushPayload;
        //}
    }

    /// <summary>
    /// 推送对象参数
    /// </summary>
    public class JGPushObjectParam
    {
        ///// <summary>
        ///// 推送平台设置
        ///// </summary>
        //public string platform { get; set; }
        ///// <summary>
        ///// 推送设备指定
        ///// </summary>
        //public string audience { get; set; }
        ///// <summary>
        ///// 通知内容体。是被推送到客户端的内容。与 message 一起二者必须有其一，可以二者并存
        ///// </summary>
        //public JGNotificationParam notification { get; set; }
        ///// <summary>
        ///// 消息内容体。是被推送到客户端的内容。与 notification 一起二者必须有其一，可以二者并存
        ///// </summary>
        //public JGMessageParam message { get; set; }
        ///// <summary>
        ///// 短信渠道补充送达内容体
        ///// </summary>
        //public string sms_message { get; set; }
        ///// <summary>
        ///// 推送参数
        ///// </summary>
        //public string options { get; set; }
        ///// <summary>
        ///// 用于防止 api 调用端重试造成服务端的重复推送而定义的一个标识符。
        ///// </summary>
        //public string cid { get; set; }
        /// <summary>
        /// 推送的消息内容
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// 通知参数
    /// </summary>
    public class JGNotificationParam
    {
        /// <summary>
        /// 通知内容
        /// </summary>
        public string alert { get; set; }
    }

    /// <summary>
    /// 自定义消息
    /// </summary>
    public class JGMessageParam
    {
        /// <summary>
        /// 消息内容本身
        /// </summary>
        public string msg_content { get; set; }
    }
}
