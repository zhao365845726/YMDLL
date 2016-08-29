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
    /// <remarks>数据库操作方式</remarks>
    public enum OperaType
    {
        CREATE = 1,         //创建
        ALTER = 2,          //修改
        DROP = 3,           //删除
        ADD = 4,            //添加字段
        RENAME = 5,         //重命名
    }

    /// <summary>
    /// 月份枚举类型
    /// </summary>
    /// <remarks>月份枚举类型</remarks>
    public enum MonthType
    {
        January = 1,      //一月
        February = 2,     //二月
        Marth = 3,        //三月
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

    /// <summary>
    /// MYSQL数据类型
    /// </summary>
    /// <remarks>MYSQL数据类型</remarks>
    public enum MysqlDataType
    {
        TINYINT = 0,
        SMALLINT = 1,
        MEDIUMINT = 2,
        INT = 3,
        INTEGER = 4,
        BIGINT = 5,
        BIT = 6,
        REAL = 7,
        DOUBLE = 8,
        FLOAT = 9,
        DECIMAL = 10,
        NUMERIC = 11,
        CHAR = 12,
        VARCHAR = 13,
        DATE = 14,
        TIME = 15,
        YEAR = 16,
        TIMESTAMP = 17,
        DATETIME = 18,
        TINYBLOB = 19,
        BLOB = 20,
        MEDIUMBLOB = 21,
        LONGBLOB = 22,
        TINYTEXT = 23,
        TEXT = 24,
        MEDIUMTEXT = 25,
        LONGTEXT = 26,
        ENUM = 27,
        SET = 28,
        BINARY = 29,
        VARBINARY = 30,
        POINT = 31,
        LINESTRING = 32,
        POLYGON = 33,
        GEOMETRY = 34,
        MULTIPOINT = 35,
        MULTILINESTRING = 36,
        MULTIPOLYGON = 37,
        GEOMETRYCOLLECTION = 38,
    }

    /// <summary>
    /// SQLSERVER数据类型
    /// </summary>
    /// <remarks>SQLSERVER数据类型</remarks>
    public enum SqlServerDataType
    {
        BIGINT = 0,
        BINARY = 1,
        BIT = 2,
        CHAR = 3,
        DATE = 4,
        DATETIME = 5,
        DATETIME2 = 6,
        DATETIMEOFFSET = 7,
        DECIMAL = 8,
        FLOAT = 9,
        GEOGRAPHY = 10,
        GEOMETRY = 11,
        HIERARCHYID = 12,
        IMAGE = 13,
        INT = 14,
        MONEY = 15,
        NCHAR = 16,
        NTEXT = 17,
        NUMERIC = 18,
        NVARCHAR = 19,
        REAL = 20,
        SMALLDATETIME = 21,
        SMALLINT = 22,
        SMALLMONEY = 23,
        SQL_VARIANT = 24,
        TEXT = 25,
        TIME = 26,
        TIMESTAMP = 27,
        TINYINT = 28,
        UNIQUEIDENTIFIER = 29,
        VARBINARY = 30,
        VARCHAR = 31,
        XML = 32,
    }
}
