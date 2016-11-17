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
    public class ta_Department : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("dept_name", "depname");             //部门名称
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);    //公司ID
            dicMap.Add("address", "address");               //部门地址
            dicMap.Add("tel", "tel");                       //部门电话
            dicMap.Add("dept_type", "layer");               //部门类型(职能|区|大区)
            dicMap.Add("if_deleted", "flagdeleted");        //删除标记
            dicMap.Add("create_time", "exdate:DateTime");   //创建时间
            dicMap.Add("update_time", "moddate:DateTime");  //更新时间
            dicMap.Add("dept_code", "header");              //部门简拼
            dicMap.Add("erp_id", "deptno");                 //部门编码
            dicMap.Add("status", "depttype");               //部门的状态
            dicMap.Add("longitude", "cox:String?Default=0");                 //经度
            dicMap.Add("latitude", "coy:String?Default=0");                  //纬度
            
            //新增的字段
            dicMap.Add("old_dept_id", "deptid");             //原部门ID
            dicMap.Add("old_pid", "pid");                    //原系统部门归属
            return dicMap;
        }

        /// <summary>
        /// 部门类的构造函数
        /// </summary>
        public ta_Department()
        {
            sTableName = "ta_Department";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "deptno";
            dTableName = "erp_department";
            dTableDescript = "部门表";
            dPolitContentDescript = "父级ID|状态|职能|区|店|组";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 部门信息
        /// </summary>
        public void importDepartment()
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

        /// <summary>
        /// 更新数据
        /// </summary>
        public bool UpdateData(string typeName)
        {
            try
            {
                if (typeName == "删除")
                {
                    _dmysql.Update("erp_department", "if_deleted = 1", "and if_deleted = -1");
                    m_Result += "\n" + dTableDescript + "中删除标识更新成功";
                }
                else if (typeName == "父级ID")
                {
                    _dmysql.Update("erp_department", "erp_pid = SUBSTRING(erp_id,1,(LENGTH(erp_id)-2))", "");
                    m_Result += "\n" + dTableDescript + "中父级ID更新成功";
                }
                else if (typeName == "状态")
                {
                    _dmysql.Update("erp_department", "status = '有效'", "and status <> '关闭'");
                    m_Result += "\n" + dTableDescript + "中状态更新成功";
                }
                else if (typeName == "职能")
                {
                    _dmysql.Update("erp_department", "dept_type = '职能'", "");
                    m_Result += "\n" + dTableDescript + "中职能更新成功";
                }
                else if (typeName == "大区")
                {
                    _dmysql.Update("erp_department", "dept_type = '大区'", "and LENGTH(erp_id)=2 and erp_id > 01 and erp_id < 99 ");
                    m_Result += "\n" + dTableDescript + "中大区更新成功";
                }
                else if (typeName == "其他")
                {
                    string AreaIds = "04,05,06,07,08,20,21,29,30";
                    string[] saAreaId = AreaIds.Split(',');
                    foreach (string aid in saAreaId)
                    {
                        _dmysql.Update("erp_department", "dept_type = '区'", "and LENGTH(erp_id)=4 and erp_id like '" + aid + "%'");
                        _dmysql.Update("erp_department", "dept_type = '店'", "and LENGTH(erp_id)=6 and erp_id like '" + aid + "%'");
                        _dmysql.Update("erp_department", "dept_type = '组'", "and LENGTH(erp_id)=8 and erp_id like '" + aid + "%'");
                    }

                    m_Result += "\n" + dTableDescript + "中" + dPolitContentDescript + "更新成功";
                }
                else if (typeName == "加盟店")
                {
                    strTable = "erp_department a JOIN erp_join_stores b ON a.dept_name = b.Fstores_name ";
                    strValues = "a.join_stores_id = b.Fdocument_id,a.join_stores_name = a.dept_name";
                    strWhere = "and a.dept_name = b.Fstores_name";
                    _dmysql.Update(strTable, strValues, strWhere);
                    m_Result += "\n" + dTableDescript + "中状态更新成功";
                }
                else
                {
                    m_Result += "\n对不起,没有可更新的内容";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                m_Result += "\n" + dTableDescript + "中更新异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
