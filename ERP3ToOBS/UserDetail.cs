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
    public class UserDetail : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("UserName", "username");
            dicMap.Add("Sex", "gender");
            dicMap.Add("Email", "email");
            dicMap.Add("PhoneNo", "tel");
            dicMap.Add("LastPostTime", ":String?Default=");
            dicMap.Add("HeadPortrait", ":String?Default=");
            dicMap.Add("loginTime", ":String?Default=" + DateTime.Now.ToShortDateString());
            dicMap.Add("loginContinueCount", ":String?Default=");
            dicMap.Add("shareTime", ":String?Default=");
            dicMap.Add("shareCount", ":String?Default=");
            dicMap.Add("suggestTime", ":String?Default=");
            dicMap.Add("suggestCount", ":String?Default=");
            dicMap.Add("score", ":String?Default=");
            dicMap.Add("old_id", "id");
            return dicMap;
        }

        /// <summary>
        /// 合同实收费用类的构造函数
        /// </summary>
        public UserDetail()
        {
            sTableName = "erp_user";
            sColumns = CombineSourceField(FieldMap());
            sWhere = " and user_status = '在职'";
            sOrder = "create_time";
            dTableName = "zhUserDetails";
            dTableDescript = "用户详情信息";
            dPolitContentDescript = "用户UserId|UserMark";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导合同实收费用信息
        /// </summary>
        public void importUserDetail()
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
                strSQL = "update zhUserDetails set zhUserDetails.UserId = tbUser.Id,zhUserDetails.MarkId = tbUser.MarkId from tbUser where zhUserDetails.old_id = tbUser.old_id";
                _dsqlserver.dbExec(strSQL);

                strSQL = "update zhUserDetails set PhoneNo = REPLACE(PhoneNo,';','|')";
                _dsqlserver.dbExec(strSQL);

                m_Result += "\n用户UserID|UserMark更新成功";
                return true;
            }
            catch (Exception ex)
            {
                m_Result += "\n用户UserID|UserMark更新异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
