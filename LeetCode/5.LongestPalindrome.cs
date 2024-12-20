using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace LeetCode
{
    public class _5_LongestPalindrome
    {
        //暴力破解法
        public static string BruteForce(string s)
        {
            var result = string.Empty;
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

        //动态规划
        public static string DynamicProgramming(string s)
        {
            var length = s.Length;
            //判断该组合是否是回文字符串，行为起始点，列为结尾点
            var dp = new bool[length, length];
            //最长回文字符串，初始为0
            var result = string.Empty;
            //从回文长度为1开始判断，到字符长度n为止
            for (var len = 1; len <= length; len++)
            {
                for (var startIndex = 0; startIndex < length; startIndex++)
                {
                    //结束索引 = 起始索引 + 间隔（len - 1）
                    var endIndex = startIndex + len - 1;
                    //结束索引超出字符串长度，结束本次循环
                    if (endIndex >= length)
                    {
                        break;
                    }

                    //回文字符串的公式就是子字符串也是回文，并且当前起始字符和结束字符相等，
                    //所以得出公式 dp[startIndex+1,endIndex-1] && s[startIndex] == s[endIndex]
                    //其中回文长度为1和2两种特殊情况需要单独处理，其特殊性在于他们不存在子字符串
                    //回文长度为1时，自身当然等于自身
                    //回文长度为2时，起始字符和结束字符是相邻的，只要相邻的字符相等就可以
                    dp[startIndex,
                       endIndex] = (len == 1
                                                || len == 2
                                                || dp[startIndex + 1, endIndex - 1]) && s[startIndex] == s[endIndex];
                    //当前字符串是回文，并且当前回文长度大于最长回文长度时，修改result
                    if (dp[startIndex, endIndex] && len > result.Length)
                    {
                        result = s.Substring(startIndex, len);
                    }
                }
            }

            return result;
        }

        //中心扩散法
        public static string CenterExpand(string s)
        {
            //如果字符串为空或只有一个字符，直接返回该字符串
            if (s == null || s.Length < 1)
            {
                return "";
            }

            //记录最长回文子串的起始位置和结束位置
            var startIndex = 0;
            var endIndex = 0;
            //遍历每个字符，同时处理回文字串长度为奇偶的情况，
            //即以该字符或该字符与其下一个字符之间为中心的回文
            for (var i = 0; i < s.Length; i++)
            {
                //获取回文字串长度为奇数的情况，
                //即以当前字符为中心的回文长度
                var oddLength = PalindromicLength(s, i, i);
                //获取回文字串长度为偶数的情况，
                //即以当前字符和下一个字符之间的空隙为中心的回文长度
                var evenLength = PalindromicLength(s, i, i + 1);
                //取两种情况下的最长长度
                var maxLength = Math.Max(oddLength, evenLength);
                //如果找到更长的回文子串，更新起始位置和长度
                if (maxLength > endIndex - startIndex)
                {
                    //重新计算起始位置
                    startIndex = i - (maxLength - 1) / 2;
                    //重新计算结束位置
                    endIndex = i + maxLength / 2;
                }
            }

            //返回最长回文子串
            return s[startIndex..(endIndex + 1)];
        }

        //从中心向外扩展，检查并返回回文串的长度
        public static int PalindromicLength(string s, int leftIndex, int rightIndex)
        {
            //左边界大于等于首字符，右边界小于等于尾字符，并且左右字符相等
            while (leftIndex >= 0 && rightIndex < s.Length && s[leftIndex] == s[rightIndex])
            {
                //从中心往两端扩展一位

                //向左扩展
                --leftIndex;
                //向右扩展
                ++rightIndex;
            }

            //返回回文串的长度（注意本来应该是rightIndex - leftIndex + 1，
            //但是满足条件后leftIndex、rightIndex又分别向左和右又各扩展了一位，
            //因此需要把这两位减掉，所以最后公式为rightIndex - leftIndex - 1）  
            return rightIndex - leftIndex - 1;
        }

        //中心扩散法-合并奇偶
        public static string CenterExpandMergeOddEven(string s)
        {
            //如果字符串为空或只有一个字符，直接返回该字符串
            if (s == null || s.Length < 1)
            {
                return "";
            }

            // 预处理字符串，插入#字符以统一处理奇偶回文串
            //比如：s="cabac"转化为s="#c#a#b#a#c#"
            var tmp = $"#{string.Join("#", s.ToCharArray())}#";

            //记录最长回文子串的起始位置和最大长度
            var startIndex = 0;
            var maxLength = 0;
            //从左到右遍历处理过的字符串，求每个字符的回文串半径
            for (var i = 0; i < tmp.Length; ++i)
            {
                //计算当前以i为中心的回文串半径
                var radius = PalindromicRadius(tmp, i, i);
                //如果当前计算的半径大于maxLength，就更新startIndex
                if (radius > maxLength)
                {
                    startIndex = (i - radius) / 2;
                    maxLength = radius;
                }
            }

            //根据startIndex和maxLength，从原始字符串中截取返回
            return s.Substring(startIndex, maxLength);
        }

        //马拉车法
        public static string Manacher(string s)
        {
            if (s == null || s.Length == 0)
            {
                return "";
            }

            var tmp = $"#{string.Join("#", s.ToCharArray())}#";

            //rightIndex表示目前计算出的最右端范围，rightIndex和左边都是已探测过的
            var rightIndex = 0;
            //centerIndex最右端位置的中心对称点
            var centerIndex = 0;
            //记录最长回文子串的起始位置和最大长度
            var startIndex = 0;
            var maxLength = 0;
            //radiusPoints数组记录所有已探测过的回文串半径，后面我们再计算i时，根据radiusPoints[iMirror]计算i
            var radiusPoints = new int[tmp.Length];
            //从左到右遍历处理过的字符串，求每个字符的回文串半径
            for (var i = 0; i < tmp.Length; i++)
            {
                //根据i和right的位置分为两种情况：
                //1、i<=right利用已知的信息来计算i
                //2、i>right，说明i的位置时未探测过的，只能用中心探测法
                if (i <= rightIndex)
                {
                    // 找出i关于前面中心的对称
                    var iMirror = 2 * centerIndex - i;
                    //这句是关键，不用再像中心探测那样，一点点的往左/右扩散，
                    //根据已知信息减少不必要的探测，必须选择两者中的较小者作为左右探测起点
                    var minRadiusLength = Math.Min(rightIndex - i, radiusPoints[iMirror]);
                    //这里左右-1和+1是因为对称性可以直接跳过相应的回文串半径
                    radiusPoints[i] = PalindromicRadius(tmp, i - minRadiusLength - 1, i + minRadiusLength + 1);
                }
                else
                {
                    //i落在rightIndex右边，是没被探测过的，只能用中心探测法
                    //这里左右-1和+1，是因为中心点字符肯定是回文串，可以直接跳过
                    radiusPoints[i] = PalindromicRadius(tmp, i - 1, i + 1);
                }

                //大于rightIndex，说明可以更新最右端范围了，同时更新centerIndex
                if (i + radiusPoints[i] > rightIndex)
                {
                    centerIndex = i;
                    rightIndex = i + radiusPoints[i];
                }

                //找到了一个更长的回文半径，更新原始字符串的startIndex位置
                if (radiusPoints[i] > maxLength)
                {
                    startIndex = (i - radiusPoints[i]) / 2;
                    maxLength = radiusPoints[i];
                }
            }

            //根据start和maxLen，从原始字符串中截取一段返回
            return s.Substring(startIndex, maxLength);
        }

        //回文串半径
        public static int PalindromicRadius(string s, int leftIndex, int rightIndex)
        {
            //左边界大于等于首字符，右边界小于等于尾字符，并且左右字符相等
            while (leftIndex >= 0 && rightIndex < s.Length && s[leftIndex] == s[rightIndex])
            {
                //从中心往两端扩展一位

                //向左扩展
                --leftIndex;
                //向右扩展
                ++rightIndex;
            }

            //返回回文串半径（注意本来应该是（rightIndex - leftIndex + 1）/2，
            //但是满足条件后leftIndex、rightIndex又分别向左和右又各扩展了一位，
            //因此需要把这两位减掉，因为中心元素不在计算返回还需要减掉一位，
            //因此（rightIndex - leftIndex + 1 - 2 - 1）/2
            //所以最后公式为（rightIndex - leftIndex - 1）/2）  
            return (rightIndex - leftIndex - 2) / 2;
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
