using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using YMDLL.Class;
using YMDLL.Common;

namespace AgencyEntity
{
    public class Test : Base
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
        public Test()
        {
            sTableName = "Property";
            sColumns = "CityName,DistrictName,EstateID,RoomNo,Floor,Trade,Status,CountF,CountT,CountW,CountY,PropertyUsage,PropertyType,PropertyDirection,PropertyLook,PropertyBuy,PropertyCommission,PropertySource,Square,PriceUnit,Price,PriceBase,RentPriceUnit,RentPrice,MgtPrice,TrustDate,EmpID,DeptID,PropertyDecoration,PropertyFloor,Remark,RegPerson,RegDate,PropertyTrust,KeyNo,CompleteYear,PropertyOccupy,PropertyOwn,SquareUse,LastFollowDate,FlagDeleted,OwnerName,OwnerTel,ContactName,PropertyNo,PropertyID,PriceLine,RentPriceLine,BuildNo,PropertyCertificate,FloorAll,HandOverDate,TrustNo,PropertyTax,OwnerMobile,Country,Privy,IDCard";       
            sOrder = "CityName";
            dTableName = "h_house_resource";
            dColumns = "urbanArea,district,FY_houseDict,houseNo,floor,tradeWay,state,location,hall,defend,balcony,purpose,type,toward,apartment,billhead,pay,source,acreage,marketArea,marketPrice,power,hireArea,hirePrice,manageFee,FY_entrustDate,FY_attrBroker,organ,decorate,mating,memo,FY_importBroker,FY_importDate,entrustWay,keyNo,housingYear,actuality,equities,within,FY_followDate,FY_status,owner,phoneNumber,linkman,serialNumber,serialNumberOld,marketFloorPrice,hireFloorPrice,ridgepole,certificate,FY_totalFloor,FY_handingDate,entrustNo,expenses,link,nationality,limits,houseCard";           
        }

        /// <summary>
        /// 导栋座单元数据
        /// </summary>
        public void importTest()
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
                    string strTemp = "'" + FilterSpecialCharacter(row["CityName"].ToString()) + "','" +
                        FilterSpecialCharacter(row["DistrictName"].ToString()) + "','" +
                        FilterSpecialCharacter(row["EstateID"].ToString()) + "','" +
                        FilterSpecialCharacter(row["RoomNo"].ToString()) + "','" +
                        FilterSpecialCharacter(row["Floor"].ToString()) + "','" +
                        FilterSpecialCharacter(row["Trade"].ToString()) + "','" +
                        FilterSpecialCharacter(row["Status"].ToString()) + "','" +
                        FilterSpecialCharacter(row["CountF"].ToString()) + "','" +
                        FilterSpecialCharacter(row["CountT"].ToString()) + "','" +
                        FilterSpecialCharacter(row["CountW"].ToString()) + "','" +
                        FilterSpecialCharacter(row["CountY"].ToString()) + "','" +
                        FilterSpecialCharacter(row["PropertyUsage"].ToString()) + "','" +
                        FilterSpecialCharacter(row["PropertyType"].ToString()) + "','" +
                        FilterSpecialCharacter(row["PropertyDirection"].ToString()) + "','" +
                        FilterSpecialCharacter(row["PropertyLook"].ToString()) + "','" +
                        FilterSpecialCharacter(row["PropertyBuy"].ToString()) + "','" +
                        FilterSpecialCharacter(row["PropertyCommission"].ToString()) + "','" +
                        FilterSpecialCharacter(row["PropertySource"].ToString()) + "'," +
                        Convert.ToDecimal(row["Square"].ToString().Trim()) + "," +
                        Convert.ToDecimal(row["PriceUnit"].ToString().Trim()) + "," +
                        Convert.ToDecimal(row["Price"].ToString().Trim()) + "," +
                        Convert.ToDecimal(row["PriceBase"].ToString().Trim()) + "," +
                        Convert.ToDecimal(row["RentPriceUnit"].ToString().Trim()) + "," +
                        Convert.ToDecimal(row["RentPrice"].ToString().Trim()) + ",'" +
                        FilterSpecialCharacter(row["MgtPrice"].ToString()) + "','" +
                        FilterSpecialCharacter(row["TrustDate"].ToString()) + "','" +
                        FilterSpecialCharacter(row["EmpID"].ToString()) + "','" +
                        FilterSpecialCharacter(row["DeptID"].ToString()) + "','" +
                        FilterSpecialCharacter(row["PropertyDecoration"].ToString()) + "','" +
                        FilterSpecialCharacter(row["PropertyFloor"].ToString()) + "','" +
                        FilterSpecialCharacter(row["Remark"].ToString()) + "','" +
                        FilterSpecialCharacter(row["RegPerson"].ToString()) + "','" +
                        FilterSpecialCharacter(row["RegDate"].ToString()) + "','" +
                        FilterSpecialCharacter(row["PropertyTrust"].ToString()) + "','" +
                        FilterSpecialCharacter(row["KeyNo"].ToString()) + "','" +
                        FilterSpecialCharacter(row["CompleteYear"].ToString()) + "','" +
                        FilterSpecialCharacter(row["PropertyOccupy"].ToString()) + "','" +
                        FilterSpecialCharacter(row["PropertyOwn"].ToString()) + "','" +
                        FilterSpecialCharacter(row["SquareUse"].ToString()) + "','" +
                        FilterSpecialCharacter(row["LastFollowDate"].ToString()) + "','" +
                        FilterSpecialCharacter(row["FlagDeleted"].ToString()) + "','" +
                        FilterSpecialCharacter(row["OwnerName"].ToString()) + "','" +
                        FilterSpecialCharacter(row["OwnerTel"].ToString()) + "','" +
                        FilterSpecialCharacter(row["ContactName"].ToString()) + "','" +
                        FilterSpecialCharacter(row["PropertyNo"].ToString()) + "','" +
                        FilterSpecialCharacter(row["PropertyID"].ToString()) + "'," +
                        Convert.ToDecimal(row["PriceLine"].ToString().Trim()) + "," +
                        Convert.ToDecimal(row["RentPriceLine"].ToString().Trim()) + ",'" +
                        FilterSpecialCharacter(row["BuildNo"].ToString()) + "','" +
                        FilterSpecialCharacter(row["PropertyCertificate"].ToString()) + "','" +
                        FilterSpecialCharacter(row["FloorAll"].ToString()) + "','" +
                        FilterSpecialCharacter(row["HandOverDate"].ToString()) + "','" +
                        FilterSpecialCharacter(row["TrustNo"].ToString()) + "','" +
                        FilterSpecialCharacter(row["PropertyTax"].ToString()) + "','" +
                        FilterSpecialCharacter(row["OwnerMobile"].ToString()) + "','" +
                        FilterSpecialCharacter(row["Country"].ToString()) + "','" +
                        FilterSpecialCharacter(row["Privy"].ToString()) + "','" +
                        FilterSpecialCharacter(row["IDCard"].ToString()) + "'";
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
