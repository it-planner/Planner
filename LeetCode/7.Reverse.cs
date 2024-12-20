using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LeetCode
{
    public class _7_Reverse
    {
        //字符串long
        public static int ReverseLongString(int x)
        {
            //是否为负数
            var isNegative = x < 0;
            //取绝对值,必须要先转为long类型
            //否则int.MinValue -2147483648会报错
            var abs = Math.Abs((long)x);
            //把值转为字符串并反转，得到字符集合
            var reversedChars = abs.ToString().Reverse();
            if (isNegative)
            {
                //如果是负数则在字节数组前加入负号'-'
                reversedChars = reversedChars.Prepend('-');
            }

            //转换为long类型，并且是有效的int值，则返回结果
            if (long.TryParse(reversedChars.ToArray(), out long result) && result >= int.MinValue && result <= int.MaxValue)
            {
                return (int)result;
            }

            return 0;
        }

        //字符串int
        public static int ReverseIntString(int x)
        {
            //把值转为字符串，并去掉负号'-'，最后反转，得到字符集合
            var reversed = x.ToString().TrimStart('-').Reverse();

            //转换为int，成功则返回
            if (int.TryParse(reversed.ToArray(), out int result))
            {
                //根据原始符号，返回结果
                return x < 0 ? -result : result;
            }

            return 0;
        }

        //数学方法
        public static int ReverseMath(int x)
        {
            var result = 0;
            while (x != 0)
            {
                //判断溢出,因为输入的是32位的有符号整数 x
                //即输入的 -2147483648<=x<=2147483647
                //所以翻转后的最后一位是1或2并不会导致溢出
                //因此只需判断九位数 > int.MaxValue / 10 或者 < int.MinValue / 10
                if (result < int.MinValue / 10 || result > int.MaxValue / 10)
                {
                    return 0;
                }

                //获取当前末尾的数字
                var digit = x % 10;
                //去掉末尾数字
                x /= 10;
                //反转并累积结果
                result = result * 10 + digit;
            }

            return result;
        }
    }

    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    [MemoryDiagnoser]
    public class _7_ReverseBenchmark : IBenchmark
    {
        /// <summary>
        /// 测试次数
        /// </summary>
        private const int TestNumber = 1_000_000;

        private readonly int[] Datas = new int[TestNumber];

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random();
            for (var j = 0; j < TestNumber; j++)
            {
                Datas[j] = random.Next(int.MinValue, int.MaxValue);
            }
        }

        [Benchmark]
        public void ReverseLongString()
        {
            foreach (var str in Datas)
            {
                _ = _7_Reverse.ReverseLongString(str);
            }
        }

        [Benchmark]
        public void ReverseIntString()
        {
            foreach (var str in Datas)
            {
                _ = _7_Reverse.ReverseIntString(str);
            }
        }


        [Benchmark]
        public void ReverseMath()
        {
            foreach (var str in Datas)
            {
                _ = _7_Reverse.ReverseMath(str);
            }
        }
    }
}
