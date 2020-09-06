using System;
using System.Collections;
using System.Threading;

namespace ThreadPoolMoreTest
{
    class Program
    {
        //这是用来保存信息的数据结构，将作为参数被传递
        public class SomeState
        {
            public int Cookie;
            public SomeState(int iCookie)
            {
                Cookie = iCookie;
            }
        }

        public class Alpha
        {
            public Hashtable HashCount;
            public ManualResetEvent eventX;
            public static int iCount = 0;
            public static int iMaxCount = 0;

            public Alpha(int MaxCount)
            {
                HashCount = new Hashtable(MaxCount);
                iMaxCount = MaxCount;
            }

            //线程池里的线程将调用Beta()方法
            public void Beta(Object state)
            {
                //输出当前线程的hash编码值和Cookie的值
                Console.WriteLine(" {0} {1} :", Thread.CurrentThread.GetHashCode(), ((SomeState)state).Cookie);
                Console.WriteLine("HashCount.Count=={0}, Thread.CurrentThread.GetHashCode()=={1}", HashCount.Count, Thread.CurrentThread.GetHashCode());
                lock (HashCount)
                {
                    //如果当前的Hash表中没有当前线程的Hash值，则添加之
                    if (!HashCount.ContainsKey(Thread.CurrentThread.GetHashCode()))
                        HashCount.Add(Thread.CurrentThread.GetHashCode(), 0);
                    HashCount[Thread.CurrentThread.GetHashCode()] =
                          ((int)HashCount[Thread.CurrentThread.GetHashCode()]) + 1;
                }
                int iX = 2000;
                Thread.Sleep(iX);
                //Interlocked.Increment()操作是一个原子操作，具体请看下面说明
                Interlocked.Increment(ref iCount);

                if (iCount == iMaxCount)
                {
                    Console.WriteLine();
                    Console.WriteLine("Setting eventX ");
                    eventX.Set();
                }
            }
        }

        public class SimplePool
        {
            public static int Main(string[] args)
            {
                Console.WriteLine("Thread Pool Sample:");
                bool w2K = false;
                int maxCount = 10;//允许线程池中运行最多10个线程
                //新建ManualResetEvent对象并且初始化为无信号状态
                ManualResetEvent eventX = new ManualResetEvent(false);
                Console.WriteLine("Queuing {0} items to Thread Pool", maxCount);
                Alpha oAlpha = new Alpha(maxCount);
                //创建工作项
                //注意初始化oAlpha对象的eventX属性
                oAlpha.eventX = eventX;
                Console.WriteLine("Queue to Thread Pool 0");
                try
                {
                    //将工作项装入线程池 
                    //这里要用到Windows 2000以上版本才有的API，所以可能出现NotSupportException异常
                    ThreadPool.QueueUserWorkItem(new WaitCallback(oAlpha.Beta), new SomeState(0));
                    w2K = true;
                }
                catch (NotSupportedException)
                {
                    Console.WriteLine("These API's may fail when called on a non-Windows 2000 system.");
                    w2K = false;
                }
                if (w2K)//如果当前系统支持ThreadPool的方法.
                {
                    for (int iItem = 1; iItem < maxCount; iItem++)
                    {
                        //插入队列元素
                        Console.WriteLine("Queue to Thread Pool {0}", iItem);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(oAlpha.Beta), new SomeState(iItem));
                    }
                    Console.WriteLine("Waiting for Thread Pool to drain");
                    //等待事件的完成，即线程调用ManualResetEvent.Set()方法
                    eventX.WaitOne(Timeout.Infinite, true);
                    //WaitOne()方法使调用它的线程等待直到eventX.Set()方法被调用
                    Console.WriteLine("Thread Pool has been drained (Event fired)");
                    Console.WriteLine();
                    Console.WriteLine("Load across threads");
                    foreach (object o in oAlpha.HashCount.Keys)
                        Console.WriteLine("{0} {1}", o, oAlpha.HashCount[o]);
                }
                Console.ReadLine();
                return 0;
            }
        }
    }
}

