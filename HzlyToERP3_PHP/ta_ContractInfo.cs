using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
using YMDLL.Common;
using YMDLL.Class;

namespace HzlyToERP3_PHP
{
    public class ta_ContractInfo : Base
    {
        /// <summary>
        /// 字段映射方法
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FieldMap()
        {
            //Dictionary<目标数据库,源数据库>
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            dicMap.Add("contract_code", "contractno");      //合同编号
            dicMap.Add("house_code", "propertyno");         //房源编号
            dicMap.Add("client_code", "inquiryno");         //客源编号
            dicMap.Add("erp_house_id", "houseid");       //房源ID
            dicMap.Add("erp_client_id", "inquiryid");       //客源ID
            dicMap.Add("client_name", "custname");          //客户姓名
            dicMap.Add("client_tel", "custmobile");         //客户电话
            dicMap.Add("client_address", "cusaddress");        //客户地址
            dicMap.Add("client_id_card", "custcard");       //客户身份证
            dicMap.Add("erp_deal_id", "contractinfoid");        //成交ID
            dicMap.Add("type", "tradetype");                    //交易状态：出售|出租（old);买卖|租赁（new)
            dicMap.Add("house_address", "address");         //房源地址
            dicMap.Add("payment", "sellprice");                 //成交价格
            dicMap.Add("payarea", "square");                //成交面积
            dicMap.Add("purpose", "propertyusage");                 //房屋用途
            //dicMap.Add("licence", "CertificateNo");         //房产证号
            dicMap.Add("contract_category", "contracttype");//成交类别
            dicMap.Add("owner_name", "owername");          //业主姓名
            dicMap.Add("owner_tel", "ownemobile");         //业主电话
            dicMap.Add("owner_address", "adress");        //业主地址
            dicMap.Add("owner_id_card", "ownercard");       //业主身份证
            //dicMap.Add("contractor_place", "SignCenter");   //签约地点|签约中心
            dicMap.Add("deal_status", "auditstate");             //合同审核状态
            dicMap.Add("company_id", ":String?Default=" + dCompanyId);          //公司ID
            dicMap.Add("if_deleted", ":String?Default=" + dDeleteMark);        //删除标记
            return dicMap;
        }

        /*

status
propertysource
inquirysource
idcard
rentprice
renunitname
rentpriceunit
unitname
priceunit
ownertel
ownerpostalcode
ownernationality
cusphone
cuspostalcode
cusnationality
signingdate
signingplace
signingstaffdep
deptid
signingstaff
signingstaffid
signingheaddep
signingheaddepid
signinghead
signingheadid
additionalterms
paycommissiondate
finisheddate
auditdate

deldate
gxfyyzxx
gxkykhxx
remark
commissionmark
discmark
agentfee
startrent
endrent
dsid
piceareaid
regperson
regpersonid
regdept
regdeptid
regdate
trustno
propertytype
flagdeleted
delperson
numgist
flagsuits
signingprivy
deadline

             */

        /// <summary>
        /// 合同实收费用类的构造函数
        /// </summary>
        public ta_ContractInfo()
        {
            sTableName = "ta_contractinfo";
            sColumns = CombineSourceField(FieldMap());
            sOrder = "RegDate";
            dTableName = "erp_deal";
            dTableDescript = "合同成交表";
            dPolitContentDescript = "交易类型|房源信息";
            dColumns = CombineDestField(FieldMap());
        }

        /// <summary>
        /// 导合同实收费用信息
        /// </summary>
        public void importContractInfo()
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
                DataTable dt = _smysql.GetPager(sTableName, sColumns, sOrder, sPageSize, sPageIndex, sWhere, out sTotalCount);

                List<String> lstValue = new List<String>();
                foreach (DataRow row in dt.Rows)
                {
                    string strTemp = GetConcatValues(FieldMap(), row);

                    lstValue.Add(strTemp);
                }
                //如果允许删除，清空目标表数据
                if (dIsDelete == true)
                {
                    _dmysql.Delete(dTableName, null);
                }
                //插入数据并返回插入的结果
                bool isResult = _dmysql.BatchInsert(dTableName, dColumns, lstValue);
                Console.Write("\n数据已经成功写入" + sPageSize * sPageIndex + "条");
                if (isResult)
                {
                    m_Result = "\n" + dTableDescript + "数据插入成功";
                    return true;
                }
                else
                {
                    m_Result = "\n" + dTableDescript + "数据插入失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                m_Result = "导出" + dTableDescript + "异常.\n异常原因：" + ex.Message;
                return false;
            }
        }
    }
}
