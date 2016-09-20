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
    public class ContractCon : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("fy_FeeID", "FeeID");
            dicMap.Add("fy_deal_id", "ContractID");
            dicMap.Add("price_name", "FeeType");
            dicMap.Add("fy_shou_fee", "Money0");
            dicMap.Add("fy_fu_fee", "Money1");
            dicMap.Add("finance_date", "FeeDate:DateTime");
            dicMap.Add("memo", "Remark");
            dicMap.Add("update_time", "ModDate:DateTime");
            dicMap.Add("if_deleted", "FlagDeleted");
            dicMap.Add("create_time", "ExDate:DateTime");
            dicMap.Add("fee_user", "FeeSource");
            dicMap.Add("status", "Audit");
            dicMap.Add("price_type", ":String?Default=");
            return dicMap;
        }

        /// <summary>
        /// 应收应付类的构造函数
        /// </summary>
        public ContractCon()
        {
            sTableName = "ContractCon";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "FeeDate";
            dTableName = "erp_receivable";
            dTableDescript = "应收应付表";
            dPolitContentDescript = "公司ID|删除标记|费用状态|收付状态|收付金额";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导合同信息
        /// </summary>
        public void importContractCon()
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
            string tmpTable = "", tmpValues = "", tmpWhere = "";
            try
            {
                tmpTable = "erp_receivable";
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

                //tmpTable = "erp_deal as a,erp_house as b";
                //tmpValues = "a.room_code = b.room_code,a.block = b.block,a.district = b.district";
                //tmpValues = "a.district = b.district,a.district_id = b.district_id,a.region = b.region,a.biz_area_id = b.region_id,a.community = b.community,a.community_id = b.community_id,a.block = b.block,a.block_id = b.block_id,a.unit_name = b.unit_name,a.unit_id = b.unit_id,a.room_code = b.room_code,a.room_id = b.room_id";
                //tmpWhere = "and a.erp_house_id = b.erp_id and a.erp_house_id <> ''";
                //_mysql.Update(tmpTable, tmpValues, tmpWhere);


                m_Result += "\n" + dTableDescript + "中" + dPolitContentDescript + "更新成功";
                return true;
            }
            catch (Exception ex)
            {
                m_Result += "\n" + dTableDescript + "中" + dPolitContentDescript + "更新异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
