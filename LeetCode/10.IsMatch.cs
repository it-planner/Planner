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
    public class _10_IsMatch
    {
        public class Solution1
        {
            public bool IsMatch(string s, string p)
            {
                return IsPrefixMatch(s, p, s.Length, p.Length);
            }

            public bool IsPrefixMatch(string s, string p, int sCount, int pCount)
            {
                if (pCount == 0)
                {
                    return sCount == 0;
                }
                if (sCount == 0)
                {
                    return p[pCount - 1] == '*' && IsPrefixMatch(s, p, sCount, pCount - 2);
                }
                char c1 = s[sCount - 1], c2 = p[pCount - 1];
                if (c2 != '*')
                {
                    return IsCharacterMatch(c1, c2) && IsPrefixMatch(s, p, sCount - 1, pCount - 1);
                }
                else
                {
                    bool match = IsPrefixMatch(s, p, sCount, pCount - 2);
                    if (IsCharacterMatch(c1, p[pCount - 2]))
                    {
                        match = match || IsPrefixMatch(s, p, sCount - 1, pCount);
                    }
                    return match;
                }
            }

            public bool IsCharacterMatch(char c1, char c2)
            {
                return c1 == c2 || c2 == '.';
            }
        }
        public class Solution2
        {
            public bool IsMatch(string s, string p)
            {
                int m = s.Length, n = p.Length;
                bool[][] dp = new bool[m + 1][];
                for (int i = 0; i <= m; i++)
                {
                    dp[i] = new bool[n + 1];
                }
                dp[0][0] = true;
                for (int j = 1; j <= n; j++)
                {
                    if (p[j - 1] == '*')
                    {
                        dp[0][j] = dp[0][j - 2];
                    }
                }
                for (int i = 1; i <= m; i++)
                {
                    char c1 = s[i - 1];
                    for (int j = 1; j <= n; j++)
                    {
                        char c2 = p[j - 1];
                        if (c2 != '*')
                        {
                            dp[i][j] = dp[i - 1][j - 1] && IsCharacterMatch(c1, c2);
                        }
                        else
                        {
                            dp[i][j] = dp[i][j - 2];
                            if (IsCharacterMatch(c1, p[j - 2]))
                            {
                                dp[i][j] = dp[i][j] || dp[i - 1][j];
                            }
                        }
                    }
                }
                return dp[m][n];
            }

            public bool IsCharacterMatch(char c1, char c2)
            {
                return c1 == c2 || c2 == '.';
            }
        }
        public class Solution3
        {
            public bool IsMatch(string s, string p)
            {
                int m = s.Length, n = p.Length;
                bool[] dp = new bool[n + 1];
                dp[0] = true;
                for (int j = 1; j <= n; j++)
                {
                    if (p[j - 1] == '*')
                    {
                        dp[j] = dp[j - 2];
                    }
                }
                for (int i = 1; i <= m; i++)
                {
                    bool[] dpNew = new bool[n + 1];
                    char c1 = s[i - 1];
                    for (int j = 1; j <= n; j++)
                    {
                        char c2 = p[j - 1];
                        if (c2 != '*')
                        {
                            dpNew[j] = dp[j - 1] && IsCharacterMatch(c1, c2);
                        }
                        else
                        {
                            dpNew[j] = dpNew[j - 2];
                            if (IsCharacterMatch(c1, p[j - 2]))
                            {
                                dpNew[j] = dpNew[j] || dp[j];
                            }
                        }
                    }
                    dp = dpNew;
                }
                return dp[n];
            }

            public bool IsCharacterMatch(char c1, char c2)
            {
                return c1 == c2 || c2 == '.';
            }
        }


        //解法 1: 动态规划 (DP)
        public class Solution11
        {
            public bool IsMatch(string s, string p)
            {
                int m = s.Length, n = p.Length;
                bool[,] dp = new bool[m + 1, n + 1];
                dp[0, 0] = true;

                // 初始化 dp 数组，对于模式以 "*" 开头的情况
                for (int j = 1; j <= n; j++)
                {
                    if (p[j - 1] == '*')
                    {
                        dp[0, j] = dp[0, j - 2];
                    }
                }

                // 填充 dp 数组
                for (int i = 1; i <= m; i++)
                {
                    for (int j = 1; j <= n; j++)
                    {
                        if (p[j - 1] == s[i - 1] || p[j - 1] == '.')
                        {
                            dp[i, j] = dp[i - 1, j - 1];
                        }
                        else if (p[j - 1] == '*')
                        {
                            dp[i, j] = dp[i, j - 2] || (p[j - 2] == s[i - 1] || p[j - 2] == '.') && dp[i - 1, j];
                        }
                    }
                }

                return dp[m, n];
            }
        }

        //解法 2: 递归 + 记忆化搜索 (Memoization)
        public class Solution
        {
            private Dictionary<(int, int), bool> memo = new Dictionary<(int, int), bool>();

            public bool IsMatch(string s, string p)
            {
                return IsMatchHelper(s, p, 0, 0);
            }

            private bool IsMatchHelper(string s, string p, int i, int j)
            {
                // 如果模式已经匹配完，则判断字符串是否也匹配完
                if (j == p.Length) return i == s.Length;

                // 查找是否已经计算过该状态
                if (memo.ContainsKey((i, j))) return memo[(i, j)];

                bool match = (i < s.Length && (p[j] == s[i] || p[j] == '.'));

                bool result = false;
                // 如果当前模式字符是 '*'，我们考虑两种情况
                if (j + 1 < p.Length && p[j + 1] == '*')
                {
                    result = IsMatchHelper(s, p, i, j + 2) || (match && IsMatchHelper(s, p, i + 1, j));
                }
                // 如果当前字符不是 '*'，则直接匹配下一个字符
                else if (match)
                {
                    result = IsMatchHelper(s, p, i + 1, j + 1);
                }

                // 记忆化存储结果
                memo[(i, j)] = result;
                return result;
            }
        }

        //解法 3: 递归 (暴力解法)
        public class Solution33
        {
            public bool IsMatch(string s, string p)
            {
                if (p.Length == 0) return s.Length == 0;

                bool firstMatch = (s.Length > 0 && (p[0] == s[0] || p[0] == '.'));

                if (p.Length >= 2 && p[1] == '*')
                {
                    return IsMatch(s, p.Substring(2)) || (firstMatch && IsMatch(s.Substring(1), p));
                }
                else
                {
                    return firstMatch && IsMatch(s.Substring(1), p.Substring(1));
                }
            }
        }


        //解法 4: 迭代式动态规划（空格优化版）
        public class Solution44
        {
            public bool IsMatch(string s, string p)
            {
                int m = s.Length, n = p.Length;
                bool[] dp = new bool[n + 1];
                dp[0] = true;

                for (int j = 1; j <= n; j++)
                {
                    if (p[j - 1] == '*')
                    {
                        dp[j] = dp[j - 2];
                    }
                }

                for (int i = 1; i <= m; i++)
                {
                    bool prev = dp[0];
                    dp[0] = false;
                    for (int j = 1; j <= n; j++)
                    {
                        bool temp = dp[j];
                        if (p[j - 1] == s[i - 1] || p[j - 1] == '.')
                        {
                            dp[j] = prev;
                        }
                        else if (p[j - 1] == '*')
                        {
                            dp[j] = dp[j - 2] || (p[j - 2] == s[i - 1] || p[j - 2] == '.') && prev;
                        }
                        prev = temp;
                    }
                }

                return dp[n];
            }
        }

    }

    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    [MemoryDiagnoser]
    public class _10_IsMatchBenchmark : IBenchmark
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
