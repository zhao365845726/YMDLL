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
    public class Employee : Base
    {
        public void Descript()
        {
            /*
             操作流程：
             1.3.0系统中id为自增，所以不需要导入
             2.数据导入已经成功
             3.数据更新（未编写）
             */
        }

        /// <summary>
        /// 人员类的构造函数
        /// </summary>
        public Employee()
        {
            sTableName = "Employee";
            sColumns = "EmpName,Tel,Password,Sex,Birthday,IDCard,EmpNo,EMail,JoinDate,ExDate,AwayDate,EmpID,PositionID,DeptID,Status,Brief";
            sOrder = "EmpName";
            sPageIndex = 1;
            sPageSize = 1000;
            dTableName = "erp_user";
            dColumns = "username,tel,pwd,company_id,gender,birthday,id_card_number,fy_PositionId,user_code,email,enroll_date,resign_date,create_time,update_time,if_deleted,erp_id,erp_role_id,erp_department_id,user_status,introduce";
            dIsDelete = true;
        }

        /// <summary>
        /// 导人员数据
        /// </summary>
        public void importEmployee()
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
        /// 导人员
        /// </summary>
        public bool PagerData()
        {
            try
            {
                DataTable dt = _sqlServer.GetPager(sTableName, sColumns, sOrder, sPageSize, sPageIndex, sWhere, out sTotalCount);

                List<String> lstValue = new List<String>();
                foreach (DataRow row in dt.Rows)
                {
                    string strBirthday = row["Birthday"].ToString();
                    string strExDate = row["ExDate"].ToString();
                    string strAwayDate = row["AwayDate"].ToString();

                    if (strBirthday != "")
                    {
                        strBirthday = "'" + _dateTime.DateTimeToStamp(Convert.ToDateTime(row["Birthday"].ToString())).ToString() + "'";
                    }else
                    {
                        strBirthday = "NULL";
                    }

                    if(strExDate != "")
                    {
                        strExDate = "'" + _dateTime.DateTimeToStamp(Convert.ToDateTime(row["ExDate"].ToString())).ToString() + "'";
                    }
                    else
                    {
                        strExDate = "NULL";
                    }

                    if(strAwayDate != "")
                    {
                        strAwayDate = "'" + _dateTime.DateTimeToStamp(Convert.ToDateTime(row["AwayDate"].ToString())).ToString() + "'";
                    }else
                    {
                        strAwayDate = "NULL";
                    }

                    string strTemp = "'" + row["EmpName"].ToString() + "','" +
                        row["Tel"].ToString() + "','" +
                        row["Password"].ToString() + "'," + 
                        dCompanyId + ",'" +
                        row["Sex"].ToString() + "'," +
                        strBirthday + ",'" +
                        row["IDCard"].ToString() + "','" +
                        row["PositionID"].ToString() + "','" +
                        row["EmpNo"].ToString() + "','" +
                        row["EMail"].ToString() + "'," +
                        strExDate + "," +
                        strAwayDate + ",'" +
                        _dateTime.DateTimeToStamp(DateTime.Now).ToString() + "','" +
                        _dateTime.DateTimeToStamp(DateTime.Now).ToString() + "'," + 
                        dDeleteMark + ",'" +
                        row["EmpID"].ToString() + "','" +
                        row["PositionID"].ToString() + "','" +
                        row["DeptID"].ToString() + "','" +
                        row["Status"].ToString() + "','" + 
                        row["Brief"].ToString() + "'"
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
                    m_Result = "\n人员数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "\n人员数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出人员异常.\n异常原因：" + ex.Message;
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
                //更新部门
                string tmpTable = "erp_user as a ,erp_department as b";
                string tmpValues = "a.department_id = b.dept_id";
                string tmpWhere = "and a.erp_department_id = b.fy_dept_id";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);
                //更新角色|职务
                tmpTable = "erp_user as a ,erp_role as c";
                tmpValues = "a.role_id = c.role_id,a.user_duty = c.role_name";
                tmpWhere = "and a.erp_role_id = c.erp_id";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                m_Result += "\n用户表中归属部门|职务|角色更新成功";
                return true;
            }
            catch (Exception ex)
            {
                m_Result += "\n用户表中归属部门|职务|角色更新异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
