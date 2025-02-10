using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp
{
    public class ThreadSync
    {
        #region volatile
        //控制线程的标志
        private static volatile bool _flag = false;
        //计数器
        private static int _counter = 0;

        public static void VolatileRun()
        {
            var thread1 = new Thread(Volatile1);
            var thread2 = new Thread(Volatile2);

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            //Console.WriteLine($"计数器最后的值: {counter}");
        }

        static void Volatile1()
        {
            //注意：以下两行代码可能按相反的顺序执行
            //设置计数器
            _counter = 88;
            //线程1：设置标志位，并且增加计数器
            _flag = true;
        }

        static void Volatile2()
        {
            //注意：_counter可能优先于_flag读取
            //线程2：等待标志位变为 true，然后读取计数器
            //等待 _flag 被设置为 true
            while (!_flag) ;

            //打印计数器值
            Console.WriteLine($"当前计数器的值: {_counter}");
        }
        #endregion

        #region ThreadLocal
        private static ThreadLocal<int> _threadLocalValue = new ThreadLocal<int>(() => 1);

        public static void ThreadLocalRun()
        {
            var thread1 = new Thread(ThreadLocal1);
            var thread2 = new Thread(ThreadLocal2);
            var thread3 = new Thread(ThreadLocal3);

            thread1.Start();
            thread2.Start();
            thread3.Start();
        }

        static void ThreadLocal1()
        {
            Console.WriteLine($"线程 Id : {Environment.CurrentManagedThreadId}，变量值：{_threadLocalValue.Value}");
        }

        static void ThreadLocal2()
        {
            Console.WriteLine($"线程 Id : {Environment.CurrentManagedThreadId}，变量值：{_threadLocalValue.Value}");
        }

        static void ThreadLocal3()
        {
            Console.WriteLine($"线程 Id : {Environment.CurrentManagedThreadId}，变量值：{_threadLocalValue.Value}");
        }

        #endregion

        #region ThreadStatic
        [ThreadStatic]
        public static int _threadStaticValue = 1;

        public static void ThreadStaticRun()
        {
            var thread1 = new Thread(ThreadStatic1);
            var thread2 = new Thread(ThreadStatic2);
            var thread3 = new Thread(ThreadStatic3);

            thread1.Start();
            thread2.Start();
            thread3.Start();
        }

        static void ThreadStatic1()
        {
            Console.WriteLine($"线程 Id : {Environment.CurrentManagedThreadId}，变量值：{_threadStaticValue}");
        }

        static void ThreadStatic2()
        {
            Console.WriteLine($"线程 Id : {Environment.CurrentManagedThreadId}，变量值：{_threadStaticValue}");
        }

        static void ThreadStatic3()
        {
            Console.WriteLine($"线程 Id : {Environment.CurrentManagedThreadId}，变量值：{_threadStaticValue}");
        }

        #endregion

        #region TornRead

        //共享的int64变量
        public static long _var;

        public static void TornRead()
        {
            //启动写入线程
            var writerThread = new Thread(WriteToSharedValue);
            //启动读取线程
            var readerThread = new Thread(ReadFromSharedValue);

            //启动线程
            writerThread.Start();
            readerThread.Start();

            //等待线程执行完成
            writerThread.Join();
            readerThread.Join();
        }

        //写入线程
        static void WriteToSharedValue()
        {
            //模拟分两步写入
            long high = 0x01234567;
            long low = 0x89ABCDEF;

            unsafe
            {
                //将 _var 分成高低两部分写入
                //写高 32 位
                _var = high << 32;
                // 确保读取线程能在这里读取中间值
                Thread.Sleep(0);

                //写低 32 位
                _var |= low;
            }

            Console.WriteLine($"写: 写入值 0x{_var:X16}");
        }

        //读取线程
        static void ReadFromSharedValue()
        {
            // 读取共享变量的值
            Console.WriteLine($"读: 读取值 0x{_var:X16}");
        }

        public static int Max(int val1, int val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        #endregion
    }
}
