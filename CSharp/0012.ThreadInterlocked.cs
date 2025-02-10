using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static CSharp.ThreadInterlocked;

namespace CSharp
{
    public class ThreadInterlocked
    {
        #region Read
        private static long _readValue = 0;

        public static void ReadRun()
        {
            var thread = new Thread(ModifyReadValue);
            thread.Start();

            Thread.Sleep(100);

            var value = Interlocked.Read(ref _readValue);
            Console.WriteLine("原子读取long类型变量: " + value);
        }

        static void ModifyReadValue()
        {
            _readValue = 88;
            Console.WriteLine("修改long类型变量: " + _readValue);
        }
        #endregion

        #region Increment
        private static long _incrementValue = 0;

        public static void IncrementRun()
        {
            //运行9次测试，观察每次结果
            for (var i = 1; i < 10; i++)
            {
                //启动100个线程，对变量进行递增
                var threads = new Thread[100];
                for (var j = 0; j < threads.Length; j++)
                {
                    threads[j] = new Thread(ModifyIncrementValue);
                    threads[j].Start();
                }

                //等待所有线程执行完成
                foreach (var thread in threads)
                {
                    thread.Join();
                }

                //最后打印结果
                Console.WriteLine($"第 {i} 运行结果: {_incrementValue}");
                _incrementValue = 0;
            }
        }

        static void ModifyIncrementValue()
        {
            for (var i = 0; i < 1000; i++)
            {
                //++_incrementValue;
                Interlocked.Increment(ref _incrementValue);
            }
        }

        #endregion

        #region Add
        private static long _addValue = 0;

        public static void AddRun()
        {
            for (var j = 0; j < 1000; j++)
            {
                //_addValue =_ addValue + j;
                Interlocked.Add(ref _addValue, j);
            }

            Console.WriteLine($"累加结果: {_addValue}");
            _addValue = 0;

            for (var j = 0; j < 1000; j++)
            {
                //_addValue =_ addValue - j;
                Interlocked.Add(ref _addValue, -j);
            }

            Console.WriteLine($"累减结果: {_addValue}");
        }

        #endregion

        #region Exchange
        //0 表示未锁定，1 表示锁定
        private static long _exchangeValue = 0;

        public static void ExchangeRun()
        {
            var rnd = new Random();
            //启动10个线程
            var threads = new Thread[10];
            for (var j = 0; j < threads.Length; j++)
            {
                threads[j] = new Thread(ModifyExchangeValue);
                threads[j].Start();
                //等待一段随机的时间后再开始启动另一个线程
                Thread.Sleep(rnd.Next(0, 100));
            }
        }

        static void ModifyExchangeValue()
        {
            //更新_exchangeValue为1，同时获取_exchangeValue旧值
            var oldExchangeValue = Interlocked.Exchange(ref _exchangeValue, 1);
            //如果旧值为0，表示该逻辑未被其他线程占用
            if (0 == oldExchangeValue)
            {
                //当前线程开始锁定该代码块，其他线程无法进入
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} 线程 进入锁");

                //模拟一些工作
                //这里可以实现安全的处理非线程安全资源访问的代码
                Thread.Sleep(100);

                //释放锁
                Interlocked.Exchange(ref _exchangeValue, 0);

                //当前线程释放完锁，其他线程可以进入该代码块
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} 线程 退出锁");
            }
            else
            {
                Console.WriteLine($"    {Thread.CurrentThread.ManagedThreadId} 线程 无法进入锁");
            }
        }

        #endregion


        #region CompareExchange
        //版本化数据
        public class VersionedData<T>(T data)
        {
            private T _data = data;
            private long _version = 0;

            //获取当前数据和版本号
            public (T Data, long Version) GetData()
            {
                return (_data, _version);
            }

            //基于版本号尝试更新数据
            public bool TryUpdateData(T data, long expectedVersion)
            {
                //如果_version与预期版本号相同，
                //则对预期版本号加1后再替换为_version，
                //同时返回_version旧值
                var oldVersion = Interlocked.CompareExchange(ref _version, expectedVersion + 1, expectedVersion);
                //如果_version旧值与预期版本号相同
                if (oldVersion == expectedVersion)
                {
                    //则版本号匹配，更新数据
                    _data = data;
                    return true;
                }

                //否则版本号不匹配，更新失败
                return false;
            }
        }

        public static void CompareExchangeRun()
        {
            var versionedData = new VersionedData<string>("初始化数据");

            //线程 1 尝试更新数据
            var thread1 = new Thread(ModifyCompareExchangeValue);

            //线程 2 尝试更新数据
            var thread2 = new Thread(ModifyCompareExchangeValue);

            thread1.Start(versionedData);
            thread2.Start(versionedData);

            thread1.Join();
            thread2.Join();

            //最终结果
            var (finalData, finalVersion) = versionedData.GetData();
            Console.WriteLine($"最终数据为 [{finalData}], 最终版本号为 [{finalVersion}]");
        }

        static void ModifyCompareExchangeValue(object param)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var versionedData = (VersionedData<string>)param;
            var (data, version) = versionedData.GetData();
            Console.WriteLine($"线程 {threadId} : 当前数据为 [{data}], 当前版本号为 [{version}]");
            Console.WriteLine("---------------------------------------------------");

            var newData = $"线程 {threadId} 数据";
            var success = versionedData.TryUpdateData(newData, version);
            Console.WriteLine($"线程 {threadId} 更新数据： [{(success ? "成功" : "失败")}]");
            Console.WriteLine($"    数据预期更新为：[{newData}]");
            Console.WriteLine($"    版本号预期更新为：[{version + 1}]");
            Console.WriteLine("---------------------------------------------------");
        }

        #endregion


        #region And

        public static void AndRun()
        {
            var a = 10; // 二进制: 1010
            var b = 5; // 二进制: 0101
            
            var oldA = Interlocked.And(ref a, b);

            //1010 & 0101 = 0000 = 0
            Console.WriteLine("操作后的值: " + a); 
            Console.WriteLine("返回的结果: " + oldA); 
        }


        #endregion
    }
}
