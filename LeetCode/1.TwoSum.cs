using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace LeetCode
{
    /// <summary>
    /// 1. 两数之和
    /// </summary>
    internal class _1_TwoSum
    {
        public static int[] TwoSumForFor(int[] nums, int target)
        {
            for (var i = 0; i < nums.Length; i++)
            {
                for (var j = i + 1; j < nums.Length; j++)
                {
                    if (nums[i] + nums[j] == target)
                    {
                        return [i, j];
                    }
                }
            }

            return [];
        }

        public static int[] TwoSumForForBidirectional(int[] nums, int target)
        {
            for (var i = 0; i < nums.Length; i++)
            {
                var front = nums[i];
                var backIndex = nums.Length - 1 - i;
                var back = nums[backIndex];
                for (var j = i + 1; j < nums.Length; j++)
                {
                    if (front + nums[j] == target)
                    {
                        return [i, j];
                    }

                    if (back + nums[j - 1] == target)
                    {
                        return [j - 1, backIndex];
                    }
                }
            }

            return [];
        }

        public static int[] TwoSumForLastIndexOf(int[] nums, int target)
        {
            var length = nums.Length;
            for (var i = 0; i < length; i++)
            {
                var j = Array.LastIndexOf(nums, target - nums[i], length - 1, length - 1 - i);
                if (j >= 0 && j != i)
                {
                    return [i, j];
                }
            }

            return [];
        }

        public static int[] TwoSumForIndexOf(int[] nums, int target)
        {
            for (var i = 0; i < nums.Length; i++)
            {
                var j = Array.IndexOf(nums, nums[i], 0, i);
                if (j >= 0 && j != i)
                {
                    return [i, j];
                }

                nums[i] = target - nums[i];
            }

            return [];
        }

        public static int[] TwoSumDictionary(int[] nums, int target)
        {
            var dic = new Dictionary<int, int>();
            for (var i = 0; i < nums.Length; i++)
            {
                if (dic.TryGetValue(nums[i], out var value))
                {
                    return [value, i];
                }

                dic.TryAdd(target - nums[i], i);
            }

            return [];
        }

        public static int[] TwoSumDictionaryBidirectional(int[] nums, int target)
        {
            var dic = new Dictionary<int, int>();
            for (var i = 0; i < nums.Length; i++)
            {
                var front = nums[i];
                if (dic.TryGetValue(front, out var frontDiffIndex))
                {
                    return [frontDiffIndex, i];
                }

                dic.TryAdd(target - front, i);

                var backIndex = nums.Length - 1 - i;
                var back = nums[backIndex];

                if (dic.TryGetValue(back, out var backDiffIndex))
                {
                    return [backDiffIndex, backIndex];
                }

                dic.TryAdd(target - back, backIndex);
            }

            return [];
        }
    }

    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    [MemoryDiagnoser]
    public class _1_TwoSumBenchmark : IBenchmark
    {
        /// <summary>
        /// 测试次数
        /// </summary>
        private const int TestNumber = 10000;

        /// <summary>
        /// 数组长度
        /// </summary>
        private readonly int[] ArrayLengths = [100, 1000, 10000];

        private readonly Dictionary<int, (int[] nums, int target)[]> Datas = [];


        [GlobalSetup]
        public void Setup()
        {
            var random = new Random();

            foreach (var al in ArrayLengths)
            {
                var nu = new (int[] nums, int target)[TestNumber];
                for (var j = 0; j < TestNumber; j++)
                {
                    var list = new List<int>();
                    for (var i = 0; i < al - 2; i++)
                    {
                        list.Add(random.Next(1, al - 1));
                    }

                    var first = random.Next(1, al - 1) - 1;
                    var second = random.Next(1, al - 1) - 1;
                    list.Insert(first, 0);
                    list.Insert(second + 1, 0);
                    var _nums1 = list.ToArray();
                    nu[j] = (_nums1, 0);
                }

                Datas.Add(al, nu);
            }
        }

        [Benchmark]
        public void TwoSumForFor_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumForFor(nums, target));
        }

        [Benchmark]
        public void TwoSumForFor_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumForFor(nums, target));
        }

        [Benchmark]
        public void TwoSumForFor_10000_10000()
        {
            var res = Datas[10000];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumForFor(nums, target));
        }

        [Benchmark]
        public void TwoSumForForBidirectional_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumForForBidirectional(nums, target));
        }

        [Benchmark]
        public void TwoSumForForBidirectional_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumForForBidirectional(nums, target));
        }

        [Benchmark]
        public void TwoSumForForBidirectional_10000_10000()
        {
            var res = Datas[10000];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumForLastIndexOf(nums, target));
        }

        [Benchmark]
        public void TwoSumForLastIndexOf_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumForLastIndexOf(nums, target));
        }

        [Benchmark]
        public void TwoSumDictionary_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumDictionary(nums, target));
        }

        [Benchmark]
        public void TwoSumForLastIndexOf_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumForLastIndexOf(nums, target));
        }

        [Benchmark]
        public void TwoSumDictionary_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumDictionary(nums, target));
        }

        [Benchmark]
        public void TwoSumForLastIndexOf_10000_10000()
        {
            var res = Datas[10000];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumForLastIndexOf(nums, target));
        }

        [Benchmark]
        public void TwoSumDictionary_10000_10000()
        {
            var res = Datas[10000];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumDictionary(nums, target));
        }

        [Benchmark]
        public void TwoSumForIndexOf_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumForIndexOf(nums, target));
        }

        [Benchmark]
        public void TwoSumForIndexOf_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumForIndexOf(nums, target));
        }

        [Benchmark]
        public void TwoSumForIndexOf_10000_10000()
        {
            var res = Datas[10000];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumForIndexOf(nums, target));
        }

        [Benchmark]
        public void TwoSumDictionaryBidirectional_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumDictionaryBidirectional(nums, target));
        }

        [Benchmark]
        public void TwoSumDictionaryBidirectional_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumDictionaryBidirectional(nums, target));
        }

        [Benchmark]
        public void TwoSumDictionaryBidirectional_10000_10000()
        {
            var res = Datas[10000];
            Handle(res, (nums, target) => _ = _1_TwoSum.TwoSumDictionaryBidirectional(nums, target));
        }

        private static void Handle((int[] nums, int target)[] res, Func<int[], int, int[]> func)
        {
            foreach (var (nums, target) in res)
            {
                func(nums, target);
            }
        }
    }
}
