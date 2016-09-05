using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
using YMDLL.Common;
using YMDLL.Class;

namespace AgencyToERP_PHP 
{
    public class ContractAct : Base
    {
        public void Descript()
        {
            //Dictionary<目标数据库,源数据库>
            //表增加索引：deal_id,
        }

        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("fy_FeeID", "FeeID");            //实收实付费用ID
            dicMap.Add("fy_deal_id", "ContractID");     //合同ID
            dicMap.Add("price_name", "FeeType");        //费用名称
            dicMap.Add("fy_shou_fee", "Money0");        //实收费用
            dicMap.Add("fy_fu_fee", "Money1");          //实付费用
            dicMap.Add("finance_date", "FeeDate:DateTime");        //收付日期
            dicMap.Add("price_way", "CashType");        //付款方式
            dicMap.Add("fee_user", "FeeSource");        //收付方来源
            dicMap.Add("memo", "Remark");               //摘要
            dicMap.Add("status", "Audit");              //费用审核状态
            dicMap.Add("confirm_date", "AuditDate1:DateTime");      //确认日期
            dicMap.Add("check_date", "AuditDate2:DateTime");        //审核日期
            dicMap.Add("pigeonhole_date", "AuditDate3:DateTime");   //归档日期
            dicMap.Add("confirm_user_name", "AuditPerson1");        //确认人
            dicMap.Add("check_user_name", "AuditPerson2");          //审核人
            dicMap.Add("pigeonhole_username", "AuditPerson3");      //归档人
            dicMap.Add("entry_user_name", "RegPerson");             //录入人
            dicMap.Add("create_time", "RegDate:DateTime");          //录入日期
            return dicMap;
        }

        /// <summary>
        /// 合同实收费用类的构造函数
        /// </summary>
        public ContractAct()
        {
            sTableName = "ContractAct";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "RegDate";
            dTableName = "erp_collect_pay";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导合同实收费用信息
        /// </summary>
        public void importContractAct()
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
                    m_Result = "\n合同实收费用数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "\n合同实收费用数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出合同实收费用异常.\n异常原因：" + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新关联数据
        /// </summary>
        public bool UpdateData()
        {
            string tmpTable = "", tmpValues = "", tmpWhere = "";
            try
            {
                tmpTable = "erp_collect_pay";
                //更新合同实收费用表中company_id字段和if_deleted字段
                tmpValues = "company_id = '" + dCompanyId + "',if_deleted = '" + dDeleteMark + "'";
                tmpWhere = "";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                tmpValues = "status = '待确认'";
                tmpWhere = "and status = '0'";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                tmpValues = "status = '出纳确认'";
                tmpWhere = "and status = '1'";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                tmpValues = "status = '会计审核'";
                tmpWhere = "and status = '2'";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                tmpValues = "status = '已归档'";
                tmpWhere = "and status = '3'";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                tmpValues = "price_charge = '实付',price_num = fy_fu_fee";
                tmpWhere = "and fy_shou_fee = '0'";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                tmpValues = "price_charge = '实收',price_num = fy_shou_fee";
                tmpWhere = "and fy_fu_fee = '0'";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                //更新费用名称

                //更新费用类型

                //更新实收实付同时存在的情况

                //更新合同ID
                //tmpTable = "erp_collect_pay as a,(select deal_id,erp_deal_id from erp_deal) as b";
                //tmpValues = "a.deal_id = b.deal_id";
                //tmpWhere = "and a.fy_deal_id = b.erp_deal_id";
                //_mysql.Update(tmpTable, tmpValues, tmpWhere);

                m_Result += "\n合同实收费用表中公司ID|删除标记|费用状态|收付状态|收付金额更新成功";
                return true;
            }
            catch (Exception ex)
            {
                m_Result += "\n合同实收费用表中公司ID|删除标记|费用状态|收付状态|收付金额更新异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
