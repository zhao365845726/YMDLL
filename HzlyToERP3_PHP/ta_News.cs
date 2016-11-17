﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
using YMDLL.Common;
using YMDLL.Class;

namespace HzlyToERP3_PHP
{
    public class ta_News : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("title", "newstitle");               //公告标题
            dicMap.Add("content", "content");               //公告内容
            dicMap.Add("type", "newstype");                 //公告类型
            dicMap.Add("input_date", "createtime:DateTime");//录入日期
            dicMap.Add("if_recycle", ":String?Default=0");        //回收标记
            dicMap.Add("if_deleted", ":String?Default=" + dDeleteMark);        //删除标记
            dicMap.Add("title_color", ":String?Default=");  //标题颜色
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);      //公司ID
            dicMap.Add("if_alert", ":String?Default=0");    //是否提醒
            dicMap.Add("file", ":String?Default=");         //文件附件
            dicMap.Add("if_bold", ":String?Default=0");     //是否粗体
            dicMap.Add("tab_order", ":String?Default=0");   //公告顺序

            //新增字段
            dicMap.Add("old_newsid", "newsid");             //源新闻ID
            dicMap.Add("old_deptid", "deptid");             //源新闻发布人部门
            dicMap.Add("old_dept", "dept");                 //源部门名称
            dicMap.Add("old_empid", "empid");               //源人员id
            dicMap.Add("old_empname", "empname");           //源人员名称
            return dicMap;
        }


        /// <summary>
        /// 合同实收费用类的构造函数
        /// </summary>
        public ta_News()
        {
            sTableName = "ta_news";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "createtime";
            dTableName = "erp_office_notice";
            dTableDescript = "新闻公告";
            dPolitContentDescript = "";
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
