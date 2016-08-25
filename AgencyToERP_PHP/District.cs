using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMDLL.Class;
using YMDLL.Common;

namespace AgencyToERP_PHP
{
    public class District : Base
    {

        /// <summary>
        /// 行政区类的构造函数
        /// </summary>
        public District()
        {
            sTableName = "District";
            sColumns = "DistrictName,DistrictNo,CityName";
            sOrder = "DistrictNo";
            //sPageIndex = 1;
            //sPageSize = 1000;
            dTableName = "erp_district";
            dColumns = "district_name,company_id,if_deleted,create_time,update_time,erp_id";
        }

        /// <summary>
        /// 导行政区数据
        /// </summary>
        public bool importDistrict()
        {
            try
            {
                DataTable dt = _sqlServer.GetPager(sTableName, sColumns, sOrder, sPageSize, sPageIndex, sWhere, out sTotalCount);

                List<String> lstValue = new List<String>();
                foreach (DataRow row in dt.Rows)
                {
                    string strTemp = "'" + row["DistrictName"].ToString() + "'," +
                        dCompanyId + 
                        "," + dDeleteMark + ",'" + _dateTime.DateTimeToStamp(DateTime.Now) + 
                        "','" + _dateTime.DateTimeToStamp(DateTime.Now) +
                        "','" + row["DistrictNo"].ToString() + "'";
                    lstValue.Add(strTemp);
                }
                //清空目标表数据
                //_mysql.Delete(dTableName, null);
                //插入数据并返回插入的结果
                bool isResult = _mysql.BatchInsert(dTableName, dColumns, lstValue);
                if (isResult)
                {
                    m_Result = "行政区数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "行政区数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出行政区异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
