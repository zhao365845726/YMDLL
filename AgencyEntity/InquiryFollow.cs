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
    public class InquiryFollow : Base
    {
        public void Descript()
        {
            /* 1.目标库中cResource为int类型，增加客源ID  FY_cResource
             * 2.目标库中broker为int类型，增加经纪人ID  FY_broker
             * 3.目标库中followDate为int类型，增加跟进日期  FY_followDate
             * 4.目标库中content为varchar(8000)类型，修改字段类型为text
             */
        }

        /// <summary>
        /// 客源跟进类的构造函数
        /// </summary>
        public InquiryFollow()
        {
            sTableName = "InquiryFollow";
            sColumns = "FollowID,InquiryID,EmpID,FollowDate,Content,AlertInfo,ProcessPerson,FollowType";
            sOrder = "FollowDate";
            sPageIndex = 701;
            sPageSize = 1000;
            dTableName = "c_resource_follow";
            dColumns = "oldId,FY_cResource,FY_broker,FY_followDate,content,remindnr,empIds,method";
        }

        /// <summary>
        /// 导客源跟进
        /// </summary>
        public void importInquiryFollow()
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

                    string strAlertInfo = row["AlertInfo"].ToString().Trim();
                    strAlertInfo = strAlertInfo.Replace("'", "");
                    strAlertInfo = strAlertInfo.Replace("\\", "");

                    string strTemp = "'" + row["FollowID"].ToString().Trim() + "','" +
                        row["InquiryID"].ToString().Trim() + "','" +
                        row["EmpID"].ToString().Trim() + "','" +
                        row["FollowDate"].ToString().Trim() + "','" +
                        strContent + "','" +
                        strAlertInfo + "','" +
                        row["ProcessPerson"].ToString().Trim() + "','" +
                        row["FollowType"].ToString().Trim() + "'";
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
                    m_Result = "\n客源跟进数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "\n客源跟进数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出客源跟进异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
