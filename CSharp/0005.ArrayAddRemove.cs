using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Collections.Immutable;
using System.Text;

namespace CSharp
{
    public static class ArrayExtensions
    {
        public static T[] AddRange<T>(this T[] source, T[] added)
        {
            var size = source.Length + added.Length;
            var array = new T[size];
            Array.Copy(source, array, source.Length);
            Array.Copy(added, 0, array, source.Length, added.Length);
            return array;
        }

        public static T[] RemoveAll<T>(this T[] source, Predicate<T> match)
        {
            return Array.FindAll(source, a => !match(a));
        }
    }

    public static class ArrayHelper
    {
        public static void AddRange<T>(ref T[] source, T[] added)
        {
            var size = source.Length + added.Length;
            var array = new T[size];
            Array.Copy(source, array, source.Length);
            Array.Copy(added, 0, array, source.Length, added.Length);
            source = array;
        }

        public static void RemoveAll<T>(ref T[] source, Predicate<T> match)
        {
            source = Array.FindAll(source, a => !match(a));
        }
    }

    public class ArrayAddRemove
    {
        public static int[] AddByList(int[] source, int[] added)
        {
            var list = source.ToList();
            list.AddRange(added);
            return list.ToArray();
        }

        public static int[] AddByConcat(int[] source, int[] added)
        {
            return source.Concat(added).ToArray();
        }

        public static int[] AddByCopy(int[] source, int[] added)
        {
            var size = source.Length + added.Length;
            var array = new int[size];
            // 复制原数组  
            Array.Copy(source, array, source.Length);
            // 添加新元素  
            Array.Copy(added, 0, array, source.Length, added.Length);
            return array;
        }

        public static int[] AddBySpan(int[] source, int[] added)
        {
            Span<int> sourceSpan = source;
            Span<int> addedSpan = added;
            Span<int> span = new int[source.Length + added.Length];
            // 复制原数组
            sourceSpan.CopyTo(span);
            // 添加新元素
            addedSpan.CopyTo(span.Slice(sourceSpan.Length));

            return span.ToArray();
        }

        public static int[] RemoveByList(int[] source, int[] added)
        {
            var list = source.ToList();
            list.RemoveAll(x => added.Contains(x));
            return list.ToArray();
        }

        public static int[] RemoveByWhere(int[] source, int[] added)
        {
            return source.Where(x => !added.Contains(x)).ToArray();
        }

        public static int[] RemoveByArray(int[] source, int[] added)
        {
            return Array.FindAll(source, x => !added.Contains(x));
        }

        public static int[] RemoveByForList(int[] source, int[] added)
        {
            var list = new List<int>();
            foreach (int item in source)
            {
                if (!added.Contains(item))
                {
                    list.Add(item);
                }
            }

            return list.ToArray();
        }

        public static int[] RemoveByForMarkCopy(int[] source, int[] added)
        {
            var idx = 0;
            foreach (var item in source)
            {
                if (!added.Contains(item))
                {
                    //标记有效元素
                    source[idx++] = item;
                }
            }

            //创建新数组并复制有效元素
            var array = new int[idx];
            Array.Copy(source, array, idx);
            return array;
        }

        public static int[] RemoveByForMarkResize(int[] source, int[] added)
        {
            var idx = 0;
            foreach (var item in source)
            {
                if (!added.Contains(item))
                {
                    //标记有效元素
                    source[idx++] = item;
                }
            }

            //调整数组大小
            Array.Resize(ref source, idx);
            return source;
        }
    }


    [MemoryDiagnoser]
    public class ArrayAddRemoveBenchmark
    {
        /// <summary>
        /// 测试次数
        /// </summary>
        private const int TestNumber = 10000;

        /// <summary>
        /// 数组长度
        /// </summary>
        private readonly int[] ArrayLengths = new int[4] { 100, 1000, 5000, 10000 };

        private readonly Dictionary<int, (int[] nums, int[] target)[]> Datas = new();


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
                    for (var i = 0; i < al; i++)
                    {
                        list.Add(random.Next(1, al));
                    }

                    var _nums1 = list.ToArray();

                    var list2 = new List<int>();
                    for (var i = 0; i < al; i++)
                    {
                        list2.Add(random.Next(1, al));
                    }

                    var _nums2 = list2.ToArray();
                    nu[j] = (_nums1, _nums2);
                }

                Datas.Add(al, nu);
            }
        }

        #region Add
        [Benchmark]
        public void AddByList_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = ArrayAddRemove.AddByList(nums, target));
        }
        [Benchmark]
        public void AddByConcat_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = ArrayAddRemove.AddByConcat(nums, target));
        }
        [Benchmark]
        public void AddByCopy_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = ArrayAddRemove.AddByCopy(nums, target));
        }
        [Benchmark]
        public void AddBySpan_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = ArrayAddRemove.AddBySpan(nums, target));
        }

        [Benchmark]
        public void AddByList_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.AddByList(nums, target));
        }
        [Benchmark]
        public void AddByConcat_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.AddByConcat(nums, target));
        }
        [Benchmark]
        public void AddByCopy_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.AddByCopy(nums, target));
        }
        [Benchmark]
        public void AddBySpan_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.AddBySpan(nums, target));
        }

        [Benchmark]
        public void AddByList_10000_10000()
        {
            var res = Datas[10000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.AddByList(nums, target));
        }
        [Benchmark]
        public void AddByConcat_10000_10000()
        {
            var res = Datas[10000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.AddByConcat(nums, target));
        }
        [Benchmark]
        public void AddByCopy_10000_10000()
        {
            var res = Datas[10000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.AddByCopy(nums, target));
        }
        [Benchmark]
        public void AddBySpan_10000_10000()
        {
            var res = Datas[10000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.AddBySpan(nums, target));
        }
        #endregion

        #region Remove
        [Benchmark]
        public void RemoveByWhere_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByWhere(nums, target));
        }
        [Benchmark]
        public void RemoveByList_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByList(nums, target));
        }
        [Benchmark]
        public void RemoveByArray_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByArray(nums, target));
        }
        [Benchmark]
        public void RemoveByForList_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByForList(nums, target));
        }
        [Benchmark]
        public void RemoveByForMarkCopy_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByForMarkCopy(nums, target));
        }
        [Benchmark]
        public void RemoveByForMarkResize_10000_100()
        {
            var res = Datas[100];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByForMarkResize(nums, target));
        }

        [Benchmark]
        public void RemoveByWhere_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByWhere(nums, target));
        }
        [Benchmark]
        public void RemoveByList_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByList(nums, target));
        }
        [Benchmark]
        public void RemoveByArray_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByArray(nums, target));
        }
        [Benchmark]
        public void RemoveByForList_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByForList(nums, target));
        }
        [Benchmark]
        public void RemoveByForMarkCopy_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByForMarkCopy(nums, target));
        }
        [Benchmark]
        public void RemoveByForMarkResize_10000_1000()
        {
            var res = Datas[1000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByForMarkResize(nums, target));
        }

        [Benchmark]
        public void RemoveByWhere_10000_5000()
        {
            var res = Datas[5000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByWhere(nums, target));
        }
        [Benchmark]
        public void RemoveByList_10000_5000()
        {
            var res = Datas[5000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByList(nums, target));
        }
        [Benchmark]
        public void RemoveByArray_10000_5000()
        {
            var res = Datas[5000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByArray(nums, target));
        }
        [Benchmark]
        public void RemoveByForList_10000_5000()
        {
            var res = Datas[5000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByForList(nums, target));
        }
        [Benchmark]
        public void RemoveByForMarkCopy_10000_5000()
        {
            var res = Datas[5000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByForMarkCopy(nums, target));
        }
        [Benchmark]
        public void RemoveByForMarkResize_10000_5000()
        {
            var res = Datas[5000];
            Handle(res, (nums, target) => _ = ArrayAddRemove.RemoveByForMarkResize(nums, target));
        }
        #endregion

        private static void Handle((int[] nums, int[] target)[] res, Func<int[], int[], int[]> func)
        {
            foreach (var (nums, target) in res)
            {
                func(nums, target);
            }
        }
    }
}
