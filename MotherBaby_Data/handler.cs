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
        case TableType.ORDER:
          {
            //声明对象
            Order order = new Order();
            //导入数据
            order.importOrders();
            //输出结果
            strResult = order.m_Result;
            break;
          }
        case TableType.TEST:
          {
            break;
          }
      }
    }
  }

  public enum TableType
  {
    ORDER = 0,              //订单
    ROOM = 1,               //设备

    TEST = 99,              //测试
  }
}
