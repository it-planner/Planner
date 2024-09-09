using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Diagnostics.Metrics;
using System.Text;

namespace LeetCode
{
    public class _2_LengthOfLongestSubstring
    {
        public static int SlidingWindow(string s)
        {
            //start指针
            var startIndex = 0;
            //end指针
            var endIndex = 0;
            //当前不重复子串长度
            var currentLength = 0;
            //最长不重复子串长度
            var maxLength = 0;
            //一直处理直到end指针不小于字符串长度
            while (endIndex < s.Length)
            {
                //获取待处理字符
                var pendingChar = s[endIndex];
                //判断待处理字符串是否在当前子串中存在
                for (var i = startIndex; i < endIndex; i++)
                {
                    //如果子串中已经存在待处理字符
                    if (pendingChar == s[i])
                    {
                        //把start指针跳转至子串中重复字符下一个位置
                        startIndex = i + 1;
                        //重新计算当前不重复子串长度
                        currentLength = endIndex - startIndex;
                        break;
                    }
                }

                //end指针向后移动一位
                endIndex++;
                //当前不重复子串长度加1
                currentLength++;
                //比较并更新最大不重复子串长度
                if (currentLength > maxLength)
                {
                    maxLength = currentLength;
                }
            }

            return maxLength;
        }

        public static int SlidingWindowDictionary(string s)
        {
            //start指针
            var startIndex = 0;
            //end指针
            var endIndex = 0;
            //当前不重复子串长度
            var currentLength = 0;
            //最长不重复子串长度
            var maxLength = 0;
            //字典表，存储已存在字符
            var dic = new Dictionary<char, int>();
            //一直处理直到end指针不小于字符串长度
            while (endIndex < s.Length)
            {
                //获取待处理字符
                var pendingChar = s[endIndex];
                //判断待处理字符是否在字典表中存在，并且其索引位置在当前子串中
                if (dic.TryGetValue(pendingChar, out var value) && value >= startIndex)
                {
                    //把start指针跳转至子串中重复字符下一个位置
                    startIndex = value + 1;
                    //重新计算当前不重复子串长度
                    currentLength = endIndex - startIndex;
                }
                //更新字典表已存在字符最后的索引位置
                dic[pendingChar] = endIndex;

                //end指针向后移动一位
                endIndex++;
                //当前不重复子串长度加1
                currentLength++;
                //比较并更新最大不重复子串长度
                if (currentLength > maxLength)
                {
                    maxLength = currentLength;
                }
            }

            return maxLength;
        }

        public static int SlidingWindowArray(string s)
        {
            //start指针
            var startIndex = 0;
            //end指针
            var endIndex = 0;
            //当前不重复子串长度
            var currentLength = 0;
            //最长不重复子串长度
            var maxLength = 0;
            //定义可能存在的字符数组，并全部填充为-1
            var arr = new int[128];
            Array.Fill(arr, -1);
            //一直处理直到end指针不小于字符串长度
            while (endIndex < s.Length)
            {
                //获取待处理字符
                var pendingChar = s[endIndex];
                //判断待处理字符索引位置是否在当前子串内
                if (arr[pendingChar] >= startIndex)
                {
                    //把start指针跳转至子串中重复字符下一个位置
                    startIndex = arr[pendingChar] + 1;
                    //重新计算当前不重复子串长度
                    currentLength = endIndex - startIndex;
                }
                //更新数组中已存在字符最后的索引位置
                arr[pendingChar] = endIndex;

                //end指针向后移动一位
                endIndex++;
                //当前不重复子串长度加1
                currentLength++;
                //比较并更新最大不重复子串长度
                if (currentLength > maxLength)
                {
                    maxLength = currentLength;
                }
            }

            return maxLength;
        }
    }

    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    [MemoryDiagnoser]
    public class _3_LengthOfLongestSubstring : IBenchmark
    {
        /// <summary>
        /// 测试次数
        /// </summary>
        private const int TestNumber = 10000;

        /// <summary>
        /// 长度
        /// </summary>
        private const int StringLength = 10000;

        private readonly string[] Datas = new string[TestNumber];

        [GlobalSetup]
        public void Setup()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=~`[]{}|;:',.<>? /";
            var random = new Random();
            for (var j = 0; j < TestNumber; j++)
            {
                char[] stringChars = new char[StringLength];

                for (int i = 0; i < StringLength; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                Datas[j] = new string(stringChars);
            }

        }

        [Benchmark]
        public void SlidingWindow()
        {
            foreach (var str in Datas)
            {
                _ = _2_LengthOfLongestSubstring.SlidingWindow(str);
            }
        }

        [Benchmark]
        public void SlidingWindowDictionary()
        {
            foreach (var str in Datas)
            {
                _ = _2_LengthOfLongestSubstring.SlidingWindowDictionary(str);
            }
        }


        [Benchmark]
        public void SlidingWindowArray()
        {
            foreach (var str in Datas)
            {
                _ = _2_LengthOfLongestSubstring.SlidingWindowArray(str);
            }
        }
    }
}
