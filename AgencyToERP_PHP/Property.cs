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
            sTableName = "Property";
            sColumns = "PropertyID,CityName,DistrictName,EstateID,RoomNo,Floor,Trade,Status,CountF,CountT,CountW,CountY,FlagRecommand,PropertyUsage,PropertyType,PropertyDirection,PropertyLook,PropertyBuy,PropertyCommission,PropertySource,Square,PriceUnit,Price,PriceBase,RentPriceUnit,RentPrice,MgtPrice,TrustDate,DeptID,EmpID,PropertyDecoration,PropertyFloor,Remark,RegPerson,RegDate,ModPerson,ModDate,PropertyTrust,KeyNo,CompleteYear,PropertyOccupy,PropertyOwn,SquareUse,LastFollowDate,FlagTrashed,FlagDeleted,OwnerName,OwnerTel,ContactName,ropertyFurniture,PropertyNo,PriceLine,RentPriceLine,BuildNo,PropertyCertificate,PropertyElectric,ExDate,FloorAll,HandOverDate,UnitName,RentUnitName,PhotoCount,Usage1,Usage2,TrustNo,PropertyTax,OwnerMobile,Country,DeptID1,EmpID2,DeptID2,EmpID1,Usage,BusDatas,Privy,FlagWeb,Word,IDCard";
            sOrder = "CityName";
            dTableName = "erp_house";
            dColumns = "room_code,block,fy_community,district,owner_name,tao_bao,if_entertained,if_key,key_code,status,entrust_code,input_username,fy_empid,fy_deptid,exclusive,exclusive_date,exclusive_expire_date,exclusive_code,open_user_name,open_user_id,open_date,open_dept_id,img_username,img_department_id,img_user_id,img_time,image_count,cover_image_url,urgent,all_money,full_year,if_only,if_book,usable_area,property_age,floor_sale_price,floor_rent_price,if_partition,produce_date,building_type,fitment,remark,kernel_remark,building_age,family,ladder,section,business,if_washroom,office_real_rate,air_type,office_level,land_planning,land_status,floor_height,company_id,contract_code,state,unit_price,public,source,launch_date,visit_way,last_follow,last_principal_follow,visit_count,last_visit,type,sale_price,rent_price,rent_unit,old_sale_price,old_rent_price,price_type,filing_code,category,title,purpose,decoration,area,floor,top_floor,room,living_room,washroom,balcony,kitchen,direction,building_structure,property,choice,community_all,rank,rent_expire_date,available_rooms,rented_rooms,if_top,have_no_room_code,if_notice_invalid,notice_invalid_user_id,notice_invalid_user_name,notice_invalid_time,common_telephone,if_deleted,deal_time,create_time,update_time,del_time,erp_id,bargaining_department_id,bargaining_user_id,bargaining_username,licence,del_username,house_tax,two_year,more_card,is_dyb,is_import,if_micro_shop,user_ids,pri_status";
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

                    string strif_Key = "0";
                    if(row["KeyNo"].ToString().Trim().Length > 0)
                    {
                        strif_Key = "1";
                    }

                    string strStatus = row["status"].ToString().Trim();
                    if(strStatus == "预定")
                    {
                        strStatus = "有效";
                    }else if(strStatus == "已租")
                    {
                        strStatus = "他租";
                    }else if(strStatus == "已售")
                    {
                        strStatus = "他售";
                    }else if(strStatus == "未知")
                    {
                        strStatus = "无效";
                    }

                    string strTemp = "'" + row["RoomNo"].ToString().Trim() + "','" +
                        strBuildNo + "','" +
                        row["EstateID"].ToString().Trim() + "','" +
                        row["DistrictName"].ToString().Trim() + "','" +
                        strOwnerName + "',0,0,'" +
                        strif_Key + "','" +
                        row["KeyNo"].ToString().Trim() + "','" +
                        strStatus + "','" +
                        row["TrustNo"].ToString().Trim() + "','" +
                        strRegPerson + "','" +
                        row["EmpID"].ToString().Trim() + "','" +
                        row["DeptID"].ToString().Trim() + "','" +


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
                        
                        row["PropertyDecoration"].ToString().Trim() + "','" +
                        row["PropertyFloor"].ToString().Trim() + "','" +
                        strRemark + "','" +
                        
                        row["RegDate"].ToString().Trim() + "','" +
                        row["PropertyTrust"].ToString().Trim() + "','" +
                        
                        row["CompleteYear"].ToString().Trim() + "','" +
                        row["PropertyOccupy"].ToString().Trim() + "','" +
                        row["PropertyOwn"].ToString().Trim() + "','" +
                        row["SquareUse"].ToString().Trim() + "','" +
                        row["LastFollowDate"].ToString().Trim() + "','" +
                        row["FlagDeleted"].ToString().Trim() + "','" +
                        
                        strOwnerTel + "','" +
                        strContactName + "','" +
                        row["PropertyNo"].ToString().Trim() + "','" +
                        row["PropertyID"].ToString().Trim() + "'," +
                        Convert.ToDecimal(row["PriceLine"].ToString().Trim()) + "," +
                        Convert.ToDecimal(row["RentPriceLine"].ToString().Trim()) + ",'" +
                        
                        row["PropertyCertificate"].ToString().Trim() + "','" +
                        row["FloorAll"].ToString().Trim() + "','" +
                        row["HandOverDate"].ToString().Trim() + "','" +
                        
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
    }
}
