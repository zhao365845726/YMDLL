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
    public class PropertyFollow : Base
    {
        public void Descript()
        {
            /* 1.目标库中broker为int类型，增加归属人  FY_broker
             * 2.目标库中houseResource为int类型，增加房源ID  FY_houseResource
             * 3.目标库中type为int类型，增加跟进类别  FY_type
             * 4.目标库中createDate为int类型，增加跟进日期  FY_createDate
             * 5.目标库中content为varchar(8000)类型，修改字段类型为text
             */
        }

        /// <summary>
        /// 房源跟进类的构造函数
        /// </summary>
        public PropertyFollow()
        {
            sTableName = "Follow";
            sColumns = "FollowID,PropertyID,EmpID,FollowDate,Content,ProcessPerson,FollowType,InquiryID";
            sOrder = "FollowDate";
            sPageIndex = 1;
            sPageSize = 10000;
            dTableName = "h_house_resource_follow";
            dColumns = "oldId,FY_houseResource,FY_broker,FY_createDate,content,empIds,FY_type,guestNo";
            //导入时间 00:23:18.5830269分
        }

        /// <summary>
        /// 导房源跟进
        /// </summary>
        public void importPropertyFollow()
        {
            if (sPageIndex > 1)
            {
                dIsDelete = false;
            }

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
        /// 分页数据
        /// </summary>
        /// <returns></returns>
        public bool PagerData()
        {
            try
            {
                DataTable dt = _sqlServer.GetPager(sTableName, sColumns, sOrder, sPageSize, sPageIndex, sWhere, out sTotalCount);

                List<String> lstValue = new List<String>();
                foreach (DataRow row in dt.Rows)
                {
                    string strContent = row["Content"].ToString().Trim();
                    strContent = strContent.Replace("'", "");
                    strContent = strContent.Replace("\\", "");
                    strContent = strContent.Replace("\n", "");
                    strContent = strContent.Replace("\r", "");
                    strContent = strContent.Replace(" ", "");

                    string strProcessPerson = row["ProcessPerson"].ToString().Trim();
                    strProcessPerson = strProcessPerson.Replace("'", "");
                    strProcessPerson = strProcessPerson.Replace("\\", "");

                    string strTemp = "'" + row["FollowID"].ToString().Trim() + "','" +
                        row["PropertyID"].ToString().Trim() + "','" +
                        row["EmpID"].ToString().Trim() + "','" +
                        row["FollowDate"].ToString().Trim() + "','" +
                        strContent + "','" +
                        strProcessPerson + "','" +
                        row["FollowType"].ToString().Trim() + "','" +
                        row["InquiryID"].ToString().Trim() + "'";
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
                    m_Result = "\n房源跟进数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "\n房源跟进数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出房源跟进异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
