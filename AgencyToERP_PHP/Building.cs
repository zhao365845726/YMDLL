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
    public class Building : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("erp_id", "BuildingID");                 //座栋ID
            dicMap.Add("fy_CommunityId", "EstateID");           //楼盘字典ID
            dicMap.Add("building_code", "BuildingName");        //座栋名称
            dicMap.Add("create_time", "ExDate:DateTime");       //创建时间
            dicMap.Add("update_time", "ModDate:DateTime");      //更新时间
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);      //公司ID
            dicMap.Add("if_deleted", "FlagDeleted");            //删除标识
            dicMap.Add("up_total_floors", "FloorAll:String?Default=1");     //地上楼层
            dicMap.Add("down_total_floors", ":String?Default=0");           //地下总层数
            dicMap.Add("lifts", "CountT:String?Default=0");                 //梯
            dicMap.Add("rooms", "CountH:String?Default=0");                 //户
            dicMap.Add("units", "Cell:String?Default=0");                   //单元
            dicMap.Add("longitude", ":String?Default=0");       //经度
            dicMap.Add("latitude", ":String?Default=0");        //纬度
            dicMap.Add("city_id", ":String?Default=0");         //城市ID
            dicMap.Add("unit_suffix",":String?Default=单元");   //单元后缀
            dicMap.Add("floor_alias_json", ":String?Default={}");           //楼层别名
            return dicMap;
        }

        /// <summary>
        /// 栋座单元类的构造函数
        /// </summary>
        public Building()
        {
            sTableName = "Building";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "ExDate";
            dTableName = "erp_community_block";
            dTableDescript = "座栋信息表";
            dPolitContentDescript = "";
            dColumns = CombineDestField(FieldMap());
        }


        /// <summary>
        /// 导栋座单元数据
        /// </summary>
        public void importBuilding()
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
        /// 导栋座单元
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
            string tmpTable = "", tmpValues = "", tmpWhere = "";
            try
            {
                tmpTable = "erp_community_block";
                tmpValues = "if_deleted = 1";
                tmpWhere = "and if_deleted = -1";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                //_mysql.ExecuteSQL("CALL UpdateCommunityBlock");

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
