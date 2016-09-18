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
    public class Estate : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("community_name", "EstateName");     //楼盘字典名称                 
            dicMap.Add("community_spell", "Spell");         //楼盘字典简拼
            dicMap.Add("address", "Address");               //详细地址
            dicMap.Add("longitude", "CoX");                 //经度
            dicMap.Add("latitude", "CoY");                  //纬度
            dicMap.Add("district_name", "DistrictName");    //行政区名称
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);      //公司ID
            dicMap.Add("if_deleted", "FlagDeleted");        //删除标记
            dicMap.Add("erp_id", "EstateID");               //房友楼盘字典ID
            dicMap.Add("fy_AreaID", "AreaID");              //房友片区ID
            dicMap.Add("create_time", "ExDate:DateTime");   //创建时间
            dicMap.Add("update_time", "ModDate:DateTime");  //更新时间
            dicMap.Add("if_start", "BuildingRule");         //是否开启座栋规则
            dicMap.Add("if_hot", ":String?Default=0");      //是否是热门小区
            dicMap.Add("check_status", ":String?Default=1");//审核状态
            dicMap.Add("status", ":String?Default=0");      //状态
            dicMap.Add("average", "Price");                 //均价
            dicMap.Add("community_alias", ":String?Default=");              //楼盘字典别名
            return dicMap;
        }

        /// <summary>
        /// 楼盘字典类的构造函数
        /// </summary>
        public Estate()
        {
            sTableName = "Estate";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "EstateName";
            dTableName = "erp_community";
            dTableDescript = "小区信息表";
            dPolitContentDescript = "行政区ID|区域ID|区域名称";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导楼盘字典
        /// </summary>
        public void importEstate()
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
            try
            {
                string tmpTable = "erp_community as a ,erp_region as b";
                string tmpValues = "a.district_id = b.district_id,a.biz_area_id = b.biz_area_id,a.biz_area_name = b.biz_area_name";
                string tmpWhere = "and a.fy_AreaId = b.erp_id";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                tmpTable = "erp_community";
                tmpValues = "if_deleted = 1";
                tmpWhere = "and if_deleted = -1";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

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
