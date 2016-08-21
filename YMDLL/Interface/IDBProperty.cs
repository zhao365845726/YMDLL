using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMDLL.Interface
{
    public interface IDBProperty
    {
    }

    /// <summary>
    /// 操作方式
    /// </summary>
    public enum OperaType
    {
        CREATE = 1,         //创建
        ALTER = 2,          //修改
        DROP = 3,           //删除
        ADD = 4,            //添加字段
        RENAME = 5,         //重命名
    }

    public enum DataType
    {
        String = 1,      //一月
        Integer = 2,     //二月
        Varchar = 3,        //三月
        April = 4,        //四月
        May = 5,          //五月
        June = 6,         //六月
        July = 7,         //七月
        August = 8,       //八月
        September = 9,    //九月
        October = 10,     //十月
        November = 11,    //十一月
        December = 12     //十二月
    }
}
