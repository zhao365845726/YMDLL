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
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("room_code", "RoomNo");          //房号
            dicMap.Add("block", "BuildNo");             //栋座
            dicMap.Add("fy_community", "EstateID");     //楼盘字典ID
            dicMap.Add("district", "DistrictName");     //行政区名字
            dicMap.Add("owner_name", "OwnerName");      //业主姓名
            dicMap.Add("key_code", "KeyNo");            //钥匙编号
            dicMap.Add("status", "Status");             //房源状态
            dicMap.Add("entrust_code", "TrustNo");      //委托编号
            dicMap.Add("input_username", "RegPerson");  //首次录入人
            dicMap.Add("fy_empid", "EmpID");            //原维护人ID
            dicMap.Add("fy_deptid", "DeptID");          //原维护部门ID
            //dicMap.Add("exclusive", "PropertyTrust");   //是否独家
            dicMap.Add("exclusive_date", "TrustDate:DateTime");  //独家委托日期
            dicMap.Add("usable_area", "SquareUse");     //使用面积
            dicMap.Add("floor_sale_price", "PriceLine");//出售底价
            dicMap.Add("floor_rent_price", "RentPriceLine");        //出租底价
            dicMap.Add("fitment", "PropertyFloor");     //房屋配套
            dicMap.Add("remark", "Remark");             //房源备注
            dicMap.Add("fy_building_age", "CompleteYear:String?=0");          //建造年代
            dicMap.Add("contract_code", "PropertyNo");  //房源编号
            dicMap.Add("unit_price", "PriceUnit");      //房源均价
            dicMap.Add("public", "Privy");              //房源的公私盘
            dicMap.Add("source", "PropertySource");     //房源来源
            dicMap.Add("launch_date", "HandOverDate:DateTime");     //交房日期
            dicMap.Add("fy_visit_way", "PropertyLook"); //看房方式
            dicMap.Add("last_follow", "LastFollowDate:DateTime");   //最后跟进日期
            dicMap.Add("type", "Trade");                //挂牌类型
            dicMap.Add("sale_price", "Price");          //出售总价
            dicMap.Add("rent_price", "RentPrice");      //出租总价
            dicMap.Add("rent_unit", "RentUnitName");    //租价单位
            dicMap.Add("old_sale_price", "PriceBase");  //原售价
            dicMap.Add("purpose", "PropertyUsage");     //房源用途
            dicMap.Add("decoration", "PropertyDecoration");         //装修类型
            dicMap.Add("area", "Square");               //面积
            dicMap.Add("floor", "Floor");               //楼层
            dicMap.Add("top_floor", "FloorAll");        //总楼层
            dicMap.Add("fy_room", "CountF:String?=0");              //卧室
            dicMap.Add("living_room", "CountT:String?=0");          //客厅
            dicMap.Add("washroom", "CountW:String?=0");             //卫生间
            dicMap.Add("balcony", "CountY:String?=0");              //阳台
            dicMap.Add("direction", "PropertyDirection");           //房源朝向
            dicMap.Add("property", "PropertyOwn");      //产权性质
            dicMap.Add("fy_common_telephone", "OwnerTel:String?=00000000000");          //业主电话
            dicMap.Add("if_deleted", "FlagDeleted");    //是否删除
            dicMap.Add("create_time", "RegDate:DateTime");          //录入日期
            dicMap.Add("update_time", "ModDate:DateTime");          //更新日期
            dicMap.Add("erp_id", "PropertyID");         //原房源编号
            dicMap.Add("licence", "IDCard");            //房产证号
            dicMap.Add("house_tax", "PropertyTax");     //房源税费 
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);     //公司ID
            dicMap.Add("tao_bao", ":String?Default=0");             //是否淘宝
            dicMap.Add("if_entertained", ":String?Default=0");      //是否封盘
            dicMap.Add("if_key", ":String?Default=0");              //是否有钥匙
            dicMap.Add("room_id", ":String?Default=0");             //房号ID
            dicMap.Add("unit_id", ":String?Default=0");             //单元ID
            dicMap.Add("block_id", ":String?Default=0");            //栋座ID
            dicMap.Add("city_id", ":String?Default=0");             //城市ID
            dicMap.Add("fy_key_userid", "EmpID1");                  //钥匙人ID
            dicMap.Add("fy_key_deptid", "DeptID1");                 //钥匙人部门ID
            return dicMap;
        }

        /// <summary>
        /// 房源类的构造函数
        /// </summary>
        public Property()
        {
            sTableName = "Property";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "RegDate";
            dTableName = "erp_house";
            dTableDescript = "房源表";
            dPolitContentDescript = "行政区|片区|楼盘字典及ID";
            dColumns = CombineDestField(FieldMap());
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
                    //string strPropertyTrust = row["PropertyTrust"].ToString().Trim();
                    //if (strPropertyTrust.IndexOf("独家") != -1)
                    //{
                    //    strPropertyTrust = "1";
                    //}else
                    //{
                    //    strPropertyTrust = "0";
                    //}

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
                    m_Result = "\n" + dTableName + "数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "\n" + dTableName + "数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出" + dTableName + "异常.\n异常原因：" + ex.Message;
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

                //更新房源的小区id和小区名称
                tmpTable = "erp_house a,erp_community b";
                tmpValues = "a.community = b.community_name,a.community_id = b.community_id,a.region = b.biz_area_name,a.region_id = b.biz_area_id,a.district_id = b.district_id";
                tmpWhere = "and a.fy_community = b.erp_id";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                //更新钥匙人和钥匙人所在部门
                tmpTable = "erp_house a LEFT JOIN erp_user b ON a.fy_key_userid = b.erp_id";
                tmpValues = "a.key_user_id = b.id,a.key_username = b.username,a.key_department_id = b.department_id,a.if_key = 1";
                tmpWhere = "and a.fy_key_userid = b.erp_id";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                //更新钥匙管理的状态
                tmpTable = "erp_house";
                tmpValues = "key_status = '已收'";
                tmpWhere = "if_key = 1";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);
                
                tmpValues = "status = '有效'";
                tmpWhere = "and status = '预定'";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                tmpValues = "status = '他租'";
                tmpWhere = "and status = '已租'";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                tmpValues = "status = '他售'";
                tmpWhere = "and status = '已售'";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                tmpValues = "status = '无效'";
                tmpWhere = "and status = '未知'";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                m_Result += "\n" + dTableName + "中的" + dTableDescript + "更新成功";
                return true;
            }
            catch (Exception ex)
            {
                m_Result += "\n" + dTableName + "中的" + dTableDescript + "更新异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
