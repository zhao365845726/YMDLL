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
    public class Area : Base
    {
        /// <summary>
        /// 片区类的构造函数
        /// </summary>
        public Area()
        {
            sTableName = "Area";
            sColumns = "AreaName,DistrictName,AreaID";
            sOrder = "AreaName";
            //sPageIndex = 1;
            //sPageSize = 1000;
            dTableName = "erp_region";
            dColumns = "biz_area_name,district,company_id,if_deleted,create_time,update_time,erp_id";
        }

        /// <summary>
        /// 导片区
        /// </summary>
        public bool importArea()
        {
            try
            {
                DataTable dt = _sqlServer.GetPager(sTableName, sColumns, sOrder, sPageSize, sPageIndex, sWhere, out sTotalCount);

                List<String> lstValue = new List<String>();
                foreach (DataRow row in dt.Rows)
                {
                    string strTemp = "'" + row["AreaName"].ToString() + "','" +
                        row["DistrictName"].ToString() + "'" +
                        "," + dCompanyId + "," + dDeleteMark + 
                        ",'" + _dateTime.DateTimeToStamp(DateTime.Now) + 
                        "','" + _dateTime.DateTimeToStamp(DateTime.Now) +
                        "','" + row["AreaID"].ToString() + "'";
                    lstValue.Add(strTemp);
                }
                //清空目标表数据
                //_mysql.Delete(dTableName, null);
                //插入数据并返回插入的结果
                bool isResult = _mysql.BatchInsert(dTableName, dColumns, lstValue);
                if (isResult)
                {
                    m_Result = "片区数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "片区数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出片区异常.\n异常原因：" + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新关联数据
        /// </summary>
        public bool updateData()
        {
            try
            {
                string tmpTable = "erp_region as a,erp_district as b";
                string tmpValues = "a.district_id = b.district_id";
                string tmpWhere = "and a.district = b.district_name";
                _mysql.Update(tmpTable,tmpValues,tmpWhere);
                m_Result += "\n小区的行政区ID更新成功";
                return true;
            }
            catch(Exception ex)
            {
                m_Result += "\n小区的行政区ID更新异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
