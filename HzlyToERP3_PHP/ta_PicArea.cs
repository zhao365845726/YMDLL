using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzlyToERP3_PHP
{
    public class ta_PicArea : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("biz_area_name", "areaname");            //片区名称
            dicMap.Add("district", ":String?Default=");         //商圈名称
            dicMap.Add("district_id", ":String?Default=0");     //商圈ID
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);                  //公司ID
            dicMap.Add("if_deleted", "flagdeleted");            //删除标记
            dicMap.Add("longitude", ":String?Default=0.0");     //经度
            dicMap.Add("latitude", ":String?Default=0.0");      //纬度
            dicMap.Add("erp_id", "areaid");                     //房友默认ID
            dicMap.Add("create_time", "dxdate:DateTime");       //创建时间
            dicMap.Add("update_time", "moddate:DateTime");      //更新时间
            //新增字段
            dicMap.Add("old_dsid", "dsid");                     //源-行政区ID
            dicMap.Add("old_areano", "areano");                 //源-片区编号
            dicMap.Add("old_flagtrashed", "flagtrashed");       //源-片区回收标识
            dicMap.Add("old_remark", "remark");                 //源-片区注释
            return dicMap;
        }

        /// <summary>
        /// 行政区类的构造函数
        /// </summary>
        public ta_PicArea()
        {
            sTableName = "ta_picearea";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "dsid,areano";
            dTableName = "erp_region";
            dTableDescript = "片区,商圈信息表";
            dPolitContentDescript = "行政区ID";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导行政区信息
        /// </summary>
        public void importPicArea()
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
                Console.Write("\n数据已经成功写入");
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
