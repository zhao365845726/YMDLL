using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YMDLL
{
    public class YM_Web
    {
        /// <summary>
        /// HTML对象
        /// </summary>
        public HtmlDocument hdoc;

        //帮助方法根据name获得元素
        /// <summary>
        /// 根据Name获取元素
        /// </summary>
        public HtmlElement GetElement_Name(string name)
        {
            HtmlElement e = hdoc.All[name];
            return e;
        }
        //根据Id获取元素
        /// <summary>
        /// 根据ID获取元素
        /// </summary>
        public HtmlElement GetElement_Id(string id)
        {
            HtmlElement e = hdoc.GetElementById(id);
            return e;
        }

        //根据Type获取元素
        /// <summary>
        /// 根据类型获取元素
        /// </summary>
        private HtmlElement GetElement_Type(string type)
        {
            HtmlElement e = null;
            HtmlElementCollection elements = hdoc.GetElementsByTagName("a");
            foreach (HtmlElement element in elements)
            {
                if (element.GetAttribute("action-type") == type)
                {
                    e = element;
                }
            }
            return e;
        }
    }
}
