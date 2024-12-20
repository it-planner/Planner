using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace LeetCode
{
    public class _9_IsPalindrome
    {
        //反转字符串法
        public static bool IsPalindrome1(int x)
        {
            var strX = x.ToString();
            //反转字符串
            var reversedStr = new string(strX.Reverse().ToArray());
            //比较入参和反转后字符串是否相等
            return strX == reversedStr;
        }

        //反转字符数组法
        public static bool IsPalindrome2(int x)
        {
            //x转为字符数组
            var strXChars = x.ToString().ToCharArray();
            //深拷贝一份字符数组用于反转
            var reversedChars = strXChars.ToArray();
            //反转字符数组
            Array.Reverse(reversedChars);
            //比较入参和反转后字符串是否相等
            return strXChars.SequenceEqual(reversedChars);
        }

        //双指针
        public static bool IsPalindrome3(int x)
        {
            //负数肯定不是回文数
            if (x < 0)
            {
                return false;
            }

            //定义除数变量，用于从前截取数字
            var div = 1;

            //通过循环对x取整，然后在乘10
            //求得可以获取x第1位数字对应的除数
            //比如12345，则除数为10000
            while (x / div >= 10)
            {
                div *= 10;
            }

            while (x > 0)
            {
                //获取左边数字
                var left = x / div;
                //获取右边数字
                var right = x % 10;
                //比较左右数字是否相等
                if (left != right)
                {
                    //不相等则直接返回
                    return false;
                }

                //去掉左边数字
                x = x % div;
                //去掉右边数字
                x = x / 10;
                //因为去掉两个数字
                //所以除数需除100
                div /= 100;
            }

            //为回文数字
            return true;
        }

        //反转全部数字
        public static bool IsPalindrome4(int x)
        {
            //负数肯定不是回文数
            if (x < 0)
            {
                return false;
            }

            //把入参赋值给临时变量
            int temp = x;
            //反转结果
            long reversed = 0;
            //从后往前循环处理整数中的每一个数字
            while (x != 0)
            {
                //获取x的个位数字
                int digit = temp % 10;
                //移除x的个位数字
                temp = temp / 10;
                //把x的个位数字拼接到反转结果的个位上
                reversed = reversed * 10 + digit;
            }

            //直接比较入参和反转后的整数是否相等
            return x == reversed;
        }


        //反转一半数字
        public static bool IsPalindrome5(int x)
        {
            //特殊情况：
            //当 x < 0 时，x 不是回文数。
            //同样地，如果数字的最后一位是 0，为了使该数字为回文，
            //则其第一位数字也应该是 0，只有 0 满足这一属性
            if (x < 0 || (x % 10 == 0 && x != 0))
            {
                return false;
            }

            var revertedNumber = 0;
            while (x > revertedNumber)
            {
                //获取x的个位数字
                var digit = x % 10;
                //把x的个位数字拼接到反转结果的个位上
                revertedNumber = revertedNumber * 10 + digit;
                //移除x的个位数字
                x /= 10;
            }

            //当数字长度为奇数时，例如，当输入为 12321 时，
            //在 while 循环的末尾我们可以得到 x = 12，revertedNumber = 123，
            //因此可得 x == revertedNumber/10
            //而当数字长度为偶数时，则 x == revertedNumber。
            return x == revertedNumber || x == revertedNumber / 10;
        }
    }

    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    [MemoryDiagnoser]
    public class _9_IsPalindromeBenchmark : IBenchmark
    {
        /// <summary>
        /// 测试次数
        /// </summary>
        private const int TestNumber = 100_0000;

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
        public void IsPalindrome1()
        {
            foreach (var str in Datas)
            {
                _ = _9_IsPalindrome.IsPalindrome1(str);
            }
        }

        [Benchmark]
        public void IsPalindrome2()
        {
            foreach (var str in Datas)
            {
                _ = _9_IsPalindrome.IsPalindrome2(str);
            }
        }

        [Benchmark]
        public void IsPalindrome3()
        {
            foreach (var str in Datas)
            {
                _ = _9_IsPalindrome.IsPalindrome3(str);
            }
        }


        [Benchmark]
        public void IsPalindrome4()
        {
            foreach (var str in Datas)
            {
                _ = _9_IsPalindrome.IsPalindrome4(str);
            }
        }



        [Benchmark]
        public void IsPalindrome5()
        {
            foreach (var str in Datas)
            {
                _ = _9_IsPalindrome.IsPalindrome5(str);
            }
        }
    }
}
