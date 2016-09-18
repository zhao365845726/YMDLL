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
    public class Area : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("biz_area_name", "AreaName");            //片区名称
            dicMap.Add("district", "DistrictName");             //商圈名称
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);                  //公司ID
            dicMap.Add("if_deleted", "FlagDeleted");            //删除标记
            dicMap.Add("longitude", ":String?Default=0");       //经度
            dicMap.Add("latitude", ":String?Default=0");        //纬度
            dicMap.Add("erp_id", "AreaID");                     //房友默认ID
            dicMap.Add("create_time", "ExDate:DateTime");       //创建时间
            dicMap.Add("update_time", "ModDate:DateTime");      //更新时间
            return dicMap;
        }

        /// <summary>
        /// 片区类的构造函数
        /// </summary>
        public Area()
        {
            sTableName = "Area";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "ModDate";
            dTableName = "erp_region";
            dTableDescript = "片区,商圈信息表";
            dPolitContentDescript = "行政区ID";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导片区
        /// </summary>
        public bool importArea()
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
                //清空目标表数据
                //_mysql.Delete(dTableName, null);
                //插入数据并返回插入的结果
                bool isResult = _mysql.BatchInsert(dTableName, dColumns, lstValue);
                if (isResult)
                {
                    m_Result = dTableDescript + "数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = dTableDescript + "数据插入失败";
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
        public bool updateData()
        {
            try
            {
                string tmpTable = "erp_region as a,erp_district as b";
                string tmpValues = "a.district_id = b.district_id";
                string tmpWhere = "and a.district = b.district_name";
                _mysql.Update(tmpTable,tmpValues,tmpWhere);
                m_Result += "\n" + dTableDescript + "的" + dPolitContentDescript + "更新成功";
                return true;
            }
            catch(Exception ex)
            {
                m_Result += "\n" + dTableDescript + "的" + dPolitContentDescript + "更新异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
