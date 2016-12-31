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
    public class ta_Employee : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("user_id", "uid");                       //用户详情id
            dicMap.Add("nation", "native");                     //籍贯
            dicMap.Add("nationality", "folk");                  //民族
            dicMap.Add("political_status", "polity");           //政治面貌
            dicMap.Add("graduate_college", "graduate");         //毕业院校
            dicMap.Add("address", "address");                   //家庭住址
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);
            dicMap.Add("signature", "idio");                    //个性签名
            dicMap.Add("emergency_contact", "contactname");     //常用联系人
            dicMap.Add("emergency_contact_tel", "contacttel");  //联系人电话
            dicMap.Add("recruit_channel", "source");            //招聘渠道

            dicMap.Add("marital_status", ":String?Default=");   //婚姻状态
            dicMap.Add("job", ":String?Default=");              //分管内容
            dicMap.Add("current_address", ":String?Default=");  //现住址
            dicMap.Add("age", ":String?Default=18");            //年龄
            dicMap.Add("zip_code", ":String?Default=");         //邮政编码
            dicMap.Add("nickname", ":String?Default=");         //昵称
            dicMap.Add("category", ":String?Default=");         //户口性质
            dicMap.Add("contract_status", ":String?Default=");  //合同状态
            dicMap.Add("insurance_status", ":String?Default="); //投保状态
            dicMap.Add("photo", ":String?Default=");            //头像
            dicMap.Add("city_id", ":String?Default=0");         //城市ID
            dicMap.Add("if_hot", ":String?Default=0");          //是否热门经纪人
            dicMap.Add("plane", ":String?Default=");            //个人座机

            //新增字段
            dicMap.Add("old_empid", "empid");           //源用户详情id
            dicMap.Add("old_deptid", "deptid");         //源用户部门id
            dicMap.Add("old_empno", "empno");           //源用户编号
            dicMap.Add("old_status", "status");         //源用户状态
            dicMap.Add("old_speciality", "speciality"); //源用户学校专业
            dicMap.Add("old_idcard", "idcard");         //源用户身份证
            dicMap.Add("old_birthday", "birthday:DateTime");            //源用户生日
            dicMap.Add("old_joindate", "joindate:DateTime");     //源用户入职日期
            dicMap.Add("old_awaydate", "awaydate:DateTime");     //源用户离职日期
            dicMap.Add("old_archives", "archives");     //源用户档案
            dicMap.Add("old_brief", "brief");           //源用户简介
            dicMap.Add("old_remark", "remark");         //源用户备注
            dicMap.Add("old_bankname", "bankname");     //源用户职务
            dicMap.Add("old_education_status", "eduation");         //源用户学历
            return dicMap;
        }

        /// <summary>
        /// 合同实收费用类的构造函数
        /// </summary>
        public ta_Employee()
        {
            sTableName = "ta_emplyee";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "exdate";
            dTableName = "erp_user_detail";
            dTableDescript = "用户详情";
            dPolitContentDescript = "用户详情";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导合同实收费用信息
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
