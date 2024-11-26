using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace LeetCode
{
    public class _5_LongestPalindrome
    {
        //暴力破解法
        public static string BruteForce(string s)
        {
            var result = string.Empty;
            int max = 0;
            var len = s.Length;
            //从第一个字符开始遍历，作为起始字符
            for (var i = 0; i < len; i++)
            {
                //从第二个字符来开始遍历，作为结束字符
                for (var j = i + 1; j <= len; j++)
                {
                    //取出[i,j)字符串,即包含i，不好含j，为临时字符串
                    var temp = s[i..j];
                    //如果临时字符串是回文字符串，且临时字符串长度大于目标字符串长度
                    if (IsPalindromic(temp) && temp.Length > result.Length)
                    {
                        //则更新目标字符串为当前临时字符串
                        result = temp;
                        max = Math.Max(max, j - i - 1);
                    }
                }
            }

            return result;
        }

        //判断字符串是否为回文串
        public static bool IsPalindromic(string s)
        {
            var len = s.Length;
            //遍历字符串的一半长度
            for (var i = 0; i < len / 2; i++)
            {
                //如果对称位置不同，则不为回文串
                if (s[i] != s[len - i - 1])
                {
                    return false;
                }
            }
            return true;
        }

        public static string CenterExpand(string s)
        {
            if (s == null || s.Length < 1)
            {
                return "";
            }

            int start = 0, end = 0;
            for (int i = 0; i < s.Length; i++)
            {
                int len1 = CenterExpand(s, i, i);
                int len2 = CenterExpand(s, i, i + 1);
                int len = Math.Max(len1, len2);
                if (len > end - start)
                {
                    start = i - (len - 1) / 2;
                    end = i + len / 2;
                }
            }
            return s[start..(end + 1)];
        }

        public static int CenterExpand(string s, int left, int right)
        {
            while (left >= 0 && right < s.Length && s[left] == s[right])
            {
                --left;
                ++right;
            }
            return right - left - 1;
        }

        public static string MergedArraySort(string s)
        {
            var result = s[0].ToString();
            for (var i = 0; i < s.Length; i++)
            {
                var left = i;
                var right = s.Length - 1;
                var l = "";
                var r = "";
                var b = -1;
                var bb = right;
                while (left < right)
                {
                    if (s[left] == s[right])
                    {
                        if (b < 0 && bb != right)
                        {
                            b = right;
                            bb = right;
                        }

                        l += s[left];
                        r = s[right] + r;
                        left++;
                        right--;
                        var rr = "";
                        if (left == right)
                        {
                            rr = l + s[left] + r;
                        }
                        else if (left == right + 1)
                        {
                            rr = l + r;
                        }


                        if (result.Length < rr.Length)
                        {
                            result = rr;
                        }
                    }
                    else
                    {
                        left = i;
                        right--;
                        if (b > 0)
                        {
                            right = b;
                            b = -1;
                        }
                        l = "";
                        r = "";
                    }
                }

                l = "";
                r = "";
            }

            return result;
        }

    }

    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    [MemoryDiagnoser]
    public class _5_LongestPalindromeBenchmark : IBenchmark
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
