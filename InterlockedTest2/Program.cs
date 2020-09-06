using System;
using System.Threading;

namespace InterlockedTest2
{
    class Program
    {
        static int counter = 1;
        static void Main(string[] args)
        {
            Thread t1 = new Thread(new ThreadStart(F1));
            Thread t2 = new Thread(new ThreadStart(F2));

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            System.Console.ReadKey();
        }

        static void F1()
        {
            for (int i = 0; i < 5; i++)
            {
                Interlocked.Increment(ref counter);
                System.Console.WriteLine("Counter++ {0}", counter);
                Thread.Sleep(10);
            }
        }

        static void F2()
        {
            for (int i = 0; i < 5; i++)
            {
                Interlocked.Decrement(ref counter);
                System.Console.WriteLine("Counter-- {0}", counter);
                Thread.Sleep(10);
            }
        }
    }
}
