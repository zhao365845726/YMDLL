using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
using YMDLL.Common;
using YMDLL.Class;

namespace AgencyToERP_PHP 
{
    public class ContractFollow : Base
    {
        public void Descript()
        {
            //Dictionary<目标数据库,源数据库>
        }

        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("contract_code", "ContractNo");      //合同编号
            dicMap.Add("house_code", "PropertyNo");         //房源编号
            dicMap.Add("client_code", "InquiryNo");         //客源编号
            dicMap.Add("erp_house_id", "PropertyID");       //房源ID
            dicMap.Add("erp_client_id", "InquiryID");       //客源ID
            dicMap.Add("client_name", "CustName");          //客户姓名
            dicMap.Add("client_tel", "CustMobile");         //客户电话
            dicMap.Add("client_address", "CustAdd");        //客户地址
            dicMap.Add("client_id_card", "CustCard");       //客户身份证
            dicMap.Add("erp_deal_id", "ContractID");        //成交ID
            dicMap.Add("type", "Trade");                    //交易状态：出售|出租（old);买卖|租赁（new)
            dicMap.Add("house_address", "Address");         //房源地址
            dicMap.Add("payment", "Price");                 //成交价格
            dicMap.Add("payarea", "Square");                //成交面积
            dicMap.Add("purpose", "Usage");                 //房屋用途
            dicMap.Add("licence", "CertificateNo");         //房产证号
            dicMap.Add("contract_category", "ContractType");//成交类别
            dicMap.Add("owner_name", "OwnerName");          //业主姓名
            dicMap.Add("owner_tel", "OwnerMobile");         //业主电话
            dicMap.Add("owner_address", "OwnerAdd");        //业主地址
            dicMap.Add("owner_id_card", "OwnerCard");       //业主身份证
            dicMap.Add("contractor_place", "SignCenter");   //签约地点|签约中心
            dicMap.Add("deal_status", "Audit");             //合同审核状态
            return dicMap;
        }

        /// <summary>
        /// 合同类的构造函数
        /// </summary>
        public ContractFollow()
        {
            sTableName = "Contract";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "RegDate";
            dTableName = "erp_deal";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导合同信息
        /// </summary>
        public void importContractFollow()
        {
            PagerData();

            sRows = sTotalCount;

            //数据的总数小于等于页面大小数时候
            if (sRows > sPageSize)
            {
                //统计页数
                int totalPage = 0;
                if (sRows % sPageSize > 0)
                {
                    totalPage = (sRows / sPageSize) + 1;
                    //如果有多页的，增加第二页数据的时候就不许删除
                    dIsDelete = false;
                }
                else
                {
                    totalPage = (sRows / sPageSize);
                }

                for (int i = 1; i < totalPage; i++)
                {
                    sPageIndex = sPageIndex + 1;
                    PagerData();
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        /// <returns></returns>
        public bool PagerData()
        {
            try
            {
                DataTable dt = _sqlServer.GetPager(sTableName, sColumns, sOrder, sPageSize, sPageIndex, sWhere, out sTotalCount);

                List<String> lstValue = new List<String>();
                foreach (DataRow row in dt.Rows)
                {
                    string strTemp = GetConcatValues(FieldMap(), row);

                    lstValue.Add(strTemp);
                }
                //如果允许删除，清空目标表数据
                if (dIsDelete == true)
                {
                    _mysql.Delete(dTableName, null);
                }
                //插入数据并返回插入的结果
                bool isResult = _mysql.BatchInsert(dTableName, dColumns, lstValue);
                Console.Write("\n数据已经成功写入" + sPageSize * sPageIndex + "条");
                if (isResult)
                {
                    m_Result = "\n合同数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "\n合同数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出合同异常.\n异常原因：" + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新关联数据
        /// </summary>
        public bool UpdateData()
        {
            string tmpTable = "", tmpValues = "", tmpWhere = "";
            try
            {
                tmpTable = "erp_deal";
                //更新交易状态字段
                tmpValues = "type = '买卖'";
                tmpWhere = "and type = '出售'";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);
                //更新合同类别字段
                tmpValues = "type = '租赁'";
                tmpWhere = "and type = '出租'";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);
                //更新合同表中company_id字段和if_deleted字段
                tmpValues = "company_id = '" + dCompanyId + "',if_deleted = '" + dDeleteMark + "'";
                tmpWhere = "";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);
                //更新合同表审核状态字段
                tmpValues = "deal_status = '已审核'";
                tmpWhere = "";
                _mysql.Update(tmpTable, tmpValues, tmpWhere);

                //tmpTable = "erp_deal as a,erp_house as b";
                //tmpValues = "a.room_code = b.room_code,a.block = b.block,a.district = b.district";
                //tmpValues = "a.district = b.district,a.district_id = b.district_id,a.region = b.region,a.biz_area_id = b.region_id,a.community = b.community,a.community_id = b.community_id,a.block = b.block,a.block_id = b.block_id,a.unit_name = b.unit_name,a.unit_id = b.unit_id,a.room_code = b.room_code,a.room_id = b.room_id";
                //tmpWhere = "and a.erp_house_id = b.erp_id and a.erp_house_id <> ''";
                //_mysql.Update(tmpTable, tmpValues, tmpWhere);


                m_Result += "\n合同表中交易类型|房源信息更新成功";
                return true;
            }
            catch (Exception ex)
            {
                m_Result += "\n合同表中交易类型|房源信息更新异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
