using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using YMDLL.Class;

namespace YMDLL.Common
{
    public class StringHelper
    {
        /// <summary>
        /// 字符串帮助类构造函数
        /// </summary>
        public StringHelper()
        {
            _dateTime = new CS_CalcDateTime();
        }

        /// <summary>
        /// 字符串帮助类析构函数
        /// </summary>
        ~StringHelper()
        {
            throw new System.NotImplementedException();
        }

        public CS_CalcDateTime _dateTime;
        /// <summary>
        /// 字典结果
        /// </summary>
        public string DicResult { get; set; }

        /// <summary>
        /// 解析字典
        /// </summary>
        /// <param name="dicData">字典数据</param>
        /// <param name="row">元组数据</param>
        public string ParseDictionary(Dictionary<String, String> dicData, DataRow row)
        {
            string strResult = "";
            string[] saItem;    //是否有数据类型跟随
            string[] strItemOne, strItemTwo, strItemThree;   //是字符串的时候是否有默认值
            string[] strSplit = new string[2];

            foreach (KeyValuePair<string, string> dicItem in dicData)
            {
                //如果没有#号的标识符
                if(dicItem.Value.IndexOf("#") == -1)
                {
                    //字典的结果等于当前目标库字段的值
                    DicResult += "'" + FilterChar(row[dicItem.Value].ToString().Trim()) + "',";
                }
                else    //如果存在#号标识符
                {
                    //判断如果值里面的格式是 XXXX#DATETIME 的时候
                    saItem = dicItem.Value.Split('#');
                    if (saItem[1].ToUpper() == "DATETIME" || 
                        saItem[1].ToUpper() == "DATE" || 
                        saItem[1].ToUpper() == "TIME")
                    {
                        strResult += "'" + _dateTime.DateTimeToStamp(row[saItem[0]].ToString().Trim()).ToString() + "',";
                    }
                    else if(saItem[1].IndexOf("=") != -1)
                    {
                        //三目运算符存在
                        strItemOne = saItem[1].Split('=');
                        if (strItemOne[0].ToString().Trim().ToUpper() == "STRING")
                        {
                            //判断是字符串是否为空值--判断如果字段名为空的时候赋值为默认值
                            if (row[saItem[0]].ToString().Trim() == "" ||
                                saItem[0].ToString().Trim() == "")
                            {
                                //如果为空值，则赋值为默认值
                                row[saItem[0]] = strItemOne[1].ToString().Trim();
                                DicResult += "'" + row[saItem[0]] + "',";
                            }else
                            {
                                DicResult += "'" + FilterChar(row[saItem[0]].ToString().Trim()) + "',";
                            }
                        }
                        else if (strItemOne[0].ToString().Trim().ToUpper() == "ARRAY")
                        {
                            if(Regex.IsMatch(strItemOne[1].ToString().Trim(), "[^{\\w*}$]"))
                            {
                                DicResult += "";
                            }
                            else
                            {
                                DicResult += "";
                            }
                        }
                    }
                    else if (saItem[1].IndexOf('?') != -1)
                    {
                        strItemOne = saItem[1].Split('?');

                    }
                }
            }
            strResult = strResult.Substring(0, strResult.Length - 1);
            return strResult;
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <returns></returns>
        public string FilterChar(string strValue)
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
    }
}
