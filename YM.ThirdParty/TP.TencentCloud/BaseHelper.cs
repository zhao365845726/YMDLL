using QCloudAPI_SDK.Center;
using QCloudAPI_SDK.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ML.ThirdParty.TencentCloud
{
    /// <summary>
    /// 腾讯云基类
    /// </summary>
    public class BaseHelper
    {
        /// <summary>
        /// SecretId
        /// </summary>
        public const string _SECRET_ID = "AKIDGMDpBHCoFQL2WmPIDuMZXFhy2qTtFxb3";

        /// <summary>
        /// SecretKey
        /// </summary>
        public const string _SECRET_KEY = "KQGqCJAk49xVnoa6uTlnYbnlTv9AwTlw";

        /// <summary>
        /// 请求方式
        /// </summary>
        public const string _REQUEST_METHOD = "GET";

        /// <summary>
        /// 默认区域
        /// </summary>
        public const string _DEFAULT_REGION = "bj";

        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="actionname"></param>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        public string ReturnResult(string actionname, SortedDictionary<string, object> requestParams)
        {
            SortedDictionary<string, object> config = new SortedDictionary<string, object>(StringComparer.Ordinal);
            config["SecretId"] = _SECRET_ID;
            config["SecretKey"] = _SECRET_KEY;
            config["RequestMethod"] = _REQUEST_METHOD;
            config["DefaultRegion"] = _DEFAULT_REGION;

            QCloudAPIModuleCenter module = new QCloudAPIModuleCenter(new Vod(), config);
            return module.Call(actionname, requestParams);
        }
    }
}
