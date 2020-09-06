using System;
using System.Threading;

namespace ThreadTest_Lock
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadTest_Lock testLock = new ThreadTest_Lock();
            Thread th1 = new Thread(testLock.Add1);
            Thread th2 = new Thread(testLock.Add2);

            th1.Start();
            th2.Start();
            th1.Join();
            th2.Join();

            Console.WriteLine($"V0：count = {testLock.count}");
            Console.ReadKey();
        }

        public class ThreadTest_Lock
        {
            private object _lockObj = null;
            public int count = 0;

            public ThreadTest_Lock()
            {
                _lockObj = new object();
            }

            public void Add1()
            {
                int index = 0;
                while (index++ < 10000000)//1000万
                {
                    lock (_lockObj)
                    {
                        ++count;
                    }
                }
                Console.WriteLine($"V0: index1 = {index}");
            }

            public void Add2()
            {
                int index = 0;
                while (++index < 10000000)//1000万
                {
                    lock (_lockObj)
                    {
                        count++;
                    }
                }
                Console.WriteLine($"V0: index2 = {index}");
            }
        }
    }
}
