using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMDLL.Class;
using YMDLL.Common;

namespace AgencyEntity
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
            sPageIndex = 1;
            sPageSize = 1000;
            dTableName = "b_bus_area";
            dColumns = "name,oldId,type,parentId";
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
                        Convert.ToInt32(row["DistrictNo"].ToString()) +
                        ",761,'" +
                        row["CityName"].ToString() + "'";
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
