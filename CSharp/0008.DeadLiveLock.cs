using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp
{
    public class DeadLiveLock
    {
        //锁1
        private static readonly object lock1 = new();
        //锁2
        private static readonly object lock2 = new();

        //模拟两个线程死锁
        public static void ThreadDeadLock()
        {
            //线程1
            var thread1 = new Thread(Thread1New);
            //线程2
            var thread2 = new Thread(Thread2New);

            //线程1 启动
            thread1.Start();
            //线程2 启动
            thread2.Start();

            //等待 线程1 执行完毕
            thread1.Join();
            //等待 线程2 执行完毕
            thread2.Join();
        }

        //线程1
        public static void Thread1()
        {
            //线程1 首先获取 锁1
            lock (lock1)
            {
                Console.WriteLine("线程1: 已获取 锁1");
                //模拟一些操作
                Thread.Sleep(1000);
                Console.WriteLine("线程1: 等待获取 锁2");
                //线程1 等待 锁2
                lock (lock2)
                {
                    Console.WriteLine("线程1: 已获取 锁2");
                }
            }
        }

        //线程2
        public static void Thread2()
        {
            //线程2 首先获取 锁2
            lock (lock2)
            {
                Console.WriteLine("线程2: 已获取 锁2");
                //模拟一些操作
                Thread.Sleep(1000);
                Console.WriteLine("线程2: 等待获取 锁1");
                //线程2 等待 锁1
                lock (lock1)
                {
                    Console.WriteLine("线程2: 已获取 锁1");
                }
            }
        }


        //线程1
        public static void Thread1New()
        {
            //线程1 首先获取 锁1
            lock (lock1)
            {
                Console.WriteLine("线程1: 已获取 锁1");
                //模拟一些操作
                Thread.Sleep(1000);
                Console.WriteLine("线程1: 等待获取 锁2");
                //线程1 等待 锁2
                lock (lock2)
                {
                    Console.WriteLine("线程1: 已获取 锁2");
                }
            }
        }

        //线程2
        public static void Thread2New()
        {
            //线程2 首先获取 锁2
            lock (lock1)
            {
                Console.WriteLine("线程2: 已获取 锁2");
                //模拟一些操作
                Thread.Sleep(1000);
                Console.WriteLine("线程2: 等待获取 锁1");
                //线程2 等待 锁1
                lock (lock2)
                {
                    Console.WriteLine("线程2: 已获取 锁1");
                }
            }
        }


        //模拟两个任务死锁
        public static async Task TaskDeadLock()
        {
            //启动 任务1
            var task1 = Task.Run(() => Task1());
            //启动 任务2
            var task2 = Task.Run(() => Task2());

            //等待两个任务完成
            await Task.WhenAll(task1, task2);
        }

        //任务1
        public static async Task Task1()
        {
            //任务1 首先获取 锁1
            lock (lock1)
            {
                Console.WriteLine("任务1: 已获取 锁1");

                //模拟一些操作
                Task.Delay(1000).Wait();

                //任务1 等待 锁2
                Console.WriteLine("任务1: 等待获取 锁2");
                lock (lock2)
                {
                    Console.WriteLine("任务1: 已获取 锁2");
                }
            }
        }

        //任务2
        public static async Task Task2()
        {
            //线程2 首先获取 锁2
            lock (lock2)
            {
                Console.WriteLine("任务2: 已获取 锁2");

                //模拟一些操作
                Task.Delay(100).Wait();

                // 任务2 等待 锁1
                Console.WriteLine("任务2: 等待获取 锁1");
                lock (lock1)
                {
                    Console.WriteLine("任务2: 获取 锁1");
                }
            }
        }
    }
}
