using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzlyToERP3_PHP
{
    public class ta_Attachment : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            //dicMap.Add("image_id", "");
            //dicMap.Add("room_id", "");
            dicMap.Add("url", "attachurl");
            dicMap.Add("if_deleted", ":String?Default=" + dDeleteMark);
            //dicMap.Add("user_id", "");
            //dicMap.Add("username", "");
            //dicMap.Add("review_user_id", "");
            //dicMap.Add("review_username", "");
            //dicMap.Add("department_id", "");
            dicMap.Add("create_time", "createtime:DateTime");
            //dicMap.Add("update_time", "");
            //dicMap.Add("checked_time", "");
            //dicMap.Add("house_status", "");
            //dicMap.Add("status", "");
            //dicMap.Add("type", "");
            //dicMap.Add("info", "");
            //dicMap.Add("house_type", "");
            //dicMap.Add("content", "");
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);
            //dicMap.Add("image_type", "");
            //dicMap.Add("fk_code", "");
            //dicMap.Add("timeNum", "");
            //dicMap.Add("house_id", "");
            //dicMap.Add("set_review", "");
            //dicMap.Add("erp_house_id", "");
            //dicMap.Add("erp_user_id", "");

            //新增字段
            dicMap.Add("old_phototype", "phototype");
            dicMap.Add("old_empid", "empid");
            dicMap.Add("old_empname", "empname");
            dicMap.Add("old_smallurl", "smallurl");
            dicMap.Add("old_uploaddate", "uploaddate");
            dicMap.Add("old_attachmentid", "attachmentid");
            dicMap.Add("old_attachname", "attachname");
            return dicMap;
        }

        /// <summary>
        /// 合同实收费用类的构造函数
        /// </summary>
        public ta_Attachment()
        {
            sTableName = "ta_attachment";
            sColumns = CombineSourceField(FieldMap());
            //sOrder = "createtime";
            dTableName = "erp_house_image";
            dTableDescript = "房源图片表";
            dPolitContentDescript = "";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导合同实收费用信息
        /// </summary>
        public void importAttachment()
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
