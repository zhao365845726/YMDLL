using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMDLL.Interface
{
    public interface IOperaExcel
    {

        /// <summary>
        /// 导出数据到EXCEL
        /// </summary>
        /// <param name="m_SavefilePath">文件保存地址</param>
        void ExportDataToExcel(string m_SavefilePath);
    }
}
