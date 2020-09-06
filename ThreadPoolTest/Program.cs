using System;
using System.Threading;

namespace ThreadPoolTest
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
            Console.WriteLine("主线程结束！");
            Console.ReadKey();
        }
        public static void testFun(object obj)
        {
            Console.WriteLine(string.Format("{0}:第{1}个线程", DateTime.Now.ToString(), obj.ToString()));
            Thread.Sleep(5000);
        }
    }
}
