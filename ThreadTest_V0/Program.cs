using System;
using System.Threading;

namespace ThreadTest_V0
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadTest_V0 testV0 = new ThreadTest_V0();
            Thread th1 = new Thread(testV0.Add1);
            Thread th2 = new Thread(testV0.Add2);

            th1.Start();
            th2.Start();
            th1.Join();
            th2.Join();

            Console.WriteLine($"V0：count = {testV0.count}");
            Console.ReadKey();
        }

        public class ThreadTest_V0
        {
            public int count = 0;
            public void Add1()
            {
                int index = 0;
                while (index++ < 10000000)//1000万
                {
                    ++count;
                }
                Console.WriteLine($"V0: index1 = {index}");
            }

            public void Add2()
            {
                int index = 0;
                while (++index < 10000000)//1000万
                {
                    count++;
                }
                Console.WriteLine($"V0: index2 = {index}");
            }
        }
    }
}
