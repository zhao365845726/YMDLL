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
    public class Building : Base
    {
        public void Descript()
        {
            /* 1.目标库中house_dict_id为int类型，增加楼盘字典ID  FY_EstateID
             */
        }

        /// <summary>
        /// 栋座单元类的构造函数
        /// </summary>
        public Building()
        {
            sTableName = "Building";
            sColumns = "EstateID,BuildingName,FloorAll,CountT,CountH,ExDate,Cell,BuildingID";
            sOrder = "BuildingID";
            dTableName = "b_house_unit_dict";
            dColumns = "FY_EstateID,name,layer,elevator,family,createDate,unitNo,oldId";
            dIsDelete = true;
            //,layerTo,unitNo,years,yearsTest
        }


        /// <summary>
        /// 导栋座单元数据
        /// </summary>
        public void importBuilding()
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
        /// 导栋座单元
        /// </summary>
        public bool PagerData()
        {
            try
            {
                DataTable dt = _sqlServer.GetPager(sTableName, sColumns, sOrder, sPageSize, sPageIndex, sWhere, out sTotalCount);

                List<String> lstValue = new List<String>();
                foreach (DataRow row in dt.Rows)
                {
                    string strTemp = "'" + row["EstateID"].ToString() + "','" +
                        row["BuildingName"].ToString() + "'," +
                        Convert.ToInt32(row["FloorAll"].ToString()) + ",'" + 
                        row["CountT"].ToString() + "','" +
                        row["CountH"].ToString() + "','" +
                        Convert.ToDateTime(row["ExDate"].ToString()) + "','" +
                        row["Cell"].ToString() + "','" +
                        row["BuildingID"].ToString() + "'";
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
                    m_Result = "\n栋座单元数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "\n栋座单元数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出栋座单元异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
