using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgencyEntity;

namespace PilotData
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
                        City city = new City();
                        //执行导数据的方法
                        city.importCity();
                        //输出结果
                        strResult = city.m_Result;
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
                        //输出结果
                        strResult = area.m_Result;
                        break;
                    }
                case TableType.ESTATE:
                    {
                        //声明楼盘字典对象
                        Estate estate = new Estate();
                        //执行导数据的方法
                        estate.importEstate();
                        //输出结果
                        strResult = estate.m_Result;
                        break;
                    }
                case TableType.BUILDING:
                    {
                        //声明栋座单元对象
                        Building building = new Building();
                        //执行导数据的方法
                        building.importBuilding();
                        //输出结果
                        strResult = building.m_Result;
                        break;
                    }
                case TableType.DEPARTMENT:
                    {
                        //声明部门对象
                        Department department = new Department();
                        //执行导数据的方法
                        department.importDepartment();
                        //输出结果
                        strResult = department.m_Result;
                        break;
                    }
                case TableType.EMPLOYEE:
                    {
                        //声明人员对象
                        Employee employee = new Employee();
                        //执行导数据的方法
                        employee.importEmployee();
                        //输出结果
                        strResult = employee.m_Result;
                        break;
                    }
                case TableType.PROPERTY:
                    {
                        //声明房源对象
                        Property property = new Property();
                        //执行导数据的方法
                        property.importProperty();
                        //输出结果
                        strResult = property.m_Result;
                        break;
                    }
                case TableType.PROPERTYBOOK:
                    {
                        //声明房源收藏夹对象

                        //执行导数据的方法

                        //输出结果

                        break;
                    }
                case TableType.PROPERTYDATA:
                    {
                        //声明房源数据对象

                        //执行导数据的方法

                        //输出结果

                        break;
                    }
                case TableType.PROPERTYINQUIRY:
                    {
                        //声明意向客户对象

                        //执行导数据的方法

                        //输出结果

                        break;
                    }
                case TableType.FOLLOW:
                    {
                        //声明房源跟进对象
                        PropertyFollow propertyfollow = new PropertyFollow();
                        //执行导数据的方法
                        propertyfollow.importPropertyFollow();
                        //输出结果
                        strResult = propertyfollow.m_Result;
                        break;
                    }
                case TableType.FOLLOW2:
                    {
                        //声明房源跟进2对象

                        //执行导数据的方法

                        //输出结果

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
                        //执行导数据的方法
                        inquiry.importInquiry();
                        //输出结果
                        strResult = inquiry.m_Result;
                        break;
                    }
                case TableType.INQUIRYBOOK:
                    {
                        //声明客源收藏夹对象

                        //执行导数据的方法

                        //输出结果

                        break;
                    }
                case TableType.INQUIRYFOLLOW:
                    {
                        //声明客源跟进对象
                        InquiryFollow inquiryfollow = new InquiryFollow();
                        //执行导数据的方法
                        inquiryfollow.importInquiryFollow();
                        //输出结果
                        strResult = inquiryfollow.m_Result;
                        break;
                    }
                case TableType.INQUIRYFOLLOW2:
                    {
                        //声明客源跟进2对象

                        //执行导数据的方法

                        //输出结果

                        break;
                    }
                case TableType.INQUIRYSCHEDULE:
                    {
                        //声明客源进度对象

                        //执行导数据的方法

                        //输出结果

                        break;
                    }
                case TableType.CONTRACT:
                    {
                        //声明合同对象
                        Contract contract = new Contract();
                        //执行导数据的方法
                        contract.importContract();
                        //输出结果
                        strResult = contract.m_Result;
                        break;
                    }
                case TableType.CONTRACTACT:
                    {
                        //声明楼盘字典对象

                        //执行导数据的方法

                        //输出结果

                        break;
                    }
                case TableType.CONTRACTCOMM:
                    {
                        //声明楼盘字典对象

                        //执行导数据的方法

                        //输出结果

                        break;
                    }
                case TableType.CONTRACTCON:
                    {
                        //声明楼盘字典对象

                        //执行导数据的方法

                        //输出结果

                        break;
                    }
                case TableType.CONTRACTFEE:
                    {
                        //声明合同费用对象

                        //执行导数据的方法

                        //输出结果

                        break;
                    }
                case TableType.CONTRACTFOLLOW:
                    {
                        //声明合同跟进对象

                        //执行导数据的方法

                        //输出结果

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
    }
}
