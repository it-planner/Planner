using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace LeetCode
{
    public class _4_FindMedianSortedArrays
    {
        /// <summary>
        /// 使用内置方法直接合并数组并排序法
        /// </summary>
        public static double MergedArraySort(int[] nums1, int[] nums2)
        {
            var merged = nums1.Concat(nums2).ToArray();
            Array.Sort(merged);
            var len = merged.Length;
            if (len % 2 == 0)
            {
                return (merged[(len / 2) - 1] + merged[len / 2]) / 2.0;
            }
            else
            {
                return merged[len / 2];
            }
        }

        /// <summary>
        /// 双指针合并数组后取值法
        /// </summary>
        public static double TwoPointerMergedArray(int[] nums1, int[] nums2)
        {
            //创建一个可以容纳两个数组的大数组
            var list = new int[nums1.Length + nums2.Length];
            //数组1的索引
            var idx1 = 0;
            //数组2的索引
            var idx2 = 0;
            //大数组的索引
            var idx = 0;
            //如果两个数组都处理完成则退出，每次大数组索引前进1位
            while (idx1 < nums1.Length || idx2 < nums2.Length)
            {
                if (idx1 >= nums1.Length)
                {
                    //如果数组1没有数据了，则把数组2当前值填充至大数组后，把数组2索引前进1位
                    list[idx] = nums2[idx2++];
                }
                else if (idx2 >= nums2.Length)
                {
                    //如果数组2没有数据了，则把数组1当前值填充至大数组，把数组1索引前进1位
                    list[idx] = nums1[idx1++];
                }
                else
                {
                    //当两个数组都有数据，则比较当前两个数，数小的则填充至大数组，并且把其对应的索引前进1位
                    if (nums1[idx1] < nums2[idx2])
                    {
                        list[idx] = nums1[idx1++];
                    }
                    else
                    {
                        list[idx] = nums2[idx2++];
                    }
                }
                //大数字索引前进1位
                idx++;
            }

            if (idx % 2 == 0)
            {
                //偶数则取中间两位数，并取平均数
                return (list[(idx / 2) - 1] + list[idx / 2]) / 2.0;
            }
            else
            {
                //奇数则去中间一位数据直接返回
                return list[idx / 2];
            }
        }

        public static double TwoPointerMergedArrayOpt(int[] nums1, int[] nums2)
        {
            var list = new int[nums1.Length + nums2.Length];
            var idx1 = 0;
            var idx2 = 0;
            var idx = 0;
            while (idx1 < nums1.Length || idx2 < nums2.Length)
            {
                if (idx1 < nums1.Length && (idx2 >= nums2.Length || nums1[idx1] < nums2[idx2]))
                {
                    list[idx] = nums1[idx1++];
                }
                else
                {
                    list[idx] = nums2[idx2++];
                }

                idx++;
            }
            if (idx % 2 == 0)
            {
                return (list[(idx / 2) - 1] + list[idx / 2]) / 2.0;
            }
            else
            {
                return list[idx / 2];
            }

        }

        public static double TwoPointerDirectValueOriginal(int[] nums1, int[] nums2)
        {
            var len = nums1.Length + nums2.Length;
            var left = 0;
            var right = 0;

            var idx1 = 0;
            var idx2 = 0;
            while (idx1 < nums1.Length || idx2 < nums2.Length)
            {
                int temp;
                if (idx1 >= nums1.Length)
                {
                    temp = nums2[idx2++];
                }
                else if (idx2 >= nums2.Length)
                {
                    temp = nums1[idx1++];
                }
                else
                {
                    if (nums1[idx1] < nums2[idx2])
                    {
                        temp = nums1[idx1++];
                    }
                    else
                    {
                        temp = nums2[idx2++];
                    }
                }

                var idx = idx1 + idx2 - 1;
                if (len % 2 == 0)
                {
                    if ((len / 2) - 1 == idx)
                    {
                        left = temp;
                    }
                    if (len / 2 == idx)
                    {
                        right = temp;
                        break;
                    }
                }
                else
                {
                    if (len / 2 == idx)
                    {
                        left = temp;
                        right = temp;
                        break;
                    }
                }
            }

            return (left + right) / 2.0;
        }

        /// <summary>
        /// 双指针直接取值法
        /// </summary>
        public static double TwoPointerDirectValue(int[] nums1, int[] nums2)
        {
            //两个数组总长度
            var len = nums1.Length + nums2.Length;
            //左数，偶数：左中位数，奇数：中位数前一位
            var left = 0;
            //右数，偶数：右中位数，奇数：中位数
            var right = 0;

            //数组1索引
            var idx1 = 0;
            //数组2索引
            var idx2 = 0;
            //当中位数位处理完则退出
            while (true)
            {
                //把上一个值先放入左数，右数存当前值
                left = right;
                if (idx1 >= nums1.Length)
                {
                    //如果数组1没有数据了，则把数组2当前值放入右数，把数组2索引前进1位
                    right = nums2[idx2++];
                }
                else if (idx2 >= nums2.Length)
                {
                    //如果数组2没有数据了，则把数组1当前值放入右数，把数组1索引前进1位
                    right = nums1[idx1++];
                }
                else
                {
                    //当两个数组都有数据，则比较当前两个数，数小的则放入右数，并且把其对应的索引前进1位
                    if (nums1[idx1] < nums2[idx2])
                    {
                        right = nums1[idx1++];
                    }
                    else
                    {
                        right = nums2[idx2++];
                    }
                }

                //当中位数位处理完则结束处理
                if (len / 2 == idx1 + idx2 - 1)
                {
                    break;
                }
            }

            if (len % 2 == 0)
            {
                //偶数则取左数和右数平均数
                return (left + right) / 2.0;
            }
            else
            {
                //奇数则直接返回右数
                return right;
            }
        }

        /// <summary>
        /// 双指针二分排除查找法
        /// </summary>
        public static double TwoPointerBinaryExclude(int[] nums1, int[] nums2)
        {
            //数组总长度
            var len = nums1.Length + nums2.Length;
            //目标数，表示中位数在数组中第几个数，偶数则代表左中位数，奇数则代表中位数
            var target = (len + 1) / 2;
            //左中位数
            int left;
            //右中位数
            int right;
            //数组1索引
            var idx1 = 0;
            //数组2索引
            var idx2 = 0;
            //当中位数位处理完则退出
            while (true)
            {
                //数组1元素没数据了，则直接在数组2中获取中位数
                if (idx1 >= nums1.Length)
                {
                    //因为数组1没有数据了，所以数组2索引前进到目标数处
                    idx2 += target - 1;

                    //直接取中位数
                    if (len % 2 == 0)
                    {
                        //偶数
                        //左中位数为当前值
                        left = nums2[idx2];
                        //右中位数为下一个值
                        right = nums2[idx2 + 1];
                    }
                    else
                    {
                        //奇数，左右中位数相同都是当前值
                        left = right = nums2[idx2];
                    }
                    break;
                }
                //数组2元素没数据了，则直接在数组1中获取中位数
                if (idx2 >= nums2.Length)
                {
                    //因为数组2没有数据了，所以数组1索引前进到目标数处
                    idx1 += target - 1;

                    //直接取中位数
                    if (len % 2 == 0)
                    {
                        //偶数
                        //左中位数为当前值
                        left = nums1[idx1];
                        //右中位数为下一个值
                        right = nums1[idx1 + 1];
                    }
                    else
                    {
                        //奇数，左右中位数相同都是当前值
                        left = right = nums1[idx1];
                    }
                    break;
                }

                //当目标数为1时，表明当前值就是要找的值
                if (target == 1)
                {
                    //直接取中位数
                    if (len % 2 == 0)
                    {
                        //偶数
                        if (nums1[idx1] < nums2[idx2])
                        {
                            //如果nums1当前值比较小，则查看其之后是否还有元素
                            if (idx1 + 1 > nums1.Length - 1)
                            {
                                //如果其之后没有元素，则左中位数为nums1当前值，右中位数为nums2当前值
                                left = nums1[idx1];
                                right = nums2[idx2];
                            }
                            else
                            {
                                //如果其之后有元素，则左中位数为nums1当前值，右中位数则为nums1当前值后一个值和nums2当前值中较小值
                                var temp = nums1[idx1 + 1];
                                left = nums1[idx1];
                                right = Math.Min(nums2[idx2], temp);
                            }
                        }
                        else
                        {
                            //如果nums2当前值比较小，则查看其之后是否还有元素
                            if (idx2 + 1 > nums2.Length - 1)
                            {
                                //如果其之后没有元素，则左中位数为nums2当前值，右中位数为nums1当前值
                                left = nums2[idx2];
                                right = nums1[idx1];
                            }
                            else
                            {
                                //如果其之后有元素，则左中位数为nums2当前值，右中位数则为nums2当前值后一个值和nums1当前值中较小值
                                var temp = nums2[idx2 + 1];
                                left = nums2[idx2];
                                right = Math.Min(nums1[idx1], temp);
                            }
                        }
                    }
                    else
                    {
                        //奇数，左右中位数相同，取nums1当前值和nums2当前值中较小值
                        left = right = Math.Min(nums1[idx1], nums2[idx2]);
                    }

                    break;
                }

                //取出目标数位置的一半
                var half = target / 2;
                //确定nums1比较数，并确保其不会大于自身长度
                var compare1 = Math.Min(idx1 + half, nums1.Length);
                //确定nums2用比较数，并确保其不会大于自身长度
                var compare2 = Math.Min(idx2 + half, nums2.Length);
                //比较两个数组比较数，compare-1因为比较数表示第几个数，减1转为索引
                if (nums1[compare1 - 1] < nums2[compare2 - 1])
                {
                    //nums1的比较数 小，则排除掉
                    //要查找的目标数需要减掉已经排除掉的个数
                    target -= compare1 - idx1;
                    //同时nums1当前索引前进到被排除掉元素的后一位
                    idx1 = compare1;
                }
                else
                {
                    //nums2的比较数 小，则排除掉
                    //要查找的目标数需要减掉已经排除掉的个数
                    target -= compare2 - idx2;
                    //同时nums2当前索引前进到被排除掉元素的后一位
                    idx2 = compare2;
                }
            }

            return (left + right) / 2.0;
        }

        /// <summary>
        /// 双指针二分查找第K小数法
        /// </summary>
        public static double TwoPointerBinaryFindKth(int[] nums1, int[] nums2)
        {
            //左第K小数，当为偶数,代表左中位数，当为奇数，代表中位数
            var leftKth = (nums1.Length + nums2.Length + 1) / 2;
            //右第K小数，当为偶数,代表右中位数，当为奇数，代表中位数（因为/向下取整特性）
            var rightKth = (nums1.Length + nums2.Length + 2) / 2;
            //获取左中位数
            var left = TwoPointerBinaryFindKth(leftKth, nums1, nums2);
            //获取右中位数
            var rigth = TwoPointerBinaryFindKth(rightKth, nums1, nums2);
            return (left + rigth) / 2.0;
        }

        /// <summary>
        /// 双指针二分查找第K小数
        /// </summary>
        public static int TwoPointerBinaryFindKth(int kth, int[] nums1, int[] nums2)
        {
            //数组1索引
            var idx1 = 0;
            //数组2索引
            var idx2 = 0;

            //找到第K小数则退出
            while (true)
            {
                //数组1没数据了，则直接在数组2中查找K
                if (idx1 >= nums1.Length)
                {
                    //因为数组1没有数据了，所以数组2索引前进到K数索引处
                    idx2 += kth - 1;
                    return nums2[idx2];
                }

                //数组2没数据，则直接在数组1中查找K
                if (idx2 >= nums2.Length)
                {
                    //因为数组2没有数据了，所以数组1索引前进到K数索引处
                    idx1 += kth - 1;
                    return nums1[idx1];
                }

                //当第K小数为1时，表明当前值就是要找的值
                if (kth == 1)
                {
                    return Math.Min(nums1[idx1], nums2[idx2]);
                }

                //取出第K小数位置的一半
                var half = kth / 2;
                //确定nums1比较数，并确保其不会大于自身长度
                var compare1 = Math.Min(idx1 + half, nums1.Length);
                //确定nums2用比较数，并确保其不会大于自身长度
                var compare2 = Math.Min(idx2 + half, nums2.Length);
                //比较两个数组比较数，compare-1因为比较数表示第几个数，减1转为索引
                if (nums1[compare1 - 1] < nums2[compare2 - 1])
                {
                    //nums1的比较数 小，则排除掉
                    //要查找的第K小数需要减掉已经排除掉的个数
                    kth -= compare1 - idx1;
                    //同时nums1当前索引前进到被排除掉元素的后一位
                    idx1 = compare1;
                }
                else
                {
                    //nums2的比较数 小，则排除掉
                    //要查找的第K小数需要减掉已经排除掉的个数
                    kth -= compare2 - idx2;
                    //同时nums2当前索引前进到被排除掉元素的后一位
                    idx2 = compare2;
                }
            }
        }

        /// <summary>
        /// 双指针二分查找平分法
        /// </summary>
        public static double TwoPointerBinaryHalves(int[] nums1, int[] nums2)
        {
            //当数组1长度比数组2长度大，则交换两个数组，保证数组1为较短的数组
            if (nums1.Length > nums2.Length)
            {
                //交换两个变量的值，C#7.0中 元组解构赋值 语法
                (nums1, nums2) = (nums2, nums1);
            }

            //左指针
            var idxLeft = 0;
            //右指针
            var idxRight = nums1.Length;
            //对数组1进行二分查找
            while (idxLeft <= idxRight)
            {
                //计算得到数组1分割后的当前索引
                var idx1 = (idxLeft + idxRight) / 2;
                //计算得到数组2分割后的当前索引
                var idx2 = ((nums1.Length + nums2.Length + 1) / 2) - idx1;
                //当数组2左边最大值大于数组1右边最小值时，左指针向右前进
                if (idx2 != 0 && idx1 != nums1.Length && nums2[idx2 - 1] > nums1[idx1])
                {
                    idxLeft = idx1 + 1;
                }
                //当数组1左边最大值大于数组2右边最小值时，右指针向左前进
                else if (idx1 != 0 && idx2 != nums2.Length && nums1[idx1 - 1] > nums2[idx2])
                {
                    idxRight = idx1 - 1;
                }
                else
                {
                    //左边最大值
                    int leftMax;
                    //如果分割到数组1的最左边即数组1当前索引为0，则左边最大值直接取数组2
                    if (idx1 == 0)
                    {
                        leftMax = nums2[idx2 - 1];
                    }
                    //如果分隔到数组2的最左边即数组2当前索引为0，则左边最大值直接取数组1
                    else if (idx2 == 0)
                    {
                        leftMax = nums1[idx1 - 1];
                    }
                    //否则左边最大值为数组1和数组2中左边较大的值
                    else
                    {
                        leftMax = Math.Max(nums1[idx1 - 1], nums2[idx2 - 1]);
                    }

                    //如果数组为计算，则直接返回左边最大值，结束计算
                    if ((nums1.Length + nums2.Length) % 2 == 1)
                    {
                        return leftMax;
                    }

                    //右边最小值
                    int rightMin;
                    //如果分隔到数组最右边即数组1索引为其自身长度，则右边最小值直接取数组2
                    if (idx1 == nums1.Length)
                    {
                        rightMin = nums2[idx2];
                    }
                    //如果分隔到数组最右边即数组2索引为其自身长度，则右边最小值直接取数组1
                    else if (idx2 == nums2.Length)
                    {
                        rightMin = nums1[idx1];
                    }
                    //否则右边最小值为数组1和数组2中右边较小的值
                    else
                    {
                        rightMin = Math.Min(nums2[idx2], nums1[idx1]);
                    }

                    return (leftMax + rightMin) / 2.0;
                }
            }
            return 0.0;
        }
    }

    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    [MemoryDiagnoser]
    public class _4_FindMedianSortedArraysBenchmark : IBenchmark
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
