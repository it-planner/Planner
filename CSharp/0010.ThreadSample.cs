using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp
{
    public class ThreadSample
    {
        public static void CreateThread()
        {
            Console.WriteLine($"主线程Id：{Thread.CurrentThread.ManagedThreadId}");
            var thread = new Thread(BusinessProcess);
            thread.Start();
        }

        //线程1
        public static void BusinessProcess()
        {
            Console.WriteLine($"BusinessProcess 线程Id：{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("开始处理业务……");
            //业务实现
            Console.WriteLine("结束处理业务……");
        }

        public static void CreateThreadParameterized()
        {
            Console.WriteLine($"主线程Id：{Thread.CurrentThread.ManagedThreadId}");
            var thread = new Thread(BusinessProcessParameterized);
            //传入参数
            thread.Start("Hello World!");
        }

        //带参业务线程
        public static void BusinessProcessParameterized(object? param)
        {
            Console.WriteLine($"BusinessProcess 线程Id：{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"参数 param 为：{param}");
            Console.WriteLine("开始处理业务……");
            //业务实现
            Console.WriteLine("结束处理业务……");
        }

        public static void CreateThreadLambda()
        {
            Console.WriteLine($"主线程Id：{Thread.CurrentThread.ManagedThreadId}");
            var thread = new Thread(() =>
            {

                Console.WriteLine($"业务线程Id：{Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine("开始处理业务……");
                //业务实现
                Console.WriteLine("结束处理业务……");
            });
            thread.Start();
        }

        public static void CreateThreadLambdaParameterized()
        {
            Console.WriteLine($"主线程Id：{Thread.CurrentThread.ManagedThreadId}");
            var param = "Hello";
            var thread1 = new Thread(() => BusinessProcessParameterized(param));
            thread1.Start();
            param = "World";
            var thread2 = new Thread(() => BusinessProcessParameterized(param));
            thread2.Start();
        }

        //带参业务线程
        public static void BusinessProcessParameterized(string param)
        {
            Console.WriteLine($"业务线程Id：{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"参数 param 为：{param}");
        }

        public static void ThreadSleep()
        {
            Console.WriteLine($"主线程Id：{Thread.CurrentThread.ManagedThreadId}");
            var thread = new Thread(() =>
            {
                Console.WriteLine($"业务线程Id：{Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"暂停线程前：{DateTime.Now:HH:mm:ss}");
                //暂停线程10秒
                Thread.Sleep(10000);
                Console.WriteLine($"暂停线程后：{DateTime.Now:HH:mm:ss}");
            });
            thread.Start();
            thread.Join();
        }

        public static void ThreadException()
        {
            Console.WriteLine($"主线程Id：{Thread.CurrentThread.ManagedThreadId}");
            try
            {
                var thread = new Thread(ThreadThrowException);
                thread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("子线程异常信息：" + ex.Message);
            }
        }

        //业务线程不处理异常，直接抛出
        public static void ThreadThrowException()
        {
            Console.WriteLine($"业务线程Id：{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("开始处理业务……");
            //业务实现
            Console.WriteLine("结束处理业务……");
            throw new Exception("异常");
        }
    }
}
