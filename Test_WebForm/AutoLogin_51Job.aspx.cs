using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YMDLL;
using YMDLL.Class.Web;
using YMDLL.Class;

namespace Test_WebForm
{
    public partial class AutoLogin_51Job : System.Web.UI.Page
    {
        public List<String> lStrValue = null;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void AutoGetTalentInfo()
        {
            String strUrl = "https://mylogin.51job.com/46586496056978054010/my/My_Pmc.php";
            String strAccount = "zhao365845726";
            String strPassword = "mz19881023";
            YM_WebAutoLogin.PostLogin(strUrl, strAccount, strPassword);
            YM_WebAutoLogin.GetPage();
            txtContext.Text = YM_WebAutoLogin.ResultHtml;
            //richTextBox1.Text = YM_WebAutoLogin.ResultHtml;
        }

        protected void btnGetData_Click(object sender, EventArgs e)
        {
            
            AutoGetTalentInfo();
            String strJLUrl = GetUrl(txtContext.Text, "简历管理");
            lStrValue.Add(YM_WebAutoLogin.GetPage(strJLUrl));

        }

        /// <summary>
        /// 获取简历管理URL链接
        /// </summary>
        /// <param name="m_Context">上下文</param>
        /// <param name="m_Keyword">搜索的关键字</param>
        public string GetUrl(string m_Context, string m_Keyword)
        {
            List<string> lStr = new List<string>();
            int iStart = 0;
            int iEnd = 0;
            if (m_Context.IndexOf(m_Keyword) != -1)
            {
                iStart = m_Context.IndexOf("<div class=\"main\">");
                lStr.Add(m_Context.Substring(iStart, m_Context.Length - iStart));
                iEnd = lStr[0].IndexOf(m_Keyword);
                lStr.Add(lStr[0].Substring(0, iEnd));
                iStart = lStr[1].LastIndexOf("href=\"");
                lStr.Add(lStr[1].Substring(iStart + 6, lStr[1].Length - iStart - 6));
                iEnd = lStr[2].IndexOf("\"");
                lStr.Add(lStr[2].Substring(0, iEnd));
                return lStr[3];
            }
            else
            {
                throw new System.NotImplementedException();
            }
        }
    }
}