using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode
{
    public class _6_Convert
    {
        //二维矩阵模拟法
        public static string Matrix(string s, int numRows)
        {
            //行数为 1 或者字符串长度小于等于行数，直接返回原字符串
            if (numRows == 1 || s.Length <= numRows)
            {
                return s;
            }

            //构建二维矩阵，用于存储 Z 字形排列的字符
            var matrix = new char[numRows, s.Length];
            //当前行索引
            var rowIndex = 0;
            //行移动步长，向下移动步长为 1 ，向上移动步长为 -1
            var rowStep = 1;

            //遍历字符串
            for (var i = 0; i < s.Length; i++)
            {
                //将当前字符放入二维矩阵中对应的位置
                matrix[rowIndex, i] = s[i];

                if (rowIndex == 0)
                {
                    //如果当前行是第一行，则改变行为 1
                    //代表字符移动方向为向下
                    rowStep = 1;
                }
                else if (rowIndex == numRows - 1)
                {
                    //如果当前行是最后一行，则改变行为 -1
                    //代表字符移动方向为向上
                    rowStep = -1;
                }

                // 根据行步长更新当前行的索引
                rowIndex += rowStep;
            }

            //用于存储最终结果的字符串
            var result = new StringBuilder();
            //遍历二维矩阵的行
            for (var r = 0; r < numRows; r++)
            {
                //遍历二维矩阵的列
                for (var c = 0; c < s.Length; c++)
                {
                    //不为空的字符添加到结果字符串中
                    if (matrix[r, c] != 0)
                    {
                        result.Append(matrix[r, c]);
                    }
                }
            }

            // 返回最终的 Z 字形变换后的字符串
            return result.ToString();
        }

        //行模拟法（压缩矩阵）
        public static string Row(string s, int numRows)
        {
            //行数为 1 或者字符串长度小于等于行数，直接返回原字符串
            if (numRows == 1 || s.Length <= numRows)
            {
                return s;
            }

            //构建字符串数组，每一个StringBuilder代表一行所有字符
            var rows = new StringBuilder[numRows];
            for (int i = 0; i < numRows; ++i)
            {
                rows[i] = new StringBuilder();
            }

            //当前行索引
            var rowIndex = 0;
            // Z 字形变换周期
            var period = numRows * 2 - 2;
            //遍历字符串
            for (int i = 0; i < s.Length; ++i)
            {
                //将当前字符添加到相对应行的末尾
                rows[rowIndex].Append(s[i]);

                //计算当前字符在周期内的那个位置
                //以最后一行的元素为分界线
                //一个周期前半部分为向下移动
                //后半部分为向上移动
                if (i % period < numRows - 1)
                {
                    //向下移动
                    ++rowIndex;
                }
                else
                {
                    //右上移动
                    --rowIndex;
                }
            }

            //把字符串数组拼接为最终结果
            var result = new StringBuilder();
            foreach (var row in rows)
            {
                result.Append(row);
            }

            return result.ToString();
        }

        //行模拟法（代码精简）
        public static string RowCompact(string s, int numRows)
        {
            //行数为 1 或者字符串长度小于等于行数，直接返回原字符串
            if (numRows == 1 || s.Length <= numRows)
            {
                return s;
            }

            //构建字符串数组，每一个StringBuilder代表一行所有字符
            var rows = Array.ConvertAll(new StringBuilder[numRows], _ => new StringBuilder());
            // Z 字形变换周期
            var period = 2 * numRows - 2;
            for (var i = 0; i < s.Length; i++)
            {
                //计算当前字符在周期内的那个位置
                var periodIndex = i % period;
                //获取当前行索引，利用周期内对称性，取最小值确保rowIndex不超过周期的中点
                var rowIndex = Math.Min(periodIndex, period - periodIndex);
                rows[rowIndex].Append(s[i]);
            }

            return string.Join<StringBuilder>("", rows);
        }

        //伪直接构建
        public static string Build(string s, int numRows)
        {
            //行数为 1 或者字符串长度小于等于行数，直接返回原字符串
            if (numRows == 1 || s.Length <= numRows)
            {
                return s;
            }

            //定义结果动态字符串
            var result = new StringBuilder();
            // Z 字形变换周期
            var period = numRows * 2 - 2;
            //遍历行
            for (var i = 0; i < numRows; ++i)
            {
                //遍历每个周期的起始位置，从 0 开始，步长为 period
                for (var j = 0; j + i < s.Length; j += period)
                {
                    //当前周期的第一个字符，添加至结果中
                    result.Append(s[j + i]);
                    //根据 Z 字形特性，在一个周期内
                    //除了第一行和最后一行只有一个字符
                    //其他行则至少有一个字符，最多有两个字符
                    //因此下面除了第一行和最后一行外，处理当前周期第二个字符
                    if (0 < i && i < numRows - 1 && j + period - i < s.Length)
                    {
                        result.Append(s[j + period - i]);
                    }
                }
            }

            return result.ToString();
        }

        //真直接构建
        public static string Build2(string s, int numRows)
        {
            //行数为 1 或者字符串长度小于等于行数，直接返回原字符串
            if (numRows == 1 || s.Length <= numRows)
            {
                return s;
            }

            //定义结果字符数组
            var result = new char[s.Length];
            // Z 字形变换周期
            var period = 2 * numRows - 2;
            //总的周期数
            var totalPeriod = (s.Length + period - 1) / period;
            //最后一个字符的周期索引
            var lastPeriodIndex = s.Length % period;
            lastPeriodIndex = lastPeriodIndex == 0 ? (period - 1) : (lastPeriodIndex - 1);
            //遍历字符串
            for (var i = 0; i < s.Length; i++)
            {
                //当前字符串周期索引
                var periodIndex = i % period;
                //当前行索引
                var rowIndex = Math.Min(periodIndex, period - periodIndex);
                //当前字符索引，以在第几个周期为基础
                var index = (i / period);

                //处理非第一行情况
                if (rowIndex > 0)
                {
                    //当前行的起始索引为此行之前所有行的所有字符总和
                    //第一行总字符数为总周期数
                    //第二行总字符数为分为两部分之后，
                    //第一部分是(totalPeriod - 1)个周期，此部分每个周期有2个元素，
                    //第二部分是最后一个周期中此行的字符个数，需要动态计算
                    index += totalPeriod + (totalPeriod - 1) * 2 * (rowIndex - 1);

                    //除首尾行外中间行最多有两个字符
                    if (rowIndex != numRows - 1)
                    {
                        //第二个字符索引起始需要在第几个周期为基础上在加一个第几个周期
                        index += (i / period);
                    }

                    //判断最后一个字符周期索引所在位置
                    //动态计算此行之前最后一个周期中包含几个字符
                    if (lastPeriodIndex < rowIndex)
                    {
                        //小于当前行数，则包含最后一个周期的所有字符
                        index += lastPeriodIndex;
                    }
                    else if (lastPeriodIndex < period - rowIndex)
                    {
                        //小于当前字符对称点，则包含当前行数减1个字符
                        index += rowIndex - 1;
                    }
                    else
                    {
                        //否则包含最后一个周期所有字符减去此行下面所有行最后一个周期的所有字符
                        index += lastPeriodIndex - ((numRows - 1 - rowIndex) * 2 + 1);
                    }
                }

                if (periodIndex <= numRows - 1)
                {
                    //处理所有行的第一个字符
                    result[index] = s[i];
                }
                else
                {
                    //处理中间行第二个字符
                    result[index + 1] = s[i];
                }

            }

            return new string(result);
        }
    }

    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    [MemoryDiagnoser]
    public class _6_ConvertBenchmark : IBenchmark
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
