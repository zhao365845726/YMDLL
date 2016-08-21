using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMDLL.Class;
using YMDLL.Common;

namespace AgencyEntity
{
    public class Estate : Base
    {
        public void Descript()
        {
            /* 1.房友表无 绿化率，容积率,摘要拼音缩写 默认填写了0
             * 2.房友表PropertyUsage 与 JAVA版ERP uses 数据类型不符，需要增加FY_uses字段作为标注(最后需要本地再update)
             * 3.添加FY_structure
             */
        }

        /// <summary>
        /// 楼盘字典类的构造函数
        /// </summary>
        public Estate()
        {
            sTableName = "Estate";
            sColumns = "EstateName,EstateID,AreaID,Spell,Address,Price,Square,MgtPrice,PropertyUsage,FrameWork,CompleteYear,OwnYear,Room,Address2,MgtCompany,MapID,CoX,CoY,DevCompany,ExDate,FlagLocked,Remark,RoomRule,BuildingRule,RoomRuleEx";
            //
            sOrder = "EstateName";
            dTableName = "b_house_dict";
            dColumns = "name,oldId,area,acronym,addres,afforest,averagePrice,cubage,grossArea,manageFee,FY_uses,FY_structure,years,equities,households,profileAddres,tenement,mapId,coX,coY,develop,acronzy,createDate,flagLocked,memo,roomRule,buildingRule,roomRuleEx";
            //
            //,parkSpace,roomRuleSample,roomRuleStr,schoolNum,plate";
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
                    string strRemark = "";
                    string strAddress = "";
                    string strAddress2 = "";
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
                    if (row["BuildingRule"].ToString().ToLower() == "false")
                    {
                        iBuildingRule = 0;
                    }
                    else
                    {
                        iBuildingRule = 1;
                    }
                    //过滤Address,Address2,Remark字段的'号
                    if (row["Address"].ToString().IndexOf("'") != -1 || row["Address"].ToString().IndexOf("’") != -1 || row["Address"].ToString().IndexOf("‘") != -1)
                    {
                        strAddress = row["Address"].ToString().Replace('\'', '"');
                    }
                    else
                    {
                        strAddress = row["Address"].ToString();
                    }
                    if (row["Address2"].ToString().IndexOf("'") != -1 || row["Address2"].ToString().IndexOf("’") != -1 || row["Address2"].ToString().IndexOf("‘") != -1)
                    {
                        strAddress2 = row["Address2"].ToString().Replace('\'', '"');
                    }
                    else
                    {
                        strAddress2 = row["Address2"].ToString();
                    }
                    if (row["Remark"].ToString().IndexOf("'") != -1 || row["Remark"].ToString().IndexOf("’") != -1 || row["Remark"].ToString().IndexOf("‘") != -1)
                    {
                        strRemark = row["Remark"].ToString().Replace('\'', '"');
                    }
                    else
                    {
                        strRemark = row["Remark"].ToString();
                    }
                    string strTemp = "'" + row["EstateName"].ToString() + "','" +
                        row["EstateID"].ToString() + "','" +
                        row["AreaID"].ToString() + "','" +
                        row["Spell"].ToString() + "','" +
                        strAddress + "',0," +
                        Convert.ToDecimal(row["Price"].ToString()) + ",0," +
                        Convert.ToDecimal(row["Square"].ToString()) + "," +
                        Convert.ToDecimal(row["MgtPrice"].ToString()) + ",'" +
                        row["PropertyUsage"].ToString() + "','" +
                        row["FrameWork"].ToString() + "','" +
                        row["CompleteYear"].ToString() + "','" +
                        row["OwnYear"].ToString() + "'," +
                        Convert.ToInt32(row["Room"].ToString()) + ",'" +
                        strAddress2 + "','" +
                        row["MgtCompany"].ToString() + "','" +
                        row["MapID"].ToString() + "','" +
                        row["CoX"].ToString() + "','" +
                        row["CoY"].ToString() + "','" +
                        row["DevCompany"].ToString() + "','','" +
                        Convert.ToDateTime(row["ExDate"].ToString()) + "'," +
                        Convert.ToInt32(iFlagLocked.ToString()) + ",'" +
                        strRemark + "'," +
                        Convert.ToInt32(row["RoomRule"].ToString()) + "," +
                        Convert.ToInt32(iBuildingRule.ToString()) +",'" +
                        row["RoomRuleEx"].ToString() + "'";

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
    }
}
