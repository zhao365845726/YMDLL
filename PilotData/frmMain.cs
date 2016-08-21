using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using MySql.Data.MySqlClient;
using YMDLL.Class;

namespace PilotData
{
    public partial class frmMain : Form
    {
        public YM_SQLServer sqlserver = new YM_SQLServer();
        public YM_MySQL mysql = new YM_MySQL();

        //主窗口构造函数
        public frmMain()
        {
            InitializeComponent();
            InitComponentValue();
        }

        /// <summary>
        /// 初始组件的值
        /// </summary>
        public void InitComponentValue()
        {
            //源数据库值配置
            comboSourceType.SelectedIndex = Convert.ToInt32(ReadAppSetting("SourceType"));
            comboSourceName.Text = ReadAppSetting("SourceName");
            comboSourcePort.SelectedIndex = Convert.ToInt32(ReadAppSetting("SourcePort"));
            comboSourceUsername.Text = ReadAppSetting("SourceUsername");
            txtSourcePwd.Text = ReadAppSetting("SourcePassword");
            lblSourceUsername.Visible = false;
            comboSourceUsername.Visible = false;
            lblSourcePwd.Visible = false;
            txtSourcePwd.Visible = false;

            //目标数据库配置
            comboDestType.SelectedIndex = Convert.ToInt32(ReadAppSetting("DestType"));
            comboDestName.Text = ReadAppSetting("DestName");
            comboDestPort.Text = ReadAppSetting("DestPort");
            comboDestUsername.Text = ReadAppSetting("DestUsername");
            txtDestPwd.Text = ReadAppSetting("DestPassword");
        }

        /// <summary>
        /// 切换身份验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboSourcePort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboSourcePort.SelectedIndex == 1)
            {
                lblSourceUsername.Visible = true;
                lblSourcePwd.Visible = true;
                comboSourceUsername.Visible = true;
                txtSourcePwd.Visible = true;
            }
            else
            {
                lblSourceUsername.Visible = false;
                lblSourcePwd.Visible = false;
                comboSourceUsername.Visible = false;
                txtSourcePwd.Visible = false;
            }
        }

        /// <summary>
        /// 源数据库连接测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConTestSource_Click(object sender, EventArgs e)
        {
            string sourceServer = comboSourceName.Text;
            string sourceDatabase = ReadAppSetting("SourceDB");
            
            if (comboSourcePort.SelectedIndex == 1)
            {
                string sourceUsername = comboSourceUsername.Text;
                string sourcePwd = txtSourcePwd.Text;
                sqlserver.dbInitialization(sourceServer, sourceDatabase, sourceUsername, sourcePwd);
            }
            else
            {
                sqlserver.dbInitialization(sourceServer, sourceDatabase);
            }
            
            bool isConn = sqlserver.dbConnect();
            if (isConn)
            {
                WriteLog("源数据库连接成功.");
            }
            else
            {
                WriteLog("源数据库连接失败.");
            }
            sqlserver.dbDisConnect();
        }

        /// <summary>
        /// 输出日志信息
        /// </summary>
        /// <param name="m_Content"></param>
        private void WriteLog(string m_Content)
        {
            rtbConsole.ForeColor = Color.ForestGreen;
            rtbConsole.Text = m_Content + "\n" + rtbConsole.Text;
        }

        /// <summary>
        /// 目标数据库连接测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConTestDest_Click(object sender, EventArgs e)
        {
            string mysqlserver = comboDestName.Text;
            string mysqlport = comboDestPort.Text;
            string mysqldatabase = ReadAppSetting("DestDB");
            string mysqlusername = comboDestUsername.Text;
            string mysqlpwd = txtDestPwd.Text;

            
            mysql.Initialize(mysqlserver, mysqldatabase, mysqlusername, mysqlpwd, mysqlport);
            bool isResult = mysql.OpenConnection();
            if (isResult)
            {
                WriteLog("目标数据库连接成功.");
            }else{
                WriteLog("目标数据库连接失败.");
            }
            mysql.CloseConnection();
        }

        /// <summary>
        /// 清空控制台信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 清空控制台ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbConsole.Text = "";
        }

        /// <summary>
        /// 读取设置
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        private string ReadAppSetting(string strKey)
        {
            return ConfigurationSettings.AppSettings[strKey].ToString();
        }

        /// <summary>
        /// 开始执行导数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            import import = new import();
            //import.ExecCommand((TableType)Enum.Parse(typeof(TableType), ReadAppSetting("EnumType"), false));
            import.ExecCommand(TableType.PROPERTY);
            WriteLog(import.strResult);
        } 
    }
}
