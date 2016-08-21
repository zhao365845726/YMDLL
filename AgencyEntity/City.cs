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
    public class City : Base
    {
        public void Descript()
        {
            /* 1.目标库中b_bus_area中的外键删除了，主键去掉了，并默认为空
             */
        }

        /// <summary>
        /// 城区类的构造函数
        /// </summary>
        public City()
        {
            sTableName = "City";
            sColumns = "CityName,CityNo";
            sOrder = "CityNo";
            dTableName = "b_bus_area";
            dColumns = "name,oldId,type";
        }
        /// <summary>
        /// 导入城区
        /// </summary>
        /// <returns></returns>
        public bool importCity()
        {
            try
            {
                DataTable dt = _sqlServer.GetPager(sTableName, sColumns, sOrder, sPageSize, sPageIndex, sWhere, out sTotalCount);

                List<String> lstValue = new List<String>();
                foreach (DataRow row in dt.Rows)
                {
                    string strTemp = "'" + row["CityName"].ToString() + 
                        "'," + Convert.ToInt32(row["CityNo"].ToString()) + ",760";
                    lstValue.Add(strTemp);
                }
                //清空目标表数据
                _mysql.Delete(dTableName, null);
                //插入数据并返回插入的结果
                bool isResult = _mysql.BatchInsert(dTableName, dColumns, lstValue);
                if (isResult)
                {
                    m_Result = "城市数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "城市数据插入失败";
                    return false;
                }
            }
            catch(Exception ex)
            {
                m_Result = "导出城市异常.\n异常原因：" + ex.Message;
                return false;
            }
            
        }

        
    }
}
