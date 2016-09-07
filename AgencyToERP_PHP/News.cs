using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
using YMDLL.Common;
using YMDLL.Class;

namespace AgencyToERP_PHP 
{
    public class News : Base
    {
        public void Descript()
        {
            //Dictionary<目标数据库,源数据库>
            //表增加索引：
        }

        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("type", "NewsType");                 //公告类型
            dicMap.Add("input_date", "RegDate:DateTime");   //录入日期
            dicMap.Add("erp_user_id", "EmpID");             //人员ID
            dicMap.Add("title", "Title");                   //公告标题
            dicMap.Add("content", "Content");               //公告内容
            //dicMap.Add("create_time", "RegDate:DateTime");  //创建时间
            dicMap.Add("update_time", "ModDate:DateTime");  //更新时间
            dicMap.Add("if_recycle", "FlagTrashed");        //回收标记
            dicMap.Add("if_deleted", "FlagDeleted");        //删除标记
            dicMap.Add("title_color", "TitleColor");        //标题颜色
            return dicMap;
        }

        /// <summary>
        /// 合同实收费用类的构造函数
        /// </summary>
        public News()
        {
            sTableName = "News";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "RegDate";
            dTableName = "erp_office_notice";
            dTableDescript = "公司公告表";
            dPolitContentDescript = "公司ID|删除标记|首页弹出|附件地址|加粗|显示顺序";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导合同实收费用信息
        /// </summary>
        public void importNews()
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
            string tmpTable = "", tmpValues = "", tmpWhere = "";
            try
            {
                tmpTable = "erp_office_notice";
                //更新公司公告表中company_id字段和if_deleted字段
                tmpValues = "company_id = '" + dCompanyId + "',if_deleted = '" + dDeleteMark + "',if_alert = 0,file = '',if_bold = 0,tab_order = 0";
                tmpWhere = "";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                //更新创建时间
                tmpValues = "create_time = input_date";
                tmpWhere = "";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                //更新用户id,部门id,用户姓名
                tmpTable = "erp_office_notice a,erp_user b";
                tmpValues = "a.department_id = b.department_id,a.username = b.username,a.user_id = b.id";
                tmpWhere = "and a.erp_user_id = b.erp_id and a.erp_user_id <> ''";
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
