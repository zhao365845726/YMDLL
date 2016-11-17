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
    public class ta_SystemUser : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("erp_id", "uid");              //原房友ID
            dicMap.Add("username", "chname");          //用户姓名
            dicMap.Add("tel", "mobile");                   //用户电话
            dicMap.Add("pwd", "password");              //用户密码
            dicMap.Add("gender", "sex");                //用户性别:0-男，1-女
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);      //公司ID
            dicMap.Add("user_code", "username");           //用户编号
            dicMap.Add("email", "email");               //用户邮箱
            dicMap.Add("create_time", "createdate:DateTime");                   //创建时间
            dicMap.Add("update_time", "modidate:DateTime");                  //更新时间
            dicMap.Add("if_deleted", "flagdeleted");    //删除标记

            //新增字段
            dicMap.Add("old_address", "adress");            //地址
            return dicMap;
        }

        /// <summary>
        /// 人员类的构造函数
        /// </summary>
        public ta_SystemUser()
        {
            sTableName = "ta_systemuser";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "username";
            dTableName = "erp_user";
            dTableDescript = "用户表";
            dPolitContentDescript = "归属部门|职务|角色|就职状态";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导人员数据
        /// </summary>
        public void importEmployee()
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
