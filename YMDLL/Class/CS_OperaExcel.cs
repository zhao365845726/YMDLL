using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YMDLL.Interface;

namespace YMDLL.Class
{
    class CS_OperaExcel: IOperaExcel
    {
        /// <summary>
        /// excel对象
        /// </summary>
        public static Excel.Application excel;
        /// <summary>
        /// 工作簿对象
        /// </summary>
        public static Excel.Workbook workbook;
        /// <summary>
        /// 工作表对象
        /// </summary>
        public static Excel.Worksheet worksheet;

        public void ExportDataToExcel(string m_SavefilePath)
        {
            excel = new Excel.Application();
            excel.Visible = true;
            workbook = excel.Workbooks.Add(Type.Missing);
            //worksheet = workbook.ActiveSheet;
            //worksheet = excel.Worksheets;

            worksheet.Cells[1, 1] = "First Name";
            worksheet.Cells[1, 2] = "Last Name";
            worksheet.Cells[1, 3] = "Full Name";
            worksheet.Cells[1, 4] = "Salary";

            worksheet.Name = "OriginalOne";

            workbook.SaveAs(m_SavefilePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            workbook = null;
            excel.Quit();
            excel = null;
        }
    }
}
