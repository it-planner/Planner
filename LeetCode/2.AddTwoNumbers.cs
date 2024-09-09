using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace LeetCode
{

    public class ListNode
    {
        public int val;
        public ListNode next;
        public ListNode(int val = 0, ListNode next = null)
        {
            this.val = val;
            this.next = next;
        }
    }

    public class _2_AddTwoNumbers
    {
        public static ListNode AddTwoNumbersIteration(ListNode l1, ListNode l2)
        {
            //创建头节点，即第一位计算结果
            var head = new ListNode(0);
            //用于迭代节点
            var current = head;
            //初始化进位值
            var carry = 0;

            //当两个链表节点都不为空并且进位值不等于0，则继续迭代
            while (l1 != null || l2 != null || carry != 0)
            {
                //以进位值为初始值定义两节点和变量，
                var sum = carry;

                //如果l1节点不为空，则累加其节点值，并且把其下个节点赋值给自身，用于下次迭代
                if (l1 != null)
                {
                    sum += l1.val;
                    l1 = l1.next;
                }

                //如果l2节点不为空，则累加其节点值，并且把其下个节点赋值给自身，用于下次迭代
                if (l2 != null)
                {
                    sum += l2.val;
                    l2 = l2.next;
                }

                //计算进位值
                carry = sum / 10;
                //以当前位值，创建下一个节点
                current.next = new ListNode(sum % 10);
                //把下个节点赋值给当前迭代节点，继续下次迭代
                current = current.next;
            }

            //返回实际结果链表的头节点
            return head.next;
        }

        public static ListNode AddTwoNumbersRecursion(ListNode l1, ListNode l2)
        {
            return AddTwoNumbersRecursive(l1, l2, 0);
        }

        private static ListNode AddTwoNumbersRecursive(ListNode l1, ListNode l2, int carry)
        {
            //当两个链表节点都为空并且进位值等于0，则结束递归
            if (l1 == null && l2 == null && carry == 0)
            {
                return null;
            }

            //以进位值为初始值定义两节点和变量，
            var sum = carry;

            //如果l1节点不为空，则累加其节点值，并且把其下个节点赋值给自身，用于下次迭代
            if (l1 != null)
            {
                sum += l1.val;
                l1 = l1.next;
            }

            //如果l2节点不为空，则累加其节点值，并且把其下个节点赋值给自身，用于下次迭代
            if (l2 != null)
            {
                sum += l2.val;
                l2 = l2.next;
            }

            //计算进位值
            carry = sum / 10;
            //以当前位值，创建下一个节点
            return new ListNode(sum % 10)
            {
                //递归点
                next = AddTwoNumbersRecursive(l1, l2, carry)
            };
        }
    }

    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    [MemoryDiagnoser]
    public class _2_AddTwoNumbersBenchmark : IBenchmark
    {
        /// <summary>
        /// 测试次数
        /// </summary>
        private const int TestNumber = 100;
        /// <summary>
        /// 数组长度
        /// </summary>
        private const int ListNodeLength = 15000;

        private readonly (ListNode l1, ListNode l2)[] Datas = new (ListNode l1, ListNode l2)[TestNumber];

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random();
            for (var j = 0; j < TestNumber; j++)
            {
                var node1 = new ListNode();
                var node2 = new ListNode();

                CreateNode(node1, random, 0);
                CreateNode(node2, random, 0);
                Datas[j] = (node1, node2);
            }

        }
        private void CreateNode(ListNode node, Random random, int count)
        {
            node.val = random.Next(0, 10);
            node.next = new ListNode();
            if (count < ListNodeLength)
            {
                count++;
                CreateNode(node, random, count);
            }
        }

        [Benchmark]
        public void AddTwoNumbersRecursion()
        {
            foreach (var (node1, node2) in Datas)
            {
                _ = _2_AddTwoNumbers.AddTwoNumbersRecursion(node1, node2);
            }
        }

        [Benchmark]
        public void AddTwoNumbersIteration()
        {
            foreach (var (node1, node2) in Datas)
            {
                _ = _2_AddTwoNumbers.AddTwoNumbersIteration(node1, node2);
            }
        }
    }
}
