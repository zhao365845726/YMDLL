using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadDemo
{
    //public delegate void ThreadStart();

    class Program
    {
        bool done;
        static bool done3;
        static object locker = new object();
        bool upper;
        
        static void Main(string[] args)
        {
            //实例13
            Thread worker = new Thread(delegate() { Console.ReadLine(); });
            if (args.Length > 0)
            {
                worker.IsBackground = true;
            }
            worker.Start();

            //实例12
            //Thread.CurrentThread.Name = "Main";
            //Thread worker = new Thread(Go12);
            //worker.Name = "worker";
            //worker.Start();
            //Go12();

            //实例11
            //Program instance1 = new Program();
            //instance1.upper = true;
            //Thread t = new Thread(instance1.Go11);
            //t.Start();
            //Program instance2 = new Program();
            //instance2.Go11();

            //实例10
            //string strText = "Before";
            //Thread t = new Thread(delegate() { WriteText(strText); });
            //strText = "After";
            //t.Start();

            //实例9
            //Thread t = new Thread(Go6);
            //t.Start(true);
            //Go6(false);

            //实例8
            //Thread t = new Thread(delegate() { Console.WriteLine("Hello!"); });
            //t.Start();

            //实例7
            //Thread t = new Thread(Go5); // No need to explicitly use ThreadStart
            //t.Start();

            //实例六
            //Thread t = new Thread(new ThreadStart(Go5));
            //t.Start();
            //Go5();

            //实例五
            //new Thread(Go4).Start();
            //Go4();

            //实例四
            //new Thread(Go3).Start();
            //Go3();

            //实例三
            //Program tt = new Program();
            //new Thread(tt.Go2).Start();
            //tt.Go2();

            //实例二（死循环）
            //new Thread(Go).Start();
            //Go();

            //实例一
            //Thread t = new Thread(WriteY);
            //t.Start();
            //while (true)
            //{
            //    Console.Write("x");
            //}
        }

        /// <summary>
        /// 实例一方法
        /// </summary>
        static void WriteY()
        {
            while (true)
            {
                Console.Write("y");
            }
        }

        /// <summary>
        /// 实例二方法
        /// </summary>
        static void Go()
        {
            for (int cycles = 0; cycles < 5; cycles++)
            {
                Console.Write('?');
            }
        }

        /// <summary>
        /// 实例三方法
        /// </summary>
        void Go2()
        {
            if (!done)
            {
                done = true;
                Console.WriteLine("Done");
            }
        }

        /// <summary>
        /// 实例四方法
        /// </summary>
        static void Go3()
        {
            if (!done3)
            {
                done3 = true;
                Console.WriteLine("Done");
                //done3 = true;
            }
        }

        /// <summary>
        /// 实例五方法
        /// </summary>
        static void Go4()
        {
            lock (locker)
            {
                if (!done3)
                {
                    Console.WriteLine("Done");
                    done3 = true;
                }
            }
        }

        /// <summary>
        /// 实例六方法
        /// </summary>
        public static void Go5()
        {
            Console.WriteLine("Hello World!");
        }

        /// <summary>
        /// 实例9方法
        /// </summary>
        /// <param name="upperCase"></param>
        static void Go6(object upperCase)
        {
            bool upper = (bool)upperCase;
            Console.WriteLine(upper ? "HELLO!" : "hello!");
        }

        /// <summary>
        /// 实例10方法
        /// </summary>
        /// <param name="strValue"></param>
        static void WriteText(string strValue)
        {
            Console.WriteLine(strValue);
        }

        /// <summary>
        /// 实例11方法
        /// </summary>
        void Go11()
        {
            Console.WriteLine(upper ? "HELLO!" : "hello");
        }

        /// <summary>
        /// 实例12方法
        /// </summary>
        static void Go12()
        {
            Console.WriteLine("hello from " + Thread.CurrentThread.Name);
        }
    }
}
