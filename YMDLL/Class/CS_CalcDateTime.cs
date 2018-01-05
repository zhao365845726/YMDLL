using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YMDLL.Interface;

namespace YMDLL.Class
{
    /// <summary>
    /// 计算日期时间类
    /// </summary>
    public class CS_CalcDateTime : ICalcDateTime
    {
        /// <summary>
        /// 当前年份的类型
        /// </summary>
        private TypeOfYear m_CurYearType;

        /// <summary>
        /// 当前年份的类型
        /// </summary>
        public TypeOfYear CurYearType
        {
            get
            {
                return  m_CurYearType;
            }
            set
            {
                m_CurYearType = value;
            }
        }

        /// <summary>
        /// 获取当前年份的天数
        /// </summary>
        public int GetCurYearDays()
        {
            int m_yeardays=0;
            if (CurYearType == TypeOfYear.PingYear)
            {
                m_yeardays = 365;
            }
            else
            {
                m_yeardays = 366;
            }
            return m_yeardays;
        }

        /// <summary>
        /// 获取当前年份的类型
        /// </summary>
        public TypeOfYear GetCurYearType()
        {
            //获取当前年份
            int m_CurYear;
            m_CurYear = Convert.ToInt32(DateTime.Now.Year.ToString());
            /*
             能被4整除且不能被100整除，或者能被400整除的年份为闰年
             */
            if (m_CurYear % 4 == 0 && m_CurYear % 100 != 0 || m_CurYear % 400 == 0)
            {
                m_CurYearType = TypeOfYear.RunYear;
            }
            else
            {
                m_CurYearType = TypeOfYear.PingYear;
            }
            return m_CurYearType;
        }

        /// <summary>
        /// 获取当前月份的天数
        /// </summary>
        public int GetDaysOfMonth(NameOfMonth m_NOM)
        {
            int m_daysofmonth=0;
            switch (m_NOM)
            {
                case NameOfMonth.January:
                    {
                        m_daysofmonth = 31;
                        break;
                    }
                case NameOfMonth.February:
                    {
                        if (CurYearType == TypeOfYear.RunYear)
                        {
                            m_daysofmonth = 29;
                        }
                        else 
                        {
                            m_daysofmonth = 28;
                        }
                        break;
                    }
                case NameOfMonth.March:
                    {
                        m_daysofmonth = 31;
                        break;
                    }
                case NameOfMonth.April:
                    {
                        m_daysofmonth = 30;
                        break;
                    }
                case NameOfMonth.May:
                    {
                        m_daysofmonth = 31;
                        break;
                    }
                case NameOfMonth.June:
                    {
                        m_daysofmonth = 30;
                        break;
                    }
                case NameOfMonth.July:
                    {
                        m_daysofmonth = 31;
                        break;
                    }
                case NameOfMonth.August:
                    {
                        m_daysofmonth = 31;
                        break;
                    }
                case NameOfMonth.September:
                    {
                        m_daysofmonth = 30;
                        break;
                    }
                case NameOfMonth.October:
                    {
                        m_daysofmonth = 31;
                        break;
                    }
                case NameOfMonth.November:
                    {
                        m_daysofmonth = 30;
                        break;
                    }
                case NameOfMonth.December:
                    {
                        m_daysofmonth = 31;
                        break;
                    }
            }
            return m_daysofmonth;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        public DateTime StampToDateTime(string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);

            return dateTimeStart.Add(toNow);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间格式</param>
        public int DateTimeToStamp(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 字符串格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间格式</param>
        public int DateTimeToStamp(string strDateTime)
        {
            DateTime time;
            //如果时间为空，则设置时间为当前的时间
            if(strDateTime == "")
            {
                return 0;
            }else
            {
                time = Convert.ToDateTime(strDateTime);
            }
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        public DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }

        public long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalMilliseconds);
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        ///// <summary>
        ///// 计算日期时间构造函数
        ///// </summary>
        //public CS_CalcDateTime()
        //{
        //    throw new System.NotImplementedException();
        //}

        ///// <summary>
        ///// 计算日期时间析构函数
        ///// </summary>
        //~CS_CalcDateTime()
        //{
        //    throw new System.NotImplementedException();
        //}
    }

   
}
