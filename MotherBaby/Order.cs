using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherBaby
{
  public class Order : Base
  {
    public void Descript()
    {
      /* 1.目标库中b_bus_area中的外键删除了，主键去掉了，并默认为空
       */
    }

    /// <summary>
    /// 城区类的构造函数
    /// </summary>
    public Order()
    {
      sTableName = "tplay_order_dest";
      sColumns = "roomid,userid,start_time,end_time,use_duration,cost,create_time";
      sOrder = "start_time";
      dTableName = "tplay_order_dest_1";
      dTableDescript = "订单表";
      dColumns = "roomid,userid,start_time,end_time,use_duration,cost,create_time";
    }

    /// <summary>
    /// 导入订单
    /// </summary>
    public void importOrders()
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
          string strTemp = string.Empty;
          strTemp = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}'",
            Convert.ToInt32(row[0].ToString()),
            Convert.ToInt32(row[1].ToString()),
            Convert.ToInt32(row[2].ToString()),
            Convert.ToInt32(row[3].ToString()),
            Convert.ToInt32(row[4].ToString()),
            Convert.ToDouble(row[5].ToString()),
            Convert.ToInt32(row[6].ToString())
            );
          lstValue.Add(strTemp);
        }
        //如果允许删除，清空目标表数据
        if (dIsDelete == true)
        {
          _dmysql.Delete(dTableName, null);
        }
        //插入数据并返回插入的结果
        bool isResult = _dmysql.BatchInsert(dTableName, dColumns, lstValue);
        if (isResult)
        {
          m_Result = "订单数据插入成功";
          return true;
        }
        else
        {
          m_Result = "订单数据插入失败";
          return false;
        }
      }
      catch (Exception ex)
      {
        m_Result = "导出订单异常.\n异常原因：" + ex.Message;
        return false;
      }

    }
  }
}
