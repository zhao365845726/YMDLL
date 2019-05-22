using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotherBaby;

namespace MotherBaby_Data
{
  public class handler
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
        //case TableType.CITY:
        //    {
        //        //声明城区对象
        //        City city = new City();
        //        //执行导数据的方法
        //        city.importCity();
        //        //输出结果
        //        strResult = city.m_Result;
        //        break;
        //    }
        //case TableType.DISTRICT:
        //    {
        //        //声明行政区对象
        //        District district = new District();
        //        //执行导数据的方法
        //        district.importDistrict();
        //        //输出结果
        //        strResult = district.m_Result;
        //        break;
        //    }
        //case TableType.AREA:
        //    {
        //        //声明片区对象
        //        Area area = new Area();
        //        //执行导数据的方法
        //        area.importArea();
        //        //输出结果
        //        strResult = area.m_Result;
        //        break;
        //    }
        //case TableType.ESTATE:
        //    {
        //        //声明楼盘字典对象
        //        Estate estate = new Estate();
        //        //执行导数据的方法
        //        estate.importEstate();
        //        //输出结果
        //        strResult = estate.m_Result;
        //        break;
        //    }
        //case TableType.BUILDING:
        //    {
        //        //声明栋座单元对象
        //        Building building = new Building();
        //        //执行导数据的方法
        //        building.importBuilding();
        //        //输出结果
        //        strResult = building.m_Result;
        //        break;
        //    }
        //case TableType.DEPARTMENT:
        //    {
        //        //声明部门对象
        //        Department department = new Department();
        //        //执行导数据的方法
        //        department.importDepartment();
        //        //输出结果
        //        strResult = department.m_Result;
        //        break;
        //    }
        //case TableType.EMPLOYEE:
        //    {
        //        //声明人员对象
        //        Employee employee = new Employee();
        //        //执行导数据的方法
        //        employee.importEmployee();
        //        //输出结果
        //        strResult = employee.m_Result;
        //        break;
        //    }
        //case TableType.PROPERTY:
        //    {
        //        //声明房源对象
        //        Property property = new Property();
        //        //property.sPageIndex = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageIndex"].ToString());
        //        //property.sPageSize = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageSize"].ToString());
        //        //property.m_ThreadEnabled = Convert.ToBoolean(ConfigurationSettings.AppSettings["ThreadEnabled"].ToString());
        //        //执行导数据的方法
        //        property.importProperty();
        //        //输出结果
        //        strResult = property.m_Result;
        //        break;
        //    }
        //case TableType.FOLLOW:
        //    {
        //        //声明房源跟进对象
        //        PropertyFollow propertyfollow = new PropertyFollow();
        //        //propertyfollow.sPageIndex = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageIndex"].ToString());
        //        //propertyfollow.sPageSize = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageSize"].ToString());
        //        //propertyfollow.m_ThreadEnabled = Convert.ToBoolean(ConfigurationSettings.AppSettings["ThreadEnabled"].ToString());
        //        //执行导数据的方法
        //        propertyfollow.importPropertyFollow();
        //        //输出结果
        //        strResult = propertyfollow.m_Result;
        //        break;
        //    }
        //case TableType.PHOTO:
        //    {
        //        //声明房源图片对象

        //        //执行导数据的方法

        //        //输出结果

        //        break;
        //    }
        //case TableType.INQUIRY:
        //    {
        //        //声明客源对象
        //        Inquiry inquiry = new Inquiry();
        //        //inquiry.sPageIndex = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageIndex"].ToString());
        //        //inquiry.sPageSize = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageSize"].ToString());
        //        //inquiry.m_ThreadEnabled = Convert.ToBoolean(ConfigurationSettings.AppSettings["ThreadEnabled"].ToString());
        //        //执行导数据的方法
        //        inquiry.importInquiry();
        //        //输出结果
        //        strResult = inquiry.m_Result;
        //        break;
        //    }
        //case TableType.INQUIRYFOLLOW:
        //    {
        //        //声明客源跟进对象
        //        InquiryFollow inquiryfollow = new InquiryFollow();
        //        //inquiryfollow.sPageIndex = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageIndex"].ToString());
        //        //inquiryfollow.sPageSize = Convert.ToInt32(ConfigurationSettings.AppSettings["SourcePageSize"].ToString());
        //        //inquiryfollow.m_ThreadEnabled = Convert.ToBoolean(ConfigurationSettings.AppSettings["ThreadEnabled"].ToString());
        //        //执行导数据的方法
        //        inquiryfollow.importInquiryFollow();
        //        //输出结果
        //        strResult = inquiryfollow.m_Result;
        //        break;
        //    }
        //case TableType.CONTRACT:
        //    {
        //        //声明合同对象
        //        Contract contract = new Contract();
        //        //执行导数据的方法
        //        contract.importContract();
        //        //输出结果
        //        strResult = contract.m_Result;
        //        break;
        //    }
        //case TableType.POSITION:
        //    {
        //        //声明合同对象
        //        Position position = new Position();
        //        //执行导数据的方法
        //        position.importPosition();
        //        //输出结果
        //        strResult = position.m_Result;
        //        break;
        //    }
        //case TableType.TEST:
        //    {
        //        //声明城区对象
        //        City city = new City();
        //        //执行导数据的方法
        //        city.ImportData();
        //        //输出结果
        //        strResult = city.m_Result;
        //        break;
        //    }
      }
    }
  }

  public enum TableType
  {
    TEST = 99,              //测试
  }
}
