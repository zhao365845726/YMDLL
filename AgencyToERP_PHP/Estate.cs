using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMDLL.Class;
using YMDLL.Common;

namespace AgencyToERP_PHP
{
    public class Estate : Base
    {
        public void Descript()
        {
            /*
             操作流程：
             1.在3.0系统erp_community表中新增fy_AreaID
             2.读取房友数据库中的数据
             3.将读取的数据写入到3.0系统中
             4.更新写入数据库后行政区ID,片区ID,片区名对应不上的问题
             5.删除erp_community表中fy_AreaID，保证前后数据的完整
             */
        }

        /// <summary>
        /// 楼盘字典类的构造函数
        /// </summary>
        public Estate()
        {
            sTableName = "Estate";
            sColumns = "EstateName,Spell,Address,CoX,CoY,DistrictName,FlagLocked,EstateID,AreaID";
            sOrder = "EstateName";
            sPageIndex = 1;
            sPageSize = 1000;
            dTableName = "erp_community";
            dFieldAdd = "fy_AreaID";
            dColumns = "community_name,community_spell,address,longitude,latitude,create_time,update_time,district_name,if_deleted,company_id,if_start,erp_id" + "," + dFieldAdd;
            dIsDelete = true;
            //,biz_area_id
        }

        /// <summary>
        /// 导楼盘字典
        /// </summary>
        public void importEstate()
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
                    int iFlagLocked = 0;
                    int iBuildingRule = 0;
                    string strAddress = "";
                    //决定是否锁盘的值
                    if (row["FlagLocked"].ToString().ToLower() == "false")
                    {
                        iFlagLocked = 0;
                    }
                    else
                    {
                        iFlagLocked = 1;
                    }
                    //决定是否开启栋座单元的值
                    //if (row["BuildingRule"].ToString().ToLower() == "false")
                    //{
                    //    iBuildingRule = 0;
                    //}
                    //else
                    //{
                    //    iBuildingRule = 1;
                    //}
                    //过滤Address,Address2,Remark字段的'号
                    if (row["Address"].ToString().IndexOf("'") != -1 || row["Address"].ToString().IndexOf("’") != -1 || row["Address"].ToString().IndexOf("‘") != -1)
                    {
                        strAddress = row["Address"].ToString().Replace('\'', '"');
                    }
                    else
                    {
                        strAddress = row["Address"].ToString();
                    }
                    string strTemp = "'" + row["EstateName"].ToString() + "','" +
                        row["Spell"].ToString() + row["EstateName"].ToString() + "','" +
                        strAddress + "','" +
                        row["CoX"].ToString() + "','" +
                        row["CoY"].ToString() + "','" +
                        _dateTime.DateTimeToStamp(DateTime.Now) + "','" +
                        _dateTime.DateTimeToStamp(DateTime.Now) + "','" +
                        row["DistrictName"].ToString() + "'," +
                        "0,102," +
                        row["FlagLocked"].ToString() + ",'" +
                        row["EstateID"].ToString() + "','" + 
                        row["AreaID"].ToString() + "'"
                        ;

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
                    m_Result = "\n楼盘字典数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "\n楼盘字典数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出楼盘字典异常.\n异常原因：" + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 添加字段
        /// </summary>
        public void AddField(string FieldName,string FieldType)
        {
            _mysql.UpdateField("add", dTableName, FieldName, FieldType);
            m_Result = _mysql.m_Message;
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// 修改字段
        /// </summary>
        public void ModifyField(string FieldName, string FieldType)
        {
            _mysql.UpdateField("modify column ", dTableName, FieldName, FieldType);
            m_Result = _mysql.m_Message;
        }

        /// <summary>
        /// 移除字段
        /// </summary>
        public void DropField(string FieldName)
        {
            _mysql.UpdateField("drop ", dTableName, FieldName, "");
            m_Result = _mysql.m_Message;
        }

        /// <summary>
        /// 更新关联数据
        /// </summary>
        public bool UpdateData()
        {
            try
            {
                string tmpTable = "erp_community as a ,erp_region as b";
                string tmpValues = "a.district_id = b.district_id,a.biz_area_id = b.biz_area_id,a.biz_area_name = b.biz_area_name";
                string tmpWhere = "and a.fy_AreaId = b.erp_id";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);
                m_Result += "\n楼盘字典表中行政区ID|区域ID|区域名称更新成功";
                return true;
            }
            catch (Exception ex)
            {
                m_Result += "\n楼盘字典表中行政区ID|区域ID|区域名称更新异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
