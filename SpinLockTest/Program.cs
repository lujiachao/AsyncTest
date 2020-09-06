using System;
using System.Threading;

namespace SpinLockTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadTest_V4 testV0 = new ThreadTest_V4();
            Thread th1 = new Thread(testV0.Add1);
            Thread th2 = new Thread(testV0.Add2);

            th1.Start();
            th2.Start();
            th1.Join();
            th2.Join();

            Console.WriteLine($"V0：count = {testV0.count}");
            Console.ReadKey();
        }

        public class ThreadTest_V4
        {
            private Spin spin = new Spin();
            public volatile int count = 0;
            public void Add1()
            {
                int index = 0;
                while (index++ < 1000000)//100万次
                {
                    spin.Enter();
                    count++;
                    spin.Exit();
                }
            }

            public void Add2()
            {
                int index = 0;
                while (index++ < 1000000)//100万次
                {
                    spin.Enter();
                    count++;
                    spin.Exit();
                }
            }
        }

        public struct Spin
        {
            private int m_lock;//0=unlock ,1=lock
            public void Enter()
            {
                while (System.Threading.Interlocked.Exchange(ref m_lock, 1) != 0)
                {
                    //可以限制自旋次数和时间，自动断开退出
                }
            }

            public void Exit()
            {
                System.Threading.Interlocked.Exchange(ref m_lock, 0);
            }
        }
    }
}
