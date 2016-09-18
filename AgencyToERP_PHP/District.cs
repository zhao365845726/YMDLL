using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMDLL.Class;
using YMDLL.Common;

namespace AgencyToERP_PHP
{
    public class District : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("district_name", "DistrictName");                //行政区名称
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);  //公司ID
            dicMap.Add("if_deleted", ":String?Default=0"); //删除标记
            dicMap.Add("erp_id", "DistrictNo");                         //行政区编号
            dicMap.Add("create_time", ":String?Default=1472011552");    //创建时间
            dicMap.Add("update_time", ":String?Default=1472011552");    //创建时间
            dicMap.Add("longitude", ":String?Default=0");               //经度
            dicMap.Add("latitude", ":String?Default=0");                //纬度
            dicMap.Add("status", ":String?Default=0");                  //状态
            return dicMap;
        }

        /// <summary>
        /// 行政区类的构造函数
        /// </summary>
        public District()
        {
            sTableName = "District";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "DistrictNo";
            dTableName = "erp_district";
            dTableDescript = "行政区信息表";
            dPolitContentDescript = "";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导行政区数据
        /// </summary>
        public bool importDistrict()
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
    }
}
