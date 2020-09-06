using System;
using System.Threading;

namespace VolatileTest
{
    public class Program
    {

        public class Volatile
        {
            public volatile int falg = 0;
        }

        static void VolatileTest()
        {
            Volatile volatiler = new Volatile();

            new Thread(
               p =>
               {
                   Thread.Sleep(1000);
                   volatiler.falg = 255;
               }).Start();

            while (true)
            {
                if (volatiler.falg == 255)
                {
                    break;
                }
            };

            Console.WriteLine("OK");
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("--start");
            VolatileTest();
            Console.WriteLine("--stop");
            Console.ReadKey();
        }
    }
}
