using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
//Add MySql Library  
using MySql.Data.MySqlClient;
using System.Data;
using YMDLL.Interface; 

namespace YMDLL.Class
{
    public class YM_MySQL : IDBProperty
    {
        #region MyRegion

        #endregion
        /// <summary>
        /// mysql连接对象
        /// </summary>
        private MySqlConnection connection;
        /// <summary>
        /// 数据库ip
        /// </summary>
        private string server;
        /// <summary>
        /// 数据库
        /// </summary>
        private string database;
        /// <summary>
        /// 数据库用户
        /// </summary>
        private string uid;
        /// <summary>
        /// 数据库密码
        /// </summary>
        private string password;
        /// <summary>
        /// 端口
        /// </summary>
        private string port;
        /// <summary>
        /// 执行的sql语句
        /// </summary>
        public string m_sql;
        /// <summary>
        /// 执行的信息
        /// </summary>
        public string m_Message;

        //private string database;  

        /// <summary>
        /// 初始化值
        /// </summary>
        public void Initialize(string server, string database, string uid, string password, string port)
        {
            //server = "localhost";  
            //database = "connectcsharptomysql";  
            //uid = "username";  
            //password = "password";  
            this.server = server;
            this.uid = uid;
            this.password = password;
            this.port = port;
            this.database = database;
            string connectionString = "Data Source=" + server + ";" + "port=" + port + ";" + "Database=" + database + ";" + "User Id=" + uid + ";" + "Password=" + password + ";" + "CharSet = utf8"; ;
            connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based on the error number.  
                //The two most common error numbers when connecting are as follows:  
                //0: Cannot connect to server.  
                //1045: Invalid user name and/or password.  
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                connection.Dispose();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //public DataTable GetSchema(string str, string[] restri)
        //{
        //    return connection.GetSchema(str, restri);
        //}
        //public DataTable GetSchema(string str)
        //{
        //    return connection.GetSchema(str);
        //}
        // Get Database List  

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columns">列名集合</param>
        /// <param name="value">插入的值</param>
        public void Insert(string tableName,string columns,string value)
        {
            string query = "INSERT INTO " + tableName + " (" + columns + ") VALUES(" + value + ")";

            //open connection  
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor  
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Execute command  
                cmd.ExecuteNonQuery();
                //close connection  
                this.CloseConnection();
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="setValues">列和值：key-value形式</param>
        /// <param name="where">where语句</param>
        public void Update(string tableName, string setValues,string where)
        {
            string query = "UPDATE " + tableName + " SET " + setValues + " WHERE 1=1 " + where;

            //Open connection  
            if (this.OpenConnection() == true)
            {
                //create mysql command  
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText  
                cmd.CommandText = query;
                //Assign the connection using Connection  
                cmd.Connection = connection;

                //Execute query  
                cmd.ExecuteNonQuery();

                //close connection  
                this.CloseConnection();
            }
        }

        /// <summary>
        /// 更新数据不关闭连接
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="setValues">列和值：key-value形式</param>
        /// <param name="where">where语句</param>
        public void UpdateNoClose(string tableName, string setValues, string where)
        {
            string query = "UPDATE " + tableName + " SET " + setValues + " WHERE 1=1 " + where;

            //create mysql command  
            MySqlCommand cmd = new MySqlCommand();
            //Assign the query using CommandText  
            cmd.CommandText = query;
            //Assign the connection using Connection  
            cmd.Connection = connection;

            //Execute query  
            cmd.ExecuteNonQuery();

        }

        /// <summary>
        /// 更新数据使用SQL语句
        /// </summary>
        public void Update(string sql)
        {
            //Open connection  
            if (this.OpenConnection() == true)
            {
                //create mysql command  
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText  
                cmd.CommandText = sql;
                //Assign the connection using Connection  
                cmd.Connection = connection;

                //Execute query  
                cmd.ExecuteNonQuery();

                //close connection  
                this.CloseConnection();
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        public void Delete(string tableName,string where)
        {
            string query = "DELETE FROM " + tableName + " WHERE 1=1 " + where;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        /// <summary>
        /// 查询数据表中单个字段，并返回列表
        /// </summary>
        /// <param name="tableName">表名</param>
        public MySqlDataReader SelectMul(string tableName, string colName, string sWhere)
        {
            string query = "SELECT " + colName + " FROM " + tableName + " WHERE 1=1 ";
            //OpenConnection();
            if(this.OpenConnection() == true)
            {
                MySqlCommand mysqlcom = new MySqlCommand(query, connection);
                MySqlDataReader mysqlread = mysqlcom.ExecuteReader();
                //this.CloseConnection();
                return mysqlread;
            }
            return null;            
        }

        /// <summary>
        /// 查询数据表中单个字段，并返回列表
        /// </summary>
        /// <param name="tableName">表名</param>
        public List<string>[] Select(string tableName,string colName,string sWhere)
        {
            string query = "SELECT " + colName + " FROM " + tableName + " WHERE 1=1 " + sWhere;

            //Create a list to store the result  
            List<string>[] list = new List<string>[1];
            list[0] = new List<string>();

            //Open connection  
            if (this.OpenConnection() == true)
            {
                //Create Command  
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command  
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list  
                while (dataReader.Read())
                {
                    list[0].Add(dataReader[colName] + "");
                }
                //close Data Reader  
                dataReader.Close();
                //close Connection  
                this.CloseConnection();
                //return list to be displayed  
                return list;
            }
            else
            {
                return list;
            }
        }

        /// <summary>
        /// 查询数据表中单个字段，并返回列表(未完待续..)
        /// </summary>
        /// <param name="tableName">表名</param>
        public List<string>[] SelectCell(string tableName, string colName, string sWhere)
        {
            string query = "SELECT " + colName + " FROM " + tableName + " WHERE 1=1 ";

            //Create a list to store the result  
            List<string>[] list = new List<string>[1];
            list[0] = new List<string>();

            //Open connection  
            if (this.OpenConnection() == true)
            {
                //Create Command  
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command  
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list  
                while (dataReader.Read())
                {
                    list[0].Add(dataReader[colName] + "");
                }
                //close Data Reader  
                dataReader.Close();
                //close Connection  
                this.CloseConnection();
                //return list to be displayed  
                return list;
            }
            else
            {
                return list;
            }
        }

        /// <summary>
        /// 查询的结果集的总行数
        /// </summary>
        /// <param name="tableName">表名</param>
        public int Count(string tableName)
        {
            string query = "SELECT Count(*) FROM " + tableName;
            int Count = -1;
            //Open Connection  
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command  
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value  
                Count = int.Parse(cmd.ExecuteScalar() + "");
                //close Connection  
                this.CloseConnection();
                return Count;
            }
            else
            {
                return Count;
            }
        }

        /// <summary>
        /// 备份数据
        /// </summary>
        public void Backup()
        {
            try
            {
                DateTime Time = DateTime.Now;
                int year = Time.Year;
                int month = Time.Month;
                int day = Time.Day;
                int hour = Time.Hour;
                int minute = Time.Minute;
                int second = Time.Second;
                int millisecond = Time.Millisecond;

                //Save file to C:\ with the current date as a filename  
                string path;
                path = "C:\\" + year + "-" + month + "-" + day + "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
                StreamWriter file = new StreamWriter(path);


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysqldump";
                psi.RedirectStandardInput = false;
                psi.RedirectStandardOutput = true;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", uid, password, server, database);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);

                string output;
                output = process.StandardOutput.ReadToEnd();
                file.WriteLine(output);
                process.WaitForExit();
                file.Close();
                process.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error , unable to backup!");
            }
        }

        /// <summary>
        /// 恢复数据
        /// </summary>
        public void Restore()
        {
            try
            {
                //Read file from C:\  
                string path;
                path = "C:\\MySqlBackup.sql";
                StreamReader file = new StreamReader(path);
                string input = file.ReadToEnd();
                file.Close();


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysql";
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = false;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", uid, password, server, database);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);
                process.StandardInput.WriteLine(input);
                process.StandardInput.Close();
                process.WaitForExit();
                process.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error , unable to Restore!");
            }
        }

        /// <summary>
        /// 执行SQL文件
        /// </summary>
        public void ExecuteSQLFile(string fileName)
        {
            string sql = File.ReadAllText(fileName, Encoding.UTF8);
            MySqlCommand myCommand = new MySqlCommand(sql);
            myCommand.Connection = connection;
            if (this.OpenConnection() == true)
            {
                myCommand.ExecuteNonQuery();
                //MessageBox.Show("..........");  
                this.CloseConnection();
            }


        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columns">列名集合</param>
        /// <param name="lstValue">数据集合</param>
        public bool BatchInsert(string tableName,string columns,List<String> lstValue)
        {
            string batchValue = "";
            foreach(string value in lstValue){
                batchValue += "(" + value + "),";
            }
            //去掉结尾的,号
            batchValue = batchValue.Substring(0, batchValue.Length - 1);

            string query = "INSERT INTO " + tableName + " (" + columns + ") VALUES " + batchValue + "";

            //open connection  
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor  
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Execute command  
                cmd.ExecuteNonQuery();
                //close connection  
                this.CloseConnection();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新表字段（包含添加|修改|删除）
        /// </summary>
        /// <param name="OperaType">操作类型（add|modify column|drop）</param>
        /// <param name="Table">表名</param>
        /// <param name="Column">字段名</param>
        /// <param name="ColType">字段类型</param>
        public void UpdateField(string OperaType, string Table, string Column, string ColType)
        {
            m_sql = "ALTER TABLE " + Table + " " + OperaType + " " + Column + " " + ColType;
            try
            {
                ExecuteSQL(m_sql);
                if (OperaType.Trim().Equals("add"))
                {
                    m_Message += "\n添加字段成功";
                }
                else if(OperaType.Trim().Equals("modify column"))
                {
                    m_Message += "\n更新字段成功";
                }
                else if (OperaType.Trim().Equals("drop"))
                {
                    m_Message += "\n删除字段成功";
                }
            }
            catch (Exception ex)
            {
                m_Message += "\n操作字段错误:" + ex.Message;
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        public void ExecuteSQL(string sql)
        {
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command  
                MySqlCommand cmd = new MySqlCommand(sql, connection);

                cmd.ExecuteNonQuery();
                cmd.Dispose();

                //close Connection  
                this.CloseConnection();
            }
            else
            {
                return;
            }
        }
    }
}
