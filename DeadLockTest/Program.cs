using System;
using System.Threading;

namespace DeadLockTest
{
    class Program
    {
        private static object lock_A = new object();
        private static object lock_B = new object();

        public void DoSomething()
        {

            lock (lock_A)
            {
                Thread.Sleep(500);
                Console.WriteLine("我是lock_A,我想要lock_B");
                lock (lock_B)
                {
                    Console.WriteLine("没出现这句话表示死锁了");
                }
            }
        }

        static void Main(string[] args)
        {
            Program a = new Program();
            Thread th = new Thread(new ThreadStart(a.DoSomething));
            th.Start();

            lock (lock_B)
            {

                Console.WriteLine("我是lock_B,我想要lock_A");
                lock (lock_A)
                {
                    Console.WriteLine("没出现这句话表示死锁了");
                }
            }

            Console.WriteLine("没出现这句话表示死锁了");
        }
    }
}
