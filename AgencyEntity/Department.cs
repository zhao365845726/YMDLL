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
    public class Department : Base
    {
        public void Descript()
        {
            /* 1.目标库中type为int类型，增加部门类型  FY_type
             * 2.目标库中删除主键，默认为空值
             * 3.更改部门唯一索引为一般索引
             */
        }

        /// <summary>
        /// 部门类的构造函数
        /// </summary>
        public Department()
        {
            sTableName = "Department20160623";
            sColumns = "DeptNo,Header,DeptName,DeptID,Tel,Address,DeptType,ExDate,ModDate";
            sOrder = "DeptNo";
            dTableName = "b_organs_20160623";
            dColumns = "id,code,name,oldId,organPresentation,remarks,FY_type,createDate,updateDate";
        }

        /// <summary>
        /// 导部门
        /// </summary>
        public void importDepartment()
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


        public bool PagerData()
        {
            try
            {
                DataTable dt = _sqlServer.GetPager(sTableName, sColumns, sOrder, sPageSize, sPageIndex, sWhere, out sTotalCount);

                List<String> lstValue = new List<String>();
                foreach (DataRow row in dt.Rows)
                {
                    string strTemp = "'" + row["DeptNo"].ToString() + "','" +
                        row["Header"].ToString() + "','" +
                        row["DeptName"].ToString() + "','" +
                        row["DeptID"].ToString() + "','" +
                        row["Tel"].ToString() + "','" +
                        row["Address"].ToString() + "','" +
                        row["DeptType"].ToString() + "','" +
                        Convert.ToDateTime(row["ExDate"].ToString()) + "','" +
                        Convert.ToDateTime(row["ModDate"].ToString()) + "'";
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
                    m_Result = "部门数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "部门数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出部门异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
