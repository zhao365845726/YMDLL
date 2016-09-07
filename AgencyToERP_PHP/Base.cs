using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMDLL.Class;
using YMDLL.Common;
using YMDLL.Interface;

namespace AgencyToERP_PHP
{
    public class Base
    {
        #region ----变量声明----
        /// <summary>
        /// 连接SQL Server数据库的对象
        /// </summary>
        public YM_SQLServer _sqlServer;
        /// <summary>
        /// 连接mysql数据库的对象
        /// </summary>
        public YM_MySQL _mysql;
        /// <summary>
        /// 时间计算对象
        /// </summary>
        public CS_CalcDateTime _dateTime;
        /// <summary>
        /// 源-表名
        /// </summary>
        public string sTableName;
        /// <summary>
        /// 源-表的列
        /// </summary>
        public string sColumns;
        /// <summary>
        /// 源-表排序的字段
        /// </summary>
        public string sOrder;
        /// <summary>
        /// 源-where语句
        /// </summary>
        public string sWhere;
        /// <summary>
        /// 源-页码
        /// </summary>
        public int sPageIndex;
        /// <summary>
        /// 源-页面大小
        /// </summary>
        public int sPageSize;
        /// <summary>
        /// 源-输出参数-获取的表数据的总结果集数量
        /// </summary>
        public int sTotalCount;
        /// <summary>
        /// 目标-表的列
        /// </summary>
        public string dColumns;
        /// <summary>
        /// 目标-表排序的字段
        /// </summary>
        public string dOrder;
        /// <summary>
        /// 目标-页码
        /// </summary>
        public int dPageIndex;
        /// <summary>
        /// 目标-页面大小
        /// </summary>
        public int dPageSize;
        /// <summary>
        /// 目标-表名
        /// </summary>
        public string dTableName;
        /// <summary>
        /// 目标-表描述
        /// </summary>
        public string dTableDescript;
        /// <summary>
        /// 目标-导入内容描述
        /// </summary>
        public string dPolitContentDescript;
        /// <summary>
        /// 目标-获取的表数据的总结果集数量
        /// </summary>
        public int dTotalCount;
        /// <summary>
        /// 目标-where语句
        /// </summary>
        public string dWhere;
        /// <summary>
        /// 返回的结果
        /// </summary>
        public string m_Result;
        /// <summary>
        /// 源-总行数
        /// </summary>
        public int sRows;
        /// <summary>
        /// 目标-是否删除表数据
        /// </summary>
        public bool dIsDelete;
        /// <summary>
        /// 表名
        /// </summary>
        public string _TableName = "";
        /// <summary>
        /// 线程启用标识
        /// </summary>
        public bool m_ThreadEnabled;
        /// <summary>
        /// 目标-字段添加
        /// </summary>
        public string dFieldAdd;
        /// <summary>
        /// 目标-字段修改
        /// </summary>
        public string dFieldModify;
        /// <summary>
        /// 目标-字段移除
        /// </summary>
        public string dFieldDrop;
        /// <summary>
        /// 目标-更新数据
        /// </summary>
        public string dUpdateData;
        /// <summary>
        /// 目标-执行
        /// </summary>
        public string dExec;
        /// <summary>
        /// 目标-公司Id
        /// </summary>
        public string dCompanyId;
        /// <summary>
        /// 目标-删除标识
        /// </summary>
        public string dDeleteMark;
        #endregion


        /// <summary>
        /// 基类的构造函数
        /// </summary>
        public Base()
        {
            _sqlServer = new YM_SQLServer();
            _mysql = new YM_MySQL();
            _sqlServer.dbInitialization(ReadAppSetting("SourceName"), ReadAppSetting("SourceDB"));
            _mysql.Initialize(ReadAppSetting("DestName"), ReadAppSetting("DestDB"), ReadAppSetting("DestUsername"), ReadAppSetting("DestPassword"), ReadAppSetting("DestPort"));
            sWhere = "1=1";
            sPageIndex = 1;
            sPageSize = 2500;
            sTotalCount = 0;
            dWhere = "1=1";
            dPageIndex = 1;
            dPageSize = 2500;
            dTotalCount = 0;
            dCompanyId = "999";
            dDeleteMark = "0";
            m_ThreadEnabled = false;
            _dateTime = new CS_CalcDateTime();
        }

        /// <summary>
        /// 基类的析构函数
        /// </summary>
        ~Base()
        {
            _sqlServer = null;
            _mysql = null;
            sTableName = null;
            sColumns = null;
            sOrder = null;
            sWhere = null;
            sPageIndex = 0;
            sPageSize = 0;
            sTotalCount = 0;
            dTableName = null;
            dColumns = null;
            dOrder = null;
            dWhere = null;
            dPageIndex = 0;
            dPageSize = 0;
            dTotalCount = 0;
            m_ThreadEnabled = false;
            _dateTime = null;
        }

        /// <summary>
        /// 读取配置文件中的数据
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public string ReadAppSetting(string strKey)
        {
            return ConfigurationSettings.AppSettings[strKey].ToString();
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        public void ImportData()
        {
            if (sPageIndex > 1)
            {
                dIsDelete = false;
            }

            ReadPagerData();

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
                    ReadPagerData();
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
        public bool ReadPagerData()
        {
            try
            {
                DataTable dt = _sqlServer.GetPager(sTableName, sColumns, sOrder, sPageSize, sPageIndex, sWhere, out sTotalCount);

                List<String> lstValue = new List<String>();
                foreach (DataRow row in dt.Rows)
                {
                    string strTemp = "'" + row["InquiryID"].ToString() + "'";
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
                    m_Result = "\n" + _TableName + "数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "\n" + _TableName + "数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出" + _TableName + "异常.\n异常原因：" + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <returns></returns>
        public string FilterSpecialCharacter(string strValue)
        {
            //去除俩边多余的空格
            string strResult = strValue.Trim();
            //去掉文字中包含的单引号
            strResult = strValue.Replace("'", "");
            //去掉文字中包含的反斜杠符号
            strResult = strResult.Replace("\\", "");
            return strResult;
        }

        /// <summary>
        /// 组合房友数据的值【导入的目标是mysql数据库的时候】
        /// </summary>
        /// Dictionary<DestField, SourceField>
        /// Dictionary<DestField, SourceField:DateTime>
        /// Dictionary<DestField, SourceField:String?Default=Value>
        /// Dictionary<DestField, SourceField:Array?Key1=Value1;Key2=Value2;Key3=Value3>
        /// <returns></returns>
        public string GetConcatValues(Dictionary<string, string> dicData, DataRow drRow)
        {
            string strResult = "";
            string[] saItem;    //是否有数据类型跟随
            string[] strItemOne,strItemTwo,strItemThree;   //是字符串的时候是否有默认值

            foreach (KeyValuePair<string, string> dicItem in dicData)
            {
                if(dicItem.Value.IndexOf(":") == -1)
                {
                    strResult += "'" + FilterSpecialCharacter(drRow[dicItem.Value].ToString().Trim()) + "',";
                }
                else
                {
                    //判断如果值里面的格式是 XXXX:DATETIME 的时候
                    saItem = dicItem.Value.Split(':');
                    if(saItem[1].ToUpper() == "DATETIME" || saItem[1].ToUpper() == "DATE" || saItem[1].ToUpper() == "TIME")
                    {
                        strResult += "'" + _dateTime.DateTimeToStamp(drRow[saItem[0]].ToString().Trim()).ToString() + "',";
                    }else if(saItem[1].IndexOf('?') != -1)
                    {
                        strItemOne = saItem[1].Split('?');
                        if(strItemOne[0].ToString().Trim().ToUpper() == "STRING")
                        {
                            //判断是字符串的
                            if (strItemOne[1].ToString().IndexOf('=') != -1)
                            {
                                //判断字符串中有Default或者其他值
                                strItemTwo = strItemOne[1].ToString().Trim().Split('=');
                                //string strTemp = drRow[saItem[0]].ToString();
                                if (strItemTwo[0].ToString().Trim().ToUpper() == "DEFAULT")
                                {
                                    //增加内容为默认值
                                    strResult += "'" + strItemTwo[1].ToString().Trim() + "',";
                                }else if (strItemTwo[0].ToString().Trim().ToUpper() == "" && drRow[saItem[0]].ToString().Trim() == "")
                                {
                                    //判断源字段存储的值为空，默认参数的key为空的时候走默认值
                                    strResult += "'" + strItemTwo[1].ToString().Trim() + "',";
                                }
                                else
                                {
                                    strResult += "'" + FilterSpecialCharacter(drRow[saItem[0]].ToString().Trim()) + "',";
                                }
                            }
                        }
                        else if(strItemOne[0].ToString().Trim().ToUpper() == "ARRAY")
                        {

                        }
                    }
                }
            }
            strResult = strResult.Substring(0, strResult.Length - 1);
            return strResult;
        }

        /// <summary>
        /// 合并Key字段(合并目标数据库字段)
        /// </summary>
        /// <param name="dicData"></param>
        /// <returns></returns>
        public string CombineDestField(Dictionary<string,string> dicData)
        {
            string strResult = "";
            foreach(KeyValuePair<string,string> dicItem in dicData)
            {
                strResult += dicItem.Key + ",";
            }
            strResult = strResult.Substring(0, strResult.Length - 1);
            return strResult;
        }

        /// <summary>
        /// 合并Value字段(合并源数据库字段)
        /// </summary>
        /// <param name="dicData"></param>
        /// <returns></returns>
        public string CombineSourceField(Dictionary<string, string> dicData)
        {
            string strResult = "";
            string[] saField;
            foreach (KeyValuePair<string, string> dicItem in dicData)
            {
                if(dicItem.Value.IndexOf(":") == -1)
                {
                    strResult += dicItem.Value + ",";
                }
                else
                {
                    saField = dicItem.Value.Split(':');
                    if(saField[0].ToString().Trim() == "")
                    {
                        strResult += "";
                    }else
                    {
                        strResult += saField[0] + ",";
                    }
                }
            }
            strResult = strResult.Substring(0, strResult.Length - 1);
            return strResult;
        }

        #region ----字段操作区域----
        /// <summary>
        /// 添加字段
        /// </summary>
        public void AddField(string FieldName, string FieldType)
        {
            _mysql.UpdateField("add", dTableName, FieldName, FieldType);
            m_Result = _mysql.m_Message;
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
        #endregion

        #region ----索引操作区域----
        /// <summary>
        /// 新增表字段索引
        /// </summary>
        public bool AddIndex(List<string> lstField,MysqlIndexType type)
        {
            switch (type)
            {
                case MysqlIndexType.INDEX:

                    break;
                case MysqlIndexType.UNIQUE:

                    break;
                case MysqlIndexType.PRIMARYKEY:

                    break;
            }
            return true;
        }

        /// <summary>
        /// 修改表字段索引
        /// </summary>
        public bool ModifyIndex(List<string> lstField,MysqlIndexType type)
        {
            switch (type)
            {
                case MysqlIndexType.INDEX:

                    break;
                case MysqlIndexType.UNIQUE:

                    break;
                case MysqlIndexType.PRIMARYKEY:

                    break;
            }
            return true;
        }

        /// <summary>
        /// 移除表字段索引
        /// </summary>
        public bool DropIndex(List<string> lstField, MysqlIndexType type)
        {
            switch (type)
            {
                case MysqlIndexType.INDEX:

                    break;
                case MysqlIndexType.UNIQUE:

                    break;
                case MysqlIndexType.PRIMARYKEY:

                    break;
            }
            return true;
        }
        #endregion
    }
}
