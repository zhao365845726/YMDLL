using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMDLL.Class;
using YMDLL.Common;
using MySql.Data.MySqlClient;

namespace AgencyToERP_PHP
{
    public class Department : Base
    {
        public void Descript()
        {
            /*
             操作流程：
             1.3.0系统中dept_id为自增，所以不需要导入
             2.
             */
        }

        /// <summary>
        /// 部门类的构造函数
        /// </summary>
        public Department()
        {
            sTableName = "Department";
            sColumns = "DeptName,DeptNo,Address,Tel,DeptType,DeptID,Header";
            sOrder = "DeptNo";
            dTableName = "erp_department";
            dColumns = "dept_name,company_id,address,tel,dept_type,if_deleted,create_time,update_time,erp_id,erp_pid,dept_code,status,fy_dept_id";
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
                    string strStatus = "",strType = "";
                    if(row["DeptType"].ToString() == "关闭")
                    {
                        strStatus = row["DeptType"].ToString();
                    }

                    if (row["DeptType"].ToString() != "关闭")
                    {
                        strType = row["DeptType"].ToString();
                    }

                    string strTemp = "'" + row["DeptName"].ToString() + "'," +
                        dCompanyId + ",'" +
                        row["Address"].ToString() + "','" +
                        row["Tel"].ToString() + "','" +
                        strType + "'," +
                        dDeleteMark + ",'" +
                        _dateTime.DateTimeToStamp(DateTime.Now).ToString() + "','" +
                        _dateTime.DateTimeToStamp(DateTime.Now).ToString() + "','" +
                        row["DeptNo"].ToString() + "','" +
                        row["DeptNo"].ToString().Substring(0, row["DeptNo"].ToString().Length - 2) + "','" +
                        row["Header"].ToString() + "','" +
                        strStatus + "','" + 
                        row["DeptID"].ToString() + "'"
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
        /// 更新数据
        /// </summary>
        public bool UpdateData(string typeName)
        {
            try
            {
                if(typeName == "职能")
                {
                    _mysql.Update("erp_department", "dept_type = '职能'", "");
                    m_Result += "\n部门表中更新成功";
                }
                else if(typeName == "大区")
                {
                    _mysql.Update("erp_department", "dept_type = '大区'", "and LENGTH(erp_id)=2 and erp_id > 01 and erp_id < 99 ");
                    m_Result += "\n部门表中更新成功";
                }
                else if(typeName == "其他")
                {
                    string AreaIds = "04,05,06,07,08,20,21,29,30";
                    string[] saAreaId = AreaIds.Split(',');
                    foreach(string aid in saAreaId)
                    {
                        _mysql.Update("erp_department", "dept_type = '区'", "and LENGTH(erp_id)=4 and erp_id like '" + aid + "%'");
                        _mysql.Update("erp_department", "dept_type = '店'", "and LENGTH(erp_id)=6 and erp_id like '" + aid + "%'");
                        _mysql.Update("erp_department", "dept_type = '组'", "and LENGTH(erp_id)=8 and erp_id like '" + aid + "%'");
                    }

                    m_Result += "\n部门表中更新成功";
                }else if(typeName == "更新归属")
                {
                    MySqlDataReader dr = _mysql.SelectMul("erp_department", "dept_id,dept_name,dept_code,pid,erp_id,erp_pid", "");
                    List<string> lstUp = new List<string>();
                    while (dr.Read())
                    {
                        lstUp.Add("UPDATE erp_department SET pid = (select dept_id from erp_department where erp_id = (SELECT erp_pid from erp_department WHERE dept_id = " + dr[0].ToString() + ")) WHERE 1 = 1 and dept_id = " + dr[0].ToString());

                        //_mysql.UpdateNoClose("erp_department", "pid = (select dept_id from erp_department where erp_id = (SELECT erp_pid from erp_department WHERE dept_id = " + dr[0].ToString() + "))", "and dept_id = " + dr[0].ToString());
                    }
                    _mysql.CloseConnection();

                    foreach(string esql in lstUp)
                    {
                        _mysql.Update(esql);
                    }

                    //foreach (string colValue in dr)
                    //{
                    //    _mysql.Update("erp_department", "pid = (select dept_id from erp_department where erp_id = (SELECT erp_pid from erp_department WHERE dept_id = " + colValue[0].ToString() + "))", "and dept_id = " + colValue[1].ToString());
                    //}
                }
                else
                {
                    m_Result += "\n对不起,没有可更新的内容";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                m_Result += "\n部门表中更新异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
