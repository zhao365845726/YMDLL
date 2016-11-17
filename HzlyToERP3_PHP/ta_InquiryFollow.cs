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
    public class ta_InquiryFollow : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("erp_client_id", "inquiryid");              //房源ID
            dicMap.Add("erp_user_id", "empid");                 //用户ID
            dicMap.Add("follow_up_date", "followdate:DateTime");//跟进日期
            dicMap.Add("content", "content");                   //跟进内容
            dicMap.Add("follow_way", "followmethod");           //跟进方式
            dicMap.Add("if_deleted", "flagdeleted");            //删除标记
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);      //公司ID
            dicMap.Add("create_time", "createdate:DateTime");   //录入日期
            dicMap.Add("update_time", "moddate:DateTime");      //更新日期
            dicMap.Add("if_abnormal", ":String?Default=0");     //是否异常
            dicMap.Add("if_stick", ":String?Default=0");        //是否置顶

            //新增字段
            dicMap.Add("old_followid", "followid");             //源房源跟进ID
            dicMap.Add("old_department_id", "deptid");          //源房源跟进部门ID
            return dicMap;
        }

        /// <summary>
        /// 合同实收费用类的构造函数
        /// </summary>
        public ta_InquiryFollow()
        {
            sTableName = "ta_followother";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "createdate";
            sWhere = " and inquiryid <> ''";
            dTableName = "erp_client_follow";
            dTableDescript = "客源跟进表";
            dPolitContentDescript = "";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导合同实收费用信息
        /// </summary>
        public void importInquiryFollow()
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
