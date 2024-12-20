using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Numerics;
using static LeetCode._8_MyAtoi;

namespace LeetCode
{
    public class _8_MyAtoi
    {
        //解法 1：手动处理每个字符（经典解法）
        public static int MyAtoi1(string s)
        {
            //结果
            var result = 0;
            //当前处理字符索引
            var index = 0;
            //标记正负数
            var sign = 1;
            //字符串长度
            var length = s.Length;

            //去除开头的空格
            while (index < length && s[index] == ' ')
            {
                //处理下一个字符
                index++;
            }

            //处理正负符号
            if (index < length && (s[index] == '+' || s[index] == '-'))
            {
                //标记正负数
                sign = s[index] == '-' ? -1 : 1;
                //处理下一个字符
                index++;
            }

            //转换数字字符为数字
            while (index < length && char.IsDigit(s[index]))
            {
                //计算当前字符数值
                var digit = s[index] - '0';

                //检查是否溢出
                if (result > (int.MaxValue - digit) / 10)
                {
                    return sign == 1 ? int.MaxValue : int.MinValue;
                }

                //累积当前字符至结果中
                result = result * 10 + digit;
                //处理下一个字符
                index++;
            }

            //返回结果
            return sign * result;
        }

        //解法 2：正则表达式法
        public static int MyAtoi2(string s)
        {
            //使用正则表达式匹配符合要求的部分
            //^\s*：表示匹配字符串开头的零个或多个空白字符（空格、制表符等）。
            //[+-]?：表示符号（+ 或 -）可选。
            //\d+：表示一个或多个数字。
            var match = System.Text.RegularExpressions.Regex.Match(s, @"^\s*[+-]?\d+");

            //匹配成功，并且可以转换为数值
            if (match.Success && BigInteger.TryParse(match.Value, out var result))
            {
                //大于int最大值
                if (result > int.MaxValue)
                {
                    return int.MaxValue;
                }

                //小于int最小值
                if (result < int.MinValue)
                {
                    return int.MinValue;
                }

                //返回结果
                return (int)result;
            }

            return 0;
        }

        //解法 3：状态机法
        public int MyAtoi3(string s)
        {
            Automaton automaton = new Automaton();
            return automaton.Atoi(s);
        }

        public class Automaton
        {
            //0:"开始"状态
            private const int Start = 0;
            //1:"标记符号"状态
            private const int Signed = 1;
            //2:"处理数字"状态
            private const int InNumber = 2;
            //3:"结束"状态
            private const int End = 3;
            //符号:1为正数，0为负数
            private int _sign = 1;
            //数值结果
            private long _answer = 0;
            //记录当前处理状态
            private int _state = Start;
            //状态表
            private readonly Dictionary<int, int[]> _table = new Dictionary<int, int[]>()
            {
                {Start,new int[]{ Start, Signed, InNumber, End}},
                {Signed,new int[]{ End, End, InNumber, End}},
                {InNumber,new int[]{ End, End, InNumber, End}},
                {End,new int[]{ End, End, End, End}},
            };

            //处理当前字符
            private void Handle(char c)
            {
                //获取当前状态
                var currentState = GetState(c);
                //转移状态
                _state = _table[_state][currentState];
                switch (_state)
                {
                    //处理数字
                    case InNumber:
                        _answer = _answer * 10 + c - '0';
                        //溢出判断
                        _answer = _sign == 1 ? Math.Min(_answer, int.MaxValue) : Math.Min(_answer, -(long)int.MinValue);
                        break;
                    //处理正负号
                    case Signed:
                        _sign = c == '+' ? 1 : -1;
                        break;
                    case Start:
                    case End:
                        break;
                }
            }

            //获取当前字符对应状态
            private static int GetState(char c)
            {
                //空格
                if (char.IsWhiteSpace(c))
                {
                    return Start;
                }

                //正负号
                if (c == '+' || c == '-')
                {
                    return Signed;
                }

                //数字
                if (char.IsDigit(c))
                {
                    return InNumber;
                }

                //其他
                return End;
            }

            //字符串转换为整数
            public int Atoi(string s)
            {
                var length = s.Length;
                for (int i = 0; i < length; ++i)
                {
                    Handle(s[i]);
                }

                return (int)(_sign * _answer);
            }
        }
    }

    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    [MemoryDiagnoser]
    public class _8_MyAtoiBenchmark : IBenchmark
    {
        /// <summary>
        /// 测试次数
        /// </summary>
        private const int TestNumber = 10000;

        /// <summary>
        /// 数组长度
        /// </summary>
        private readonly int[] ArrayLengths = [100, 1000, 10000];

        private readonly Dictionary<int, (int[] nums, int[] target)[]> Datas = [];


        [GlobalSetup]
        public void Setup()
        {
            var random = new Random();

            foreach (var al in ArrayLengths)
            {
                var nu = new (int[] nums, int[] target)[TestNumber];
                for (var j = 0; j < TestNumber; j++)
                {
                    var list = new List<int>();
                    var l1 = random.Next(2) == 0 ? al : al - 1;
                    for (var i = 0; i < l1; i++)
                    {
                        list.Add(random.Next(1, al));
                    }

                    var _nums1 = list.ToArray();

                    var list2 = new List<int>();
                    var l2 = random.Next(2) == 0 ? al : al - 1;
                    for (var i = 0; i < l2; i++)
                    {
                        list2.Add(random.Next(1, al));
                    }

                    var _nums2 = list2.ToArray();
                    nu[j] = (_nums1, _nums2);
                }

                Datas.Add(al, nu);
            }
        }

        [Benchmark]
        public void MergedArraySort_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.MergedArraySort(nums, target));
        }
        [Benchmark]
        public void TwoPointerMergedArray_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.TwoPointerMergedArray(nums, target));
        }
        [Benchmark]
        public void TwoPointerDirectValue_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.TwoPointerDirectValue(nums, target));
        }
        [Benchmark]
        public void TwoPointerBinaryExclude_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.TwoPointerBinaryExclude(nums, target));
        }
        [Benchmark]
        public void TwoPointerBinaryFindKth_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.TwoPointerBinaryFindKth(nums, target));
        }
        [Benchmark]
        public void TwoPointerBinaryHalves_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.TwoPointerBinaryHalves(nums, target));
        }

        [Benchmark]
        public void MergedArraySort_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.MergedArraySort(nums, target));
        }
        [Benchmark]
        public void TwoPointerMergedArray_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.TwoPointerMergedArray(nums, target));
        }
        [Benchmark]
        public void TwoPointerDirectValue_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.TwoPointerDirectValue(nums, target));
        }
        [Benchmark]
        public void TwoPointerBinaryExclude_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.TwoPointerBinaryExclude(nums, target));
        }
        [Benchmark]
        public void TwoPointerBinaryFindKth_1000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.TwoPointerBinaryFindKth(nums, target));
        }
        [Benchmark]
        public void TwoPointerBinaryHalves_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.TwoPointerBinaryHalves(nums, target));
        }

        [Benchmark]
        public void MergedArraySort_10000_10000()
        {
            var res = Datas[10000];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.MergedArraySort(nums, target));
        }
        [Benchmark]
        public void TwoPointerMergedArray_10000_10000()
        {
            var res = Datas[10000];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.TwoPointerMergedArray(nums, target));
        }
        [Benchmark]
        public void TwoPointerDirectValue_10000_10000()
        {
            var res = Datas[10000];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.TwoPointerDirectValue(nums, target));
        }
        [Benchmark]
        public void TwoPointerBinaryExclude_10000_10000()
        {
            var res = Datas[10000];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.TwoPointerBinaryExclude(nums, target));
        }
        [Benchmark]
        public void TwoPointerBinaryFindKth_10000_10000()
        {
            var res = Datas[10000];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.TwoPointerBinaryFindKth(nums, target));
        }
        [Benchmark]
        public void TwoPointerBinaryHalves_10000_10000()
        {
            var res = Datas[10000];
            Handle(res, (nums, target) => _ = _4_FindMedianSortedArrays.TwoPointerBinaryHalves(nums, target));
        }
        private static void Handle((int[] nums, int[] target)[] res, Func<int[], int[], double> func)
        {
            foreach (var (nums, target) in res)
            {
                func(nums, target);
            }
        }
    }
}
