/*
 * Created by SharpDevelop.
 * User: lihp
 * Date: 2016/11/8
 * Time: 12:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
//using System.Windows.Forms;


namespace ML.ThirdParty.LLYTPay
{
	/// <summary>
	/// Description of Class1
	/// </summary>
	public class QueryPaymentRequestBean {


	/** 商户付款流水号 . */
	private string no_order;

		public string No_order {
			get {
				return no_order;
			}
			set {
				no_order = value;
			}
		}
	/** 连连银通付款流水号 . */
	private string oid_paybill;

		public string Oid_paybill {
			get {
				return oid_paybill;
			}
			set {
				oid_paybill = value;
			}
		}
	/** 商户编号 . */
	private string oid_partner;

		public string Oid_partner {
			get {
				return oid_partner;
			}
			set {
				oid_partner = value;
			}
		}
	/** 用户来源 . */
	private string platform;

		public string Platform {
			get {
				return platform;
			}
			set {
				platform = value;
			}
		}
	/** api当前版本号 . */
	private string api_version;

		public string Api_version {
			get {
				return api_version;
			}
			set {
				api_version = value;
			}
		}
	/** 签名方式 . */
	private string sign_type;

		public string Sign_type {
			get {
				return sign_type;
			}
			set {
				sign_type = value;
			}
		}
	/** 签名方 . */
	private string sign;

		public string Sign {
			get {
				return sign;
			}
			set {
				sign = value;
			}
		}
}
	
}
