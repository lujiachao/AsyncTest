using System;
using System.Threading;

namespace ThreadPoolUpperTest
{
    class Program
    {
        const int cycleNum = 10;
        static int cnt = 10;
        static AutoResetEvent myEvent = new AutoResetEvent(false);
        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(5, 5);
            for (int i = 1; i <= cycleNum; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(testFun), i.ToString());
            }
            Console.WriteLine("主线程执行！");
            myEvent.WaitOne();
            Console.WriteLine("线程池终止！");
            Console.WriteLine("主线程结束！");
            Console.ReadKey();
        }
        public static void testFun(object obj)
        {
            cnt -= 1;
            Console.WriteLine(string.Format("{0}:第{1}个线程", DateTime.Now.ToString(), obj.ToString()));
            Thread.Sleep(5000);
            if (cnt == 0)
            {
                myEvent.Set();
            }
        }
    }
}
