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
    public class Base
    {
        /// <summary>
        /// 连接SQL Server数据库的对象
        /// </summary>
        public YM_SQLServer _sqlServer;
        /// <summary>
        /// 连接mysql数据库的对象
        /// </summary>
        public YM_MySQL _mysql;
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
        /// 时间计算对象
        /// </summary>
        public CS_CalcDateTime _dateTime;
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
        /// 目标-公司Id
        /// </summary>
        public string dCompanyId;
        /// <summary>
        /// 目标-删除标识
        /// </summary>
        public string dDeleteMark;

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
            dCompanyId = "104";
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
            strResult = strValue.Replace("\\", "");
            return strResult;
        }
    }
}
