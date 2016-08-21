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
    public class Contract : Base
    {
        public void Descript()
        {
            /* 1.目标库中house_dict_id为int类型，增加楼盘字典ID  FY_EstateID
             */
        }

        /// <summary>
        /// 合同类的构造函数
        /// </summary>
        public Contract()
        {
            sTableName = "City";
            sColumns = "Id,CityName,CityNo";
            sOrder = "CityNo";
            dTableName = "b_bus_area";
            dColumns = "id,name,oldId";
        }

        /// <summary>
        /// 导合同
        /// </summary>
        public bool importContract()
        {
            try
            {
                DataTable dt = _sqlServer.GetPager(sTableName, sColumns, sOrder, sPageSize, sPageIndex, sWhere, out sTotalCount);

                List<String> lstValue = new List<String>();
                foreach (DataRow row in dt.Rows)
                {
                    string strTemp = "'" + row["DistrictName"].ToString() + "'," +
                        Convert.ToInt32(row["DistrictNo"].ToString()) +
                        ",760";
                    lstValue.Add(strTemp);
                }
                //清空目标表数据
                //_mysql.Delete(dTableName, null);
                //插入数据并返回插入的结果
                bool isResult = _mysql.BatchInsert(dTableName, dColumns, lstValue);
                if (isResult)
                {
                    m_Result = "合同数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "合同数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出合同异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
