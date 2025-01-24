using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp
{
    public class ThreadSync
    {
        //共享的int64变量
        public static long _var;  

        public static void Run()
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
    }
}
