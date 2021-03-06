﻿using System;
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
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("username", "EmpName");          //用户姓名
            dicMap.Add("tel", "Tel");                   //用户电话
            dicMap.Add("pwd", "Password");              //用户密码
            dicMap.Add("gender", "Sex");                //用户性别
            dicMap.Add("birthday", "Birthday:DateTime");                    //用户生日
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);      //公司ID
            dicMap.Add("id_card_number", "IDCard");     //用户身份证
            dicMap.Add("user_code", "EmpNo");           //用户编号
            dicMap.Add("email", "EMail");               //用户邮箱
            dicMap.Add("enroll_date", "JoinDate:DateTime");                 //入职日期
            dicMap.Add("resign_date", "AwayDate:DateTime");                 //离职日期
            dicMap.Add("create_time", "ExDate:DateTime");                   //创建时间
            dicMap.Add("update_time", "ModDate:DateTime");                  //更新时间
            dicMap.Add("if_deleted", "FlagDeleted");    //删除标记
            dicMap.Add("erp_id", "EmpID");              //原房友ID
            dicMap.Add("erp_role_id", "PositionID");    //原房友职务ID
            dicMap.Add("erp_department_id", "DeptID");  //原房友部门ID
            dicMap.Add("user_status", "Status");        //用户状态
            dicMap.Add("introduce", "Brief");           //用户简介
            return dicMap;
        }

        /// <summary>
        /// 人员类的构造函数
        /// </summary>
        public Employee()
        {
            sTableName = "Employee";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "EmpName";
            dTableName = "erp_user";
            dTableDescript = "用户表";
            dPolitContentDescript = "归属部门|职务|角色|就职状态";
            dColumns = CombineDestField(FieldMap());
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
                    string strTemp = GetConcatValues(FieldMap(), row);
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
                    m_Result = "\n" + dTableDescript + "数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "\n" + dTableDescript + "数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出" + dTableDescript + "异常.\n异常原因：" + ex.Message;
                return false;
            }
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

                tmpTable = "erp_user";
                tmpValues = "user_status = '在职'";
                tmpWhere = "and user_status = '正式'";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                tmpTable = "erp_user";
                tmpValues = "if_deleted = 1";
                tmpWhere = "and if_deleted = -1";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                m_Result += "\n" + dTableDescript + "中" + dPolitContentDescript + "更新成功";
                return true;
            }
            catch (Exception ex)
            {
                m_Result += "\n" + dTableDescript + "中" + dPolitContentDescript + "更新异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
