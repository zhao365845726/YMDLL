using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
using YMDLL.Common;
using YMDLL.Class;

namespace ERP3ToOBS
{
    public class User : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("MarkId", ":String?Default=" + Guid.NewGuid().ToString());
            dicMap.Add("UserName", "username");
            dicMap.Add("UserPwd", "pwd");
            dicMap.Add("CityId", ":String?Default=" + GetCityIdByName(dCityName));
            dicMap.Add("IsAble", ":String?Default=1");
            dicMap.Add("IfChangePwd", ":String?Default=1");
            dicMap.Add("AddDate", ":String?Default=" + DateTime.Now.ToShortDateString());
            dicMap.Add("SHLimit", ":String?Default=60");
            dicMap.Add("RHLimit", ":String?Default=60");
            dicMap.Add("RefreshLimit", ":String?Default=120");
            dicMap.Add("Description", ":String?Default=");
            dicMap.Add("Hot", ":String?Default=0");
            dicMap.Add("old_id", "id");

            return dicMap;
        }

        /// <summary>
        /// 合同实收费用类的构造函数
        /// </summary>
        public User()
        {
            sTableName = "erp_user";
            sColumns = CombineSourceField(FieldMap());
            sWhere = " and user_status = '在职'";
            sOrder = "create_time";
            dTableName = "tbUser";
            dTableDescript = "用户基本信息表";
            dPolitContentDescript = "用户名|城市ID";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导合同实收费用信息
        /// </summary>
        public void importUser()
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
                    _dsqlserver.dbExec("delete from " + dTableName);
                }
                //插入数据并返回插入的结果
                bool isResult = _dsqlserver.BatchInsert(dTableName, dColumns, lstValue);
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
            string strSQL = "";
            try
            {
                strSQL = "update tbUser set UserId = UserName";
                _dsqlserver.dbExec(strSQL);

                m_Result += "\n用户UserID更新成功";
                return true;
            }
            catch (Exception ex)
            {
                m_Result += "\n用户UserID更新异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
