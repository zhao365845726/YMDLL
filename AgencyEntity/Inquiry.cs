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
    public class Inquiry : Base
    {
        public void Descript()
        {
            /* 1.目标库中attrBrokerOld为int类型，增加归属人ID  FY_attrBrokerOld
             * 2.目标库中pupr为int类型，增加公私客  FY_pupr
             * 3.目标库中quality为int类型，增加优质客户  FY_quality
             * 4.目标库中attrBroker为int类型，增加优质客户  FY_attrBroker
             * 5.目标库中floor为float类型，增加优质客户  FY_floor
             * 6.目标库中remark内容太长 FY_remark
             */
        }

        /// <summary>
        /// 客源类的构造函数
        /// </summary>
        public Inquiry()
        {
            sTableName = "Inquiry20160623";
            sColumns = "InquiryID,CustName,CustTel,Contact,SquareMin,SquareMax,PriceMin,PriceMax,AreaID,Position,Floor,Trade,Status,CountF,CountT,CountW,CountY,PropertyUsage,PropertyType,PropertyDirection,PropertyDecoration,PropertyCommission,TrustDate,DeptID,EmpID,Remark,RegPerson,RegDate,FlagPrivate,FlagRecommand,CustAddress,LastFollowDate,CustType,InquiryNo,CustGrade,InquirySource,PropertyFloor,UnitName,Country,CustOccupy,CustIntent,CustPeriod";
            sOrder = "InquiryID";
            //sPageSize = 10000;
            dTableName = "c_resource_20160623";
            dColumns = "oldId,customer,phone,contact,area1,area2,price1,price2,district,reach,floors,business,status,room,hall,wei,ytai,usages,type,orient,fixtrue,compayment,entrDate,organOld,FY_attrBrokerOld,FY_remark,FY_attrBroker,importDate,FY_pupr,FY_quality,living,followDate,mold,customerNoOld,grade,source,FY_floor,units,natives,trade,intends,expDate";
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

                    string strCustTel = row["CustTel"].ToString().Trim();
                    strCustTel = strCustTel.Replace("'", "");
                    strCustTel = strCustTel.Replace("\\", "");

                    string strContact = row["Contact"].ToString().Trim();
                    strContact = strContact.Replace("'", "");
                    strContact = strContact.Replace("\\", "");

                    string strPosition = row["Position"].ToString().Trim();
                    strPosition = strPosition.Replace("'", "");
                    strPosition = strPosition.Replace("\\", "");

                    string strRemark = row["Remark"].ToString().Trim();
                    strRemark = strRemark.Replace("'", "");
                    strRemark = strRemark.Replace("\\", "");
                    
                    string strRegPerson = row["RegPerson"].ToString().Trim();
                    strRegPerson = strRegPerson.Replace("'", "");
                    strRegPerson = strRegPerson.Replace("\\", "");
                    
                    string strCustAddress = row["CustAddress"].ToString().Trim();
                    strCustAddress = strCustAddress.Replace("'", "");
                    strCustAddress = strCustAddress.Replace("\\", "");

                    string strCustOccupy = row["CustOccupy"].ToString().Trim();
                    strCustOccupy = strCustOccupy.Replace("'", "");
                    strCustOccupy = strCustOccupy.Replace("\\", "");
                    
                    string strTemp = "'" + row["InquiryID"].ToString().Trim() + "','" +
                        strCustName + "','" +
                        strCustTel + "','" +
                        strContact + "','" +
                        Convert.ToDecimal(row["SquareMin"].ToString().Trim()) + "','" +
                        Convert.ToDecimal(row["SquareMax"].ToString().Trim()) + "','" +
                        Convert.ToDecimal(row["PriceMin"].ToString().Trim()) + "','" +
                        Convert.ToDecimal(row["PriceMax"].ToString().Trim()) + "','" +
                        row["AreaID"].ToString().Trim() + "','" +
                        strPosition + "','" +
                        row["Floor"].ToString().Trim() + "','" +
                        row["Trade"].ToString().Trim() + "','" +
                        row["Status"].ToString().Trim() + "','" +
                        row["CountF"].ToString().Trim() + "','" +
                        row["CountT"].ToString().Trim() + "','" +
                        row["CountW"].ToString().Trim() + "','" +
                        row["CountY"].ToString().Trim() + "','" +
                        row["PropertyUsage"].ToString().Trim() + "','" +
                        row["PropertyType"].ToString().Trim() + "','" +
                        row["PropertyDirection"].ToString().Trim() + "','" +
                        row["PropertyDecoration"].ToString().Trim() + "','" +
                        row["PropertyCommission"].ToString().Trim() + "','" +
                        row["TrustDate"].ToString().Trim() + "','" +
                        row["DeptID"].ToString().Trim() + "','" +
                        row["EmpID"].ToString().Trim() + "','" +
                        strRemark + "','" +
                        strRegPerson + "','" +
                        row["RegDate"].ToString().Trim() + "','" +
                        row["FlagPrivate"].ToString().Trim() + "','" +
                        row["FlagRecommand"].ToString().Trim() + "','" +
                        strCustAddress + "','" +
                        row["LastFollowDate"].ToString().Trim() + "','" +
                        row["CustType"].ToString().Trim() + "','" +
                        row["InquiryNo"].ToString().Trim() + "','" +
                        row["CustGrade"].ToString().Trim() + "','" +
                        row["InquirySource"].ToString().Trim() + "','" +
                        row["PropertyFloor"].ToString().Trim() + "','" +
                        row["UnitName"].ToString().Trim() + "','" +
                        row["Country"].ToString().Trim() + "','" +
                        strCustOccupy + "','" +
                        row["CustIntent"].ToString().Trim() + "','" +
                        row["CustPeriod"].ToString().Trim() + "'";
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

    }
}
