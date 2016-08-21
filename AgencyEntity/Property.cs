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
    public class Property : Base
    {
        public void Descript()
        {
            /* 1.目标库中houseDict为int类型，增加楼盘字典ID  FY_houseDict
             * 2.目标库中importBroker为int类型，增加录入经纪人  FY_importBroker
             * 3.目标库中entrustDate，houseDict，attrBroker，importBroker，importDate，followDate,status,handingDate,totalFloor类型无法导入，增加字段FY_entrustDate，FY_houseDict，FY_attrBroker，FY_importBroker，FY_importDate,FY_followDate,FY_status,FY_handingDate,FY_totalFloor
             */

            /* 导数据注意事项
             * 避免'，\，
             */
        }

        /// <summary>
        /// 房源类的构造函数
        /// </summary>
        public Property()
        {
            sTableName = "Property20160623";
            sColumns = "CityName,DistrictName,EstateID,RoomNo,Floor,Trade,Status,CountF,CountT,CountW,CountY,PropertyUsage,PropertyType,PropertyDirection,PropertyLook,PropertyBuy,PropertyCommission,PropertySource,Square,PriceUnit,Price,PriceBase,RentPriceUnit,RentPrice,MgtPrice,TrustDate,EmpID,DeptID,PropertyDecoration,PropertyFloor,Remark,RegPerson,RegDate,PropertyTrust,KeyNo,CompleteYear,PropertyOccupy,PropertyOwn,SquareUse,LastFollowDate,FlagDeleted,OwnerName,OwnerTel,ContactName,PropertyNo,PropertyID,PriceLine,RentPriceLine,BuildNo,PropertyCertificate,FloorAll,HandOverDate,TrustNo,PropertyTax,OwnerMobile,Country,Privy,IDCard";
            //电话加密，暂时不导    ,OwnerTel,OwnerMobile
            sOrder = "CityName";
            dTableName = "h_house_resource_20160623";
            dColumns = "urbanArea,district,FY_houseDict,houseNo,floor,tradeWay,state,location,hall,defend,balcony,purpose,type,toward,apartment,billhead,pay,source,acreage,marketArea,marketPrice,power,hireArea,hirePrice,manageFee,FY_entrustDate,FY_attrBroker,organ,decorate,mating,memo,FY_importBroker,FY_importDate,entrustWay,keyNo,housingYear,actuality,equities,within,FY_followDate,FY_status,owner,phoneNumber,linkman,serialNumber,serialNumberOld,marketFloorPrice,hireFloorPrice,ridgepole,certificate,FY_totalFloor,FY_handingDate,entrustNo,expenses,link,nationality,limits,houseCard";
            //,phoneNumber,link
        }

        /// <summary>
        /// 导栋座单元数据
        /// </summary>
        public void importProperty()
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
        /// 导房源
        /// </summary>
        public bool PagerData()
        {
            try
            {
                DataTable dt = _sqlServer.GetPager(sTableName, sColumns, sOrder, sPageSize, sPageIndex, sWhere, out sTotalCount);

                List<String> lstValue = new List<String>();
                foreach (DataRow row in dt.Rows)
                {
                    string strRemark = row["Remark"].ToString().Trim();
                    strRemark = strRemark.Replace("'", "");
                    strRemark = strRemark.Replace("\\", "");

                    string strOwnerName = row["OwnerName"].ToString().Trim();
                    strOwnerName = strOwnerName.Replace("'", "");
                    strOwnerName = strOwnerName.Replace("\\", "");

                    string strOwnerTel = row["OwnerTel"].ToString().Trim();
                    strOwnerTel = strOwnerTel.Replace("'", "");
                    strOwnerTel = strOwnerTel.Replace("\\", "");

                    string strContactName = row["ContactName"].ToString().Trim();
                    strContactName = strContactName.Replace("'", "");
                    strContactName = strContactName.Replace("\\", "");

                    string strRegPerson = row["RegPerson"].ToString().Trim();
                    strRegPerson = strRegPerson.Replace("'", "");
                    strRegPerson = strRegPerson.Replace("\\", "");

                    string strOwnerMobile = row["OwnerMobile"].ToString().Trim();
                    strOwnerMobile = strOwnerMobile.Replace("'", "");
                    strOwnerMobile = strOwnerMobile.Replace("\\", "");

                    string strBuildNo = row["BuildNo"].ToString().Trim();
                    strBuildNo = strBuildNo.Replace("'", "");
                    strBuildNo = strBuildNo.Replace("\\", "");

                    string strTemp = "'" + row["CityName"].ToString().Trim() + "','" +
                        row["DistrictName"].ToString().Trim() + "','" +
                        row["EstateID"].ToString().Trim() + "','" +
                        row["RoomNo"].ToString().Trim() + "','" +
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
                        row["PropertyLook"].ToString().Trim() + "','" +
                        row["PropertyBuy"].ToString().Trim() + "','" +
                        row["PropertyCommission"].ToString().Trim() + "','" +
                        row["PropertySource"].ToString().Trim() + "'," +
                        Convert.ToDecimal(row["Square"].ToString().Trim()) + "," +
                        Convert.ToDecimal(row["PriceUnit"].ToString().Trim()) + "," +
                        Convert.ToDecimal(row["Price"].ToString().Trim()) + "," +
                        Convert.ToDecimal(row["PriceBase"].ToString().Trim()) + "," +
                        Convert.ToDecimal(row["RentPriceUnit"].ToString().Trim()) + "," +
                        Convert.ToDecimal(row["RentPrice"].ToString().Trim()) + ",'" +
                        row["MgtPrice"].ToString().Trim() + "','" +
                        row["TrustDate"].ToString().Trim() + "','" +
                        row["EmpID"].ToString().Trim() + "','" +
                        row["DeptID"].ToString().Trim() + "','" +
                        row["PropertyDecoration"].ToString().Trim() + "','" +
                        row["PropertyFloor"].ToString().Trim() + "','" +
                        strRemark + "','" +
                        strRegPerson + "','" +
                        row["RegDate"].ToString().Trim() + "','" +
                        row["PropertyTrust"].ToString().Trim() + "','" +
                        row["KeyNo"].ToString().Trim() + "','" +
                        row["CompleteYear"].ToString().Trim() + "','" +
                        row["PropertyOccupy"].ToString().Trim() + "','" +
                        row["PropertyOwn"].ToString().Trim() + "','" +
                        row["SquareUse"].ToString().Trim() + "','" +
                        row["LastFollowDate"].ToString().Trim() + "','" +
                        row["FlagDeleted"].ToString().Trim() + "','" +
                        strOwnerName + "','" +
                        strOwnerTel + "','" +
                        strContactName + "','" +
                        row["PropertyNo"].ToString().Trim() + "','" +
                        row["PropertyID"].ToString().Trim() + "'," +
                        Convert.ToDecimal(row["PriceLine"].ToString().Trim()) + "," +
                        Convert.ToDecimal(row["RentPriceLine"].ToString().Trim()) + ",'" +
                        strBuildNo + "','" +
                        row["PropertyCertificate"].ToString().Trim() + "','" +
                        row["FloorAll"].ToString().Trim() + "','" +
                        row["HandOverDate"].ToString().Trim() + "','" +
                        row["TrustNo"].ToString().Trim() + "','" +
                        row["PropertyTax"].ToString().Trim() + "','" +
                        strOwnerMobile + "','" +
                        row["Country"].ToString().Trim() + "','" +
                        row["Privy"].ToString().Trim() + "','" +
                        row["IDCard"].ToString().Trim() + "'";
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
                    m_Result = "\n房源数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "\n房源数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出房源异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
