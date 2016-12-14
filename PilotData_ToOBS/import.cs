using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP3ToOBS;
using YMDLL.Interface;

namespace PilotData_ToOBS
{
    public class import
    {
        public string strResult = "";
        public Base bObject = new Base();
        /// <summary>
        /// 执行命令行
        /// </summary>
        /// <param name="Value"></param>
        public void ExecCommand(TableType Value)
        {
            bObject.dFieldAdd = ConfigurationSettings.AppSettings["DestAddField"].ToString();
            bObject.dFieldDrop = ConfigurationSettings.AppSettings["DestDropField"].ToString();
            bObject.dUpdateData = ConfigurationSettings.AppSettings["DestUpdateData"].ToString();
            bObject.dExec = ConfigurationSettings.AppSettings["DestExec"].ToString();
            switch (Value)
            {
                case TableType.USER:
                    {
                        //声明用户对象
                        User user = new User();
                        if(bObject.dFieldAdd == "true")
                        {
                            user.AddField("old_id", "varchar(11)");
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            user.importUser();
                        }
                        if(bObject.dUpdateData == "true")
                        {
                            //更新用户UserId数据
                            user.UpdateData();
                        }
                        //输出结果
                        strResult = user.m_Result;
                        break;
                    }
                case TableType.USERDETAIL:
                    {
                        //声明用户对象
                        UserDetail userdetail = new UserDetail();
                        if (bObject.dFieldAdd == "true")
                        {
                            userdetail.AddField("old_id", "varchar(11)");
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            userdetail.importUserDetail();
                        }
                        if (bObject.dUpdateData == "true")
                        {
                            //更新用户UserId数据
                            userdetail.UpdateData();
                        }
                        //输出结果
                        strResult = userdetail.m_Result;
                        break;
                    }
            }
        }
    }

    public enum TableType
    {
        USER = 0,               //用户基本信息
        USERDETAIL = 1,         //用户详细信息
    }
}
