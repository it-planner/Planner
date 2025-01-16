using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp
{
    public class InitialThread
    {
        public static void CreateThread()
        {
            Console.WriteLine($"主线程 是否为后台线程：{Thread.CurrentThread.IsBackground}");
            var thread1 = new Thread(() => Console.WriteLine("Hello World"));
            Console.WriteLine($" 线程1 默认为后台线程：{thread1.IsBackground}");
            thread1.IsBackground = true;
            Console.WriteLine($" 线程1 设置为后台线程：{thread1.IsBackground}");
            thread1.Start();
        }

        class ThreadPriorityTest
        {
            //是否执行，确保一个线程修改此值后，其他线程立刻查看到最新值
            static volatile bool isRun = true;

            //确保每个线程都有独立的副本存储计数统计值
            [ThreadStatic]
            static long threadCount;

            //停止运行
            public void Stop()
            {
                isRun = false;
            }

            //打印线程名称对应优先级以及计数总数
            public void Print()
            {
                threadCount = 0;
                while (isRun)
                {
                    threadCount++;
                }

                Console.WriteLine($"{Thread.CurrentThread.Name} 优先级为{Thread.CurrentThread.Priority,8} 总执行计数为：{threadCount,-13:N0}");
            }
        }

        public static void PriorityTest()
        {
            //Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(1);
            var threadPriorityTest = new ThreadPriorityTest();

            //创建3个线程，并设置优先级
            var thread1 = new Thread(threadPriorityTest.Print)
            {
                Name = "线程1"
            };

            var thread2 = new Thread(threadPriorityTest.Print)
            {
                Name = "线程2",
                Priority = ThreadPriority.Lowest
            };

            var thread3 = new Thread(threadPriorityTest.Print)
            {
                Name = "线程3",
                Priority = ThreadPriority.Highest
            };

            //启动3个线程
            thread1.Start();
            thread2.Start();
            thread3.Start();

            //休眠3秒
            Thread.Sleep(10000);

            //停止运行
            threadPriorityTest.Stop();

            //等待所有线程完成
            thread1.Join();
            thread2.Join();
            thread3.Join();
        }
    }
}
