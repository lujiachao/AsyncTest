using System;
using System.Threading;

namespace MutexTest
{
    public class MutexSample
    {
        static Mutex gM1;
        static Mutex gM2;
        static Mutex gM3;
        static AutoResetEvent Event1 = new AutoResetEvent(false);
        static AutoResetEvent Event2 = new AutoResetEvent(false);

        public static void Main(String[] args)
        {
            Console.WriteLine("Mutex Sample ");
            //创建一个Mutex对象，并且命名为MyMutex
            gM1 = new Mutex(true, "MyMutex");
            //创建一个未命名的Mutex 对象.
            gM2 = new Mutex(true);
            gM3 = new Mutex(true, ",MutexTest");
            Console.WriteLine(" - Main Owns gM1 and gM2");

            AutoResetEvent[] evs = new AutoResetEvent[2];
            evs[0] = Event1;
            evs[1] = Event2;

            MutexSample tm = new MutexSample();
            Thread t1 = new Thread(new ThreadStart(tm.t1Start));
            Thread t2= new Thread(new ThreadStart(tm.t2Start));
            t1.Start();// 使用Mutex.WaitOne()方法等待gM1的释放
            t2.Start();// 使用Mutex.WaitOne()方法等待gM2的释放

            Thread.Sleep(2000);
            Console.WriteLine(" - Main releases gM1");
            gM1.ReleaseMutex(); //线程t1结束条件满足

            Thread.Sleep(1000);
            Console.WriteLine(" - Main releases gM2");
            gM2.ReleaseMutex(); //线程t2结束条件满足
                

            //等待所有四个线程结束
            WaitHandle.WaitAll(evs);
            Console.WriteLine(" Mutex Sample");
            Console.ReadLine();
        }

        public void t1Start()
        {
            Console.WriteLine("t1Start started, gM1.WaitOne( )");
            gM1.WaitOne();//等待gM1的释放
            Console.WriteLine("t1Start finished, gM1.WaitOne( ) satisfied");
            Event1.Set();//线程结束，将Event2设置为有信号状态
        }

        public void t2Start()
        {
            Console.WriteLine("t2Start started, gM2.WaitOne( )");
            gM2.WaitOne();//等待gM2被释放
            Console.WriteLine("t2Start finished, gM2.WaitOne( )");
            Event2.Set();//线程结束，将Event4设置为有信号状态
        }
    }

}
