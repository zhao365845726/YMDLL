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
    public class PropertyFollow : Base
    {
        public void Descript()
        {

        }

        /// <summary>
        /// 房源跟进类的构造函数
        /// </summary>
        public PropertyFollow()
        {
            sTableName = "Follow";
            sColumns = "FollowID,PropertyID,EmpID,FollowDate,Content,FollowType";
            sOrder = "FollowDate";
            dTableName = "erp_house_follow";
            dColumns = "fy_followId,erp_house_id,erp_user_id,follow_up_date,content,follow_way,company_id,if_deleted,create_time";
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

                    string strTemp = "'" + row["FollowID"].ToString().Trim() + "','" +
                        row["PropertyID"].ToString().Trim() + "','" +
                        row["EmpID"].ToString().Trim() + "','" +
                        _dateTime.DateTimeToStamp(row["FollowDate"].ToString().Trim()).ToString() + "','" +
                        strContent + "','" +
                        row["FollowType"].ToString().Trim() + "','" +
                        dCompanyId + "','" + dDeleteMark + "','" +
                        _dateTime.DateTimeToStamp(DateTime.Now).ToString() + "'"
                        ;
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

        /// <summary>
        /// 添加字段
        /// </summary>
        public void AddField(string FieldName, string FieldType)
        {
            _mysql.UpdateField("add", dTableName, FieldName, FieldType);
            m_Result = _mysql.m_Message;
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// 修改字段
        /// </summary>
        public void ModifyField(string FieldName, string FieldType)
        {
            _mysql.UpdateField("modify column ", dTableName, FieldName, FieldType);
            m_Result = _mysql.m_Message;
        }

        /// <summary>
        /// 移除字段
        /// </summary>
        public void DropField(string FieldName)
        {
            _mysql.UpdateField("drop ", dTableName, FieldName, "");
            m_Result = _mysql.m_Message;
        }

        /// <summary>
        /// 更新关联数据
        /// </summary>
        public bool UpdateData()
        {
            try
            {
                List<string>[] lstHouseId = new List<string>[1];
                lstHouseId = _mysql.Select("erp_house", "id", "");
                string tmpTable = "", tmpValues = "", tmpWhere = "";
                foreach (string strId in lstHouseId[0])
                {
                    //更新录入人ID，录入人部门ID
                    tmpTable = "erp_house as a,erp_department as b";
                    tmpValues = "a.input_department_id = b.dept_id,a.principal_department_id = b.dept_id";
                    tmpWhere = "and SUBSTRING_INDEX(a.input_username,'.',1) = b.dept_name and a.id = " + strId;
                    _mysql.Update(tmpTable, tmpValues, tmpWhere);
                    //更新维护人ID,维护人部门ID
                    tmpTable = "erp_house as a,erp_user as b";
                    tmpValues = "a.input_user_id = b.id,a.principal_user_id = b.id,a.principal_username = b.username,a.input_username = b.username";
                    tmpWhere = "and SUBSTRING_INDEX(a.input_username,'.',-1) = b.username and a.input_department_id = b.department_id and a.id = " + strId;
                    _mysql.Update(tmpTable, tmpValues, tmpWhere);
                }

                m_Result += "\n房源的行政区|片区|楼盘字典及ID更新成功";
                return true;
            }
            catch (Exception ex)
            {
                m_Result += "\n房源的行政区|片区|楼盘字典及ID更新异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
