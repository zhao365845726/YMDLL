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
    public class Inquiry : Base
    {
        public void Descript()
        {
        }

        /// <summary>
        /// 客源类的构造函数
        /// </summary>
        public Inquiry()
        {
            sTableName = "Inquiry";
            sColumns = "InquiryID,CustName,Contact,SquareMin,SquareMax,PriceMin,PriceMax,AreaID,Trade,Status,PropertyUsage,PropertyDirection,PropertyDecoration,DeptID,EmpID,Remark,RegPerson,RegDate,FlagPrivate,LastFollowDate,InquiryNo,CustGrade,InquirySource,Country,CustOccupy";
            sOrder = "InquiryID";
            dTableName = "erp_client";
            dColumns = "erp_id,customer_name,fy_Contact,area_min,area_max,pay_min,pay_max,fy_AreaID,type,status,purpose,orientation,decoration,fy_DeptID,fy_EmpID,remark,input_username,create_time,public,last_follow,client_code,rank,source,nationality,profession";
        }

        /// <summary>
        /// 导客源
        /// </summary>
        public void importInquiry()
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
                    string strCustName = row["CustName"].ToString().Trim();
                    strCustName = strCustName.Replace("'", "");
                    strCustName = strCustName.Replace("\\", "");

                    string strContact = row["Contact"].ToString().Trim();
                    strContact = strContact.Replace("'", "");
                    strContact = strContact.Replace("\\", "");
                    if(strContact == "")
                    {
                        strContact = "0";
                    }

                    string strRemark = row["Remark"].ToString().Trim();
                    strRemark = strRemark.Replace("'", "");
                    strRemark = strRemark.Replace("\\", "");
                    
                    string strRegPerson = row["RegPerson"].ToString().Trim();
                    strRegPerson = strRegPerson.Replace("'", "");
                    strRegPerson = strRegPerson.Replace("\\", "");

                    string strCustOccupy = row["CustOccupy"].ToString().Trim();
                    strCustOccupy = strCustOccupy.Replace("'", "");
                    strCustOccupy = strCustOccupy.Replace("\\", "");
                    
                    string strTemp = "'" + row["InquiryID"].ToString().Trim() + "','" +
                        strCustName + "','" +
                        strContact + "','" +
                        Convert.ToDecimal(row["SquareMin"].ToString().Trim()) + "','" +
                        Convert.ToDecimal(row["SquareMax"].ToString().Trim()) + "','" +
                        Convert.ToDecimal(row["PriceMin"].ToString().Trim()) + "','" +
                        Convert.ToDecimal(row["PriceMax"].ToString().Trim()) + "','" +
                        row["AreaID"].ToString().Trim() + "','" +
                        row["Trade"].ToString().Trim() + "','" +
                        row["Status"].ToString().Trim() + "','" +
                        row["PropertyUsage"].ToString().Trim() + "','" +
                        row["PropertyDirection"].ToString().Trim() + "','" +
                        row["PropertyDecoration"].ToString().Trim() + "','" +
                        row["DeptID"].ToString().Trim() + "','" +
                        row["EmpID"].ToString().Trim() + "','" +
                        strRemark + "','" +
                        strRegPerson + "','" +
                        _dateTime.DateTimeToStamp(row["RegDate"].ToString().Trim()).ToString() + "','" +
                        row["FlagPrivate"].ToString().Trim() + "','" +
                        _dateTime.DateTimeToStamp(row["LastFollowDate"].ToString().Trim()).ToString() + "','" +
                        row["InquiryNo"].ToString().Trim() + "','" +
                        row["CustGrade"].ToString().Trim() + "','" +
                        row["InquirySource"].ToString().Trim() + "','" +
                        row["Country"].ToString().Trim() + "','" +
                        strCustOccupy + "'";
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
                    m_Result = "\n客源数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "\n客源数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出客源异常.\n异常原因：" + ex.Message;
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
