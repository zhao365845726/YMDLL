using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMDLL.Interface
{
    /// <summary>
    /// 计算日期时间接口
    /// </summary>
    public interface ICalcDateTime
    {

        /// <summary>
        /// 当前年份的类型
        /// </summary>
        TypeOfYear CurYearType
        {
            get;
            set;
        }
        /// <summary>
        /// 获取当前年份的天数
        /// </summary>
        int GetCurYearDays();

        /// <summary>
        /// 获取当前年份的类型
        /// </summary>
        TypeOfYear GetCurYearType();

        /// <summary>
        /// 获取当前月份有多少天
        /// </summary>
        int GetDaysOfMonth(NameOfMonth m_NOM);
    }

    public enum TypeOfYear
    {
        PingYear=0,
        RunYear=1,
    }

    public enum NameOfMonth
    {
        January=1,      //一月
        February=2,     //二月
        March=3,        //三月
        April=4,        //四月
        May=5,          //五月
        June=6,         //六月
        July=7,         //七月
        August=8,       //八月
        September=9,    //九月
        October=10,     //十月
        November=11,    //十一月
        December=12     //十二月
    }

    public enum NameOfWeek
    {
        Monday = 1,      //星期一
        Tuesday = 2,     //星期二
        Wednesday = 3,   //星期三
        Thursday = 4,    //星期四
        Friday = 5,      //星期五
        Saturday = 6,    //星期六
        Sunday = 7,      //星期七
    }
}
