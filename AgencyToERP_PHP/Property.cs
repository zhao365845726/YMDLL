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
            sColumns = "RoomNo,BuildNo,EstateID,DistrictName,OwnerName,KeyNo,Status,TrustNo,RegPerson,EmpID,DeptID,PropertyTrust,TrustDate,SquareUse,PriceLine,RentPriceLine,PropertyFloor,Remark,CompleteYear,PropertyNo,PriceUnit,Privy,PropertySource,HandOverDate,PropertyLook,LastFollowDate,Trade,Price,RentPrice,RentUnitName,PriceBase,PropertyUsage,PropertyDecoration,Square,Floor,FloorAll,CountF,CountT,CountW,CountY,PropertyDirection,PropertyOwn,OwnerTel,FlagDeleted,RegDate,ModDate,PropertyID,IDCard,PropertyTax";
            //"CityName,FlagRecommand,PropertyType,PropertyLook,PropertyBuy,PropertyCommission,RentPriceUnit,MgtPrice,ModPerson,PropertyOccupy,FlagTrashed,ContactName,PropertyFurniture,PropertyCertificate,PropertyElectric,ExDate,UnitName,PhotoCount,Usage1,Usage2,OwnerMobile,Country,DeptID1,EmpID2,DeptID2,EmpID1,Usage,BusDatas,FlagWeb,Word,IDCard";
            sOrder = "RegDate";
            //sPageIndex = 97;
            //sPageSize = 1;
            dTableName = "erp_house";
            dColumns = "room_code,block,fy_community,district,owner_name,tao_bao,if_entertained,if_key,key_code,status,entrust_code,input_username,fy_empid,fy_deptid,exclusive,exclusive_date,usable_area,floor_sale_price,floor_rent_price,fitment,remark,building_age,company_id,contract_code,unit_price,public,source,launch_date,fy_visit_way,last_follow,type,sale_price,rent_price,rent_unit,old_sale_price,purpose,decoration,area,floor,top_floor,room,living_room,washroom,balcony,direction,property,fy_common_telephone,if_deleted,create_time,update_time,erp_id,licence,house_tax";
            //dIsDelete = true;
            //img_username,img_department_id,img_user_id,img_time,image_count,cover_image_url,
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
                    if(strOwnerTel == "")
                    {
                        strOwnerTel = "00000000000";
                    }

                    string strRegPerson = row["RegPerson"].ToString().Trim();
                    strRegPerson = strRegPerson.Replace("'", "");
                    strRegPerson = strRegPerson.Replace("\\", "");

                    string strBuildNo = row["BuildNo"].ToString().Trim();
                    strBuildNo = strBuildNo.Replace("'", "");
                    strBuildNo = strBuildNo.Replace("\\", "");

                    //string strPropertyUsage = row["PropertyUsage"].ToString().Trim();
                    //string strSection = "", strBusiness ="";
                    //if(strPropertyUsage == "商铺")
                    //{
                    //    strSection = row["Usage"].ToString().Trim();
                    //    strBusiness = row["PropertyFloor"].ToString().Trim();
                    //}

                    string strPropertyTrust = row["PropertyTrust"].ToString().Trim();
                    if (strPropertyTrust.IndexOf("独家") != -1)
                    {
                        strPropertyTrust = "1";
                    }else
                    {
                        strPropertyTrust = "0";
                    }

                    string strif_Key = "0";
                    if(row["KeyNo"].ToString().Trim().Length > 0)
                    {
                        strif_Key = "1";
                    }

                    string strStatus = row["Status"].ToString().Trim();
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

                    string strRegDate = row["RegDate"].ToString().Trim();
                    if(strRegDate != "")
                    {
                        strRegDate = _dateTime.DateTimeToStamp(strRegDate).ToString();
                    }else
                    {
                        strRegDate = _dateTime.DateTimeToStamp(DateTime.Now).ToString();
                    }

                    string strModDate = row["ModDate"].ToString().Trim();
                    if (strModDate != "")
                    {
                        strModDate = _dateTime.DateTimeToStamp(strModDate).ToString();
                    }
                    else
                    {
                        strModDate = _dateTime.DateTimeToStamp(DateTime.Now).ToString();
                    }

                    string strTrustDate = row["TrustDate"].ToString().Trim();
                    if (strTrustDate != "")
                    {
                        strTrustDate = _dateTime.DateTimeToStamp(strTrustDate).ToString();
                    }
                    else
                    {
                        strTrustDate = _dateTime.DateTimeToStamp(DateTime.Now).ToString();
                    }

                    string strHandOverDate = row["HandOverDate"].ToString().Trim();
                    if (strHandOverDate != "")
                    {
                        strHandOverDate = _dateTime.DateTimeToStamp(strHandOverDate).ToString();
                    }
                    else
                    {
                        strHandOverDate = _dateTime.DateTimeToStamp(DateTime.Now).ToString();
                    }

                    string strLastFollowDate = row["LastFollowDate"].ToString().Trim();
                    if (strLastFollowDate != "")
                    {
                        strLastFollowDate = _dateTime.DateTimeToStamp(strLastFollowDate).ToString();
                    }
                    else
                    {
                        strLastFollowDate = _dateTime.DateTimeToStamp(DateTime.Now).ToString();
                    }

                    string strCountF = row["CountF"].ToString().Trim();
                    if(strCountF == "")
                    {
                        strCountF = "0";
                    }

                    string strCountT = row["CountT"].ToString().Trim();
                    if (strCountT == "")
                    {
                        strCountT = "0";
                    }

                    string strCountW = row["CountW"].ToString().Trim();
                    if (strCountW == "")
                    {
                        strCountW = "0";
                    }

                    string strCountY = row["CountY"].ToString().Trim();
                    if(strCountY == "")
                    {
                        strCountY = "0";
                    }

                    string strCompleteYear = row["CompleteYear"].ToString().Trim();
                    if(strCompleteYear == "")
                    {
                        strCompleteYear = "0";
                    }

                    //List<int> strTempTest = new List<int>();
                    //strTempTest.Add(_dateTime.DateTimeToStamp(row["TrustDate"].ToString().Trim()));
                    //strTempTest.Add(_dateTime.DateTimeToStamp(row["HandOverDate"].ToString().Trim()));
                    //strTempTest.Add(_dateTime.DateTimeToStamp(row["LastFollowDate"].ToString().Trim()));
                    //strTempTest.Add(_dateTime.DateTimeToStamp(row["RegDate"].ToString().Trim()));
                    //strTempTest.Add(_dateTime.DateTimeToStamp(row["ModDate"].ToString().Trim()));

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
                        strPropertyTrust + "','" +
                        strTrustDate + "','" +
                        row["SquareUse"].ToString().Trim() + "'," +    //使用面积
                        Convert.ToDecimal(row["PriceLine"].ToString().Trim()) + "," +   //出售底价
                        Convert.ToDecimal(row["RentPriceLine"].ToString().Trim()) + ",'" +  //出租底价
                        row["PropertyFloor"].ToString().Trim() + "','" +        //配套
                        strRemark + "','" +     //备注
                        strCompleteYear + "'," + dCompanyId + ",'" +     //建筑年代
                        row["PropertyNo"].ToString().Trim() + "'," +   //房源编号
                        Convert.ToDecimal(row["PriceUnit"].ToString().Trim()) + ",'" +  //均价
                        row["Privy"].ToString().Trim() + "','" +    //公私盘
                        row["PropertySource"].ToString().Trim() + "','" +    //房源来源
                        strHandOverDate + "','" +  //交房日期
                        row["PropertyLook"].ToString().Trim() + "','" +  //看房方式
                        strLastFollowDate + "','" +  //最后跟进日
                        row["Trade"].ToString().Trim() + "'," +
                        Convert.ToDecimal(row["Price"].ToString().Trim()) + "," +
                        Convert.ToDecimal(row["RentPrice"].ToString().Trim()) + ",'" +
                        row["RentUnitName"].ToString().Trim() + "'," +
                        Convert.ToDecimal(row["PriceBase"].ToString().Trim()) + ",'" +
                        row["PropertyUsage"].ToString().Trim() + "','" +
                        row["PropertyDecoration"].ToString().Trim() + "'," +
                        Convert.ToDecimal(row["Square"].ToString().Trim()) + ",'" +
                        row["Floor"].ToString().Trim() + "','" +
                        row["FloorAll"].ToString().Trim() + "','" +
                        strCountF + "','" +
                        strCountT + "','" +
                        strCountW + "','" +
                        strCountY + "','" +
                        row["PropertyDirection"].ToString().Trim() + "','" +        //朝向
                        row["PropertyOwn"].ToString().Trim() + "','" +        //产权
                        strOwnerTel + "','" +       //房源常用电话
                        dDeleteMark + "','" +      //删除标记
                        strRegDate + "','" +   //录入日
                        strModDate + "','" +   //修改日
                        row["PropertyID"].ToString().Trim() + "','" +
                        row["IDCard"].ToString().Trim() + "','" +
                        row["PropertyTax"].ToString().Trim() + "'";
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
                foreach(string strId in lstHouseId[0])
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
