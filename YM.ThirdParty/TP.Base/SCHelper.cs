﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YMDLL.Class;
using YMDLL.Common;

namespace ML.ThirdParty.Base
{
    /// <summary>
    /// 善彩帮助类
    /// </summary>
    public class SCHelper
    {
        /// <summary>
        /// 网页操作对象
        /// </summary>
        public static CS_OperaWeb ow = new CS_OperaWeb();
        /// <summary>
        /// 日期时间操作对象
        /// </summary>
        public static CS_CalcDateTime cdt = new CS_CalcDateTime();
        /// <summary>
        /// 善彩测试商户号
        /// </summary>
        public static string SH_CS_Number = "29002000143";
        /// <summary>
        /// 善彩商户号
        /// </summary>
        public static string SH_Number = "29002000145";
        /// <summary>
        /// 善彩测试链接
        /// </summary>
        public static string SC_CS_URL = "http://122.144.133.213:8047/ChinaLotteryPlatform/lotteryAgent";
        /// <summary>
        /// 善彩链接
        /// </summary>
        public static string SC_URL = "http://150.242.239.195:8047/ChinaLotteryPlatform/lotteryAgent";
        /// <summary>
        /// 善彩测试环境的Key
        /// </summary>
        public static string SC_KEY = "AUTOLOTTERY";

        /// <summary>
        /// 自动生成校验码
        /// </summary>
        /// <param name="abi"></param>
        /// <returns></returns>
        public static string GeneralKeyGen(AgentBaseInfo abi)
        {
            return Md5.GetMD5String(SC_KEY + abi.Service + abi.TimeStamp + SC_KEY);    //校验码
        }

        /// <summary>
        /// 格式化参数
        /// </summary>
        /// <param name="dicParam"></param>
        /// <returns></returns>
        public static string FormatParam(Dictionary<string,string> dicParam)
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

        /// <summary>
        /// 彩种查询
        /// </summary>
        public static void GetLotteryKind(AgentBaseInfo abi)
        {
            string strUrl = SC_URL + "!lotteryCodeQuery";
            string result = string.Empty;
            Dictionary<string, string> dicKind = new Dictionary<string, string>();
            abi.TimeStamp = cdt.GetTimeStamp();
            dicKind.Add("merid", SH_Number);
            dicKind.Add("service", abi.Service);
            dicKind.Add("timestamp", abi.TimeStamp);
            dicKind.Add("digest", GeneralKeyGen(abi));
            result = ow.HttpPostData(strUrl, FormatParam(dicKind));

            Console.Write("GetLotteryKind---->" + result);
            Console.ReadLine();
        }

        /// <summary>
        /// 彩种期次查询
        /// </summary>
        public static string GetLotteryAwardPeriod(LotteryAwardPeriodParam lapp)
        {
            string strUrl = SC_URL + "!queryPeriodInfo";
            string result = string.Empty;
            Dictionary<string, string> dicAwardPeriod = new Dictionary<string, string>();
            string strTimeStamp = cdt.GetTimeStamp();
            dicAwardPeriod.Add("merid", SH_Number);
            dicAwardPeriod.Add("service", "queryPeriodInfo");
            dicAwardPeriod.Add("lotteryCode", lapp.lotteryCode);
            dicAwardPeriod.Add("periodCode", lapp.periodCode);
            dicAwardPeriod.Add("status", lapp.status.ToString());
            dicAwardPeriod.Add("size", lapp.size.ToString());
            dicAwardPeriod.Add("timestamp", strTimeStamp);
            AgentBaseInfo abi = new AgentBaseInfo();
            abi.TimeStamp = strTimeStamp;
            abi.Service = "queryPeriodInfo";
            dicAwardPeriod.Add("digest", GeneralKeyGen(abi));
            result = ow.HttpPostData(strUrl, FormatParam(dicAwardPeriod));

            return result;
        }

        /// <summary>
        /// 投注结果通知[善彩调用我的接口]
        /// </summary>
        public static void BettingResultNotice()
        {
            string strUrl = SC_URL;
            string result = string.Empty;
            string str_merid = "29002000143",                       //商户号
                str_lotteryCode = "51",                             //彩种代码
                str_orderID = "29002000215201609050001580315",      //系统订单号
                str_ticketStatus = "0",                             //出票状态(0:初始状态,1:成功,-1:投注失败)
                str_message = "",                                   //响应结果
                str_service = "notify",                             //操作描述
                str_timestamep = cdt.GetTimeStamp(),                //时间戳
                str_key = "AUTOLOTTERY",                            //生成秘钥的key
                str_digest = Md5.GetMD5String(str_key + str_service + str_timestamep + str_key);            //校验码
            string param = string.Format("merid={0}&lotteryCode={1}&orderID={2}&ticketStatus={3}&message={4}&service={5}&timestamp={6}&digest={7}",
                str_merid, str_lotteryCode, str_orderID, str_ticketStatus, str_message, str_service, str_timestamep, str_digest);
            result = ow.HttpPostData(strUrl, param);


            Console.Write("BettingResultNotice---->" + result);
            Console.ReadLine();
        }

        /// <summary>
        /// 订单状态查询
        /// </summary>
        public static void GetOrderStatus(AgentBaseInfo abi)
        {
            string strUrl = SC_URL + "!queryOrder";
            string result = string.Empty;
            Dictionary<string, string> dicOrderStatus = new Dictionary<string, string>();
            abi.TimeStamp = cdt.GetTimeStamp();
            dicOrderStatus.Add("merid", SH_Number);
            dicOrderStatus.Add("lotteryCode", abi.LotteryCode);
            dicOrderStatus.Add("orderID", abi.OrderId);
            dicOrderStatus.Add("service", abi.Service);
            dicOrderStatus.Add("timestamp", abi.TimeStamp);
            dicOrderStatus.Add("digest", GeneralKeyGen(abi));
            dicOrderStatus.Add("agentOrderID", abi.AgentOrderId);
            result = ow.HttpPostData(strUrl, FormatParam(dicOrderStatus));
            Console.Write("GetOrderStatus---->" + result);
            Console.ReadLine();
        }

        /// <summary>
        /// 开奖公告查询
        /// </summary>
        public static void GetOpenNotice(AgentBaseInfo abi)
        {
            string strUrl = SC_URL + "!queryOpenNotice";
            string result = string.Empty;
            Dictionary<string, string> dicOpenNotice = new Dictionary<string, string>();
            abi.TimeStamp = cdt.GetTimeStamp();
            dicOpenNotice.Add("merid", SH_Number);
            dicOpenNotice.Add("lotteryCode", abi.LotteryCode);
            dicOpenNotice.Add("periodCode", abi.PeriodCode);
            dicOpenNotice.Add("service", abi.Service);
            dicOpenNotice.Add("timestamp", abi.TimeStamp);
            dicOpenNotice.Add("digest", GeneralKeyGen(abi));
            result = ow.HttpPostData(strUrl, FormatParam(dicOpenNotice));
            Console.Write("GetOpenNotice---->" + result);
            Console.ReadLine();
        }

        /// <summary>
        /// 期次返奖查询
        /// </summary>
        public static string GetWinRecordPageList(AwardWinRecordParam awrp)
        {
            string strUrl = SC_URL + "!queryWinRecordPageList";
            string result = string.Empty;
            Dictionary<string, string> dicWinRecordPageList = new Dictionary<string, string>();
            string strTimeStamp = cdt.GetTimeStamp();
            dicWinRecordPageList.Add("merid", SH_Number);
            dicWinRecordPageList.Add("lotteryCode", awrp.lotteryCode);
            dicWinRecordPageList.Add("periodCode", awrp.periodCode);
            dicWinRecordPageList.Add("page", awrp.page.ToString());
            dicWinRecordPageList.Add("pageSize", awrp.pageSize.ToString());
            dicWinRecordPageList.Add("service", "queryWinRecordPageList");
            dicWinRecordPageList.Add("timestamp", strTimeStamp);
            AgentBaseInfo abi = new AgentBaseInfo();
            abi.TimeStamp = strTimeStamp;
            abi.Service = "queryWinRecordPageList";
            dicWinRecordPageList.Add("digest", GeneralKeyGen(abi));
            result = ow.HttpPostData(strUrl, FormatParam(dicWinRecordPageList));
            return result;
        }

        /// <summary>
        /// 竞彩订单返奖查询
        /// </summary>
        public static string GetOrderPrize(OrderPrizeQueryParam opqp)
        {
            string strUrl = SC_URL + "!orderPrizeQuery";
            string result = string.Empty;
            Dictionary<string, string> dicOrderPrize = new Dictionary<string, string>();
            string strTimeStamp = cdt.GetTimeStamp();
            dicOrderPrize.Add("merid", SH_Number);
            dicOrderPrize.Add("lotteryCode", opqp.LotteryCode);
            dicOrderPrize.Add("orderID", opqp.OrderId);
            dicOrderPrize.Add("service", "orderPrizeQuery");
            dicOrderPrize.Add("timestamp", strTimeStamp);
            AgentBaseInfo abi = new AgentBaseInfo();
            abi.TimeStamp = strTimeStamp;
            abi.Service = "queryWinRecordPageList";
            dicOrderPrize.Add("digest", GeneralKeyGen(abi));
            result = ow.HttpPostData(strUrl, FormatParam(dicOrderPrize));
            //new Base_SysLogBll().InsertLog("竞彩订单返奖查询--->" + FormatParam(dicOrderPrize));
            Console.WriteLine(FormatParam(dicOrderPrize));
            return result;
        }

        /// <summary>
        /// 投注
        /// </summary>
        public static string BetLottery(AgentBaseInfo abi)
        {
            string strUrl = SC_URL + "!bet";
            string result = string.Empty;
            Dictionary<string, string> dicLottery = new Dictionary<string, string>();
            abi.TimeStamp = cdt.GetTimeStamp();
            dicLottery.Add("merid", SH_Number);
            dicLottery.Add("agentOrderId", abi.AgentOrderId);
            dicLottery.Add("userid", "bblgroup");
            dicLottery.Add("cardType", abi.CardType);
            dicLottery.Add("cardNumber", abi.CardNumber);
            dicLottery.Add("name", abi.UserName);
            dicLottery.Add("mail", abi.UserMail);
            dicLottery.Add("mobile", abi.UserMobile);
            dicLottery.Add("lotteryCode", abi.LotteryCode);
            dicLottery.Add("periodCode", abi.PeriodCode);
            dicLottery.Add("castcode", abi.CastCode);
            dicLottery.Add("isAdd", "0");
            dicLottery.Add("amount", abi.Amount);
            dicLottery.Add("count", abi.Count);
            dicLottery.Add("msg", abi.Msg);
            dicLottery.Add("service", abi.Service);
            dicLottery.Add("timestamp", abi.TimeStamp);
            dicLottery.Add("digest", GeneralKeyGen(abi));
            string strParamResult = FormatParam(dicLottery);
            result = ow.HttpPostData(strUrl, strParamResult);
            //new Base_SysLogBll().InsertLog("提交给善彩的参数:" + strParamResult);
            return result;
        }

        /// <summary>
        /// 投注
        /// </summary>
        public static string BetLotteryConsole(AgentBaseInfo abi)
        {
            string strUrl = SC_URL + "!bet";
            string result = string.Empty;
            Dictionary<string, string> dicLottery = new Dictionary<string, string>();
            abi.TimeStamp = cdt.GetTimeStamp();
            dicLottery.Add("merid", SH_Number);
            dicLottery.Add("agentOrderId", abi.AgentOrderId);
            dicLottery.Add("userid", "bblgroup");
            dicLottery.Add("cardType", abi.CardType);
            dicLottery.Add("cardNumber", abi.CardNumber);
            dicLottery.Add("name", abi.UserName);
            dicLottery.Add("mail", abi.UserMail);
            dicLottery.Add("mobile", abi.UserMobile);
            dicLottery.Add("lotteryCode", abi.LotteryCode);
            dicLottery.Add("periodCode", abi.PeriodCode);
            dicLottery.Add("castcode", abi.CastCode);
            dicLottery.Add("isAdd", "0");
            dicLottery.Add("amount", abi.Amount);
            dicLottery.Add("count", abi.Count);
            dicLottery.Add("msg", abi.Msg);
            dicLottery.Add("service", abi.Service);
            dicLottery.Add("timestamp", abi.TimeStamp);
            dicLottery.Add("digest", GeneralKeyGen(abi));
            string strParamResult = FormatParam(dicLottery);
            result = ow.HttpPostData(strUrl, strParamResult);
            return result;
        }

        /// <summary>
        /// 投注
        /// </summary>
        public static string BetLotteryBeta(AgentBaseInfo abi)
        {
            string strUrl = SC_CS_URL + "!bet";
            string result = string.Empty;
            Dictionary<string, string> dicLottery = new Dictionary<string, string>();
            abi.TimeStamp = cdt.GetTimeStamp();
            dicLottery.Add("merid", SH_CS_Number);
            dicLottery.Add("agentOrderId", abi.AgentOrderId);
            dicLottery.Add("userid", "bblgroup");
            dicLottery.Add("cardType", abi.CardType);
            dicLottery.Add("cardNumber", abi.CardNumber);
            dicLottery.Add("name", abi.UserName);
            dicLottery.Add("mail", abi.UserMail);
            dicLottery.Add("mobile", abi.UserMobile);
            dicLottery.Add("lotteryCode", abi.LotteryCode);
            dicLottery.Add("periodCode", abi.PeriodCode);
            dicLottery.Add("castcode", abi.CastCode);
            dicLottery.Add("isAdd", "0");
            dicLottery.Add("amount", abi.Amount);
            dicLottery.Add("count", abi.Count);
            dicLottery.Add("msg", abi.Msg);
            dicLottery.Add("service", abi.Service);
            dicLottery.Add("timestamp", abi.TimeStamp);
            dicLottery.Add("digest", GeneralKeyGen(abi));
            string strParamResult = FormatParam(dicLottery);
            result = ow.HttpPostData(strUrl, strParamResult);
            //new Base_SysLogBll().InsertLog("提交给善彩的参数:" + strParamResult);
            return result;
        }

        /// <summary>
        /// 投注参数效验
        /// </summary>
        public static void BetLotteryCheck()
        {
            string strUrl = SC_URL + "!betCheck";
            string result = string.Empty;
            string str_merid = "29002000143",   //商户号
                str_agentOrderId = "",          //代理商订单号
                str_userid = "",                //用户id
                str_cardType = "",              //证件类型
                str_cardNumber = "",            //证件号码
                str_name = "",                  //用户姓名
                str_mail = "",                  //用户邮箱
                str_mobile = "",                //用户手机
                str_lotteryCode = "51",         //彩种
                str_periodCode = "2015024",     //期次号
                str_castCode = "",              //投注号码
                str_isAdd = "",                 //是否追加
                str_amount = "",                //总金额
                str_count = "",                 //总注数
                str_msg = "",                   //备注
                str_service = "bet",            //操作描述
                str_timestamep = cdt.GetTimeStamp(),        //时间戳
                str_key = "AUTOLOTTERY",                    //生成秘钥的key
                str_digest = Md5.GetMD5String(str_key + str_service + str_timestamep + str_key);        //校验码
            string param = string.Format("merid={0}&agentOrderId={1}&userid={2}&cardType={3}&cardNumber={4}&name={5}&mail={6}&mobile={7}&lotteryCode={8}&periodCode={9}&castCode={10}&isAdd={11}&amount={12}&count={13}&msg={14}&service={15}&timestamp={16}&digest={17}",
                str_merid, str_agentOrderId, str_userid, str_cardType, str_cardNumber, str_name, str_mail, str_mobile, str_lotteryCode, str_periodCode, str_castCode, str_isAdd, str_amount, str_count, str_msg, str_service, str_timestamep, str_digest);
            result = ow.HttpPostData(strUrl, param);


            Console.Write("BetLotteryCheck---->" + result);
            Console.ReadLine();
        }

        /// <summary>
        /// 期次通知
        /// </summary>
        public static void GetPeriodNotify()
        {
            string strUrl = SC_URL + "!queryOpenNotice";
            string result = string.Empty;
            string str_merid = "29002000143",       //商户号
                str_lotteryCode = "51",             //彩种代码
                str_period = "[{'periodCode':'','startTime':'','stopTime':'','lotteryTime':'','prizeCode':''}]",        //期次(Json数组格式)
                str_status = "1",                   //通知状态
                str_size = "",                      //期次条数
                str_service = "periodNotify",       //操作描述
                str_timestamep = cdt.GetTimeStamp(),//时间戳
                str_key = "AUTOLOTTERY",            //生成秘钥的key
                str_digest = Md5.GetMD5String(str_key + str_service + str_timestamep + str_key);        //校验码
            string param = string.Format("merid={0}&lotteryCode={1}&period={2}&status={3}&size={4}&service={5}&timestamp={6}&digest={7}",
                str_merid, str_lotteryCode, str_period, str_status, str_size, str_service, str_timestamep, str_digest);
            result = ow.HttpPostData(strUrl, param);


            Console.Write("GetPeriodNotify---->" + result);
            Console.ReadLine();
        }

        /// <summary>
        /// 代理商订单查询
        /// </summary>
        public static string GetAgentOrder(AgentBaseInfo abi)
        {
            string strUrl = SC_URL + "!queryAgentOrder";
            string result = string.Empty;
            Dictionary<string, string> dicAgentOrder = new Dictionary<string, string>();
            abi.TimeStamp = cdt.GetTimeStamp();
            dicAgentOrder.Add("merid", SH_Number);
            dicAgentOrder.Add("lotteryCode", abi.LotteryCode);
            dicAgentOrder.Add("orderID", abi.OrderId);
            dicAgentOrder.Add("agentOrderId", abi.AgentOrderId);
            dicAgentOrder.Add("service", abi.Service);
            dicAgentOrder.Add("timestamp", abi.TimeStamp);
            dicAgentOrder.Add("digest", GeneralKeyGen(abi));
            string strParamResult = FormatParam(dicAgentOrder);
            result = ow.HttpPostData(strUrl, strParamResult);
            //new Base_SysLogBll().InsertLog("提交给善彩的代理商订单查询参数:" + strParamResult);
            return result;
        }

        /// <summary>
        /// 代理商余额查询
        /// </summary>
        public static string GetAgentAccount(AgentBaseInfo abi)
        {
            string strUrl = SC_URL + "!agentAccountQuery";
            string result = string.Empty;
            Dictionary<string, string> dicAgentAccount = new Dictionary<string, string>();
            abi.TimeStamp = cdt.GetTimeStamp();
            dicAgentAccount.Add("merid", SH_Number);
            dicAgentAccount.Add("lotteryCode", abi.LotteryCode);
            dicAgentAccount.Add("orderID", abi.OrderId);
            dicAgentAccount.Add("service", abi.Service);
            dicAgentAccount.Add("timestamp", abi.TimeStamp);
            dicAgentAccount.Add("digest", GeneralKeyGen(abi));
            string strParamResult = FormatParam(dicAgentAccount);
            result = ow.HttpPostData(strUrl, strParamResult);
            //new Base_SysLogBll().InsertLog("提交给善彩的代理商余额查询参数:" + strParamResult);
            return result;
        }

        /// <summary>
        /// 代理商中奖订单兑奖
        /// </summary>
        public static void GetAgentCashPrize(AgentBaseInfo abi)
        {
            string strUrl = SC_URL + "!orderPeriodAward";
            string result = string.Empty;
            Dictionary<string, string> dicAgentCashPrize = new Dictionary<string, string>();
            abi.TimeStamp = cdt.GetTimeStamp();
            dicAgentCashPrize.Add("merid", SH_Number);
            dicAgentCashPrize.Add("service", abi.Service);
            dicAgentCashPrize.Add("timestamp", abi.TimeStamp);
            dicAgentCashPrize.Add("digest", GeneralKeyGen(abi));
            dicAgentCashPrize.Add("money", abi.Money);
            dicAgentCashPrize.Add("orderId", abi.OrderId);
            dicAgentCashPrize.Add("lotteryCode", abi.LotteryCode);
            result = ow.HttpPostData(strUrl, FormatParam(dicAgentCashPrize));
            Console.Write("GetAgentCashPrize---->" + result);
            Console.ReadLine();
        }
    }

    /// <summary>
    /// 彩票期次参数
    /// </summary>
    public class LotteryAwardPeriodParam
    {
        ///// <summary>
        ///// 加密key
        ///// </summary>
        //public string key { get; set; }
        ///// <summary>
        ///// 方法
        ///// </summary>
        //public string service { get; set; }
        /// <summary>
        /// 彩票代码
        /// </summary>
        public string lotteryCode { get; set; }
        /// <summary>
        /// 期次代码
        /// </summary>
        public string periodCode { get; set; }
        /// <summary>
        /// 销售状态(-1:已售;0:在售)
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 查询数
        /// </summary>
        public int size { get; set; }
    }

    /// <summary>
    /// 期次返奖参数
    /// </summary>
    public class AwardWinRecordParam
    {
        /// <summary>
        /// 彩种代码
        /// </summary>
        public string lotteryCode { get; set; }
        /// <summary>
        /// 期次编码
        /// </summary>
        public string periodCode { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 页面大小
        /// </summary>
        public int pageSize { get; set; }
    }

    /// <summary>
    /// 竞彩订单返奖查询参数
    /// </summary>
    public class OrderPrizeQueryParam
    {
        /// <summary>
        /// 彩种代码
        /// </summary>
        public string LotteryCode { get; set; }
        /// <summary>
        /// 平台订单号
        /// </summary>
        public string OrderId { get; set; }
    }

    /// <summary>
    /// 商户基本信息
    /// </summary>
    public class AgentBaseInfo
    {
        ///// <summary>
        ///// 商户号
        ///// </summary>
        //public string Number { get; set; }
        /// <summary>
        /// 操作描述
        /// </summary>
        public string Service { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }
        /// <summary>
        /// 生成秘钥使用的Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 校验码
        /// </summary>
        public string Digest { get; set; }
        /// <summary>
        /// 彩种编号
        /// </summary>
        public string LotteryCode { get; set; }
        /// <summary>
        /// 期次编号
        /// </summary>
        public string PeriodCode { get; set; }
        /// <summary>
        /// 销售状态（已售:-1,在售:0）
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 查询数(最多50期)
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 订单号(系统订单号/非商户订单号)
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string AgentOrderId { get; set; }
        /// <summary>
        /// 第几页
        /// </summary>
        public string Page { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public string PageSize { get; set; }
        ///// <summary>
        ///// 用户ID
        ///// </summary>
        //public string UserId { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string UserMail { get; set; }
        /// <summary>
        /// 用户手机
        /// </summary>
        public string UserMobile { get; set; }
        /// <summary>
        /// 投注号码
        /// </summary>
        public string CastCode { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 总注数
        /// </summary>
        public string Count { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string Money { get; set; }
    }
}

