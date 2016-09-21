using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgencyToERP_PHP;
using YMDLL.Interface;

namespace PilotDataConsoleT3
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
                        if(bObject.dExec == "true")
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
                        if(bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            area.importArea();
                        }
                        if(bObject.dUpdateData == "true")
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
                        if(bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            estate.AddField("fy_AreaID", "varchar(100)");
                        }
                        if(bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            estate.importEstate();
                        }
                        if(bObject.dUpdateData == "true")
                        {
                            //更新数据
                            estate.UpdateData();
                        }
                        if(bObject.dFieldDrop == "true")
                        {
                            estate.DropField("fy_AreaID");
                        }
                        
                        //输出结果
                        strResult = estate.m_Result;
                        break;
                    }
                case TableType.BUILDING:
                    {
                        //声明栋座单元对象
                        Building building = new Building();
                        if (bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            building.AddField("fy_CommunityId", "varchar(100)");
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            building.importBuilding();
                        }
                        if (bObject.dUpdateData == "true")
                        {
                            //更新数据
                            building.UpdateData();
                        }
                        if(bObject.dFieldDrop == "true")
                        {
                            building.DropField("fy_CommunityId");
                        }
                        
                        //输出结果
                        strResult = building.m_Result;

                        break;
                    }
                case TableType.DEPARTMENT:
                    {
                        //声明部门对象
                        Department department = new Department();
                        if(bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            department.AddField("fy_dept_id", "varchar(100)");
                            department.AddField("fy_tel", "varchar(100)");
                        }
                        if(bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            department.importDepartment();
                        }
                        if(bObject.dUpdateData == "true")
                        {
                            //更新数据
                            department.UpdateData("删除");
                            department.UpdateData("父级ID");
                            department.UpdateData("职能");
                            department.UpdateData("大区");
                            department.UpdateData("其他");
                            department.UpdateData("状态");
                            department.UpdateData("加盟店");
                        }
                        if(bObject.dFieldDrop == "true")
                        {
                            department.DropField("fy_dept_id");
                            department.DropField("fy_tel");
                        }

                        //输出结果
                        strResult = department.m_Result;
                        break;
                    }
                case TableType.EMPLOYEE:
                    {
                        //声明人员对象
                        Employee employee = new Employee();
                        if(bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            //employee.AddField("fy_PositionId", "varchar(100)");
                        }
                        if(bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            employee.importEmployee();
                        }
                        if(bObject.dUpdateData == "true")
                        {
                            //增加更新数据的方法调用
                            employee.UpdateData();
                        }
                        if (bObject.dFieldDrop == "true")
                        {
                            //删除字段
                            //employee.DropField("fy_PositionId");
                        }

                        //输出结果
                        strResult = employee.m_Result;
                        break;
                    }
                case TableType.PROPERTY:
                    {
                        //声明房源对象
                        Property property = new Property();
                        if (bObject.dFieldAdd == "true")
                        {
                            property.AddField("fy_community", "varchar(100)");
                            property.AddField("fy_empid", "varchar(100)");
                            property.AddField("fy_deptid", "varchar(100)");
                            property.AddField("fy_building_age", "varchar(50)");
                            property.AddField("fy_visit_way", "varchar(20)");
                            property.AddField("fy_room", "varchar(20)");
                            property.AddField("fy_common_telephone", "varchar(1000)");
                            property.AddField("fy_key_userid", "varchar(100)");
                            property.AddField("fy_key_deptid", "varchar(100)");
                            property.CreateIndex("erp_house", "fy_community", "fy_community", MysqlIndexType.INDEX);
                            property.CreateIndex("erp_house", "fy_empid", "fy_empid", MysqlIndexType.INDEX);
                            property.CreateIndex("erp_house", "fy_key_userid", "fy_key_userid", MysqlIndexType.INDEX);
                            property.CreateIndex("erp_house", "id", "id", MysqlIndexType.INDEX);
                            property.CreateIndex("erp_house", "status", "status", MysqlIndexType.INDEX);
                            property.CreateIndex("erp_house", "if_key", "if_key", MysqlIndexType.INDEX);
                        }

                        if(bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            property.importProperty();
                        }

                        //if(bObject.dUpdateData == "true")
                        //{
                        //    //更新数据
                        //    property.UpdateData();
                        //}

                        //删除字段
                        if(bObject.dFieldDrop == "true")
                        {
                            property.DropField("fy_community");
                            property.DropField("fy_empid");
                            property.DropField("fy_deptid");
                            property.DropField("fy_building_age");
                            property.DropField("fy_room");
                            property.DropField("fy_visit_way");
                            property.DropField("fy_common_telephone");
                            property.DropField("fy_key_userid");
                            property.DropField("fy_key_deptid");
                            property.DropIndex("erp_house", "fy_community", MysqlIndexType.INDEX);
                            property.DropIndex("erp_house", "fy_empid", MysqlIndexType.INDEX);
                            property.DropIndex("erp_house", "fy_key_userid", MysqlIndexType.INDEX);
                            property.DropIndex("erp_house", "id", MysqlIndexType.INDEX);
                            property.DropIndex("erp_house", "status", MysqlIndexType.INDEX);
                            property.DropIndex("erp_house", "if_key", MysqlIndexType.INDEX);
                        }

                        //输出结果
                        strResult = property.m_Result;
                        break;
                    }
                case TableType.FOLLOW:
                    {
                        //声明房源跟进对象
                        PropertyFollow propertyfollow = new PropertyFollow();
                        if (bObject.dFieldAdd == "true")
                        {
                            //新增字段
                            propertyfollow.AddField("fy_followId", "varchar(100)");
                        }
                        //if(bObject.dUpdateData == "true")
                        //{
                        //    propertyfollow.UpdateData();
                        //}
                        if(bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            propertyfollow.importPropertyFollow();
                        }

                        if(bObject.dFieldDrop == "true")
                        {
                            propertyfollow.DropField("fy_followId");
                        }
                        
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
                        if(bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            inquiry.AddField("fy_AreaID", "varchar(100)");
                            inquiry.AddField("fy_DeptID", "varchar(100)");
                            inquiry.AddField("fy_EmpID", "varchar(100)");
                            inquiry.AddField("fy_Contact", "varchar(50)");
                            inquiry.AddField("fy_floor_max", "varchar(100)");
                            inquiry.CreateIndex("erp_client", "fy_EmpID", "fy_EmpID", MysqlIndexType.INDEX);
                            inquiry.CreateIndex("erp_client", "erp_id", "erp_id", MysqlIndexType.INDEX);
                            inquiry.CreateIndex("erp_client", "fy_AreaID", "fy_AreaID", MysqlIndexType.INDEX);
                        }
                        if(bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            inquiry.importInquiry();
                        }
                        if(bObject.dFieldDrop == "true")
                        {
                            //删除字段
                            inquiry.DropField("fy_AreaID");
                            inquiry.DropField("fy_DeptID");
                            inquiry.DropField("fy_EmpID");
                            inquiry.DropField("fy_Contact");
                            inquiry.DropField("fy_floor_max");
                            inquiry.DropIndex("erp_client", "erp_id", MysqlIndexType.INDEX);
                            inquiry.DropIndex("erp_client", "fy_EmpID", MysqlIndexType.INDEX);
                            inquiry.DropIndex("erp_client", "fy_AreaID", MysqlIndexType.INDEX);
                        }
                        //if(bObject.dUpdateData == "true")
                        //{
                        //    //更新数据
                        //    inquiry.UpdateData();
                        //}

                        //输出结果
                        strResult = inquiry.m_Result;
                        break;
                    }
                case TableType.INQUIRYFOLLOW:
                    {
                        //声明客源跟进对象
                        InquiryFollow inquiryfollow = new InquiryFollow();
                        if(bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            inquiryfollow.AddField("fy_followId", "varchar(100)");
                            inquiryfollow.AddField("fy_client_id", "varchar(100)");
                            inquiryfollow.AddField("fy_user_id", "varchar(100)");
                            inquiryfollow.AddField("fy_if_deleted", "varchar(20)");
                            inquiryfollow.CreateIndex("erp_client_follow", "fy_client_id", "fy_client_id", MysqlIndexType.INDEX);
                            inquiryfollow.CreateIndex("erp_client_follow", "fy_user_id", "fy_user_id", MysqlIndexType.INDEX);
                        }
                        if(bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            inquiryfollow.importInquiryFollow();
                        }
                        if(bObject.dFieldDrop == "true")
                        {
                            //删除字段
                            inquiryfollow.DropField("fy_followId");
                            inquiryfollow.DropField("fy_client_id");
                            inquiryfollow.DropField("fy_user_id");
                            inquiryfollow.DropField("fy_if_deleted");
                            inquiryfollow.DropIndex("erp_client_follow", "fy_client_id", MysqlIndexType.INDEX);
                            inquiryfollow.DropIndex("erp_client_follow", "fy_user_id", MysqlIndexType.INDEX);
                        }
                        
                        //输出结果
                        strResult = inquiryfollow.m_Result;
                        break;
                    }
                case TableType.CONTRACT:
                    {
                        //声明合同对象
                        Contract contract = new Contract();
                        if(bObject.dFieldAdd == "true")
                        {
                            contract.CreateIndex("erp_deal", "erp_deal_id", "erp_deal_id", MysqlIndexType.INDEX);
                            contract.CreateIndex("erp_deal", "erp_house_id", "erp_house_id", MysqlIndexType.INDEX);
                            contract.CreateIndex("erp_deal", "erp_client_id", "erp_client_id", MysqlIndexType.INDEX);
                        }
                        if(bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            contract.importContract();
                        }
                        //if(bObject.dUpdateData == "true")
                        //{
                        //    //更新数据
                        //    contract.UpdateData();
                        //}
                        if(bObject.dFieldDrop == "true")
                        {
                            contract.DropIndex("erp_deal", "erp_deal_id", MysqlIndexType.INDEX);
                            contract.DropIndex("erp_deal", "erp_house_id", MysqlIndexType.INDEX);
                            contract.DropIndex("erp_deal", "erp_client_id", MysqlIndexType.INDEX);
                        }
                        
                        //输出结果
                        strResult = contract.m_Result;
                        break;
                    }
                case TableType.CONTRACTACT:
                    {
                        //声明合同实收实付对象
                        ContractAct contractact = new ContractAct();
                        if(bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            contractact.AddField("fy_FeeID", "varchar(100)");
                            contractact.AddField("fy_deal_id", "varchar(100)");
                            contractact.AddField("fy_shou_fee", "varchar(20)");
                            contractact.AddField("fy_fu_fee", "varchar(20)");
                            //contractact.AddField("fy_feedate", "varchar(100)");
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            contractact.importContractAct();
                        }
                        if (bObject.dUpdateData == "true")
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
                        //声明合同实收实付对象
                        ContractComm contractcomm = new ContractComm();
                        if (bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            contractcomm.AddField("fy_DealId", "varchar(100)");
                            contractcomm.AddField("fy_UserId", "varchar(100)");
                            contractcomm.CreateIndex("erp_deal_separate", "fy_DealId", "fy_DealId",MysqlIndexType.INDEX);
                            contractcomm.CreateIndex("erp_deal_separate", "fy_UserId", "fy_UserId", MysqlIndexType.INDEX);
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            contractcomm.importContractComm();
                        }
                        //if (bObject.dUpdateData == "true")
                        //{
                        //    //更新数据
                        //    //contractcomm.UpdateData();
                        //}
                        if(bObject.dFieldDrop == "true")
                        {
                            //删除字段
                            contractcomm.DropField("fy_DealId");
                            contractcomm.DropField("fy_UserId");
                            contractcomm.DropIndex("erp_deal_separate", "fy_DealId", MysqlIndexType.INDEX);
                            contractcomm.DropIndex("erp_deal_separate", "fy_UserId", MysqlIndexType.INDEX);
                        }
                        break;
                    }
                case TableType.CONTRACTCON:
                    {
                        //声明合同实收实付对象
                        ContractCon contractcon = new ContractCon();
                        
                        if (bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            contractcon.AddField("fy_FeeID", "varchar(100)");
                            contractcon.AddField("fy_deal_id", "varchar(100)");
                            contractcon.AddField("fy_shou_fee", "varchar(20)");
                            contractcon.AddField("fy_fu_fee", "varchar(20)");
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            contractcon.importContractCon();
                        }
                        if (bObject.dUpdateData == "true")
                        {
                            //更新数据
                            contractcon.UpdateData();
                        }
                        if (bObject.dFieldDrop == "true")
                        {
                            contractcon.DropField("fy_shou_fee");
                            contractcon.DropField("fy_fu_fee");
                            contractcon.DropField("fy_deal_id");
                            contractcon.DropField("fy_FeeID");
                        }

                        //输出结果
                        strResult = contractcon.m_Result;
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
                        if(bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            position.importPosition();
                        }
                        if(bObject.dUpdateData == "true")
                        {
                            //更新角色数据
                            position.UpdateData();
                        }
                        
                        //输出结果
                        strResult = position.m_Result;
                        break;
                    }
                case TableType.JOINSTORE:
                    {
                        //声明加盟店对象
                        JoinStores joinstores = new JoinStores();
                        if(bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            joinstores.importJoinStores();
                        }
                        
                        //输出结果
                        strResult = joinstores.m_Result;
                        break;
                    }
                case TableType.NEWS:
                    {
                        //声明新闻公告对象
                        News news = new News();
                        if (bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            news.AddField("erp_user_id", "varchar(60)");
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            news.importNews();
                        }
                        if (bObject.dUpdateData == "true")
                        {
                            //更新数据
                            news.UpdateData();
                        }
                        if(bObject.dFieldDrop == "true")
                        {
                            news.DropField("erp_user_id");
                        }

                        //输出结果
                        strResult = news.m_Result;
                        break;
                    }
                case TableType.MESSAGE:
                    {
                        //声明情报站|业务审批对象
                        Message msg = new Message();
                        
                        if (bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            msg.AddField("erp_id", "varchar(60)");
                            msg.AddField("erp_title", "varchar(100)");
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            msg.importMessage();
                        }
                        if (bObject.dUpdateData == "true")
                        {
                            //更新数据
                            msg.UpdateData();
                        }
                        if (bObject.dFieldDrop == "true")
                        {
                            msg.DropField("erp_id");
                            msg.DropField("erp_title");
                        }

                        //输出结果
                        strResult = msg.m_Result;
                        break;
                    }
                case TableType.TEST:
                    {
                        //声明房源对象
                        Property property = new Property();
                        if(bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            property.AddField("fy_community", "varchar(100)");
                            property.AddField("fy_empid", "varchar(100)");
                            property.AddField("fy_deptid", "varchar(100)");
                            property.AddField("fy_visit_way", "varchar(20)");
                            property.AddField("fy_common_telephone", "varchar(1000)");
                        }
                        if(bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            property.importProperty();
                        }
                        if(bObject.dUpdateData == "true")
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

        NEWS = 27,              //新闻公告
        MESSAGE = 28,           //情报站|业务审批

        TEST = 99,              //测试
    }
}
