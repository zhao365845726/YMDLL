﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgencyToERP_PHP;

namespace PilotDataConsoleT3
{
    public class import
    {
        public string strResult = "";
        /// <summary>
        /// 执行命令行
        /// </summary>
        /// <param name="Value"></param>
        public void ExecCommand(TableType Value)
        {
            switch (Value)
            {
                case TableType.CITY:
                    {
                        //声明城区对象
                        //City city = new City();
                        ////执行导数据的方法
                        //city.importCity();
                        ////输出结果
                        //strResult = city.m_Result;
                        break;
                    }
                case TableType.DISTRICT:
                    {
                        //声明行政区对象
                        District district = new District();
                        district.dFieldAdd = ConfigurationSettings.AppSettings["DestAddField"].ToString();
                        district.dFieldDrop = ConfigurationSettings.AppSettings["DestDropField"].ToString();
                        district.dUpdateData = ConfigurationSettings.AppSettings["DestUpdateData"].ToString();
                        district.dExec = ConfigurationSettings.AppSettings["DestExec"].ToString();
                        if(district.dExec == "true")
                        {
                            //执行导数据的方法
                            district.importDistrict();
                        }
                        //输出结果
                        strResult = district.m_Result;
                        break;
                    }
                case TableType.AREA:
                    {
                        //声明片区对象
                        Area area = new Area();
                        area.dFieldAdd = ConfigurationSettings.AppSettings["DestAddField"].ToString();
                        area.dFieldDrop = ConfigurationSettings.AppSettings["DestDropField"].ToString();
                        area.dUpdateData = ConfigurationSettings.AppSettings["DestUpdateData"].ToString();
                        area.dExec = ConfigurationSettings.AppSettings["DestExec"].ToString();
                        if(area.dExec == "true")
                        {
                            //执行导数据的方法
                            area.importArea();
                        }
                        if(area.dUpdateData == "true")
                        {
                            //更新小区的行政区ID数据
                            area.updateData();
                        }
                        //输出结果
                        strResult = area.m_Result;
                        break;
                    }
                case TableType.ESTATE:
                    {
                        //声明楼盘字典对象
                        Estate estate = new Estate();
                        estate.dFieldAdd = ConfigurationSettings.AppSettings["DestAddField"].ToString();
                        estate.dFieldDrop = ConfigurationSettings.AppSettings["DestDropField"].ToString();
                        estate.dUpdateData = ConfigurationSettings.AppSettings["DestUpdateData"].ToString();
                        estate.dExec = ConfigurationSettings.AppSettings["DestExec"].ToString();
                        if(estate.dFieldAdd == "true")
                        {
                            //添加字段
                            estate.AddField("fy_AreaID", "varchar(100)");
                        }
                        if(estate.dExec == "true")
                        {
                            //执行导数据的方法
                            estate.importEstate();
                        }
                        if(estate.dUpdateData == "true")
                        {
                            //更新数据
                            estate.UpdateData();
                        }
                        
                        estate.DropField(estate.dFieldAdd);
                        //输出结果
                        strResult = estate.m_Result;
                        break;
                    }
                case TableType.BUILDING:
                    {
                        //声明栋座单元对象
                        //Building building = new Building();
                        ////执行导数据的方法
                        //building.importBuilding();
                        ////输出结果
                        //strResult = building.m_Result;
                        break;
                    }
                case TableType.DEPARTMENT:
                    {
                        //声明部门对象
                        Department department = new Department();
                        department.dFieldAdd = ConfigurationSettings.AppSettings["DestAddField"].ToString();
                        department.dFieldDrop = ConfigurationSettings.AppSettings["DestDropField"].ToString();
                        department.dUpdateData = ConfigurationSettings.AppSettings["DestUpdateData"].ToString();
                        department.dExec = ConfigurationSettings.AppSettings["DestExec"].ToString();
                        if(department.dFieldAdd == "true")
                        {
                            //添加字段
                            department.AddField("fy_dept_id", "varchar(100)");
                        }
                        if(department.dExec == "true")
                        {
                            //执行导数据的方法
                            department.importDepartment();
                        }
                        if(department.dUpdateData == "true")
                        {
                            //更新数据
                            //department.UpdateData("职能");
                            //department.UpdateData("大区");
                            //department.UpdateData("其他");
                            //department.UpdateData("状态");
                            department.UpdateData("更新归属");
                        }

                        //输出结果
                        strResult = department.m_Result;
                        break;
                    }
                case TableType.EMPLOYEE:
                    {
                        //声明人员对象
                        Employee employee = new Employee();
                        employee.dFieldAdd = ConfigurationSettings.AppSettings["DestAddField"].ToString();
                        employee.dFieldDrop = ConfigurationSettings.AppSettings["DestDropField"].ToString();
                        employee.dUpdateData = ConfigurationSettings.AppSettings["DestUpdateData"].ToString();
                        employee.dExec = ConfigurationSettings.AppSettings["DestExec"].ToString();
                        if(employee.dFieldAdd == "true")
                        {
                            //添加字段
                            employee.AddField("fy_PositionId", "varchar(100)");
                        }
                        if(employee.dExec == "true")
                        {
                            //执行导数据的方法
                            employee.importEmployee();
                        }
                        if(employee.dFieldDrop == "true")
                        {
                            //删除字段
                            employee.DropField("fy_PositionId");
                        }
                        if(employee.dUpdateData == "true")
                        {
                            //增加更新数据的方法调用
                            employee.UpdateData();
                        }
                        
                        //输出结果
                        strResult = employee.m_Result;
                        break;
                    }
                case TableType.PROPERTY:
                    {
                        //声明房源对象
                        Property property = new Property();
                        property.sPageIndex = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageIndex"].ToString());
                        property.sPageSize = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageSize"].ToString());
                        property.dFieldAdd = ConfigurationSettings.AppSettings["DestAddField"].ToString();
                        property.dFieldDrop = ConfigurationSettings.AppSettings["DestDropField"].ToString();
                        property.dUpdateData = ConfigurationSettings.AppSettings["DestUpdateData"].ToString();
                        property.dExec = ConfigurationSettings.AppSettings["DestExec"].ToString();


                        if (property.dFieldAdd == "true")
                        {
                            property.AddField("fy_community", "varchar(100)");
                            property.AddField("fy_empid", "varchar(100)");
                            property.AddField("fy_deptid", "varchar(100)");
                            property.AddField("fy_visit_way", "varchar(20)");
                            property.AddField("fy_common_telephone", "varchar(50)");
                            property.AddField("fy_building_age", "varchar(50)");
                            property.AddField("fy_room", "varchar(20)");
                        }

                        if(property.dExec == "true")
                        {
                            //执行导数据的方法
                            property.importProperty();
                        }

                        if(property.dUpdateData == "true")
                        {
                            //更新数据
                            property.UpdateData();
                        }

                        //删除字段
                        if(property.dFieldDrop == "true")
                        {
                            property.DropField("fy_community");
                            property.DropField("fy_empid");
                            property.DropField("fy_deptid");
                            property.DropField("fy_visit_way");
                            property.DropField("fy_common_telephone");
                            property.DropField("fy_building_age");
                            property.DropField("fy_room");
                        }

                        //输出结果
                        strResult = property.m_Result;
                        break;
                    }
                case TableType.FOLLOW:
                    {
                        //声明房源跟进对象
                        PropertyFollow propertyfollow = new PropertyFollow();
                        propertyfollow.sPageIndex = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageIndex"].ToString());
                        propertyfollow.sPageSize = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageSize"].ToString());
                        ////propertyfollow.m_ThreadEnabled = Convert.ToBoolean(ConfigurationSettings.AppSettings["ThreadEnabled"].ToString());
                        propertyfollow.dFieldAdd = ConfigurationSettings.AppSettings["DestAddField"].ToString();
                        propertyfollow.dFieldDrop = ConfigurationSettings.AppSettings["DestDropField"].ToString();
                        propertyfollow.dUpdateData = ConfigurationSettings.AppSettings["DestUpdateData"].ToString();
                        propertyfollow.dExec = ConfigurationSettings.AppSettings["DestExec"].ToString();

                        if (propertyfollow.dFieldAdd == "true")
                        {
                            //新增字段
                            propertyfollow.AddField("fy_followId", "varchar(100)");
                        }

                        //执行导数据的方法
                        propertyfollow.importPropertyFollow();
                        //输出结果
                        strResult = propertyfollow.m_Result;
                        break;
                    }
                case TableType.PHOTO:
                    {
                        //声明房源图片对象

                        //执行导数据的方法

                        //输出结果

                        break;
                    }
                case TableType.INQUIRY:
                    {
                        //声明客源对象
                        Inquiry inquiry = new Inquiry();
                        inquiry.sPageIndex = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageIndex"].ToString());
                        inquiry.sPageSize = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageSize"].ToString());
                        //inquiry.m_ThreadEnabled = Convert.ToBoolean(ConfigurationSettings.AppSettings["ThreadEnabled"].ToString());
                        inquiry.dFieldAdd = ConfigurationSettings.AppSettings["DestAddField"].ToString();
                        inquiry.dFieldDrop = ConfigurationSettings.AppSettings["DestDropField"].ToString();
                        inquiry.dUpdateData = ConfigurationSettings.AppSettings["DestUpdateData"].ToString();
                        inquiry.dExec = ConfigurationSettings.AppSettings["DestExec"].ToString();

                        if(inquiry.dFieldAdd == "true")
                        {
                            //添加字段
                            inquiry.AddField("fy_AreaID", "varchar(100)");
                            inquiry.AddField("fy_DeptID", "varchar(100)");
                            inquiry.AddField("fy_EmpID", "varchar(100)");
                            inquiry.AddField("fy_Contact", "varchar(50)");
                        }
                        if(inquiry.dExec == "true")
                        {
                            //执行导数据的方法
                            inquiry.importInquiry();
                        }
                        if(inquiry.dFieldDrop == "true")
                        {
                            //删除字段
                            inquiry.DropField("fy_AreaID");
                            inquiry.DropField("fy_DeptID");
                            inquiry.DropField("fy_EmpID");
                            inquiry.DropField("fy_Contact");
                        }
                        if(inquiry.dUpdateData == "true")
                        {
                            //更新数据
                            inquiry.UpdateData();
                        }

                        //输出结果
                        strResult = inquiry.m_Result;
                        break;
                    }
                case TableType.INQUIRYFOLLOW:
                    {
                        //声明客源跟进对象
                        InquiryFollow inquiryfollow = new InquiryFollow();
                        inquiryfollow.sPageIndex = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageIndex"].ToString());
                        inquiryfollow.sPageSize = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageSize"].ToString());
                        //inquiryfollow.m_ThreadEnabled = Convert.ToBoolean(ConfigurationSettings.AppSettings["ThreadEnabled"].ToString());
                        inquiryfollow.dFieldAdd = ConfigurationSettings.AppSettings["DestAddField"].ToString();
                        inquiryfollow.dFieldDrop = ConfigurationSettings.AppSettings["DestDropField"].ToString();
                        inquiryfollow.dUpdateData = ConfigurationSettings.AppSettings["DestUpdateData"].ToString();
                        inquiryfollow.dExec = ConfigurationSettings.AppSettings["DestExec"].ToString();

                        if(inquiryfollow.dFieldAdd == "true")
                        {
                            //添加字段
                            inquiryfollow.AddField("fy_followId", "varchar(100)");
                        }
                        if(inquiryfollow.dExec == "true")
                        {
                            //执行导数据的方法
                            inquiryfollow.importInquiryFollow();
                        }
                        
                        //输出结果
                        strResult = inquiryfollow.m_Result;
                        break;
                    }
                case TableType.CONTRACT:
                    {
                        //声明合同对象
                        Contract contract = new Contract();
                        contract.sPageIndex = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageIndex"].ToString());
                        contract.sPageSize = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageSize"].ToString());
                        contract.dFieldAdd = ConfigurationSettings.AppSettings["DestAddField"].ToString();
                        contract.dFieldDrop = ConfigurationSettings.AppSettings["DestDropField"].ToString();
                        contract.dUpdateData = ConfigurationSettings.AppSettings["DestUpdateData"].ToString();
                        contract.dExec = ConfigurationSettings.AppSettings["DestExec"].ToString();

                        if(contract.dExec == "true")
                        {
                            //执行导数据的方法
                            contract.importContract();
                        }
                        if(contract.dUpdateData == "true")
                        {
                            //更新数据
                            contract.UpdateData();
                        }
                        
                        //输出结果
                        strResult = contract.m_Result;
                        break;
                    }
                case TableType.CONTRACTACT:
                    {
                        //声明合同实收实付对象
                        ContractAct contractact = new ContractAct();
                        contractact.sPageIndex = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageIndex"].ToString());
                        contractact.sPageSize = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageSize"].ToString());
                        contractact.dFieldAdd = ConfigurationSettings.AppSettings["DestAddField"].ToString();
                        contractact.dFieldDrop = ConfigurationSettings.AppSettings["DestDropField"].ToString();
                        contractact.dUpdateData = ConfigurationSettings.AppSettings["DestUpdateData"].ToString();
                        contractact.dExec = ConfigurationSettings.AppSettings["DestExec"].ToString();
                        if(contractact.dFieldAdd == "true")
                        {
                            //添加字段
                            contractact.AddField("fy_FeeID", "varchar(100)");
                            contractact.AddField("fy_deal_id", "varchar(100)");
                            contractact.AddField("fy_shou_fee", "varchar(20)");
                            contractact.AddField("fy_fu_fee", "varchar(20)");
                            //contractact.AddField("fy_feedate", "varchar(100)");

                        }
                        if (contractact.dExec == "true")
                        {
                            //执行导数据的方法
                            contractact.importContractAct();
                        }
                        if (contractact.dUpdateData == "true")
                        {
                            //更新数据
                            contractact.UpdateData();
                        }

                        //输出结果
                        strResult = contractact.m_Result;
                        break;
                    }
                case TableType.CONTRACTCOMM:
                    {
                        break;
                    }
                case TableType.CONTRACTCON:
                    {
                        break;
                    }
                case TableType.CONTRACTFEE:
                    {
                        break;
                    }
                case TableType.CONTRACTFOLLOW:
                    {
                        break;
                    }
                case TableType.POSITION:
                    {
                        //声明合同对象
                        Position position = new Position();
                        position.dFieldAdd = ConfigurationSettings.AppSettings["DestAddField"].ToString();
                        position.dFieldDrop = ConfigurationSettings.AppSettings["DestDropField"].ToString();
                        position.dUpdateData = ConfigurationSettings.AppSettings["DestUpdateData"].ToString();
                        position.dExec = ConfigurationSettings.AppSettings["DestExec"].ToString();

                        if(position.dExec == "true")
                        {
                            //执行导数据的方法
                            position.importPosition();
                        }
                        
                        //输出结果
                        strResult = position.m_Result;
                        break;
                    }
                case TableType.JOINSTORE:
                    {
                        //声明加盟店对象
                        JoinStores joinstores = new JoinStores();
                        joinstores.dFieldAdd = ConfigurationSettings.AppSettings["DestAddField"].ToString();
                        joinstores.dFieldDrop = ConfigurationSettings.AppSettings["DestDropField"].ToString();
                        joinstores.dUpdateData = ConfigurationSettings.AppSettings["DestUpdateData"].ToString();
                        joinstores.dExec = ConfigurationSettings.AppSettings["DestExec"].ToString();

                        if(joinstores.dExec == "true")
                        {
                            //执行导数据的方法
                            joinstores.importJoinStores();
                        }
                        
                        //输出结果
                        strResult = joinstores.m_Result;
                        break;
                    }
                case TableType.TEST:
                    {
                        //声明房源对象
                        Property property = new Property();
                        property.dFieldAdd = ConfigurationSettings.AppSettings["DestAddField"].ToString();
                        property.dFieldDrop = ConfigurationSettings.AppSettings["DestDropField"].ToString();
                        property.dUpdateData = ConfigurationSettings.AppSettings["DestUpdateData"].ToString();
                        property.dExec = ConfigurationSettings.AppSettings["DestExec"].ToString();
                        if(property.dFieldAdd == "true")
                        {
                            //添加字段
                            property.AddField("fy_community", "varchar(100)");
                            property.AddField("fy_empid", "varchar(100)");
                            property.AddField("fy_deptid", "varchar(100)");
                            property.AddField("fy_visit_way", "varchar(20)");
                            property.AddField("fy_common_telephone", "varchar(50)");
                        }
                        if(property.dExec == "true")
                        {
                            //执行导数据的方法
                            property.importProperty();
                        }
                        if(property.dUpdateData == "true")
                        {
                            //更新数据
                            property.UpdateData();
                        }

                        //输出结果
                        strResult = property.m_Result;
                        break;
                    }
            }
        }
    }

    public enum TableType
    {
        CITY = 0,           //城市
        DISTRICT = 1,       //行政区
        AREA = 2,           //片区
        ESTATE = 3,         //小区楼盘字典
        BUILDING = 4,       //栋座单元

        DEPARTMENT = 5,     //部门
        EMPLOYEE = 6,       //人员

        PROPERTY = 7,           //房源
        PROPERTYBOOK = 8,       //房源收藏夹
        PROPERTYDATA = 9,       //
        PROPERTYINQUIRY = 10,   //意向客源
        FOLLOW = 11,            //房源跟进1
        FOLLOW2 = 12,            //房源跟进2
        PHOTO = 13,             //房源图片

        INQUIRY = 14,           //客源
        INQUIRYBOOK = 15,       //客源收藏夹
        INQUIRYFOLLOW = 16,     //客源跟进1
        INQUIRYFOLLOW2 = 17,    //客源跟进2
        INQUIRYSCHEDULE = 18,   //客源任务进度

        CONTRACT = 19,          //合同
        CONTRACTACT = 20,       //合同实收实付费用
        CONTRACTCOMM = 21,      //
        CONTRACTCON = 22,       //合同应收应付费用
        CONTRACTFEE = 23,       //合同费用
        CONTRACTFOLLOW = 24,    //合同跟进

        POSITION = 25,          //职务
        JOINSTORE = 26,         //加盟店

        TEST = 99,              //测试
    }
}
