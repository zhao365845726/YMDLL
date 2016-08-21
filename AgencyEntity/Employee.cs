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
    public class Employee : Base
    {
        public void Descript()
        {
            /* 1.目标库中status为int类型，增加人员状态  FY_status
             * 2.目标库中source为int类型，增加来源  FY_source
             * 3.目标库中position_id为int类型，增加来源  FY_position
             * 4.目标库中nationality为int类型，增加民族  FY_nationality
             * 5.目标库中face为int类型，增加政治面貌  FY_face
             * 6.目标库中sysOrgan_id为字符长度无法修改，房友数据较长无法导入
             * 7.目标库中remarks长度太短，房友数据类型为text,所以新增 FY_remarks字段
             * 8.entryDate,createDate,updateDate,Birthday,AwayDate字段为日期类型，导入识别错误，新增FY_entryDate,FY_createDate,FY_updateDate,FY_birthday,FY_outDate字段，在正式库中Update一下
             * 9.目标库中sysOrgan_id有外键关联，所以无法导入，新增 FY_sysOrgan_id作为房友部门归属的标识
             */
        }

        /// <summary>
        /// 人员类的构造函数
        /// </summary>
        public Employee()
        {
            sTableName = "Employee20160623";
            sColumns = "EmpName,Sex,Status,Folk,EmpID,Source,JoinDate,ExDate,ModDate,Tel,IDCard,Birthday,Polity,Address,Native,EMail,Education,Graduate,AwayDate,Idio,Brief,Password,EmpName AS UserName,DeptID,Remark,PositionID";
            //
            sOrder = "EmpName";
            dTableName = "b_emp_20160623";
            dColumns = "name,sex,FY_status,FY_nationality,oldId,FY_source,FY_entryDate,FY_createDate,FY_updateDate,phone,certificateNo,FY_birthday,FY_face,address,familyAddress,email,scholar,graduateSchool,FY_outDate,signature,briefIntroduction,password,userName,FY_sysOrgan_id,FY_remarks,FY_position";
            //
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
                    string strTemp = "'" + row["EmpName"].ToString() + "','" +
                        row["Sex"].ToString() + "','" +
                        row["Status"].ToString() + "','" +
                        row["Folk"].ToString() + "','" +
                        row["EmpID"].ToString() + "','" +
                        row["Source"].ToString() + "','" +
                        row["JoinDate"].ToString() + "','" +
                        row["ExDate"].ToString() + "','" +
                        row["ModDate"].ToString() + "','" +
                        row["Tel"].ToString() + "','" +
                        row["IDCard"].ToString() + "','" +
                        row["Birthday"].ToString() + "','" +
                        row["Polity"].ToString() + "','" +
                        row["Address"].ToString() + "','" +
                        row["Native"].ToString() + "','" +
                        row["EMail"].ToString() + "','" +
                        row["Education"].ToString() + "','" +
                        row["Graduate"].ToString() + "','" +
                        row["AwayDate"].ToString() + "','" +
                        row["Idio"].ToString() + "','" +
                        row["Brief"].ToString() + "','" +
                        row["Password"].ToString() + "','" +
                        row["UserName"].ToString() + "','" +
                        row["DeptID"].ToString() + "','" +
                        row["Remark"].ToString() + "','" +
                        row["PositionID"].ToString() + "'";
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
    }
}
