using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
using YMDLL.Common;
using YMDLL.Class;

namespace HzlyToERP3_PHP
{
  public class ta_House : Base
  {
    /// <summary>
    /// 字段映射方法
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string> FieldMap()
    {
      //Dictionary<目标数据库,源数据库>
      Dictionary<string, string> dicMap = new Dictionary<string, string>();
      dicMap.Add("", "");          //

      dicMap.Add("room_code", "roomno");          //房号
      dicMap.Add("key_code", "keyno:String?Default=");            //钥匙编号
      dicMap.Add("contract_code", "propertyno");  //房源编号
                                                  //dicMap.Add("floor", "floor:String?=0");               //楼层
      dicMap.Add("top_floor", "floorall:String?Default=0");        //总楼层
                                                                   //dicMap.Add("room", "countf:String?=0");              //卧室
      dicMap.Add("living_room", "countt:String?=0");          //客厅
      dicMap.Add("washroom", "countw:String?=0");             //卫生间
      dicMap.Add("balcony", "county:String?=0");              //阳台
      dicMap.Add("create_time", "regdate:DateTime");          //录入日期
      dicMap.Add("update_time", "moddate:DateTime");          //更新日期
      dicMap.Add("if_deleted", ":String?Default=" + dDeleteMark);    //是否删除
      dicMap.Add("input_username", "regperson");  //首次录入人
      dicMap.Add("remark", "remark");             //房源备注
      dicMap.Add("direction", "propertydirction");           //房源朝向
      dicMap.Add("decoration", "propertydecoration");         //装修类型
      dicMap.Add("area", "square:String?Default=0");               //面积
      dicMap.Add("licence", "idcard");            //房产证号
                                                  //dicMap.Add("public", "privy:String?=0");              //房源的公私盘
      dicMap.Add("public", ":String?Default=0");              //房源的公私盘
      dicMap.Add("last_follow", "lastfollowdate:DateTime");   //最后跟进日期
      dicMap.Add("exclusive_date", "trustdate:DateTime");  //独家委托日期
      dicMap.Add("launch_date", "handoverdate:DateTime");     //交房日期
      dicMap.Add("entrust_code", "trustno");      //委托编号
      dicMap.Add("property", "propertyown");      //产权性质
      dicMap.Add("usable_area", "squareuser");     //使用面积

      ////dicMap.Add("block", "BuildNo");             //栋座
      ////dicMap.Add("fy_community", "EstateID");     //楼盘字典ID
      ////dicMap.Add("district", "DistrictName");     //行政区名字
      //dicMap.Add("owner_name", "OwnerName");      //业主姓名
      ////dicMap.Add("status", "Status");             //房源状态
      //dicMap.Add("fy_empid", "EmpID");            //原维护人ID
      //dicMap.Add("fy_deptid", "DeptID");          //原维护部门ID
      ////dicMap.Add("exclusive", "PropertyTrust");   //是否独家
      //dicMap.Add("floor_sale_price", "PriceLine");//出售底价
      //dicMap.Add("floor_rent_price", "RentPriceLine");        //出租底价
      //dicMap.Add("fitment", "PropertyFloor");     //房屋配套
      //dicMap.Add("unit_price", "PriceUnit");      //房源均价
      ////dicMap.Add("type", "Trade");                //挂牌类型
      //dicMap.Add("sale_price", "Price");          //出售总价
      //dicMap.Add("rent_price", "RentPrice");      //出租总价
      //dicMap.Add("rent_unit", "RentUnitName");    //租价单位
      //dicMap.Add("old_sale_price", "PriceBase");  //原售价
      //dicMap.Add("fy_common_telephone", "OwnerTel:String?=00000000000");          //业主电话
      //dicMap.Add("erp_id", "PropertyID");         //原房源编号
      //dicMap.Add("house_tax", "PropertyTax");     //房源税费 
      //dicMap.Add("fy_key_userid", "EmpID1");                  //钥匙人ID
      //dicMap.Add("fy_key_deptid", "DeptID1");                 //钥匙人部门ID

      dicMap.Add("source", ":String?Default=");           //房源来源
      dicMap.Add("purpose", ":String?Default=");          //房源用途
      dicMap.Add("company_id", ":String?Default=" + dCompanyId);     //公司ID
      dicMap.Add("tao_bao", ":String?Default=0");             //是否淘宝
      dicMap.Add("if_entertained", ":String?Default=0");      //是否封盘
      dicMap.Add("if_key", ":String?Default=0");              //是否有钥匙
      dicMap.Add("room_id", ":String?Default=0");             //房号ID
      dicMap.Add("unit_id", ":String?Default=0");             //单元ID
      dicMap.Add("block_id", ":String?Default=0");            //栋座ID
      dicMap.Add("city_id", ":String?Default=0");             //城市ID

      //新增字段
      dicMap.Add("old_houseid", "houseid");               //源房源ID
      dicMap.Add("old_buildingid", "buildingid");         //源座栋id
      dicMap.Add("old_unitid", "unitid");                 //源单元id
      dicMap.Add("old_cityid", "cityname");               //源城市名称
      dicMap.Add("old_districtid", "districtname");       //源行政区id
      dicMap.Add("old_piceareaid", "piceareaid");         //源片区id
      dicMap.Add("old_estid", "estid");                   //源楼盘字典id
      dicMap.Add("old_propertyusage", "propertyusage");   //房源用途
                                                          //dicMap.Add("old_propertytype", "propertytype");     //房源类型
      dicMap.Add("old_propertysource", "propertysource"); //房源来源
      dicMap.Add("old_building_age", "completeyear:String?=0");          //建造年代
      dicMap.Add("old_tradetype", "tradetype");           //交易类型
      dicMap.Add("old_tradestatus", "tradestatus");       //交易状态
      dicMap.Add("old_visit_way", "propertylook");        //看房方式
      dicMap.Add("old_room", "countf:String?=0");         //室
      dicMap.Add("old_public", "privy:String?=0");        //公私盘
                                                          //dicMap.Add("old_floor", "floor:String?=0");               //楼层
      return dicMap;
    }

    /// <summary>
    /// 合同实收费用类的构造函数
    /// </summary>
    public ta_House()
    {
      sTableName = "ta_house";
      sColumns = CombineSourceField(FieldMap());
      sOrder = "regdate";
      dTableName = "erp_house";
      dTableDescript = "房源表";
      dPolitContentDescript = "行政区|片区|楼盘字典及ID";
      dColumns = CombineDestField(FieldMap());
    }

    /// <summary>
    /// 导合同实收费用信息
    /// </summary>
    public void importHouse()
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
    /// 分页数据
    /// </summary>
    /// <returns></returns>
    public bool PagerData()
    {
      try
      {
        DataTable dt = _smysql.GetPager(sTableName, sColumns, sOrder, sPageSize, sPageIndex, sWhere, out sTotalCount);

        List<String> lstValue = new List<String>();
        foreach (DataRow row in dt.Rows)
        {
          string strTemp = GetConcatValues(FieldMap(), row);

          lstValue.Add(strTemp);
        }
        //如果允许删除，清空目标表数据
        if (dIsDelete == true)
        {
          _dmysql.Delete(dTableName, null);
        }
        //插入数据并返回插入的结果
        bool isResult = _dmysql.BatchInsert(dTableName, dColumns, lstValue);
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
  }
}
