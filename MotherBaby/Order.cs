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
      sOrder = "roomid";
      dTableName = "tplay_order_dest_1";
      dTableDescript = "订单表";
      dColumns = "roomid,userid,start_time,end_time,use_duration,cost,create_time";
    }

    /// <summary>
    /// 导入订单
    /// </summary>
    public void importOrdersOld()
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
    /// 导入订单
    /// </summary>
    public void importOrders()
    {
      //取数据的变量(仅作为初始计算值)
      //int iQStartTime = 1551369600;   //20190301000000
      //int iQStartTime = 1554048000;   //20190401000000
      int iQStartTime = 1556640000;   //20190501000000

      //实际每天的开始时间变量
      int iActualStartTime = 0;
      
      int iAStartTime = 32400;        //实际距离开始时间的时间间隔9小时
      int iDayIndex = 0;              //天数索引
      int iDayTotalSecond = 86400;    //一天的总秒数
      RandomHelper rh = new RandomHelper();

      while(iActualStartTime <= 1559318400)
      {
        iActualStartTime = iQStartTime + (iDayIndex * iDayTotalSecond);
        sWhere = " AND start_time Between " + iActualStartTime.ToString() + " AND " + (iActualStartTime + iDayTotalSecond).ToString();
        //获取3月1日的所有设备数据
        DataTable dt = _smysql.GetPager(sTableName, "roomid,count(1) as room_used_time", sWhere, "roomid", "roomid", sPageSize, sPageIndex, out sTotalCount);
        foreach (DataRow dr in dt.Rows)
        {
          //声明列表对象
          List<String> lstValue = new List<String>();
          sWhere = " 1=1 AND roomid = '" + dr[0].ToString() + "' AND start_time Between " + iActualStartTime.ToString() + " AND " + (iActualStartTime + iDayTotalSecond).ToString();
          //sWhere = " 1=1 AND roomid = '" + dr[0].ToString() + "' ";
          DataTable dtRoom = _smysql.GetPager(sTableName, sColumns, sOrder, sPageSize, sPageIndex, sWhere, out sTotalCount);
          int iCurDayRuntimeTotal = 36000;    //当天运行总时间
          int iRemainder = 0;
          int iOrderAvgInterval = Math.DivRem(iCurDayRuntimeTotal, dtRoom.Rows.Count, out iRemainder);    //订单平均时间间隔

          //开始计算当天这台设备的开始时间及时间间隔
          int iStartTime = iActualStartTime + iAStartTime;    //20190301-上午9点
          //int iStartTime = 1554080400;    //20190401-上午9点
          //int iStartTime = 1556672400;    //20190501-上午9点

          int iCurOrderIndex = 0;         //当前订单索引
          int iStartRandMinute = 0;       //每天开始时间的随机秒数
          int iOrderRunTime = 0;          //订单运行时间
          int iStart = 0; //订单开始时间
          int iEnd = 0;   //订单结束时间
          int iNextStartTime = 0;   //下一次开始时间
          int iNextStartRandTime = 0;

          foreach (DataRow row in dtRoom.Rows)
          {
            iOrderRunTime = rh.GetRandomInt(180, 1080);       //订单运行时间
            iNextStartRandTime = rh.GetRandomInt(10, 50);   //每单开始时间的随机数
            if (iCurOrderIndex == 0)
            {
              iStartRandMinute = rh.GetRandomInt(1, 600);    //每天开始时间的随机秒数
              //订单开始时间
              iStart = iStartTime + iStartRandMinute;
              //订单结束时间
              iEnd = iStart + iOrderRunTime;
              if((iStart + iOrderAvgInterval) > iEnd)
              {
                //下一次开始时间
                iNextStartTime = iStart + iOrderAvgInterval;
              }
              else
              {
                iNextStartTime = iEnd + iNextStartRandTime;
              }
            }
            else
            {
              iStartRandMinute = rh.GetRandomInt(1, 600);    //每天开始时间的随机秒数
              //订单开始时间
              iStart = iNextStartTime + iStartRandMinute;
              //订单结束时间
              iEnd = iStart + iOrderRunTime;
              if ((iStart + iOrderAvgInterval) > iEnd)
              {
                //下一次开始时间
                iNextStartTime = iStart + iOrderAvgInterval;
              }
              else
              {
                iNextStartTime = iEnd + iNextStartRandTime;
              }
            }

            string strTemp = string.Empty;
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
            iCurOrderIndex++;
          }
          if (lstValue.Count > 0)
          {
            //写入目标表
            _dmysql.BatchInsert(dTableName, dColumns, lstValue);
          }
          else
          {
            continue;
          }
        }
        iDayIndex++;
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
        int iStartTime = 1551402000;    //20190301-上午9点
        //int iStartTime = 1554080400;    //20190401-上午9点
        //int iStartTime = 1556672400;    //20190501-上午9点

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


    /// <summary>
    /// 分页数据
    /// </summary>
    /// <returns></returns>
    public bool PagerDataBackup()
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
          if (iOrderCount % 300 == 0)
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
