using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventDemo
{
    public class ButtonClickArgs : EventArgs
    {
        public string Clicker;
    }

    public class MyButton
    {
        //定义一个delegate委托
        public delegate void ClickHanlder(object sender, ButtonClickArgs e);
        //定义事件，类型为上面定义的ClickHanlder委托
        public event ClickHanlder OnClick;

        public void Click()
        {
            ///触发之前可能做了n多操作
            ///.....
            
            ///这是触发Click事件，并传入参数Clicker为本博主MartyZane
            OnClick(this, new ButtonClickArgs() { Clicker = "MartyZane" });
        }

    }

    public class Program
    {
        static void Main(string[] args)
        {
            MyButton btn = new MyButton();
            btn.OnClick += new MyButton.ClickHanlder(btn_OnClick);
            Console.Read();
        }

        public static void btn_OnClick(object sender, ButtonClickArgs e)
        {
            Console.WriteLine("真贱，我居然被MartyZane点击了!");
        }
    }
}
