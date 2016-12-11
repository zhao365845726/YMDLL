using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HzlyToERP3_PHP;
using YMDLL.Interface;

namespace PilotData_Hzly
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
                case TableType.DISTRICT:
                    {
                        //声明行政区对象
                        ta_Dstrict m_District = new ta_Dstrict();
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            m_District.importDistrict();
                        }
                        //输出结果
                        strResult = m_District.m_Result;
                        break;
                    }
                case TableType.AREA:
                    {
                        //声明片区对象
                        ta_PicArea m_PicArea = new ta_PicArea();
                        //添加字段
                        if(bObject.dFieldAdd == "true")
                        {
                            m_PicArea.AddField("old_dsid", "varchar(40)");
                            m_PicArea.AddField("old_areano", "varchar(40)");
                            m_PicArea.AddField("old_flagtrashed", "varchar(40)");
                            m_PicArea.AddField("old_remark", "varchar(200)");
                        }
                        //执行导数据
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            m_PicArea.importPicArea();
                        }
                        //输出结果
                        strResult = m_PicArea.m_Result;
                        break;
                    }
                case TableType.ESTATE:
                    {
                        //声明楼盘字典对象
                        ta_Estate estate = new ta_Estate();
                        if (bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            estate.AddField("old_areaid", "varchar(40)");
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            estate.importEstate();
                        }
                        //输出结果
                        strResult = estate.m_Result;
                        break;
                    }
                case TableType.BUILDING:
                    {
                        //声明栋座单元对象
                        ta_Building building = new ta_Building();
                        if (bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            building.AddField("old_buildingid", "varchar(40)");
                            building.AddField("old_estateid", "varchar(40)");
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            building.importBuilding();
                        }

                        //输出结果
                        strResult = building.m_Result;
                        break;
                    }
                case TableType.DEPARTMENT:
                    {
                        //声明部门对象
                        ta_Department department = new ta_Department();
                        if (bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            department.AddField("old_dept_id", "varchar(40)");
                            department.AddField("old_pid", "varchar(40)");
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            department.importDepartment();
                        }

                        //输出结果
                        strResult = department.m_Result;
                        break;
                    }
                case TableType.EMPLOYEE:
                    {
                        //声明人员对象
                        ta_SystemUser employee = new ta_SystemUser();
                        if (bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            employee.AddField("old_address", "varchar(200)");
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            employee.importEmployee();
                        }

                        //输出结果
                        strResult = employee.m_Result;
                        break;
                    }
                case TableType.EMPLOYEEDETAIL:
                    {
                        //声明人员对象
                        ta_Employee detail = new ta_Employee();
                        if (bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            detail.AddField("old_empid", "varchar(40)");           
                            detail.AddField("old_deptid", "varchar(40)");         
                            detail.AddField("old_empno", "varchar(40)");           
                            detail.AddField("old_status", "varchar(40)");      
                            detail.AddField("old_speciality", "varchar(40)"); 
                            detail.AddField("old_idcard", "varchar(40)");      
                            detail.AddField("old_birthday", "varchar(40)"); 
                            detail.AddField("old_joindate", "varchar(40)");  
                            detail.AddField("old_awaydate", "varchar(40)");  
                            detail.AddField("old_archives", "varchar(40)"); 
                            detail.AddField("old_brief", "varchar(2000)");       
                            detail.AddField("old_remark", "varchar(4000)");     
                            detail.AddField("old_bankname", "varchar(200)");
                            detail.AddField("old_education_status", "varchar(40)"); 
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            detail.importEmployee();
                        }

                        //输出结果
                        strResult = detail.m_Result;
                        break;
                    }
                case TableType.PROPERTY:
                    {
                        //声明房源对象
                        ta_House house = new ta_House();
                        if (bObject.dFieldAdd == "true")
                        {
                            house.AddField("old_houseid", "varchar(40)");
                            house.AddField("old_buildingid", "varchar(40)");
                            house.AddField("old_unitid", "varchar(40)");
                            house.AddField("old_cityid", "varchar(40)");
                            house.AddField("old_districtid", "varchar(40)");
                            house.AddField("old_piceareaid", "varchar(40)");
                            house.AddField("old_estid", "varchar(40)");
                            house.AddField("old_propertyusage", "varchar(40)");
                            house.AddField("old_propertysource", "varchar(40)");
                            house.AddField("old_building_age", "varchar(40)");
                            house.AddField("old_tradetype", "varchar(40)");
                            house.AddField("old_tradestatus", "varchar(40)");
                            house.AddField("old_visit_way", "varchar(40)");
                            house.AddField("old_room", "varchar(40)");
                            house.AddField("old_public", "varchar(40)");

                            house.CreateIndex("erp_house", "old_estid", "old_estid", MysqlIndexType.INDEX);
                            house.CreateIndex("erp_house", "status", "status", MysqlIndexType.INDEX);
                        }

                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            house.importHouse();
                        }

                        //删除字段
                        if (bObject.dFieldDrop == "true")
                        {
                            house.DropField("old_buildingid");
                            house.DropField("old_unitid");
                            house.DropField("old_cityid");
                            house.DropField("old_districtid");
                            house.DropField("old_piceareaid");
                            house.DropField("old_estid");
                            house.DropField("old_propertyusage");
                            house.DropField("old_propertysource");
                            house.DropField("old_building_age");
                            house.DropField("old_tradetype");
                            house.DropField("old_tradestatus");
                            house.DropField("old_visit_way");
                            house.DropField("old_room");
                            house.DropField("old_public");

                            house.DropIndex("erp_house", "old_estid", MysqlIndexType.INDEX);
                            house.DropIndex("erp_house", "status",MysqlIndexType.INDEX);
                        }

                        //输出结果
                        strResult = house.m_Result;
                        break;
                    }
                case TableType.FOLLOW:
                    {
                        //声明房源跟进对象
                        ta_HouseFollow propertyfollow = new ta_HouseFollow();
                        if (bObject.dFieldAdd == "true")
                        {
                            //新增字段
                            propertyfollow.AddField("old_followid", "varchar(40)");
                            propertyfollow.AddField("old_department_id", "varchar(40)");
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            propertyfollow.importHouseFollow();
                        }

                        //输出结果
                        strResult = propertyfollow.m_Result;
                        break;
                    }
                case TableType.INQUIRY:
                    {
                        //声明客源对象
                        ta_Inquiry inquiry = new ta_Inquiry();
                        if (bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            inquiry.AddField("old_inquiryId", "varchar(40)");
                            inquiry.AddField("old_custmobile", "varchar(40)");
                            inquiry.AddField("old_contact", "varchar(50)");
                            inquiry.AddField("old_areaid", "varchar(40)");
                            inquiry.AddField("old_dsid", "varchar(200)");
                            inquiry.AddField("old_piceareaid", "varchar(200)");
                            inquiry.AddField("old_estateid", "varchar(40)");
                            inquiry.AddField("old_propertytype", "varchar(40)");
                            inquiry.AddField("old_propertycommission", "varchar(40)");
                            inquiry.AddField("old_deptid", "varchar(40)");
                            inquiry.AddField("old_modperson", "varchar(40)");
                            
                            inquiry.CreateIndex("erp_client", "old_inquiryId", "old_inquiryId", MysqlIndexType.INDEX);
                            inquiry.CreateIndex("erp_client", "old_areaid", "old_areaid", MysqlIndexType.INDEX);
                            inquiry.CreateIndex("erp_client", "old_dsid", "old_dsid", MysqlIndexType.INDEX);
                            inquiry.CreateIndex("erp_client", "old_piceareaid", "old_piceareaid", MysqlIndexType.INDEX);
                            inquiry.CreateIndex("erp_client", "old_estateid", "old_estateid", MysqlIndexType.INDEX);
                            inquiry.CreateIndex("erp_client", "old_deptid", "old_deptid", MysqlIndexType.INDEX);
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            inquiry.importInquiry();
                        }
                        if (bObject.dFieldDrop == "true")
                        {
                            //删除字段
                            inquiry.DropField("old_inquiryId");
                            inquiry.DropField("old_custmobile");
                            inquiry.DropField("old_contact");
                            inquiry.DropField("old_areaid");
                            inquiry.DropField("old_dsid");
                            inquiry.DropField("old_piceareaid");
                            inquiry.DropField("old_estateid");
                            inquiry.DropField("old_propertytype");
                            inquiry.DropField("old_propertycommission");
                            inquiry.DropField("old_deptid");
                            inquiry.DropField("old_modperson");

                            inquiry.DropIndex("erp_client", "old_inquiryId", MysqlIndexType.INDEX);
                            inquiry.DropIndex("erp_client", "old_areaid", MysqlIndexType.INDEX);
                            inquiry.DropIndex("erp_client", "old_dsid", MysqlIndexType.INDEX);
                            inquiry.DropIndex("erp_client", "old_piceareaid", MysqlIndexType.INDEX);
                            inquiry.DropIndex("erp_client", "old_estateid", MysqlIndexType.INDEX);
                            inquiry.DropIndex("erp_client", "old_deptid", MysqlIndexType.INDEX);
                        }

                        //输出结果
                        strResult = inquiry.m_Result;
                        break;
                    }
                case TableType.INQUIRYFOLLOW:
                    {
                        //声明客源跟进对象
                        ta_InquiryFollow inquiryfollow = new ta_InquiryFollow();
                        if (bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            inquiryfollow.AddField("old_followid", "varchar(40)");
                            inquiryfollow.AddField("old_department_id", "varchar(40)");
                            //inquiryfollow.CreateIndex("erp_client_follow", "erp_client_id", "erp_client_id", MysqlIndexType.INDEX);
                            //inquiryfollow.CreateIndex("erp_client_follow", "erp_user_id", "erp_user_id", MysqlIndexType.INDEX);
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            inquiryfollow.importInquiryFollow();
                        }

                        //输出结果
                        strResult = inquiryfollow.m_Result;
                        break;
                    }
                case TableType.INQUIRYVISIT:
                    {
                        //跟进带看信息导入
                        ta_InquiryVisit inquiryvisit = new ta_InquiryVisit();
                        if(bObject.dExec == "true")
                        {
                            inquiryvisit.importInquiryVisit();
                        }
                        strResult = inquiryvisit.m_Result;
                        break;
                    }
                case TableType.CONTRACT:
                    {
                        //声明合同对象
                        ta_ContractInfo contract = new ta_ContractInfo();
                        if (bObject.dFieldAdd == "true")
                        {
                            contract.CreateIndex("erp_deal", "erp_deal_id", "erp_deal_id", MysqlIndexType.INDEX);
                            contract.CreateIndex("erp_deal", "erp_house_id", "erp_house_id", MysqlIndexType.INDEX);
                            contract.CreateIndex("erp_deal", "erp_client_id", "erp_client_id", MysqlIndexType.INDEX);
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            contract.importContractInfo();
                        }
                        if (bObject.dFieldDrop == "true")
                        {
                            contract.DropIndex("erp_deal", "erp_deal_id", MysqlIndexType.INDEX);
                            contract.DropIndex("erp_deal", "erp_house_id", MysqlIndexType.INDEX);
                            contract.DropIndex("erp_deal", "erp_client_id", MysqlIndexType.INDEX);
                        }

                        //输出结果
                        strResult = contract.m_Result;
                        break;
                    }
                //case TableType.CONTRACTACT:
                //    {
                //        //声明合同实收实付对象
                //        ContractAct contractact = new ContractAct();
                //        if(bObject.dFieldAdd == "true")
                //        {
                //            //添加字段
                //            contractact.AddField("fy_FeeID", "varchar(100)");
                //            contractact.AddField("fy_deal_id", "varchar(100)");
                //            contractact.AddField("fy_shou_fee", "varchar(20)");
                //            contractact.AddField("fy_fu_fee", "varchar(20)");
                //            //contractact.AddField("fy_feedate", "varchar(100)");
                //        }
                //        if (bObject.dExec == "true")
                //        {
                //            //执行导数据的方法
                //            contractact.importContractAct();
                //        }
                //        if (bObject.dUpdateData == "true")
                //        {
                //            //更新数据
                //            contractact.UpdateData();
                //        }

                //        //输出结果
                //        strResult = contractact.m_Result;
                //        break;
                //    }
                //case TableType.CONTRACTCOMM:
                //    {
                //        //声明合同实收实付对象
                //        ContractComm contractcomm = new ContractComm();
                //        if (bObject.dFieldAdd == "true")
                //        {
                //            //添加字段
                //            contractcomm.AddField("fy_DealId", "varchar(100)");
                //            contractcomm.AddField("fy_UserId", "varchar(100)");
                //            contractcomm.CreateIndex("erp_deal_separate", "fy_DealId", "fy_DealId",MysqlIndexType.INDEX);
                //            contractcomm.CreateIndex("erp_deal_separate", "fy_UserId", "fy_UserId", MysqlIndexType.INDEX);
                //        }
                //        if (bObject.dExec == "true")
                //        {
                //            //执行导数据的方法
                //            contractcomm.importContractComm();
                //        }
                //        //if (bObject.dUpdateData == "true")
                //        //{
                //        //    //更新数据
                //        //    //contractcomm.UpdateData();
                //        //}
                //        if(bObject.dFieldDrop == "true")
                //        {
                //            //删除字段
                //            contractcomm.DropField("fy_DealId");
                //            contractcomm.DropField("fy_UserId");
                //            contractcomm.DropIndex("erp_deal_separate", "fy_DealId", MysqlIndexType.INDEX);
                //            contractcomm.DropIndex("erp_deal_separate", "fy_UserId", MysqlIndexType.INDEX);
                //        }
                //        break;
                //    }
                //case TableType.CONTRACTCON:
                //    {
                //        //声明合同实收实付对象
                //        ContractCon contractcon = new ContractCon();

                //        if (bObject.dFieldAdd == "true")
                //        {
                //            //添加字段
                //            contractcon.AddField("fy_FeeID", "varchar(100)");
                //            contractcon.AddField("fy_deal_id", "varchar(100)");
                //            contractcon.AddField("fy_shou_fee", "varchar(20)");
                //            contractcon.AddField("fy_fu_fee", "varchar(20)");
                //        }
                //        if (bObject.dExec == "true")
                //        {
                //            //执行导数据的方法
                //            contractcon.importContractCon();
                //        }
                //        if (bObject.dUpdateData == "true")
                //        {
                //            //更新数据
                //            contractcon.UpdateData();
                //        }
                //        if (bObject.dFieldDrop == "true")
                //        {
                //            contractcon.DropField("fy_shou_fee");
                //            contractcon.DropField("fy_fu_fee");
                //            contractcon.DropField("fy_deal_id");
                //            contractcon.DropField("fy_FeeID");
                //        }

                //        //输出结果
                //        strResult = contractcon.m_Result;
                //        break;
                //    }
                //case TableType.CONTRACTFEE:
                //    {
                //        break;
                //    }
                //case TableType.CONTRACTFOLLOW:
                //    {
                //        break;
                //    }
                case TableType.HOUSELOG:
                    {
                        //声明合同对象
                        ta_HouseLog houselog = new ta_HouseLog();
                        if(bObject.dFieldAdd == "true")
                        {
                            houselog.AddField("old_houseid", "varchar(40)");
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            houselog.importHouseLog();
                        }

                        //输出结果
                        strResult = houselog.m_Result;
                        break;
                    }
                case TableType.NEWS:
                    {
                        //声明新闻公告对象
                        ta_News news = new ta_News();
                        if (bObject.dFieldAdd == "true")
                        {
                            //添加字段
                            news.AddField("old_newsid", "varchar(40)");
                            news.AddField("old_deptid", "varchar(40)");
                            news.AddField("old_dept", "varchar(200)");
                            news.AddField("old_empid", "varchar(40)");
                            news.AddField("old_empname", "varchar(200)");
                        }
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            news.importNews();
                        }

                        //输出结果
                        strResult = news.m_Result;
                        break;
                    }
                //case TableType.MESSAGE:
                //    {
                //        //声明情报站|业务审批对象
                //        Message msg = new Message();

                //        if (bObject.dFieldAdd == "true")
                //        {
                //            //添加字段
                //            msg.AddField("erp_id", "varchar(60)");
                //            msg.AddField("erp_title", "varchar(100)");
                //        }
                //        if (bObject.dExec == "true")
                //        {
                //            //执行导数据的方法
                //            msg.importMessage();
                //        }
                //        if (bObject.dUpdateData == "true")
                //        {
                //            //更新数据
                //            msg.UpdateData();
                //        }
                //        if (bObject.dFieldDrop == "true")
                //        {
                //            msg.DropField("erp_id");
                //            msg.DropField("erp_title");
                //        }

                //        //输出结果
                //        strResult = msg.m_Result;
                //        break;
                //    }
                case TableType.TEST:
                    {
                        //声明行政区对象
                        ta_Dstrict district = new ta_Dstrict();
                        if (bObject.dExec == "true")
                        {
                            //执行导数据的方法
                            district.importDistrict();
                        }
                        //输出结果
                        strResult = district.m_Result;
                        break;
                    }
            }
        }
    }

    public enum TableType
    {
        CITY = 0,               //城市
        DISTRICT = 1,           //行政区
        AREA = 2,               //片区
        ESTATE = 3,             //小区楼盘字典
        BUILDING = 4,           //栋座单元

        DEPARTMENT = 5,         //部门
        EMPLOYEE = 6,           //人员

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
        EMPLOYEEDETAIL = 29,    //人员详情
        HOUSELOG = 30,          //房源日志
        INQUIRYVISIT = 31,      //客源带看

        TEST = 99,              //测试
    }
}
