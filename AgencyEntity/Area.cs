using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMDLL.Class;
using YMDLL.Common;

namespace AgencyEntity
{
    public class Area : Base
    {
        /// <summary>
        /// 片区类的构造函数
        /// </summary>
        public Area()
        {
            sTableName = "Area";
            sColumns = "AreaName,AreaID,DistrictName";
            sOrder = "AreaName";
            sPageIndex = 1;
            sPageSize = 1000;
            dTableName = "b_bus_area";
            dColumns = "name,oldId,type,parentId";
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
                        row["AreaID"].ToString() +
                        "',762,'" +
                        row["DistrictName"].ToString() + "'";
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
    }
}
