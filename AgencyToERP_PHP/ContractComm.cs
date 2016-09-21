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
    public class ContractComm : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("fy_DealId", "ContractID");          //合同ID
            dicMap.Add("fy_UserId", "EmpID");               //人员ID
            dicMap.Add("commission_type", "Comment");       //分成缘由
            dicMap.Add("proportion", "CommRate");           //分成比率
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);      //公司ID
            dicMap.Add("create_time", "ExDate:DateTime");   //创建时间
            dicMap.Add("update_time", "ModDate:DateTime");  //修改时间
            dicMap.Add("confirm", ":String?Default=0");                     //确认标识
            dicMap.Add("confirm_date", ":String?Default=" + dDefaultTime);  //确认日期
            dicMap.Add("confirm_user_id", ":String?Default=0");             //确认人ID
            dicMap.Add("confirm_user_name", ":String?Default=");            //确认人名称
            dicMap.Add("confirm_department_id", ":String?Default=0");       //确认人部门ID
            return dicMap;
        }

        /// <summary>
        /// 合同分成类的构造函数
        /// </summary>
        public ContractComm()
        {
            sTableName = "CONTRACTCOMM";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "ModDate";
            dTableName = "erp_deal_separate";
            dTableDescript = "合同分成表";
            dPolitContentDescript = "";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导合同分成信息
        /// </summary>
        public void importContractComm()
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
                tmpTable = "erp_deal_separate a JOIN erp_deal b";
                tmpValues = "a.deal_id = b.deal_id";
                tmpWhere = "and a.fy_DealId = b.erp_deal_id";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                tmpTable = "erp_deal_separate a JOIN erp_user b ";
                tmpValues = "a.department_id = b.department_id,a.username = b.username,a.user_id = b.id";
                tmpWhere = "and a.fy_UserId = b.erp_id";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

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
