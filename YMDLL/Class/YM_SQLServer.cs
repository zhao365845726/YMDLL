using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using YMDLL.Common;
using YMDLL.Interface;

namespace YMDLL.Class
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class YM_SQLServer : IDBProperty
    {
        #region ///私有字段集合(private)
        /// <summary>
        /// 连接字符串
        /// </summary>
        private string m_ConnectionString;

        /// <summary>
        /// SQL语句字符串
        /// </summary>
        private string m_SqlString;

        /// <summary>
        /// 读取数据库所有信息对象实例
        /// </summary>
        private SqlDataReader m_ReadResult;

        /// <summary>
        /// 返回错误信息
        /// </summary>
        private String m_Error;
        /// <summary>
        /// 字段数
        /// </summary>
        private int m_FieldCount;
        /// <summary>
        /// 行数
        /// </summary>
        private int m_RowCount;
        #endregion

        #region ///共有字段集合(public)
        /// <summary>
        /// 连接数据库对象实例
        /// </summary>
        public SqlConnection m_Conn;

        /// <summary>
        /// 执行SQL语言对象实例
        /// </summary>
        public SqlCommand m_Cmd;

        /// <summary>
        /// 执行SQL语言对象实例
        /// </summary>
        public SqlDataAdapter m_adapter;

        /// <summary>
        /// 执行SQL语言对象实例
        /// </summary>
        public DataSet m_DataSet;

        #endregion

        #region  ///共有属性集合(public)
        /// <summary>
        /// 连接字符串的属性
        /// </summary>
        public string ConnectString
        {
            get
            {
                return m_ConnectionString;
            }
            set
            {
                m_ConnectionString = value;
            }
        }

        /// <summary>
        /// 执行SQL字符串的属性
        /// </summary>
        public string SQLString
        {
            get
            {
                return m_SqlString;
            }
            set
            {
                m_SqlString = value;
            }
        }

        /// <summary>
        /// 返回读取数据的结果的属性
        /// </summary>
        public SqlDataReader ReadData
        {
            get
            {
                return m_ReadResult;
            }
            set
            {
                m_ReadResult = value;
            }
        }

        /// <summary>
        /// 字段数
        /// </summary>
        public int iFields
        {
            get
            {
                return m_FieldCount;
            }
            set
            {
                m_FieldCount = value;
            }
        }

        /// <summary>
        /// 行数
        /// </summary>
        public int iRows
        {
            get
            {
                return m_RowCount;
            }
            set
            {
                m_RowCount = value;
            }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return m_Error;
            }
            set
            {
                m_Error = value;
            }
        }
        #endregion

        #region///共有方法集合(public)
        /// <summary>
        /// 初始化连接字符串
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        public void dbInitialization(string server, string database)
        {
            ConnectString = "Data Source=" + server + ";Initial Catalog=" + database + ";Integrated Security=True";
        }

        /// <summary>
        /// 初始化连接字符串(+重载)
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <param name="userid"></param>
        /// <param name="pwd"></param>
        public void dbInitialization(string server, string database, string userid, string pwd)
        {
            ConnectString = "server=" + server + ";database=" + database + ";user id=" + userid + ";pwd=" + pwd;
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        public bool dbConnect()
        {
            try
            {
                /*容错性检查*/
                if (ConnectString == "")
                {
                    ErrorMessage = "没有要连接的数据库信息\n";
                    return false;
                }
                else
                {
                    ErrorMessage = "成功连接数据库";
                }

                m_Conn = new SqlConnection(ConnectString);
                //打开链接
                m_Conn.Open();
                return true;
            }
            catch (System.Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 断开数据库
        /// </summary>
        public void dbDisConnect()
        {
            try
            {
                //关闭数据库
                m_Conn.Close();
                //释放内存空间
                m_Conn.Dispose();
                ErrorMessage = "成功断开数据库";
            }
            catch (System.Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// 获取字段名称列表
        /// </summary>
        public String[] dbGetFieldList()
        {
            String[] m_FieldList = null;
            try
            {
                if (SQLString == "")
                {
                    ErrorMessage += "没有要执行的SQL语句\n";
                    return null;
                }

                m_Cmd = new SqlCommand(SQLString, m_Conn);
                ReadData = m_Cmd.ExecuteReader();

                if (iFields != -1 || iFields != 0 || m_FieldList != null)
                {
                    //如果字段数获取到了
                    m_FieldList = new String[iFields];
                    int i = 0;
                    while (i < iFields)
                    {
                        m_FieldList[i] = ReadData.GetName(i);
                        i++;
                    }
                    ErrorMessage = "成功获取字段列表";
                }
                else
                {
                    ErrorMessage = "dbGetFields()方法未调用，或者获取到的数据不存在,\n请检查后重新操作。";
                }
                ReadData.Close();
            }
            catch (System.Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return m_FieldList;
        }

        /// <summary>
        /// 获取字段名称 从0开始计数
        /// </summary>
        /// <param name="index">字段索引</param>
        public string dbGetFieldName(int index)
        {
            try
            {
                String strResult;
                if (SQLString == "")
                {
                    ErrorMessage += "没有要执行的SQL语句\n";
                    return null;
                }

                m_Cmd = new SqlCommand(SQLString, m_Conn);
                ReadData = m_Cmd.ExecuteReader();

                strResult = ReadData.GetName(index);
                ErrorMessage = "成功获取字段名";

                ReadData.Close();
                return strResult;
            }
            catch (System.Exception ex)
            {
                ErrorMessage = ex.Message;
                return ErrorMessage;
            }
        }

        /// <summary>
        /// 获取字段数据类型
        /// </summary>
        /// <param name="index">字段索引</param>
        public string dbGetFieldDataType(int index)
        {
            try
            {
                String strResult;
                if (SQLString == "")
                {
                    ErrorMessage += "没有要执行的SQL语句\n";
                    return null;
                }

                m_Cmd = new SqlCommand(SQLString, m_Conn);
                ReadData = m_Cmd.ExecuteReader();

                strResult = ReadData.GetDataTypeName(index);
                ErrorMessage = "成功获取字段数据类型";

                ReadData.Close();
                return strResult;
            }
            catch (System.Exception ex)
            {
                ErrorMessage = ex.Message;
                return ErrorMessage;
            }
        }

        /// <summary>
        /// 获取单行数据 从0开始计数
        /// </summary>
        /// <param name="index">指定行索引</param>
        /// 不能与GetCellData(int irow, int icol)同时使用，如果必须要在一起使用的话，
        /// 必须关闭数据库后再连接一次
        public string[] dbGetSigRowData(int index)
        {

            try
            {
                if (SQLString == "")
                {
                    ErrorMessage += "没有要执行的SQL语句\n";
                    return null;
                }

                m_Cmd = new SqlCommand(SQLString, m_Conn);
                ReadData = m_Cmd.ExecuteReader();

                String[] m_SigRowData = new String[iFields];
                int i = 0;
                //获取行数
                while (ReadData.Read() != false)
                {
                    if (i == index)
                    {
                        for (int j = 0; j < iFields; j++)
                        {
                            m_SigRowData[j] = ReadData[j].ToString();
                        }
                    }
                    i++;
                }
                ErrorMessage = "成功获取单行数据";
                ReadData.Close();
                return m_SigRowData;
            }
            catch (System.Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 获取指定单元格的内容
        /// </summary>
        /// <param name="irow">行数,从0开始计数</param>
        /// <param name="icol">列数,从0开始计数</param>
        public string dbGetCellData(int irow, int icol)
        {
            String strResult = "";
            try
            {
                dbConnect();
                if (SQLString == "")
                {
                    ErrorMessage += "没有要执行的SQL语句\n";
                    return null;
                }

                m_Cmd = new SqlCommand(SQLString, m_Conn);
                ReadData = m_Cmd.ExecuteReader();

                int i = 0;
                while (ReadData.Read() != false)
                {
                    if (i == irow)
                    {
                        for (int j = 0; j <= icol; j++)
                        {
                            if (j == icol)
                            {
                                strResult = ReadData[j].ToString();
                                ErrorMessage = "成功获取指定单元格(" + Convert.ToString(irow) + "," + Convert.ToString(icol) + ")的内容";
                            }
                        }
                    }
                    i++;
                }
                ReadData.Close();

                return strResult;
            }
            catch (System.Exception ex)
            {
                ErrorMessage = ex.Message;
                return ErrorMessage;
            }
            finally
            {
                dbDisConnect();
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="m_SQL">SQL语句</param>
        public bool dbExec(string m_SQL)
        {
            try
            {
                dbConnect();
                m_Cmd = new SqlCommand(m_SQL, m_Conn);
                iRows = m_Cmd.ExecuteNonQuery();
                return true;
            }
            catch (System.Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
            finally
            {
                dbDisConnect();
            }
        }

        /// <summary>
        /// 获取数据库中所有的数据
        /// </summary>
        public bool dbReadData()
        {
            try
            {
                if (SQLString == "")
                {
                    ErrorMessage += "没有要执行的SQL语句\n";
                    return false;
                }

                m_Cmd = new SqlCommand(SQLString, m_Conn);
                ReadData = m_Cmd.ExecuteReader();
                //获取字段数
                iFields = ReadData.FieldCount;
                //获取数据行数
                iRows = 0;
                //获取行数
                while (ReadData.Read() != false)
                {
                    iRows++;
                }

                ReadData.Close();
                ErrorMessage = "已经成功获取数据";
                return true;
            }
            catch (System.Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取数据库中所有的数据,返回供绑定的数据
        /// </summary>
        public DataSet dbReadData(String m_SQL)
        {
            m_DataSet = new DataSet();
            try
            {
                dbConnect();
                if (SQLString == "")
                {
                    ErrorMessage += "没有要执行的SQL语句\n";
                    return null;
                }
                m_adapter = new SqlDataAdapter(m_SQL, m_Conn);
                m_adapter.Fill(m_DataSet);
                //获取数据行数
                iRows = m_DataSet.Tables.Count;

                ErrorMessage = "已经成功获取数据";
                return m_DataSet;
            }
            catch (System.Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }
            finally
            {
                dbDisConnect();
            }
        }

        #endregion     


        #region ///分页方法(public)
        /// <summary>
        /// 获取分页数据（单表分页）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columns">要取的列名（逗号分开）</param>
        /// <param name="order">排序</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="where">查询条件</param>
        /// <param name="totalCount">总记录数</param>
        public DataTable GetPager(string tableName, string columns, string order, int pageSize, int pageIndex, string where, out int totalCount)
        {
            SqlParameter[] paras = {
                                   new SqlParameter("@tablename",SqlDbType.VarChar,100),
                                   new SqlParameter("@columns",SqlDbType.VarChar,5000),
                                   new SqlParameter("@order",SqlDbType.VarChar,100),
                                   new SqlParameter("@pageSize",SqlDbType.Int),
                                   new SqlParameter("@pageIndex",SqlDbType.Int),
                                   new SqlParameter("@where",SqlDbType.VarChar,2000),
                                   new SqlParameter("@totalCount",SqlDbType.Int)
                                   };
            paras[0].Value = tableName;
            paras[1].Value = columns;
            paras[2].Value = order;
            paras[3].Value = pageSize;
            paras[4].Value = pageIndex;
            paras[5].Value = where;
            paras[6].Direction = ParameterDirection.Output;   //输出参数
            paras[6].Value = 0;

            DataTable dt = SqlHelper.GetDataTable(ConnectString, CommandType.StoredProcedure, "sp_Pager", paras);
            totalCount = Convert.ToInt32(paras[6].Value);   //赋值输出参数，即当前记录总数
            return dt;
        }
        #endregion

        /// <summary>
        /// 更新表字段（包含添加|修改|删除）
        /// </summary>
        //public void dbUpdateField(string OperaType,string Table,string Column,string ColType)
        //{
        //    m_SqlString = "ALTER TABLE " + Table + " " + OperaType + " " + Column + " " + ColType;
        //    try
        //    {
        //        dbExec(m_SqlString);
        //        ErrorMessage += "\n更新字段成功";
        //    }
        //    catch(Exception ex)
        //    {
        //        ErrorMessage += "\n更新字段错误:" + ex.Message;
        //    }
        //}
    }
}
