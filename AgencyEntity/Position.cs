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
    public class Position : Base
    {
        /// <summary>
        /// 职务类的构造函数
        /// </summary>
        public Position()
        {
            sTableName = "Position";
            sColumns = "PositionID,PositionName";
            sOrder = "PositionID";
            dTableName = "FY_Position";
            dColumns = "OldId,Name";
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
                    string strTemp = "'" + row["PositionID"].ToString() + "','" +
                        row["PositionName"].ToString() + "'";
                    lstValue.Add(strTemp);
                }
                //清空目标表数据
                _mysql.Delete(dTableName, null);
                //插入数据并返回插入的结果
                bool isResult = _mysql.BatchInsert(dTableName, dColumns, lstValue);
                if (isResult)
                {
                    m_Result = "\n职务数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "\n职务数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出职务异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
