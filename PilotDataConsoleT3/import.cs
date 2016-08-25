using System;
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
                        //执行导数据的方法
                        district.importDistrict();
                        //输出结果
                        strResult = district.m_Result;
                        break;
                    }
                case TableType.AREA:
                    {
                        //声明片区对象
                        Area area = new Area();
                        //执行导数据的方法
                        area.importArea();
                        //更新小区的行政区ID数据
                        area.updateData();
                        //输出结果
                        strResult = area.m_Result;
                        break;
                    }
                case TableType.ESTATE:
                    {
                        //声明楼盘字典对象
                        Estate estate = new Estate();
                        //执行导数据的方法
                        estate.AddField("fy_AreaID", "varchar(100)");
                        estate.importEstate();
                        estate.UpdateData();
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
                        //执行导数据的方法
                        department.AddField("fy_dept_id", "varchar(100)");
                        department.importDepartment();
                        //更新数据
                        department.UpdateData("职能");
                        department.UpdateData("大区");
                        department.UpdateData("其他");
                        //department.UpdateData("更新归属");
                        //输出结果
                        strResult = department.m_Result;
                        break;
                    }
                case TableType.EMPLOYEE:
                    {
                        //声明人员对象
                        Employee employee = new Employee();
                        employee.AddField("fy_PositionId", "varchar(100)");
                        //执行导数据的方法
                        employee.importEmployee();
                        //
                        employee.DropField("fy_PositionId");

                        //增加更新数据的方法调用
                        employee.UpdateData();
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

                        property.AddField("fy_community", "varchar(100)");
                        property.AddField("fy_empid", "varchar(100)");
                        property.AddField("fy_deptid", "varchar(100)");
                        property.AddField("fy_visit_way", "varchar(20)");
                        property.AddField("fy_common_telephone", "varchar(50)");
                        //执行导数据的方法
                        property.importProperty();

                        //更新数据
                        property.UpdateData();

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
                        //新增字段
                        propertyfollow.AddField("fy_followId", "varchar(100)");
                        
                        //执行导数据的方法
                        //propertyfollow.importPropertyFollow();
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
                        //Inquiry inquiry = new Inquiry();
                        ////inquiry.sPageIndex = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageIndex"].ToString());
                        ////inquiry.sPageSize = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageSize"].ToString());
                        ////inquiry.m_ThreadEnabled = Convert.ToBoolean(ConfigurationSettings.AppSettings["ThreadEnabled"].ToString());
                        ////执行导数据的方法
                        //inquiry.importInquiry();
                        ////输出结果
                        //strResult = inquiry.m_Result;
                        break;
                    }
                case TableType.INQUIRYFOLLOW:
                    {
                        //声明客源跟进对象
                        //InquiryFollow inquiryfollow = new InquiryFollow();
                        ////inquiryfollow.sPageIndex = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageIndex"].ToString());
                        ////inquiryfollow.sPageSize = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageSize"].ToString());
                        ////inquiryfollow.m_ThreadEnabled = Convert.ToBoolean(ConfigurationSettings.AppSettings["ThreadEnabled"].ToString());
                        ////执行导数据的方法
                        //inquiryfollow.importInquiryFollow();
                        ////输出结果
                        //strResult = inquiryfollow.m_Result;
                        break;
                    }
                case TableType.CONTRACT:
                    {
                        //声明合同对象
                        //Contract contract = new Contract();
                        ////执行导数据的方法
                        //contract.importContract();
                        ////输出结果
                        //strResult = contract.m_Result;
                        break;
                    }
                case TableType.POSITION:
                    {
                        //声明合同对象
                        Position position = new Position();
                        //执行导数据的方法
                        position.importPosition();
                        //输出结果
                        strResult = position.m_Result;
                        break;
                    }
                case TableType.JOINSTORE:
                    {
                        //声明加盟店对象
                        JoinStores joinstores = new JoinStores();
                        //执行导数据的方法
                        joinstores.importJoinStores();
                        //输出结果
                        strResult = joinstores.m_Result;
                        break;
                    }
                case TableType.TEST:
                    {
                        //声明房源对象
                        Property property = new Property();
                        //property.AddField("fy_community", "varchar(100)");
                        //property.AddField("fy_empid", "varchar(100)");
                        //property.AddField("fy_deptid", "varchar(100)");
                        //property.AddField("fy_visit_way", "varchar(20)");
                        //property.AddField("fy_common_telephone", "varchar(50)");
                        //执行导数据的方法
                        //property.importProperty();
                        property.UpdateData();

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
        CONTRACTACT = 20,
        CONTRACTCOMM = 21,
        CONTRACTCON = 22,
        CONTRACTFEE = 23,
        CONTRACTFOLLOW = 24,

        POSITION = 25,          //职务
        JOINSTORE = 26,         //加盟店

        TEST = 99,              //测试
    }
}
