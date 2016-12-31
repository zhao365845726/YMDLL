using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
using YMDLL.Common;
using YMDLL.Class;

namespace HzlyToERP3_PHP
{
    public class ta_Inquiry : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            /*
             select * from ta_inquiry;
                select count(1) from ta_inquiry;
                desc ta_inquiry;
             */

            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("customer_name", "custname");        //客源名称
            dicMap.Add("client_code", "inquiryno");         //客源编号
            dicMap.Add("identity_card", "custtitle");                   //证件号码
            dicMap.Add("nationality", "custcountry");           //国籍
            dicMap.Add("rank", "custgrade");                //客源等级
            dicMap.Add("rent_mode", "custoccupy");          //整租or合租
            dicMap.Add("type", "trade");                    //交易状态
            //            dicMap.Add("prefer_region", "dsid");                //需求区域
            dicMap.Add("purpose", "propertyusage");         //房屋用途
            dicMap.Add("orientation", "propertydirction"); //装修
            dicMap.Add("area_min", "squremin:String?Default=0");            //面积下限
            dicMap.Add("area_max", "squremax:String?Default=0");            //面积上限
            dicMap.Add("pay_min", "priceminforbuy");              //价格下限
            dicMap.Add("pay_max", "pricemaxforbuy:String?Default=0");              //价格上限
            dicMap.Add("decoration", "propertydecoration"); //朝向
            //dicMap.Add("floor_max", "floormax");                        //楼层上限
            dicMap.Add("floor_min", "floormin:String?=0");               //楼层下限
            //dicMap.Add("status", "status:String?=有效");                 //状态
            dicMap.Add("source", "inquirysource");          //客源来源
            dicMap.Add("remark", "remark");                 //备注
            dicMap.Add("if_deleted", ":String?Default=" + dDeleteMark);         //是否删除
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);  //公司ID
            dicMap.Add("create_time", "regdate:DateTime");  //录入日期
            dicMap.Add("update_time", "modidate:DateTime");  //更新日期
            dicMap.Add("input_username", "regperson");      //录入人
            dicMap.Add("last_follow", "lastfollowdate:DateTime");       //最后跟进日
            dicMap.Add("fitment", "propertyfoor");                     //配套设置

            dicMap.Add("public", ":String?Default=1");            //公私标记【房友里面的0为私客】
            //dicMap.Add("last_principal_follow", "LastFollowDate:DateTime");     //维护人最后跟进日
            //dicMap.Add("common_telephone", "CustMobile");   //客源常用电话(加密的无法导)
            dicMap.Add("tao_bao", ":String?Default=0");     //是否在淘宝池
            dicMap.Add("tao_bao_reason", ":String?Default=");         //进入淘宝池的原因
            dicMap.Add("tao_bao_date", ":String?Default=0");            //进入淘宝池时间
            dicMap.Add("business", ":String?Default=");     //需求行业
            dicMap.Add("rent_year", ":String?Default=0");   //租赁年限
            dicMap.Add("if_transfer_fee", ":String?Default=1");         //是否接受转让费（0:是，1:否)
            //dicMap.Add("office_building_level", "Usage");   //写字楼等级
            dicMap.Add("salary", ":String?Default=");       //月收入
            dicMap.Add("work_unit", ":String?Default=");    //工作单位
            dicMap.Add("rent_staff", ":String?Default=");   //入住类型
            dicMap.Add("if_entertained", ":String?Default=0");          //是否封盘
            dicMap.Add("entertained_date", ":String?Default=0");        //封盘时间
            //dicMap.Add("entertained_user_id", ":String?Default=0");      //封盘人id
            dicMap.Add("entertained_user_name", ":String?Default=");    //封盘人姓名
            //dicMap.Add("entertained_dept_id", ":String?Default=0");      //封盘人部门id
            dicMap.Add("entertained_reason", ":String?Default=");       //封盘原因
            dicMap.Add("entertained_end_date", ":String?Default=0");    //封盘到期时间
            dicMap.Add("core_memo", ":String?Default=");                //核心备注
            dicMap.Add("city_id", ":String?Default=0");                 //默认城市的id
            dicMap.Add("building_age_max", ":String?Default=0");        //房龄上限
            dicMap.Add("building_age_min", ":String?Default=0");        //房龄下限

            //新增字段
            dicMap.Add("old_inquiryId", "inquiryid");               //源客源ID
            dicMap.Add("old_custmobile", "custmobile");             //源客源电话
            dicMap.Add("old_contact", "contact");            //联系人
            dicMap.Add("old_areaid", "areaid");              //需求城市ID
            dicMap.Add("old_dsid", "dsid");                //需求区域
            dicMap.Add("old_piceareaid", "piceareaid");         //需求片区ID
            dicMap.Add("old_estateid", "estateid");             //需求楼盘字典ID
            dicMap.Add("old_propertytype", "propertytype");
            dicMap.Add("old_propertycommission", "propertycommission");
            dicMap.Add("old_deptid", "deptid");              //部门ID
            dicMap.Add("old_modperson", "modperson");           //修改人
            return dicMap;
        }

        /// <summary>
        /// 合同实收费用类的构造函数
        /// </summary>
        public ta_Inquiry()
        {
            sTableName = "ta_inquiry";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "regdate";
            dTableName = "erp_client";
            dTableDescript = "客源表";
            dPolitContentDescript = "录入人ID|录入人部门ID|维护人id|维护人姓名|维护人部门id|维护人最后跟进日期|客源需求区域";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导合同实收费用信息
        /// </summary>
        public void importInquiry()
        {
            PagerData();

            sRows = sTotalCount;

            //数据的总数小于等于页面大小数时候
            if (sRows > sPageSize)
            {
                //统计页数
                int totalPage = 0;
                if (sRows % sPageSize > 0)
                {
                    totalPage = (sRows / sPageSize) + 1;
                    //如果有多页的，增加第二页数据的时候就不许删除
                    dIsDelete = false;
                }
                else
                {
                    totalPage = (sRows / sPageSize);
                }

                for (int i = 1; i < totalPage; i++)
                {
                    sPageIndex = sPageIndex + 1;
                    PagerData();
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        /// <returns></returns>
        public bool PagerData()
        {
            try
            {
                DataTable dt = _smysql.GetPager(sTableName, sColumns, sOrder, sPageSize, sPageIndex, sWhere, out sTotalCount);

                List<String> lstValue = new List<String>();
                foreach (DataRow row in dt.Rows)
                {
                    string strTemp = GetConcatValues(FieldMap(), row);

                    lstValue.Add(strTemp);
                }
                //如果允许删除，清空目标表数据
                if (dIsDelete == true)
                {
                    _dmysql.Delete(dTableName, null);
                }
                //插入数据并返回插入的结果
                bool isResult = _dmysql.BatchInsert(dTableName, dColumns, lstValue);
                Console.Write("\n数据已经成功写入" + sPageSize * sPageIndex + "条");
                if (isResult)
                {
                    m_Result = "\n" + dTableDescript + "数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "\n" + dTableDescript + "数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出" + dTableDescript + "异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
