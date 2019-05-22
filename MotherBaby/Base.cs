using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMDLL.Class;
using YMDLL.Common;
using YMDLL.Interface;

namespace MotherBaby
{
  public class Base
  {
    #region ----变量声明----
    /// <summary>
    /// 连接SQL Server数据库的对象
    /// </summary>
    public YM_MySQL _smysql;
    /// <summary>
    /// 连接mysql数据库的对象
    /// </summary>
    public YM_MySQL _dmysql;
    /// <summary>
    /// 时间计算对象
    /// </summary>
    public CS_CalcDateTime _dateTime;
    /// <summary>
    /// 源-表名
    /// </summary>
    public string sTableName;
    /// <summary>
    /// 源-表的列
    /// </summary>
    public string sColumns;
    /// <summary>
    /// 源-表排序的字段
    /// </summary>
    public string sOrder;
    /// <summary>
    /// 源-where语句
    /// </summary>
    public string sWhere;
    /// <summary>
    /// 源-页码
    /// </summary>
    public int sPageIndex;
    /// <summary>
    /// 源-页面大小
    /// </summary>
    public int sPageSize;
    /// <summary>
    /// 源-输出参数-获取的表数据的总结果集数量
    /// </summary>
    public int sTotalCount;
    /// <summary>
    /// 目标-表的列
    /// </summary>
    public string dColumns;
    /// <summary>
    /// 目标-表排序的字段
    /// </summary>
    public string dOrder;
    /// <summary>
    /// 目标-页码
    /// </summary>
    public int dPageIndex;
    /// <summary>
    /// 目标-页面大小
    /// </summary>
    public int dPageSize;
    /// <summary>
    /// 目标-表名
    /// </summary>
    public string dTableName;
    /// <summary>
    /// 目标-表描述
    /// </summary>
    public string dTableDescript;
    /// <summary>
    /// 目标-导入内容描述
    /// </summary>
    public string dPolitContentDescript;
    /// <summary>
    /// 目标-获取的表数据的总结果集数量
    /// </summary>
    public int dTotalCount;
    /// <summary>
    /// 目标-where语句
    /// </summary>
    public string dWhere;
    /// <summary>
    /// 目标-默认时间戳
    /// </summary>
    public string dDefaultTime;
    /// <summary>
    /// 返回的结果
    /// </summary>
    public string m_Result;
    /// <summary>
    /// 源-总行数
    /// </summary>
    public int sRows;
    /// <summary>
    /// 目标-是否删除表数据
    /// </summary>
    public bool dIsDelete;
    /// <summary>
    /// 表名
    /// </summary>
    public string _TableName = "";
    /// <summary>
    /// 线程启用标识
    /// </summary>
    public bool m_ThreadEnabled;
    /// <summary>
    /// 目标-字段添加
    /// </summary>
    public string dFieldAdd;
    /// <summary>
    /// 目标-字段修改
    /// </summary>
    public string dFieldModify;
    /// <summary>
    /// 目标-字段移除
    /// </summary>
    public string dFieldDrop;
    /// <summary>
    /// 目标-更新数据
    /// </summary>
    public string dUpdateData;
    /// <summary>
    /// 目标-执行
    /// </summary>
    public string dExec;
    /// <summary>
    /// 目标-公司Id
    /// </summary>
    public string dCompanyId;
    /// <summary>
    /// 目标-删除标识
    /// </summary>
    public string dDeleteMark;

    /// <summary>
    /// 临时的表名
    /// </summary>
    public string strTable;
    /// <summary>
    /// 临时的值
    /// </summary>
    public string strValues;
    /// <summary>
    /// 临时的条件
    /// </summary>
    public string strWhere;
    #endregion


    /// <summary>
    /// 基类的构造函数
    /// </summary>
    public Base()
    {
      _smysql = new YM_MySQL();
      _dmysql = new YM_MySQL();
      _smysql.Initialize(ReadAppSetting("SourceName"), ReadAppSetting("SourceDB"), ReadAppSetting("SourceUsername"), ReadAppSetting("SourcePassword"), ReadAppSetting("SourcePort"));
      _dmysql.Initialize(ReadAppSetting("DestName"), ReadAppSetting("DestDB"), ReadAppSetting("DestUsername"), ReadAppSetting("DestPassword"), ReadAppSetting("DestPort"));
      sWhere = "1=1";
      sPageIndex = Convert.ToInt32(ReadAppSetting("SourcePageIndex"));
      sPageSize = Convert.ToInt32(ReadAppSetting("SourcePageSize"));
      sTotalCount = 0;
      dWhere = "1=1";
      dPageIndex = Convert.ToInt32(ReadAppSetting("DestPageIndex"));
      dPageSize = Convert.ToInt32(ReadAppSetting("DestPageSize"));
      dTotalCount = 0;
      dCompanyId = ReadAppSetting("DestCompanyId");
      dDeleteMark = "0";
      dDefaultTime = "1472011552";
      m_ThreadEnabled = false;
      _dateTime = new CS_CalcDateTime();
    }

    /// <summary>
    /// 基类的析构函数
    /// </summary>
    ~Base()
    {
      _smysql = null;
      _dmysql = null;
      sTableName = null;
      sColumns = null;
      sOrder = null;
      sWhere = null;
      sPageIndex = 0;
      sPageSize = 0;
      sTotalCount = 0;
      dTableName = null;
      dColumns = null;
      dOrder = null;
      dWhere = null;
      dPageIndex = 0;
      dPageSize = 0;
      dTotalCount = 0;
      m_ThreadEnabled = false;
      _dateTime = null;
    }

    /// <summary>
    /// 读取配置文件中的数据
    /// </summary>
    /// <param name="strKey"></param>
    /// <returns></returns>
    public string ReadAppSetting(string strKey)
    {
      return ConfigurationSettings.AppSettings[strKey].ToString();
    }


    /// <summary>
    /// 过滤特殊字符
    /// </summary>
    /// <returns></returns>
    public string FilterSpecialCharacter(string strValue)
    {
      //去除俩边多余的空格
      string strResult = strValue.Trim();
      //去掉文字中包含的单引号
      strResult = strValue.Replace("'", "");
      //去掉文字中包含的反斜杠符号
      strResult = strResult.Replace("\\", "");
      //去掉文字中包含的换行符号
      strResult = strResult.Replace("\n", "");
      //去掉文字中包含的制表符号
      strResult = strResult.Replace("\r", "");
      //去掉文字中包含的空格符号
      strResult = strResult.Replace(" ", "");
      return strResult;
    }

    /// <summary>
    /// 组合房友数据的值【导入的目标是mysql数据库的时候】
    /// </summary>
    /// Dictionary<DestField, SourceField>
    /// Dictionary<DestField, SourceField:DateTime>
    /// Dictionary<DestField, SourceField:String?Default=Value>
    /// Dictionary<DestField, SourceField:Array?Key1=Value1;Key2=Value2;Key3=Value3>
    /// <returns></returns>
    public string GetConcatValues(Dictionary<string, string> dicData, DataRow drRow)
    {
      string strResult = "";
      string[] saItem;    //是否有数据类型跟随
      string[] strItemOne, strItemTwo, strItemThree;   //是字符串的时候是否有默认值

      foreach (KeyValuePair<string, string> dicItem in dicData)
      {
        if (dicItem.Value.IndexOf(":") == -1)
        {
          if (dicItem.Value.IndexOf(".") != -1)
          {
            string[] arrField = dicItem.Value.Split('.');
            strResult += "'" + FilterSpecialCharacter(drRow[arrField[1]].ToString().Trim()) + "',";
          }
          else
          {
            strResult += "'" + FilterSpecialCharacter(drRow[dicItem.Value].ToString().Trim()) + "',";
          }
        }
        else
        {
          //判断如果值里面的格式是 XXXX:DATETIME 的时候
          saItem = dicItem.Value.Split(':');
          if (saItem[1].ToUpper() == "DATETIME" || saItem[1].ToUpper() == "DATE" || saItem[1].ToUpper() == "TIME")
          {
            strResult += "'" + _dateTime.DateTimeToStamp(drRow[saItem[0]].ToString().Trim()).ToString() + "',";
          }
          //else if(saItem[1].ToUpper() == "DOUBLE")
          //{
          //    strResult += "'" + Convert.ToDouble(drRow[saItem[0]].ToString().Trim()).ToString() + "',";
          //}
          else if (saItem[1].IndexOf('?') != -1)
          {
            strItemOne = saItem[1].Split('?');
            if (strItemOne[0].ToString().Trim().ToUpper() == "STRING")
            {
              //判断是字符串的
              if (strItemOne[1].ToString().IndexOf('=') != -1)
              {
                //判断字符串中有Default或者其他值
                strItemTwo = strItemOne[1].ToString().Trim().Split('=');
                //string strTemp = drRow[saItem[0]].ToString();
                if (strItemTwo[0].ToString().Trim().ToUpper() == "DEFAULT")
                {
                  //增加内容为默认值
                  strResult += "'" + strItemTwo[1].ToString().Trim() + "',";
                }
                else if (strItemTwo[0].ToString().Trim().ToUpper() == "" && drRow[saItem[0]].ToString().Trim() == "")
                {
                  //判断源字段存储的值为空，默认参数的key为空的时候走默认值
                  strResult += "'" + strItemTwo[1].ToString().Trim() + "',";
                }
                else
                {
                  strResult += "'" + FilterSpecialCharacter(drRow[saItem[0]].ToString().Trim()) + "',";
                }
              }
            }
            else if (strItemOne[0].ToString().Trim().ToUpper() == "ARRAY")
            {

            }
          }
        }
      }
      strResult = strResult.Substring(0, strResult.Length - 1);
      return strResult;
    }

    /// <summary>
    /// 合并Key字段(合并目标数据库字段)
    /// </summary>
    /// <param name="dicData"></param>
    /// <returns></returns>
    public string CombineDestField(Dictionary<string, string> dicData)
    {
      string strResult = "";
      foreach (KeyValuePair<string, string> dicItem in dicData)
      {
        strResult += dicItem.Key + ",";
      }
      strResult = strResult.Substring(0, strResult.Length - 1);
      return strResult;
    }

    /// <summary>
    /// 合并Value字段(合并源数据库字段)
    /// </summary>
    /// <param name="dicData"></param>
    /// <returns></returns>
    public string CombineSourceField(Dictionary<string, string> dicData)
    {
      string strResult = "";
      string[] saField;
      foreach (KeyValuePair<string, string> dicItem in dicData)
      {
        if (dicItem.Value.IndexOf(":") == -1)
        {
          strResult += dicItem.Value + ",";
        }
        else
        {
          saField = dicItem.Value.Split(':');
          if (saField[0].ToString().Trim() == "")
          {
            strResult += "";
          }
          else
          {
            strResult += saField[0] + ",";
          }
        }
      }
      strResult = strResult.Substring(0, strResult.Length - 1);
      return strResult;
    }

    #region ----字段操作区域----
    /// <summary>
    /// 添加字段
    /// </summary>
    public void AddField(string FieldName, string FieldType)
    {
      _dmysql.UpdateField("add", dTableName, FieldName, FieldType);
      m_Result = _dmysql.m_Message;
    }

    /// <summary>
    /// 修改字段
    /// </summary>
    public void ModifyField(string FieldName, string FieldType)
    {
      _dmysql.UpdateField("modify column ", dTableName, FieldName, FieldType);
      m_Result = _dmysql.m_Message;
    }

    /// <summary>
    /// 移除字段
    /// </summary>
    public void DropField(string FieldName)
    {
      _dmysql.UpdateField("drop ", dTableName, FieldName, "");
      m_Result = _dmysql.m_Message;
    }
    #endregion

    #region ----索引操作区域----
    /// <summary>
    /// 新增表字段索引
    /// </summary>
    /// <param name="TableName">表名</param>
    /// <param name="IndexCols">字段集合</param>
    /// <param name="IndexName">索引名称</param>
    /// <param name="type">索引类型</param>
    /// <returns></returns>
    public bool CreateIndex(string TableName, string IndexCols, string IndexName, MysqlIndexType type)
    {
      string strsql = "";
      switch (type)
      {
        case MysqlIndexType.INDEX:
          strsql = "create index " + IndexName + " on " + TableName + " (" + IndexCols + ");";
          break;
        case MysqlIndexType.UNIQUE:
          strsql = "create unique index " + IndexName + " on " + TableName + " (" + IndexCols + ");";
          break;
        default:
          return false;
      }
      _dmysql.ExecuteSQL(strsql);
      return true;
    }

    /// <summary>
    /// 修改表字段索引
    /// </summary>
    public bool AlterIndex(List<string> lstField, MysqlIndexType type)
    {
      switch (type)
      {
        case MysqlIndexType.INDEX:

          break;
        case MysqlIndexType.UNIQUE:

          break;
        case MysqlIndexType.PRIMARYKEY:

          break;
      }
      return true;
    }

    /// <summary>
    /// 移除表字段索引
    /// </summary>
    /// <param name="TableName">表名</param>
    /// <param name="IndexName">索引名</param>
    /// <param name="type">索引类型</param>
    /// <returns></returns>
    public bool DropIndex(string TableName, string IndexName, MysqlIndexType type)
    {
      string strsql = "";
      switch (type)
      {
        case MysqlIndexType.INDEX:
          strsql = "drop index " + IndexName + " on " + TableName;
          break;
        default:
          return false;
      }
      _dmysql.ExecuteSQL(strsql);
      return true;
    }
    #endregion
  }
}
