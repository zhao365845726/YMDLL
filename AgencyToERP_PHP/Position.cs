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
    public class Position : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("role_name", "PositionName");        //角色名称
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);      //公司ID
            dicMap.Add("create_time", "ExDate:DateTime");   //创建时间
            dicMap.Add("update_time", "ModDate:DateTime");  //修改时间
            dicMap.Add("if_deleted", "FlagDeleted");        //删除标识
            dicMap.Add("erp_id", "PositionID");             //原职务ID
            dicMap.Add("role_type", ":String?Default=职能");//角色类型
            dicMap.Add("house_sale", ":String?Default=0");  //出售查看电话次数
            dicMap.Add("house_rent", ":String?Default=0");  //出租查看电话次数
            dicMap.Add("xq_sale", ":String?Default=0");     //求购查看电话次数
            dicMap.Add("xq_rent", ":String?Default=0");     //求租查看电话次数
            return dicMap;
        }

        /// <summary>
        /// 职务类的构造函数
        /// </summary>
        public Position()
        {
            sTableName = "Position";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "PositionID";
            dTableName = "erp_role";
            dTableDescript = "角色表";
            dPolitContentDescript = "删除标识|角色介绍";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导职务角色
        /// </summary>
        public bool importPosition()
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
                //清空目标表数据
                _mysql.Delete(dTableName, null);
                //插入数据并返回插入的结果
                bool isResult = _mysql.BatchInsert(dTableName, dColumns, lstValue);
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
                tmpTable = "erp_role";
                tmpValues = "if_deleted = 1";
                tmpWhere = "and if_deleted = -1";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                tmpValues = "role_desc = role_name";
                tmpWhere = "";
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
