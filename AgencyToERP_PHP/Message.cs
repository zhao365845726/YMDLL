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
    public class Message : Base
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
            dicMap.Add("erp_id", "MessageID");              //原情报站ID
            dicMap.Add("sub_type", "MessageType");          //业务审批类型
            dicMap.Add("content", "Content");               //业务审批的内容
            dicMap.Add("create_time", "SendDate:DateTime"); //创建时间
            dicMap.Add("update_time", "ModDate:DateTime");  //修改时间
            dicMap.Add("erp_title", "Title");               //情报站标题
            dicMap.Add("username", "SendPerson");           //申请人
            dicMap.Add("review_username", "RecvPerson");    //审核人
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);      //公司id
            dicMap.Add("if_deleted", ":String?Default=" + dDeleteMark);     //删除标记
            dicMap.Add("status", ":String?Default=已审核");//审批状态
            dicMap.Add("reject_reason",":String?Default="); //驳回原因
            dicMap.Add("type",":String?Default=房源");     //资源类型
            dicMap.Add("fk_code", ":String?Default=");      //资源编号
            dicMap.Add("house_status", ":String?Default=有效");             //房源状态
            dicMap.Add("process_mode", ":String?Default=");                //举报后，处理的方式
            dicMap.Add("is_import", ":String?Default=0");   //
            dicMap.Add("review_condition", ":String?Default=");              //审核后的操作
            return dicMap;
        }

        /// <summary>
        /// 合同实收费用类的构造函数
        /// </summary>
        public Message()
        {
            sTableName = "Message";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "SendDate";
            dTableName = "erp_business_review";
            dTableDescript = "业务审批表";
            dPolitContentDescript = "公司ID|删除标记|申请状态|类型|资源编号|资源ID|资源状态";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导合同实收费用信息
        /// </summary>
        public void importMessage()
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
                tmpTable = "erp_business_review";
                //更新公司公告表中company_id字段和if_deleted字段
                //tmpValues = "status = '已审核',reject_reason = '',type = '房源',fk_code = '',house_status = '有效',process_mode = '',is_import = 0,review_condition = ''";
                //tmpWhere = "";
                //_mysql.Update(tmpTable, tmpValues, tmpWhere);

                tmpWhere = "and sub_type in ('出售','广告','留言','端口申报','综合','')";
                _mysql.Delete(tmpTable, tmpWhere);

                tmpValues = "sub_type = '申请房源修改'";
                tmpWhere = "and sub_type = '修改'";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                tmpValues = "sub_type = '房源议价'";
                tmpWhere = "and sub_type = '砍价'";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                //更新业务审批的房源编号
                tmpValues = "fk_code = erp_title";
                tmpWhere = "and erp_title REGEXP '^[A-Za-z]*[0-9]*$'";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                //更新业务审批申请人，申请人部门
                List<string>[] lstMsgId = new List<string>[1];
                lstMsgId = _mysql.Select("erp_business_review", "document_id", "");
                foreach (string strId in lstMsgId[0])
                {
                    //更新业务审批申请人，申请人部门
                    tmpTable = "erp_business_review as a,erp_user as b";
                    tmpValues = "a.user_id = b.id,a.department_id = b.department_id";
                    tmpWhere = "and SUBSTRING_INDEX(a.username,'.',-1) = b.username and a.document_id = " + strId;
                    _mysql.Update(tmpTable, tmpValues, tmpWhere);
                }

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
