using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMDLL.Class;
using YMDLL.Common;

namespace AgencyToERP_PHP
{
    public class Inquiry : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("erp_id", "InquiryID");              //客源ID
            dicMap.Add("customer_name", "CustName");        //客源名称
            dicMap.Add("fy_Contact", "Contact");            //联系人
            dicMap.Add("area_min", "SquareMin");            //面积下限
            dicMap.Add("area_max", "SquareMax");            //面积上限
            dicMap.Add("pay_min", "PriceMin");              //价格下限
            dicMap.Add("pay_max", "PriceMax");              //价格上限
            dicMap.Add("fy_AreaID", "AreaID");              //区域ID
            dicMap.Add("type", "Trade");                    //交易状态
            dicMap.Add("status", "Status");                 //状态
            dicMap.Add("purpose", "PropertyUsage");         //房屋用途
            dicMap.Add("orientation", "PropertyDirection"); //装修
            dicMap.Add("decoration", "PropertyDecoration"); //朝向
            dicMap.Add("fy_DeptID", "DeptID");              //部门ID
            dicMap.Add("fy_EmpID", "EmpID");                //人员ID
            dicMap.Add("remark", "Remark");                 //备注
            dicMap.Add("input_username", "RegPerson");      //录入人
            dicMap.Add("create_time", "RegDate:DateTime");  //录入日期
            dicMap.Add("update_time", "ModDate:DateTime");  //更新日期
            dicMap.Add("public", "FlagPrivate");            //公私标记【房友里面的0为私客】
            dicMap.Add("last_follow", "LastFollowDate:DateTime");       //最后跟进日
            //dicMap.Add("last_principal_follow", "LastFollowDate:DateTime");     //维护人最后跟进日
            dicMap.Add("client_code", "InquiryNo");         //客源编号
            dicMap.Add("rank", "CustGrade");                //客源等级
            dicMap.Add("source", "InquirySource");          //客源来源
            dicMap.Add("nationality", "Country");           //国籍
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);  //公司ID
            dicMap.Add("if_deleted", ":String?Default=" + dDeleteMark); //是否删除
            //dicMap.Add("common_telephone", "CustMobile");   //客源常用电话(加密的无法导)
            dicMap.Add("tao_bao", ":String?Default=0");     //是否在淘宝池
            dicMap.Add("tao_bao_reason", ":String?Default=");         //进入淘宝池的原因
            dicMap.Add("tao_bao_date", ":String?Default=0");            //进入淘宝池时间
            dicMap.Add("business", ":String?Default=");     //需求行业
            dicMap.Add("rent_year", ":String?Default=0");   //租赁年限
            dicMap.Add("if_transfer_fee", ":String?Default=1");         //是否接受转让费（0:是，1:否)
            dicMap.Add("office_building_level", "Usage");   //写字楼等级
            dicMap.Add("salary", ":String?Default=");       //月收入
            dicMap.Add("rent_mode", "CustOccupy");          //整租or合租
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
            dicMap.Add("identity_card", "CustTitle");                   //证件号码
            dicMap.Add("prefer_region", "DistrictName");                //需求区域
            dicMap.Add("city_id", ":String?Default=0");                 //默认城市的id
            dicMap.Add("fitment", "PropertyFloor");                     //配套设置
            dicMap.Add("fy_floor_max", "Floor");                        //楼层上限
            dicMap.Add("floor_min", ":String?Default=0");               //楼层下限
            dicMap.Add("building_age_max", ":String?Default=0");        //房龄上限
            dicMap.Add("building_age_min", ":String?Default=0");        //房龄下限

            return dicMap;
        }

        /// <summary>
        /// 客源类的构造函数
        /// </summary>
        public Inquiry()
        {
            sTableName = "Inquiry";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "InquiryID";
            dTableName = "erp_client";
            dTableDescript = "客源表";
            dPolitContentDescript = "录入人ID|录入人部门ID|维护人id|维护人姓名|维护人部门id|维护人最后跟进日期|客源需求区域";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导客源
        /// </summary>
        public void importInquiry()
        {
            if (sPageIndex > 1)
            {
                dIsDelete = false;
            }

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
                DataTable dt = _sqlServer.GetPager(sTableName, sColumns, sOrder, sPageSize, sPageIndex, sWhere, out sTotalCount);

                List<String> lstValue = new List<String>();
                foreach (DataRow row in dt.Rows)
                {
                    string strTemp = GetConcatValues(FieldMap(), row);
                    lstValue.Add(strTemp);
                }
                //如果允许删除，清空目标表数据
                if (dIsDelete == true)
                {
                    _mysql.Delete(dTableName, null);
                }
                //插入数据并返回插入的结果
                bool isResult = _mysql.BatchInsert(dTableName, dColumns, lstValue);
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

        /// <summary>
        /// 更新关联数据
        /// </summary>
        public bool UpdateData()
        {
            try
            {
                //string tmpTable = "", tmpValues = "", tmpWhere = "";
                //更新录入人ID,录入人部门ID,维护人id,维护人姓名,维护人部门id[已写入存储过程]
                //tmpTable = "erp_client a JOIN erp_user b ON a.fy_EmpID = b.erp_id";
                //tmpValues = "a.input_user_id = b.id,a.input_department_id = b.department_id,a.principal_user_id = b.id,a.principal_username = b.username,a.principal_department_id = b.department_id";
                //tmpWhere = "and a.fy_EmpID = b.erp_id";
                //_mysql.Update(tmpTable, tmpValues, tmpWhere);

                ////更新维护人最后跟进日期[已写入存储过程]
                //tmpTable = "erp_client";
                //tmpValues = "last_principal_follow = last_follow";
                //tmpWhere = "";
                //_mysql.Update(tmpTable, tmpValues, tmpWhere);

                //tmpValues = "purpose = '住宅'";
                //tmpWhere = "and purpose = '' or purpose is NULL";
                //_mysql.Update(tmpTable, tmpValues, tmpWhere);

                //tmpValues = "public = 1";
                //tmpWhere = "and public = 0";
                //_mysql.Update(tmpTable, tmpValues, tmpWhere);

                //更新客源需求区域的内容[已写入存储过程]
                //tmpTable = "erp_client a JOIN erp_region b ON a.fy_AreaId = b.erp_id JOIN erp_community c ON b.biz_area_name = c.biz_area_name";
                //tmpValues = "a.prefer_region = CONCAT('-',c.community_name,'-',b.district,'-',b.biz_area_name,';'),a.prefer_region_json = CONCAT('[{\"district_id\":\"', b.district_id, '\",\"district\":\"', b.district, '\",\"region_id\":\"', b.biz_area_id, '\",\"region\":\"', b.biz_area_name, '\",\"community_id\":\"', c.community_id, '\",\"community\":\"', c.community_name, '\",\"type\":\"district\"}]')";
                //tmpWhere = "";
                //_mysql.Update(tmpTable, tmpValues, tmpWhere);

                m_Result += "\n" + dTableDescript + "的" + dPolitContentDescript + "更新成功";
                return true;
            }
            catch (Exception ex)
            {
                m_Result += "\n" + dTableDescript + "的" + dPolitContentDescript + "更新异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
