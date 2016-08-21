using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Net;

//截取网页信息
namespace InterceptWebInfo
{
    public class Cutout
    {
        /// <summary>
        /// 网页URL
        /// </summary>
        private string m_WebUrl;
        /// <summary>
        /// 网页状态
        /// </summary>
        private int m_GetUrlState = 0;

        /// <summary>
        /// 获取URL
        /// </summary>
        public string GetUrl
        {
            get
            {
                return m_WebUrl;
            }
            set
            {
                m_WebUrl = value;
            }
        }

        /// <summary>
        /// 获取URL状态 
        /// 1.第一种规则 2.第二种规则 3.第三种规则 4.第四种规则 
        /// </summary>
        public int GetUrlState
        {
            get
            {
                return m_GetUrlState;
            }
            set
            {
                m_GetUrlState = value;
            }
        }
        /// <summary>
        /// 取出俩个字符间的字符串信息
        /// </summary>
        public string CO_MarginStr(string m_Source, string m_First, string m_Second)
        {
            //取出俩个字符间的字符串信息
            int ipos0, ipos1;
            String strResult;
            try
            {
                ipos0 = m_Source.IndexOf(m_First);
                ipos1 = m_Source.IndexOf(m_Second, ipos0);
                strResult = m_Source.Substring(ipos0 + m_First.Length, ipos1 - ipos0 - m_First.Length);
                return strResult;
            }
            catch (Exception ex)
            {
                return ex.Message;     
            }
        }

        /// <summary>
        /// 找出指定字符的数量
        /// </summary>
        public int CO_AppointCharCount(string m_Source,string m_Char)
        {
            int icount = 0;
            bool isExist = true;
            int ipos0,ipos1;
            String strTemp;
            while (isExist == true)
            {
                ipos0 = m_Source.IndexOf(m_Char);
                if (ipos0 == -1)
                {
                    isExist = false;
                }
                else
                {
                    strTemp = m_Source.Substring(ipos0 + 1, m_Source.Length - ipos0 - 1);
                    m_Source = strTemp;
                    icount++;
                }
            } 
            //返回指定字符的数量
            return icount;
        }

        /// <summary>
        /// 搜房小区网截取信息
        /// </summary>
        public string CO_SouFunArea(string m_Source,string m_Split)
        {
            /*
             * 范例网址：http://yingzhihejinchangsushe.soufun.com/ 
             */
            String strTemp0, strTemp1, strTemp2, strTemp3, strTemp4, strTemp5, strTemp6, strTemp7, strTemp8, strTemp9, strTemp10;
            String strTemp11, strTemp12, strTemp13,strTemp14,strTemp15;
            Cutout co = new Cutout();
            strTemp0 = co.CO_MarginStr(m_Source, "<!--" + m_Split + " begin-->", "<!--" + m_Split + " end-->");
            //去除本段合作编辑者的内容
            if (strTemp0.IndexOf("本段合作编辑者：")!=-1)
            {
                strTemp14 = strTemp0.Substring(0, strTemp0.IndexOf("本段合作编辑者："));
                //如果找不到</dl>标记就自动添加标记
                if (strTemp14.IndexOf("</dl>") == -1)
                {
                    strTemp14 += "</dl>";
                }
            }
            else
            {
                strTemp14 = strTemp0;
            }
            
            strTemp1 = co.CO_MarginStr(strTemp14, "<dl", "</dl>");
            if (strTemp1.IndexOf("<strong>")!=-1)
            {
                strTemp2 = strTemp1.Replace("<strong>", "");
            }
            else
            {
                strTemp2 = strTemp1;
            }
            if (strTemp2.IndexOf("</strong>")!=-1)
            {
                strTemp3 = strTemp2.Replace("</strong>","");
            }
            else
            {
                strTemp3 = strTemp2;
            }
            if (strTemp3.IndexOf("</dt>")!=-1)
            {
                strTemp4 = strTemp3.Replace("</dt>", "");
            }
            else
            {
                strTemp4 = strTemp3;
            }
            if (strTemp4.IndexOf("</dd>")!=-1)
            {
                strTemp5 = strTemp4.Replace("</dd>", "");
            }
            else
            {
                strTemp5 = strTemp4;
            }
            if (strTemp5.IndexOf("<dd>")!=-1)
            {
                strTemp6 = strTemp5.Replace("<dd>", "=");
            }
            else
            {
                strTemp6 = strTemp5;
            }
            if (strTemp6.IndexOf("&nbsp;")!=-1)
            {
                strTemp7 = strTemp6.Replace("&nbsp;", "");
            }
            else
            {
                strTemp7 = strTemp6;
            }
            if (strTemp7.IndexOf("<dt>")!=-1)
            {
                strTemp8 = strTemp7.Replace("<dt>", "=");
            }
            else
            {
                strTemp8 = strTemp7;
            }
            if (strTemp8.IndexOf(">")!=-1)
            {
                strTemp9 = strTemp8.Substring(strTemp8.IndexOf(">") + 1, strTemp8.Length - strTemp8.IndexOf(">") - 1);
            }
            else
            {
                strTemp9 = strTemp8;
            }
            if (strTemp9.IndexOf(" ")!=-1)
            {
                strTemp10 = strTemp9.Replace(" ", "");
            }
            else
            {
                strTemp10 = strTemp9;
            }
            //去除制表符
            if (strTemp10.IndexOf("\t")!=-1)
            {
                strTemp11 = strTemp10.Replace("\t", "");
            }
            else
            {
                strTemp11 = strTemp10;
            }
            if (strTemp11.IndexOf("\n")!=-1)
            {
                strTemp12 = strTemp11.Replace("\n", "");
            }
            else
            {
                strTemp12 = strTemp11;   
            }
            //去除换行符
            if (strTemp12.IndexOf("\r") != -1)
            {
                strTemp13 = strTemp12.Replace("\r", "");
            }
            else
            {
                strTemp13 = strTemp12;
            }
            //<br>保留
            strTemp14 = CO_EliminateWord(strTemp13, "<", ">");
            return strTemp14;
        }

        /// <summary>
        /// 搜房小区网截取信息2
        /// </summary>
        public string CO_SouFunArea2(string m_Source,string m_Split)
        {
            /*
             * 范例网址：http://esf.nc.soufun.com/housing/2311093170.htm 
             */
            String strTemp0, strTemp1, strTemp2, strTemp3, strTemp4, strTemp5, strTemp6, strTemp7, strTemp8, strTemp9, strTemp10;
            String strTemp11, strTemp12, strTemp13, strTemp14, strTemp15,strTemp16,strTemp17,strTemp18,strTemp19,strTemp20;
            String strStart, strEnd;
            int ipos0, ipos1, ipos2, ipos3;
            int iTemp;
            Cutout co = new Cutout();
            strTemp0 = co.CO_MarginStr(m_Source, "<!--楼盘详细信息 begin-->", "<!--楼盘详细信息 end-->") + "dli015";
            if (strTemp0.IndexOf(m_Split)==-1)
            {
                strTemp1 = "找不到指定信息";
            }
            else
            {
                strTemp1 = strTemp0.Substring(0, strTemp0.IndexOf(m_Split));
            }
            ipos0 = strTemp1.LastIndexOf("id");
            strTemp2 = strTemp1.Substring(ipos0 + 1, strTemp1.Length - ipos0 - 1);
            //截取代号
            strTemp3 = co.CO_MarginStr(strTemp2, "=\"", "\">");
            strTemp4 = strTemp3.Substring(3, 2);
            //截取m_Split所指定的信息内容
            iTemp = Convert.ToInt32(strTemp4) + 1;
            strStart = "dli0" + strTemp4;
            strEnd = "dli0" + Convert.ToString(iTemp);
            strTemp5 = co.CO_MarginStr(strTemp0, strStart, strEnd);
            if (strTemp5.IndexOf("<tbody>")!=-1)
            {
                strTemp6 = strTemp5.Substring(0, strTemp5.IndexOf("<tbody>"));
            }
            else
            {
                strTemp6 = strTemp5;
            }
            if (strTemp6.IndexOf("<table")!=-1 && strTemp6.IndexOf("</table>")!=-1)
            {
                strTemp7 = co.CO_MarginStr(strTemp6, "<table", "</table>");
            }
            else
            {
                strTemp7 = strTemp6;
            }
            //去除标记
            if (strTemp7.IndexOf("<tr>")!=-1)
            {
                strTemp8 = strTemp7.Replace("<tr>", "");
            }
            else
            {
                strTemp8 = strTemp7;
            }
            if (strTemp8.IndexOf("</tr>") != -1)
            {
                strTemp9 = strTemp8.Replace("</tr>", "");
            }
            else
            {
                strTemp9 = strTemp8;
            }
            if (strTemp9.IndexOf("<th>")!=-1)
            {
                strTemp10 = strTemp9.Replace("<th>", "=");
            }
            else
            {
                strTemp10 = strTemp9;
            }
            if (strTemp10.IndexOf("</th>") != -1)
            {
                strTemp11 = strTemp10.Replace("</th>", "");
            }
            else
            {
                strTemp11 = strTemp10;
            }
            if (strTemp11.IndexOf("<td>")!=-1)
            {
                strTemp12 = strTemp11.Replace("<td>", "");
            }
            else
            {
                strTemp12 = strTemp11;
            }
            if (strTemp12.IndexOf("</td>")!=-1)
            {
                strTemp13 = strTemp12.Replace("</td>", "");
            }
            else
            {
                strTemp13 = strTemp12;
            }
            if (strTemp13.IndexOf("<td title=\"\">")!=-1)
            {
                strTemp14 = strTemp13.Replace("<td title=\"\">", "");
            }
            else
            {
                strTemp14 = strTemp13;
            }
            if (strTemp14.IndexOf(" ")!=-1)
            {
                strTemp15 = strTemp14.Replace(" ", "");
            }
            else
            {
                strTemp15 = strTemp14;
            }
            if (strTemp15.IndexOf("\n")!=-1)
            {
                strTemp16 = strTemp15.Replace("\n", "");
            }
            else
            {
                strTemp16 = strTemp15;
            }
            if (strTemp16.IndexOf(">")!=-1)
            {
                ipos1 = strTemp16.IndexOf(">");
                strTemp17 = strTemp16.Substring(ipos1 + 1, strTemp16.Length - ipos1 - 1);
            }
            else
            {
                strTemp17 = strTemp16;
            }
            if (strTemp17.IndexOf("\r")!=-1)
            {
                strTemp18 = strTemp17.Replace("\r", "");
            }
            else
            {
                strTemp18 = strTemp17;
            }
            if (strTemp18.IndexOf("\t")!=-1)
            {
                strTemp19 = strTemp18.Replace("\t", "");
            }
            else
            {
                strTemp19 = strTemp18;
            }
            //strTemp20 = CO_EliminateWord(strTemp19, "<", ">");
            return strTemp19;
        }

        /// <summary>
        /// 消除文字中间没用的字符信息
        /// </summary>
        public string CO_EliminateWord(string m_Source,string m_Startchar,string m_Endchar)
        {
            String strResult = "", strLastResult;
            int ipos0, ipos1;
            ipos0 = m_Source.IndexOf(m_Endchar);
            ipos1 = m_Source.IndexOf(m_Startchar);
            try
            {
                if (ipos0 == -1)
                {
                    //没有找到结束符号
                    if (ipos1 == -1)
                    {
                        //没有找到开始符号
                        return m_Source;
                    }
                    else
                    {
                        //找到了开始符号
                        //截取完字符以后回调函数
                        return CO_EliminateWord(m_Source.Remove(ipos1 - 1), m_Startchar, m_Endchar);
                    }
                }
                else
                {
                    //找到结束符号
                    if (ipos1 == -1)
                    {
                        //没有找到开始符号
                        //截取完字符以后回调函数
                        return CO_EliminateWord(m_Source.Remove(0, ipos0), m_Startchar, m_Endchar); 
                    }
                    else
                    {
                        //找到了开始符号
                        //截取完字符以后回调函数
                        return CO_EliminateWord(m_Source.Remove(ipos1, ipos0 - ipos1 + 1), m_Startchar, m_Endchar);  
                    }
                }
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
            
        }

        /// <summary>
        /// 搜房小区网截取信息3
        /// </summary>
        public string CO_SouFunArea3(string m_Source, string m_Split)
        {
            /*
             * 范例网址：http://kaixuanguangchang0791.soufun.com/xiangqing/
             */
            String strTemp0, strTemp1, strTemp2, strTemp3, strTemp4, strTemp5, strTemp6, strTemp7, strTemp8, strTemp9, strTemp10;
            String strTemp11, strTemp12, strTemp13, strTemp14, strTemp15, strTemp16, strTemp17, strTemp18, strTemp19, strTemp20;
            String strStart, strEnd;
            int ipos0, ipos1, ipos2, ipos3;
            int iTemp;
            Cutout co = new Cutout();
            try
            {
                strTemp0 = co.CO_MarginStr(m_Source, "<!--Nav end-->", "<!--楼盘管家-->");
                strTemp1 = strTemp2 = "";
                //截取最后一个m_Split以后的信息
                ipos0 = strTemp0.LastIndexOf(m_Split);
                if (ipos0 != -1)
                {
                    strTemp1 = strTemp0.Substring(ipos0, strTemp0.Length - ipos0 - 1);
                }
                //截取m_Split到第一个</div>的值
                ipos0 = strTemp1.IndexOf("</div>");
                if (ipos0 != -1)
                {
                    strTemp2 = strTemp1.Substring(0, ipos0);
                }
                //消除标记
                if (strTemp1.IndexOf("<dl>") != -1)
                {
                    strTemp3 = strTemp2.Replace("<dl>", "");
                }
                else
                {
                    strTemp3 = strTemp2;
                }
                if (strTemp3.IndexOf("</dl>") != -1)
                {
                    strTemp4 = strTemp3.Replace("</dl>", "");
                }
                else
                {
                    strTemp4 = strTemp3;
                }
                if (strTemp4.IndexOf("<dt>") != -1)
                {
                    strTemp5 = strTemp4.Replace("<dt>", "=");
                }
                else
                {
                    strTemp5 = strTemp4;
                }
                if (strTemp5.IndexOf("</dt>") != -1)
                {
                    strTemp6 = strTemp5.Replace("</dt>", "");
                }
                else
                {
                    strTemp6 = strTemp5;
                }
                if (strTemp6.IndexOf("<dd>") != -1)
                {
                    strTemp7 = strTemp6.Replace("<dd>", "=");
                }
                else
                {
                    strTemp7 = strTemp6;
                }
                if (strTemp7.IndexOf("</dd>") != -1)
                {
                    strTemp8 = strTemp7.Replace("</dd>", "");
                }
                else
                {
                    strTemp8 = strTemp7;
                }
                strTemp9 = CO_EliminateWord(strTemp8, "<", ">");
                if (strTemp9.IndexOf(" ") != -1)
                {
                    strTemp10 = strTemp9.Replace(" ", "");
                }
                else
                {
                    strTemp10 = strTemp9;
                }
                if (strTemp10.IndexOf("\n") != -1)
                {
                    strTemp11 = strTemp10.Replace("\n", "");
                }
                else
                {
                    strTemp11 = strTemp10;
                }
                if (strTemp11.IndexOf("\t") != -1)
                {
                    strTemp12 = strTemp11.Replace("\t", "");
                }
                else
                {
                    strTemp12 = strTemp11;
                }
                if (strTemp12.IndexOf("\r") != -1)
                {
                    strTemp13 = strTemp12.Replace("\r", "");
                }
                else
                {
                    strTemp13 = strTemp12;
                }
                if (strTemp13.IndexOf("编辑本段") != -1)
                {
                    strTemp14 = strTemp13.Replace("编辑本段", "");
                }
                else
                {
                    strTemp14 = strTemp13;
                }
                if (strTemp14.IndexOf("&nbsp")!=-1)
                {
                    strTemp15 = strTemp14.Replace("&nbsp", "");
                }
                else
                {
                    strTemp15 = strTemp14;
                }
                if (strTemp15.IndexOf(";") != -1)
                {
                    strTemp16 = strTemp15.Replace(";", "");
                }
                else
                {
                    strTemp16 = strTemp15;
                }
                return strTemp16;
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
            
        }

        /// <summary>
        /// 搜房小区网截取信息4
        /// </summary>
        public string CO_SouFunArea4(string m_Source, string m_Split)
        {
            /*
             * 范例网址：http://kaixuanguangchang0791.soufun.com/xiangqing/
             */
            String strTemp0, strTemp1, strTemp2, strTemp3, strTemp4, strTemp5, strTemp6, strTemp7, strTemp8, strTemp9, strTemp10;
            String strTemp11, strTemp12, strTemp13, strTemp14, strTemp15, strTemp16, strTemp17, strTemp18, strTemp19, strTemp20;
            String strStart, strEnd;
            int ipos0, ipos1, ipos2, ipos3;
            int iTemp;
            strTemp6 = "";
            Cutout co = new Cutout();
            try
            {
                strTemp0 = co.CO_MarginStr(m_Source, "<!--列表内容  begin-->", "<!--列表内容  end-->");
                strTemp1 = strTemp2 = "";
                if (strTemp0.IndexOf("</h1>")!=-1)
                {
                    strTemp1 = strTemp0.Replace("</h1>", "*");
                }
                
                //截取最后一个m_Split以后的信息
                ipos0 = strTemp1.IndexOf(m_Split);
                if (ipos0 != -1)
                {
                    strTemp2 = strTemp1.Substring(ipos0, strTemp1.Length - ipos0 - 1);
                }
                if (m_Split !="基本信息")
                {
                    if (strTemp2.IndexOf("</strong>") != -1)
                    {
                        strTemp10 = strTemp2.Replace("</strong>", "*");
                    }
                    else
                    {
                        strTemp10 = strTemp2;
                    }
                }
                else
                {
                    strTemp10 = strTemp2;
                }
                
                ipos1 = strTemp10.IndexOf("*");
                strTemp3 = strTemp10.Substring(ipos1 + 1, strTemp10.Length - ipos1 - 1);
                ipos2 = strTemp3.IndexOf("*");
                strTemp4 = strTemp3.Substring(0, ipos2);
                ipos3 = strTemp4.LastIndexOf(">");
                strTemp5 = strTemp4.Substring(0, ipos3);
                if (strTemp5.IndexOf("<td")!=-1)
                {
                    strTemp6 = strTemp5.Replace("<td", "*");
                }
                else
                {
                    strTemp6 = strTemp5;
                }
                strTemp7 = CO_EliminateAllWord(strTemp6);
                if (strTemp7.IndexOf("*")!=-1)
                {
                    strTemp8 = strTemp7.Replace("*", "=");
                }
                else
                {
                    strTemp8 = strTemp7;
                }
                
                return strTemp8;
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }

        }

        /// <summary>
        /// 搜房小区网截取信息4
        /// </summary>
        public string CO_SouFunArea5(string m_Source, string m_Split)
        {
            /*
             * 范例网址：http://zhongyangxiangxie.soufun.com/
             */
            String strTemp0, strTemp1, strTemp2, strTemp3, strTemp4, strTemp5, strTemp6, strTemp7, strTemp8, strTemp9, strTemp10;
            String strTemp11, strTemp12, strTemp13, strTemp14, strTemp15, strTemp16, strTemp17, strTemp18, strTemp19, strTemp20;
            String strStart, strEnd;
            int ipos0, ipos1, ipos2, ipos3;
            int iTemp;
            strTemp6 = "";
            Cutout co = new Cutout();
            try
            {
                strTemp0 = co.CO_MarginStr(m_Source, "<!--列表内容  begin-->", "<!--列表内容  end-->");
                strTemp1 = strTemp2 = "";
                if (strTemp0.IndexOf("</h1>") != -1)
                {
                    strTemp1 = strTemp0.Replace("</h1>", "*");
                }

                //截取最后一个m_Split以后的信息
                ipos0 = strTemp1.IndexOf(m_Split);
                if (ipos0 != -1)
                {
                    strTemp2 = strTemp1.Substring(ipos0, strTemp1.Length - ipos0 - 1);
                }
                if (m_Split != "基本信息")
                {
                    if (strTemp2.IndexOf("</strong>") != -1)
                    {
                        strTemp10 = strTemp2.Replace("</strong>", "*");
                    }
                    else
                    {
                        strTemp10 = strTemp2;
                    }
                }
                else
                {
                    strTemp10 = strTemp2;
                }

                ipos1 = strTemp10.IndexOf("*");
                strTemp3 = strTemp10.Substring(ipos1 + 1, strTemp10.Length - ipos1 - 1);
                ipos2 = strTemp3.IndexOf("*");
                strTemp4 = strTemp3.Substring(0, ipos2);
                ipos3 = strTemp4.LastIndexOf(">");
                strTemp5 = strTemp4.Substring(0, ipos3);
                if (strTemp5.IndexOf("<td") != -1)
                {
                    strTemp6 = strTemp5.Replace("<td", "*");
                }
                else
                {
                    strTemp6 = strTemp5;
                }
                strTemp7 = CO_EliminateAllWord(strTemp6);
                if (strTemp7.IndexOf("*") != -1)
                {
                    strTemp8 = strTemp7.Replace("*", "=");
                }
                else
                {
                    strTemp8 = strTemp7;
                }

                return strTemp8;
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }

        }

        /// <summary>
        /// 搜房小区网截取信息4
        /// </summary>
        public string CO_SouFunArea6(string m_Source, string m_Split)
        {
            /*
             * 范例网址：http://kaixuanzhongxinhg.soufun.com/#xxxx
             */
            String strTemp0, strTemp1, strTemp2, strTemp3, strTemp4, strTemp5, strTemp6, strTemp7, strTemp8, strTemp9, strTemp10;
            String strResult;
            int ipos0, ipos1;
            Cutout co = new Cutout();
            try
            {
                if (m_Split =="基本信息")
                {
                    strTemp0 = co.CO_MarginStr(m_Source, "<!--发送到手机 end-->", "</table>");
                    strTemp1 = strTemp0.Replace("</td>", "*");
                    strTemp2 = CO_EliminateAllWord(strTemp1);
                    if (strTemp2.IndexOf("交通状况")!=-1)
                    {
                        strTemp3 = "*" + strTemp2.Substring(0, strTemp2.IndexOf("交通状况"));
                    }
                    else
                    {
                        strTemp3 = "*" + strTemp2;
                    }
                    strResult = strTemp3.Replace("*", "<br>");

                }
                else
                {
                    strTemp0 = co.CO_MarginStr(m_Source, "<!--left详细信息 begin-->", "<!--left详细信息 end-->");
                    strTemp1 = strTemp0.Replace("</td>", "*");
                    strTemp2 = strTemp1.Substring(0, strTemp1.IndexOf(m_Split));
                    ipos0 = strTemp2.LastIndexOf("id");
                    strTemp3 = strTemp2.Substring(ipos0, strTemp2.Length - ipos0);
                    strTemp4 = co.CO_MarginStr(strTemp3, "id=\"", "\">");
                    ipos1 = strTemp1.LastIndexOf(strTemp4.Replace("of","oli"));
                    strTemp5 = strTemp1.Substring(ipos1, strTemp1.Length - ipos1);
                    strTemp6 = strTemp5.Substring(0, strTemp5.IndexOf("id"));
                    strTemp7 = strTemp6.Replace("</div>", "*");
                    strTemp8 = CO_EliminateAllWord(strTemp7);
                    strTemp9 = strTemp8.Replace("*", "<br>");
                    strResult = "<br>" + strTemp9;

                }


                return strResult;
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }

        }

        /// <summary>
        /// 搜房小区网截取信息4
        /// </summary>
        public string CO_SouFunArea7(string m_Source, string m_Split)
        {
            /*
             * 范例网址：http://daduhui0791.soufun.com/
             */
            String strTemp0, strTemp1, strTemp2="", strTemp3, strTemp4, strTemp5, strTemp6, strTemp7, strTemp8, strTemp9, strTemp10;
            String strTemp11, strTemp12, strTemp13, strTemp14, strTemp15, strTemp16, strTemp17, strTemp18, strTemp19, strTemp20;
            String strStart, strEnd;
            int ipos0, ipos1, ipos2, ipos3;
            int iTemp;
            Cutout co = new Cutout();
            try
            {
                strTemp0 = co.CO_MarginStr(m_Source, "<!--列表内容  begin-->", "<!--列表内容  end-->");
                if (m_Split == "基本信息")
                {
                    strTemp1 = CO_MarginStr(strTemp0, m_Split, "预售许可证");
                }
                else
                {
                    strTemp1 = CO_MarginStr(strTemp0, m_Split, "</div>");
                }

                if (strTemp1.IndexOf("</tr>") != -1)
                {
                    strTemp2 = strTemp1.Replace("</tr>", "*");
                }
                else
                {
                    strTemp2 = strTemp1;
                }

                if (strTemp2.IndexOf("</td>") != -1)
                {
                    strTemp17 = strTemp2.Replace("</td>", "*");
                }
                else
                {
                    strTemp17 = strTemp2;
                }

                //消除标记
                if (strTemp17.IndexOf("<dl>") != -1)
                {
                    strTemp3 = strTemp17.Replace("<dl>", "");
                }
                else
                {
                    strTemp3 = strTemp17;
                }
                if (strTemp3.IndexOf("</dl>") != -1)
                {
                    strTemp4 = strTemp3.Replace("</dl>", "*");
                }
                else
                {
                    strTemp4 = strTemp3;
                }
                if (strTemp4.IndexOf("<dt>") != -1)
                {
                    strTemp5 = strTemp4.Replace("<dt>", "");
                }
                else
                {
                    strTemp5 = strTemp4;
                }
                if (strTemp5.IndexOf("</dt>") != -1)
                {
                    strTemp6 = strTemp5.Replace("</dt>", "*");
                }
                else
                {
                    strTemp6 = strTemp5;
                }
                if (strTemp6.IndexOf("<dd>") != -1)
                {
                    strTemp7 = strTemp6.Replace("<dd>", "");
                }
                else
                {
                    strTemp7 = strTemp6;
                }
                if (strTemp7.IndexOf("</dd>") != -1)
                {
                    strTemp8 = strTemp7.Replace("</dd>", "*");
                }
                else
                {
                    strTemp8 = strTemp7;
                }
                strTemp9 = CO_EliminateWord(strTemp8, "<", ">");
                if (strTemp9.IndexOf(" ") != -1)
                {
                    strTemp10 = strTemp9.Replace(" ", "");
                }
                else
                {
                    strTemp10 = strTemp9;
                }
                if (strTemp10.IndexOf("\n") != -1)
                {
                    strTemp11 = strTemp10.Replace("\n", "");
                }
                else
                {
                    strTemp11 = strTemp10;
                }
                if (strTemp11.IndexOf("\t") != -1)
                {
                    strTemp12 = strTemp11.Replace("\t", "");
                }
                else
                {
                    strTemp12 = strTemp11;
                }
                if (strTemp12.IndexOf("\r") != -1)
                {
                    strTemp13 = strTemp12.Replace("\r", "");
                }
                else
                {
                    strTemp13 = strTemp12;
                }
                if (strTemp13.IndexOf("编辑本段") != -1)
                {
                    strTemp14 = strTemp13.Replace("编辑本段", "");
                }
                else
                {
                    strTemp14 = strTemp13;
                }
                if (strTemp14.IndexOf("&nbsp") != -1)
                {
                    strTemp15 = strTemp14.Replace("&nbsp", "");
                }
                else
                {
                    strTemp15 = strTemp14;
                }

                strTemp16 = "<br>" + strTemp15.Replace("*", "<br>");
                return strTemp16;
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }

        }


        /// <summary>
        /// 获取链接地址
        /// </summary>
        /// <param name="m_Url">原网址地址</param>
        /// <param name="m_Info">链接的信息</param>
        public string CO_GetLinkAddress(string m_Url,string m_Info)
        {
            //获取网页源代码
            String strCode = CO_GetPage(m_Url);
            String strTemp0, strTemp1, strTemp2 ="",strTemp3;
            try
            {
                if (strCode.IndexOf(m_Info)!=-1)
                {
                    //截取信息到m_Info的位置
                    strTemp0 = strCode.Substring(0, strCode.IndexOf(m_Info));
                    //往回截取到href
                    int ipos0;
                    ipos0 = strTemp0.LastIndexOf("href");
                    if (ipos0!=-1)
                    {
                        strTemp1 = strTemp0.Substring(ipos0, strTemp0.Length - ipos0);
                        if (strTemp1.IndexOf("target=")!=-1)
                        {
                            strTemp3 = CO_MarginStr(strTemp1, "href=\"", "target");
                            strTemp2 = strTemp3.Substring(0, strTemp3.IndexOf("\""));
                        }
                        else
                        {
                            strTemp2 = CO_MarginStr(strTemp1, "href=\"", "\">");
                        }
                        
                    }
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

            return strTemp2;
        }


        /// <summary>
        /// 网页获取时先下载网页数据包，然后再获取网页源代码
        /// </summary>
        /// <param name="url">网页原地址</param>
        private string CO_GetPage(string url)
        {
            string pageCode;
            try
            {
                HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                webRequest.Timeout = 30000;
                webRequest.Method = "GET";
                webRequest.UserAgent = "Mozilla/4.0";
                webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
                if (webResponse.ContentEncoding.ToLower() == "gzip")
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (var zipStream =
                            new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                        {
                            using (StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.Default))
                            {
                                pageCode = sr.ReadToEnd();
                            }
                        }
                    }
                }
                else
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(streamReceive, Encoding.Default))
                        {
                            pageCode = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                pageCode = ex.Message;
            }
            return pageCode;
        }

        /// <summary>
        /// 消除所有的字符和数字
        /// </summary>
        public string CO_EliminateAllWord(string m_Source)
        {
            string strTemp0;
            String m_Dest = "";
            if (m_Source.IndexOf("0")!=-1)
            {
                m_Dest = m_Dest = m_Source.Replace("0", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("1") != -1)
            {
                m_Dest = m_Dest = m_Source.Replace("1", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("2") != -1)
            {
                m_Dest = m_Dest = m_Source.Replace("2", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("3") != -1)
            {
                m_Dest = m_Source.Replace("3", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("4") != -1)
            {
                m_Dest = m_Source.Replace("4", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("5") != -1)
            {
                m_Dest = m_Source.Replace("5", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("6") != -1)
            {
                m_Dest = m_Source.Replace("6", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("7") != -1)
            {
                m_Dest = m_Source.Replace("7", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("8") != -1)
            {
                m_Dest = m_Source.Replace("8", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("9") != -1)
            {
                m_Dest = m_Source.Replace("9", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("a") != -1)
            {
                m_Dest = m_Source.Replace("a", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("b") != -1)
            {
                m_Dest = m_Source.Replace("b", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("c") != -1)
            {
                m_Dest = m_Source.Replace("c", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("d") != -1)
            {
                m_Dest = m_Source.Replace("d", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("e") != -1)
            {
                m_Dest = m_Source.Replace("e", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("f") != -1)
            {
                m_Dest = m_Source.Replace("f", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("g") != -1)
            {
                m_Dest = m_Source.Replace("g", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("h") != -1)
            {
                m_Dest = m_Source.Replace("h", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("i") != -1)
            {
                m_Dest = m_Source.Replace("i", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("j") != -1)
            {
                m_Dest = m_Source.Replace("j", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("k") != -1)
            {
                m_Dest = m_Source.Replace("k", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("l") != -1)
            {
                m_Dest = m_Source.Replace("l", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("m") != -1)
            {
                m_Dest = m_Source.Replace("m", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("n") != -1)
            {
                m_Dest = m_Source.Replace("n", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("o") != -1)
            {
                m_Dest = m_Source.Replace("o", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("p") != -1)
            {
                m_Dest = m_Source.Replace("p", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("q") != -1)
            {
                m_Dest = m_Source.Replace("q", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("r") != -1)
            {
                m_Dest = m_Source.Replace("r", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("s") != -1)
            {
                m_Dest = m_Source.Replace("s", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("t") != -1)
            {
                m_Dest = m_Source.Replace("t", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("u") != -1)
            {
                m_Dest = m_Source.Replace("u", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("v") != -1)
            {
                m_Dest = m_Source.Replace("v", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("w") != -1)
            {
                m_Dest = m_Source.Replace("w", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("x") != -1)
            {
                m_Dest = m_Source.Replace("x", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("y") != -1)
            {
                m_Dest = m_Source.Replace("y", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("z") != -1)
            {
                m_Dest = m_Source.Replace("z", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("A") != -1)
            {
                m_Dest = m_Source.Replace("A", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("B") != -1)
            {
                m_Dest = m_Source.Replace("B", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("C") != -1)
            {
                m_Dest = m_Source.Replace("C", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("D") != -1)
            {
                m_Dest = m_Source.Replace("D", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("E") != -1)
            {
                m_Dest = m_Source.Replace("E", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("F") != -1)
            {
                m_Dest = m_Source.Replace("F", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("G") != -1)
            {
                m_Dest = m_Source.Replace("G", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("H") != -1)
            {
                m_Dest = m_Source.Replace("H", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("I") != -1)
            {
                m_Dest = m_Source.Replace("I", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("J") != -1)
            {
                m_Dest = m_Source.Replace("J", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("K") != -1)
            {
                m_Dest = m_Source.Replace("K", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("L") != -1)
            {
                m_Dest = m_Source.Replace("L", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("M") != -1)
            {
                m_Dest = m_Source.Replace("M", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("N") != -1)
            {
                m_Dest = m_Source.Replace("N", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("O") != -1)
            {
                m_Dest = m_Source.Replace("O", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("P") != -1)
            {
                m_Dest = m_Source.Replace("P", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("Q") != -1)
            {
                m_Dest = m_Source.Replace("Q", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("R") != -1)
            {
                m_Dest = m_Source.Replace("R", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("S") != -1)
            {
                m_Dest = m_Source.Replace("S", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("T") != -1)
            {
                m_Dest = m_Source.Replace("T", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("U") != -1)
            {
                m_Dest = m_Source.Replace("U", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("V") != -1)
            {
                m_Dest = m_Source.Replace("V", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("W") != -1)
            {
                m_Dest = m_Source.Replace("W", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("X") != -1)
            {
                m_Dest = m_Source.Replace("X", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("Y") != -1)
            {
                m_Dest = m_Source.Replace("Y", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("Z") != -1)
            {
                m_Dest = m_Source.Replace("Z", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("<") != -1)
            {
                m_Dest = m_Source.Replace("<", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf(">") != -1)
            {
                m_Dest = m_Source.Replace(">", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("?") != -1)
            {
                m_Dest = m_Source.Replace("?", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("/") != -1)
            {
                m_Dest = m_Source.Replace("/", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("\\") != -1)
            {
                m_Dest = m_Source.Replace("\\", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("%") != -1)
            {
                m_Dest = m_Source.Replace("%", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("=") != -1)
            {
                m_Dest = m_Source.Replace("=", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("-") != -1)
            {
                m_Dest = m_Source.Replace("-", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("\"") != -1)
            {
                m_Dest = m_Source.Replace("\"", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("\'") != -1)
            {
                m_Dest = m_Source.Replace("\'", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0; 
            }
            if (m_Source.IndexOf("&") != -1)
            {
                m_Dest = m_Source.Replace("&", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0;
            }
            if (m_Source.IndexOf(";") != -1)
            {
                m_Dest = m_Source.Replace(";", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0;
            }
            if (m_Source.IndexOf("_") != -1)
            {
                m_Dest = m_Source.Replace("_", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0;
            }
            if (m_Source.IndexOf(".") != -1)
            {
                m_Dest = m_Source.Replace(".", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0;
            }
            if (m_Source.IndexOf("—") != -1)
            {
                m_Dest = m_Source.Replace("—", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0;
            }
            if (m_Source.IndexOf("#") != -1)
            {
                m_Dest = m_Source.Replace("#", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0;
            }
            if (m_Source.IndexOf(" ") != -1)
            {
                m_Dest = m_Source.Replace(" ", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0;
            }
            if (m_Source.IndexOf("\t") != -1)
            {
                m_Dest = m_Source.Replace("\t", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0;
            }
            if (m_Source.IndexOf("\r") != -1)
            {
                m_Dest = m_Source.Replace("\r", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0;
            }
            if (m_Source.IndexOf("\n") != -1)
            {
                m_Dest = m_Source.Replace("\n", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0;
            }
            if (m_Source.IndexOf(":") != -1)
            {
                m_Dest = m_Source.Replace(":", "");
                strTemp0 = m_Dest;
                m_Source = strTemp0;
            }
            return m_Dest;
        }

        /// <summary>
        /// 获取地图Url
        /// </summary>
        /// <param name="url">网页源地址</param>
        public string[] CO_GetMapUrl1(string url)
        {
            //获取网页源代码
            String strCode = CO_GetPage(url);
            String strTemp0, strTemp1, strTemp2 = "", strTemp3,strTemp4,strTemp5,strTemp6,strTemp7,strTemp8,strTemp9;
            String[] strpos = new String[3];
            int ipos0;
            try
            {
                //当状态为1时执行第一种截取地图网址的方式
                strTemp0 = CO_GetLinkAddress(url, "更多");
                strTemp1 = CO_GetPage(strTemp0);
                strTemp2 = CO_MarginStr(strTemp1, "<!--地理位置 begin-->", "<!--地理位置 end-->");
                ipos0 = strTemp2.IndexOf("src=\"");
                strTemp3 = strTemp2.Substring(ipos0 + 5, strTemp2.Length - ipos0 - 5);
                strTemp4 = strTemp3.Substring(0, strTemp3.IndexOf("\""));
                //获取地图的网页源代码
                strTemp5 = CO_GetPage(strTemp4);
                strTemp6 = CO_MarginStr(strTemp5, "var mapInfo", "\n");
                strTemp7 = CO_MarginStr(strTemp6, "px:", ",");
                strpos[0] = strTemp7.Replace("\"", "");
                strTemp8 = CO_MarginStr(strTemp6, "py:", ",");
                strpos[1] = strTemp8.Replace("\"", "");
                if (strTemp5.IndexOf("var param")!=-1)
                {
                    strTemp9 = CO_MarginStr(strTemp5, "var param = ", ".split");
                    strpos[2] = strTemp9.Replace("\'", "");
                }
                else
                {
                    strpos[2] = "null";
                }
                
            }
            catch (Exception ex)
            {
                return null;
            }
            return strpos;
        }

        /// <summary>
        /// 获取地图Url
        /// </summary>
        /// <param name="url">网页源地址</param>
        public string[] CO_GetMapUrl2(string url)
        {
            //获取网页源代码
            String strCode = CO_GetPage(url);
            String strTemp0, strTemp1, strTemp2 = "", strTemp3, strTemp4, strTemp5, strTemp6,strTemp7;
            String[] strpos = new String[3];
            int ipos0;
            try
            {
                //当状态为1时执行第一种截取地图网址的方式
                strTemp0 = CO_MarginStr(strCode, "<!--地图 begin-->", "<!--户型图 begin-->");
                ipos0 = strTemp0.IndexOf("src=\"");
                strTemp1 = strTemp0.Substring(ipos0 + 5, strTemp0.Length - ipos0 - 5);
                strTemp2 = strTemp1.Substring(0, strTemp1.IndexOf("\""));
                //获取地图的网页源代码
                strTemp3 = CO_GetPage(strTemp2);
                strTemp4 = CO_MarginStr(strTemp3, "var mapInfo", "\n");
                strTemp5 = CO_MarginStr(strTemp4, "px:", ",");
                strpos[0] = strTemp5.Replace("\"", "");
                strTemp6 = CO_MarginStr(strTemp4, "py:", ",");
                strpos[1] = strTemp6.Replace("\"", "");
                //获取城市地图场景的视角参数
                if (strTemp3.IndexOf("var param") != -1)
                {
                    strTemp7 = CO_MarginStr(strTemp3, "var param = ", ".split");
                    strpos[2] = strTemp7.Replace("\'", "");
                }
                else
                {
                    strpos[2] = "null";
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return strpos;
        }

        public string[] CO_GetMapUrl3(string url)
        {
            //获取网页源代码
            String strCode = CO_GetPage(url);
            String strTemp0, strTemp1, strTemp2 = "", strTemp3, strTemp4, strTemp5, strTemp6,strTemp7,strTemp8;
            String[] strpos = new String[3];
            int ipos0,ipos1;
            try
            {
                //当状态为1时执行第一种截取地图网址的方式
                ipos0 = strCode.LastIndexOf("地图交通");
                strTemp0 = strCode.Substring(ipos0, strCode.Length - ipos0);
                strTemp1 = CO_MarginStr(strTemp0, "地图交通", "</div>");
                ipos1 = strTemp1.IndexOf("src=\"");
                strTemp2 = strTemp1.Substring(ipos1 + 5, strTemp1.Length - ipos1 - 5);
                strTemp3 = strTemp2.Substring(0, strTemp2.IndexOf("\""));
                //获取地图的网页源代码
                strTemp4 = CO_GetPage(strTemp3);
                strTemp5 = CO_MarginStr(strTemp4, "var mapInfo", "\n");
                strTemp6 = CO_MarginStr(strTemp5, "px:", ",");
                strpos[0] = strTemp6.Replace("\"", "");
                strTemp7 = CO_MarginStr(strTemp5, "py:", ",");
                strpos[1] = strTemp7.Replace("\"", "");
                //获取城市地图场景的视角参数
                if (strTemp4.IndexOf("var param") != -1)
                {
                    strTemp8 = CO_MarginStr(strTemp4, "var param = ", ".split");
                    strpos[2] = strTemp8.Replace("\'", "");
                }
                else
                {
                    strpos[2] = "null";
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return strpos;
        }

        public string[] CO_GetMapUrl4(string url)
        {
            //获取网页源代码
            String strCode = CO_GetPage(url);
            String strTemp0, strTemp1, strTemp2 = "", strTemp3, strTemp4, strTemp5, strTemp6, strTemp7;
            String[] strpos = new String[3];
            int ipos0, ipos1,ipos2;
            try
            {
                //当状态为1时执行第一种截取地图网址的方式
                ipos0 = strCode.IndexOf("<!-- 地图 end -->");
                strTemp0 = strCode.Substring(0, ipos0);
                ipos1 = strTemp0.LastIndexOf("<iframe");
                strTemp1 = strTemp0.Substring(ipos1, strTemp0.Length - ipos1);
                ipos2 = strTemp1.IndexOf("src=\"");
                strTemp2 = strTemp1.Substring(ipos2 + 5, strTemp1.Length - ipos2 - 5);
                strTemp3 = strTemp2.Substring(0, strTemp2.IndexOf("\""));
                //获取地图的网页源代码
                strTemp4 = CO_GetPage(strTemp3);
                strTemp5 = CO_MarginStr(strTemp4, "var cityx=", ";");
                strpos[0] = strTemp5.Replace("\"", "");
                strTemp6 = CO_MarginStr(strTemp4, "var cityy=", ";");
                strpos[1] = strTemp6.Replace("\"", "");
                //获取城市地图场景的视角参数
                if (strTemp4.IndexOf("var param") != -1)
                {
                    strTemp7 = CO_MarginStr(strTemp4, "var param = ", ".split");
                    strpos[2] = strTemp7.Replace("\'", "");
                }
                else
                {
                    strpos[2] = "null";
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return strpos;
        }

        public string[] CO_GetMapUrl5(string url)
        {
            //获取网页源代码
            String strCode = CO_GetPage(url);
            String strTemp0, strTemp1, strTemp2, strTemp3, strTemp4, strTemp5, strTemp6, strTemp7;
            String[] strpos = new String[3];
            int ipos0, ipos1, ipos2;
            try
            {
                //当状态为1时执行第一种截取地图网址的方式
                strTemp1 = CO_MarginStr(strCode, "<!--地图搜索 begin-->", "<!--地图搜索 end-->");
                ipos2 = strTemp1.IndexOf("src=\"");
                strTemp2 = strTemp1.Substring(ipos2 + 5, strTemp1.Length - ipos2 - 5);
                strTemp3 = strTemp2.Substring(0, strTemp2.IndexOf("\""));
                //获取地图的网页源代码
                strTemp4 = CO_GetPage(strTemp3);
                strTemp5 = CO_MarginStr(strTemp4, "var cityx=", ";");
                strpos[0] = strTemp5.Replace("\"", "");
                strTemp6 = CO_MarginStr(strTemp4, "var cityy=", ";");
                strpos[1] = strTemp6.Replace("\"", "");
                //获取城市地图场景的视角参数
                if (strTemp4.IndexOf("var param") != -1)
                {
                    strTemp7 = CO_MarginStr(strTemp4, "var param = ", ".split");
                    strpos[2] = strTemp7.Replace("\'", "");
                }
                else
                {
                    strpos[2] = "null";
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return strpos;
        }

        public string[] CO_GetMapUrl7(string url)
        {
            //获取网页源代码
            String strCode = CO_GetPage(url);
            String strTemp0, strTemp1, strTemp2, strTemp3, strTemp4, strTemp5, strTemp6, strTemp7;
            String[] strpos = new String[3];
            int ipos0, ipos1, ipos2;
            try
            {
                //当状态为1时执行第一种截取地图网址的方式
                strTemp1 = CO_MarginStr(strCode, "<!--地图搜索 begin-->", "<!--地图搜索 end-->");
                ipos2 = strTemp1.IndexOf("src=\"");
                strTemp2 = strTemp1.Substring(ipos2 + 5, strTemp1.Length - ipos2 - 5);
                strTemp3 = strTemp2.Substring(0, strTemp2.IndexOf("\""));
                //获取地图的网页源代码
                strTemp4 = CO_GetPage(strTemp3);
                strTemp5 = CO_MarginStr(strTemp4, "var cityx=", ";");
                strpos[0] = strTemp5.Replace("\"", "");
                strTemp6 = CO_MarginStr(strTemp4, "var cityy=", ";");
                strpos[1] = strTemp6.Replace("\"", "");
                //获取城市地图场景的视角参数
                if (strTemp4.IndexOf("var param") != -1)
                {
                    strTemp7 = CO_MarginStr(strTemp4, "var param = ", ".split");
                    strpos[2] = strTemp7.Replace("\'", "");
                }
                else
                {
                    strpos[2] = "null";
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return strpos;
        }

    }
}
