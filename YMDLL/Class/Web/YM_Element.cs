using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YMDLL.Class
{
    public class YM_Element
    {
        /// <summary>
        /// 根据Name获取元素
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public HtmlElement GetElement_Name(WebBrowser wb,string Name)
        {
            HtmlElement e = wb.Document.All[Name];
            return e;
        }

        /// <summary>
        /// 根据Id获取元素
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public HtmlElement GetElement_Id(WebBrowser wb,string id)
        {
            HtmlElement e = wb.Document.GetElementById(id);
            return e;
        }

        /// <summary>
        /// 根据Index获取元素
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public HtmlElement GetElement_Index(WebBrowser wb,int index)
        {
            HtmlElement e = wb.Document.All[index];
            return e;
        }

        /// <summary>
        /// 根据Type获取元素，在没有Name和ID的情况下使用
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public HtmlElement GetElement_Type(WebBrowser wb,string type)
        {
            HtmlElement e = null;
            HtmlElementCollection elements = wb.Document.GetElementsByTagName("input");
            foreach(HtmlElement element in elements)
            {
                if(element.GetAttribute("type") == type)
                {
                    e = element;
                }
            }
            return e;
        }

        /// <summary>
        /// 根据Type获取元素，在没有Name和ID的情况下使用，并指定是同类type的第i个，GetElement_Type()升级版
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="type"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public HtmlElement GetElement_Type_No(WebBrowser wb, string type, int i)
        {
            int j = 1;
            HtmlElement e = null;
            HtmlElementCollection elements = wb.Document.GetElementsByTagName("input");
            foreach (HtmlElement element in elements)
            {
                if(element.GetAttribute("type") == type)
                {
                    if (j == i)
                    {
                        e = element;
                    }
                    j++;
                }
            }
            return e;
        }

        /// <summary>
        /// 获取Form表单名name，返回表单
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="form_name"></param>
        /// <returns></returns>
        public HtmlElement GetElement_Form(WebBrowser wb,string form_name)
        {
            HtmlElement e = wb.Document.Forms[form_name];
            return e;
        }

        /// <summary>
        /// 设置元素value属性的值
        /// </summary>
        /// <param name="e"></param>
        /// <param name="value"></param>
        public void Write_Value(HtmlElement e,string value)
        {
            e.SetAttribute("value", value);
        }

        /// <summary>
        /// 执行元素的方法，如：click,submit（需form表单名）等
        /// </summary>
        /// <param name="e"></param>
        /// <param name="s"></param>
        public void Btn_Click(HtmlElement e,string s)
        {
            e.InvokeMember(s);
        }
    }
}
