using System;

/**
* 商户基础 配置
* @author guoyx e-mail:guoyx@lianlian.com
* @date:2013-6-25 下午01:45:40
* @version :1.0
*
*/

namespace ML.ThirdParty.LLYTPay
{
	public class PartnerConfig
	{

		// RSA银通公钥
		public static string YT_PUB_KEY     = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCSS/DiwdCf/aZsxxcacDnooGph3d2JOj5GXWi+q3gznZauZjkNP8SKl3J2liP0O6rU/Y/29+IUe+GTMhMOFJuZm1htAtKiu5ekW0GlBMWxf4FPkYlQkPE0FtaoMP3gYfh+OwI+fIRrpW3ySn3mScnc6Z700nU/VYrRkfcSCbSnRwIDAQAB";
		// RSA商户私钥
		public static string TRADER_PRI_KEY = "MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBAMer1kZdlqdrxbjchHqMObRvfFYD0RDao/B0ev9HmPRcrlaz3O3eKcOF5NBOJslphTbgSG1saRVZVNRDYXie50TtNGbnClB0AW8c9Ixf4KAzmCr7OQ7q4fRjzXottZbbmwApuCBTcFwJHbl1aLFpSoX5NyVclr22xFTHc3HZ7TJHAgMBAAECgYBgSzjdWokrWMhULNUfHL0/jXyTJugOjsL9Vc9ziZ30SzYwzjE/7iKKDuqYovgFroP2QRfs3ZmYGCrW61/4gfnZ48ViR/ZjMGEWtmo9jEM6SZogvlx+LOOrCZykNFGCW7nmLgZyUckCT0ijU6LsPwPMWbf/+tbuxZ0dIG5/SLqGkQJBAObaLP8vTnUB9vPJby+wRusrP1Gln43GteialDuS6dI8HOlIGDo/p3WdvXExTXf5vR+OTbW8coDyjOn8pq2IeC0CQQDdbBwnD3PKgmq8SL49OR/ZX/VMEAlxFOT8LnYIQAH12SVQTto0FjfvaHESTtDV/JnkVQdif/PbvDaXkQG1AUjDAkAF/siIYAwjkcd+EU8n5+YPmXHthuWb4vs6bTlISspzwUfm7w5iBOEudshCtksSwJOezC1MePZoTuRF91/ExfSJAkB7RIWDxVl8IxDS01h9cwDlHkPMXZ00BCLate7l9uRgfswEInHdz4TCVo2kWJZwmtj9wcyDrKIQ8X4e8Q5XO2jLAkApyS9I0Nktd/SksJOcU64EQY+DGflI7/4+DZc+APMy2nleR1wDLjtCexPPkwkjtyOk96hI+1EO+jK2S4KIqN/J";
		// MD5 KEY
		public static string MD5_KEY        = "201408071000001543test_20140812";
		// 接收异步通知地址
		public static string NOTIFY_URL = "http://yourdomain/notify_url.aspx";  //请变更yourdomain为你的域名（及端口）
		// 支付结束后返回地址
		public static string URL_RETURN = "http://yourdomain/urlReturn.aspx";    //请变更为您的返回地址
		// 商户编号
		public static string            OID_PARTNER= "201706121001811585";     //请变更为您的商户号
		// 签名方式 RSA或MD5
		public static string            SIGN_TYPE="MD5";    					//请选择签名方式
		// 接口版本号，固定1.0
		public static string VERSION        = "1.0";
		// 业务类型，连连支付根据商户业务为商户开设的业务类型； （101001：虚拟商品销售、109001：实物商品销售、108001：外部账户充值）
		public static string BUSI_PARTNER   = "101001";   //请选择业务类型
	}
}

