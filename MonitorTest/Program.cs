using System;
using System.Threading;

namespace MonitorTest
{
    class Program
    {
        private static object ball = new object();

        static void Main(string[] args)
        {
            //创建并启动线程
            Thread threadPing = new Thread(ThreadPingProc);
            Thread threadPong = new Thread(ThreadPongProc);

            threadPing.Start();
            threadPong.Start();
            Console.ReadKey();
        }

        static void ThreadPingProc()
        {
            System.Console.WriteLine("Thread Ping Start!");

            //锁定ball
            lock (ball)
            {
                for (int i = 0; i < 5; i++)
                {
                    System.Console.WriteLine("ThreadPing: Ping ");
                    //通知队列中锁定对象ball的状态更改
                    Monitor.Pulse(ball);
                    //释放ball对象上的锁，并阻止该线程，直到它重新获得ball对象锁
                    Monitor.Wait(ball);
                }

                //通知队列中锁定对象ball的状态更改
                Monitor.Pulse(ball);
            }

            System.Console.WriteLine("ThreadPing: Bye!");
        }

        static void ThreadPongProc()
        {
            System.Console.WriteLine("Thread Pong Start!");

            //锁定ball
            lock (ball)
            {
                for (int i = 0; i < 5; i++)
                {
                    System.Console.WriteLine("ThreadPong: Pong ");
                    //通知队列中锁定对象ball的状态更改
                    Monitor.Pulse(ball);
                    //释放ball对象上的锁，并阻止该线程，直到它重新获得ball对象锁
                    Monitor.Wait(ball);
                }

                //通知队列中锁定对象ball的状态更改
                Monitor.Pulse(ball);
            }

            System.Console.WriteLine("ThreadPong: Bye!");
        }
    }
}
