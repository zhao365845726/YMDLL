using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ML.Utilities;

namespace MotherBaby
{
  public class Order : Base
  {
    public void Descript()
    {
      
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    public Order()
    {
      sTableName = "tplay_order_source_5";
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
        RandomHelper rh = new RandomHelper();
        //int iStartTime = 1551402000;    //20190301-上午9点
        //int iStartTime = 1554080400;    //20190401-上午9点
        int iStartTime = 1556672400;    //20190501-上午9点

        int iStartRandMinute = 0;       //每天开始时间的随机分钟数
        int iOrderTimeInterval = 0;     //2笔订单之间的随机分钟数
        int iOrderRunTime = 0;          //订单运行时间
        int iEverySecond = 86400;       //24小时的总计秒数
        int iCurDay = 0;                //天的起始，第一天,够30单,进入第二天,同时开始时间增加86400秒
        int iOrderCount = 0;
        int iStart = 0; //订单开始时间
        int iEnd = 0;   //订单结束时间
        int iNextStartTime = 0;   //下一次开始时间
        int iNextStartRandTime = 0;
        foreach (DataRow row in dt.Rows)
        {
          string strTemp = string.Empty;
          if(iOrderCount % 300 == 0)
          {
            iStartRandMinute = rh.GetRandomInt(1, 600);    //每天开始时间的随机秒数
            iOrderRunTime = rh.GetRandomInt(180, 1080);       //订单运行时间
            iNextStartRandTime = rh.GetRandomInt(100, 130);   //每单开始时间的随机数
            //订单开始时间
            iStart = iStartTime + (iCurDay * iEverySecond) + iStartRandMinute;
            //订单结束时间
            iEnd = iStart + iOrderRunTime;
            //下一次开始时间
            iNextStartTime = iStart + iNextStartRandTime;
          }
          else
          {
            //2笔订单之间的时间间隔
            iOrderTimeInterval = rh.GetRandomInt(900, 1200);
            iOrderRunTime = rh.GetRandomInt(180, 1080);       //订单运行时间
            iNextStartRandTime = rh.GetRandomInt(100, 130);   //每单开始时间的随机数
            //订单开始时间
            iStart = iNextStartTime + iOrderTimeInterval;
            //订单结束时间
            iEnd = iStart + iOrderRunTime;
            //下一次开始时间
            iNextStartTime = iStart + iNextStartRandTime;
          }
          strTemp = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}'",
            Convert.ToInt32(row[0].ToString()),
            Convert.ToInt32(row[1].ToString()),
            Convert.ToInt32(iStart.ToString()),
            Convert.ToInt32(iEnd.ToString()),
            Convert.ToInt32(iOrderRunTime.ToString()),
            Convert.ToDouble(row[5].ToString()),
            Convert.ToInt32(iStart.ToString())
            );
          lstValue.Add(strTemp);
          iOrderCount++;
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
