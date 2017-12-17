using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ML.Utilities
{
    public class HttpPostedFileWrapper : HttpPostedFileBase
    {
        // 摘要:
        //     初始化 System.Web.HttpPostedFileWrapper 类的新实例。
        //
        // 参数:
        //   httpPostedFile:
        //     通过此包装类可访问的对象。
        //
        // 异常:
        //   System.ArgumentNullException:
        //     httpApplicationState 为 null。
        public HttpPostedFileWrapper(HttpPostedFile httpPostedFile)
        {

        }


    }
}
