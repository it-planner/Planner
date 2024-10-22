using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DSA
{
    public class MyselfTreeArray<T>
    {
        //存放树节点元素
        private T[] _array;

        //初始化树为指定容量
        public MyselfTreeArray<T> Init(int capacity)
        {
            //初始化指定长度数组用于存放树节点元素
            _array = new T[capacity];
            //返回树
            return this;
        }

        //树节点数量
        public int Count
        {
            get
            {
                return _array.Length;
            }
        }

        //返回指定数据的索引   
        public int GetIndex(T data)
        {
            if (data == null)
            {
                return -1;
            }

            //根据值查找索引
            return Array.IndexOf(_array, data);
        }

        //根据子索引计算父节点索引
        public int CalcParentIndex(int childIndex)
        {
            //应用公式计算父节点索引
            var parentIndex = (childIndex + 1) / 2 - 1;

            //检查索引是否越界
            if (childIndex <= 0 || childIndex >= _array.Length
                || parentIndex < 0 || parentIndex >= _array.Length)
            {
                return -1;
            }
            //返回父节点索引
            return parentIndex;
        }

        //根据父节点索引计算左子节点索引
        public int CalcLeftChildIndex(int parentIndex)
        {
            //应用公式计算左子节点索引
            var leftChildIndex = 2 * parentIndex + 1;

            //检查索引是否越界
            if (leftChildIndex <= 0 || leftChildIndex >= _array.Length
                || parentIndex < 0 || parentIndex >= _array.Length)
            {
                return -1;
            }

            //返回左子节点索引
            return leftChildIndex;
        }

        //根据父节点索引计算右子节点索引
        public int CalcRightChildIndex(int parentIndex)
        {
            //应用公式计算右子节点索引
            var rightChildIndex = 2 * parentIndex + 2;

            //检查索引是否越界
            if (rightChildIndex <= 0 || rightChildIndex >= _array.Length
                || parentIndex < 0 || parentIndex >= _array.Length)
            {
                return -1;
            }

            //返回右子节点索引
            return rightChildIndex;
        }

        //通过节点索引获取节点值
        public T Get(int index)
        {
            //检查索引是否越界
            if (index < 0 || index >= _array.Length)
            {
                throw new IndexOutOfRangeException("无效索引");
            }

            //返回节点值
            return _array[index];
        }

        //通过节点索引获取其左子节点值
        public T GetLeftChild(int parentIndex)
        {
            //获取左子节点索引
            var leftChildIndex = CalcLeftChildIndex(parentIndex);

            //检查索引是否越界
            if (leftChildIndex < 0)
            {
                throw new IndexOutOfRangeException("无效索引");
            }

            //返回左子节点值
            return _array[leftChildIndex];
        }

        //通过节点索引获取其右子节点值
        public T GetRightChild(int parentIndex)
        {
            //获取右子节点索引
            var rightChildIndex = CalcRightChildIndex(parentIndex);

            //检查索引是否越界
            if (rightChildIndex < 0)
            {
                throw new IndexOutOfRangeException("无效索引");
            }

            //返回右子节点值
            return _array[rightChildIndex];
        }

        //通过节点索引添加或更新节点值
        public void AddOrUpdate(int index, T data)
        {
            //检查索引是否越界
            if (index < 0 || index >= _array.Length)
            {
                throw new IndexOutOfRangeException("无效索引");
            }

            //更新值
            _array[index] = data;
        }

        //通过节点值添加或更新左子节点值
        public void AddOrUpdateLeftChild(T parent, T left)
        {
            //获取节点索引
            var parentIndex = GetIndex(parent);

            //获取左子节点索引
            var leftChildIndex = CalcLeftChildIndex(parentIndex);

            //检查索引是否越界
            if (leftChildIndex < 0)
            {
                throw new IndexOutOfRangeException("无效索引");
            }

            //更新值
            _array[leftChildIndex] = left;
        }

        //通过节点值添加或更新右子节点值
        public void AddOrUpdateRightChild(T parent, T right)
        {
            //获取节点索引
            var parentIndex = GetIndex(parent);

            //获取左子节点索引
            var rightChildIndex = CalcRightChildIndex(parentIndex);

            //检查索引是否越界
            if (rightChildIndex < 0)
            {
                throw new IndexOutOfRangeException("无效索引");
            }

            //更新值
            _array[rightChildIndex] = right;
        }

        //通过节点索引删除节点及其所有后代节点
        public void Remove(int index)
        {
            //非法索引直接跳过
            if (index < 0 || index >= _array.Length)
            {
                return;
            }

            //清除节点值
            _array[index] = default;

            //获取左子节点索引
            var leftChildIndex = CalcLeftChildIndex(index);
            //递归删除左子节点及其所有后代
            Remove(leftChildIndex);

            //获取右子节点索引
            var rightChildIndex = CalcRightChildIndex(index);
            //递归删除右子节点及其所有后代
            Remove(rightChildIndex);
        }

        //通过节点值删除其左节点及其所有后代节点
        public void RemoveLeftChild(T parent)
        {
            //获取节点索引
            var parentIndex = GetIndex(parent);

            //获取左子节点索引
            var leftChildIndex = CalcLeftChildIndex(parentIndex);

            //检查索引是否越界
            if (leftChildIndex < 0)
            {
                throw new IndexOutOfRangeException("无效索引");
            }

            //删除左子节点及其所有后代
            Remove(leftChildIndex);
        }

        //通过节点值删除其右节点及其所有后代节点
        public void RemoveRightChild(T parent)
        {
            //获取节点索引
            var parentIndex = GetIndex(parent);

            //获取右子节点索引
            var rightChildIndex = CalcRightChildIndex(parentIndex);

            //检查索引是否越界
            if (rightChildIndex < 0)
            {
                throw new IndexOutOfRangeException("无效索引");
            }

            //删除右子节点及其所有后代
            Remove(rightChildIndex);
        }

        //前序遍历
        public void PreOrderTraversal(int index = 0)
        {
            //非法索引直接跳过
            if (index < 0 || index >= _array.Length)
            {
                return;
            }

            //打印
            Console.Write(_array[index]);

            //获取左子节点索引
            var leftChildIndex = CalcLeftChildIndex(index);
            //打印左子树
            PreOrderTraversal(leftChildIndex);

            //获取右子节点索引
            var rightChildIndex = CalcRightChildIndex(index);
            //打印右子树
            PreOrderTraversal(rightChildIndex);
        }

        //中序遍历
        public void InOrderTraversal(int index = 0)
        {
            //非法索引直接跳过
            if (index < 0 || index >= _array.Length)
            {
                return;
            }

            //获取左子节点索引
            var leftChildIndex = CalcLeftChildIndex(index);
            //打印左子树
            InOrderTraversal(leftChildIndex);

            //打印
            Console.Write(_array[index]);

            //获取右子节点索引
            var rightChildIndex = CalcRightChildIndex(index);
            //打印右子树
            InOrderTraversal(rightChildIndex);
        }

        //后序遍历
        public void PostOrderTraversal(int index = 0)
        {
            //非法索引直接跳过
            if (index < 0 || index >= _array.Length)
            {
                return;
            }

            //获取左子节点索引
            var leftChildIndex = CalcLeftChildIndex(index);
            //打印左子树
            PostOrderTraversal(leftChildIndex);

            //获取右子节点索引
            var rightChildIndex = CalcRightChildIndex(index);
            //打印右子树
            PostOrderTraversal(rightChildIndex);

            //打印
            Console.Write(_array[index]);
        }

        //层次遍历
        public void LevelOrderTraversal()
        {
            //创建一个队列用于层次遍历
            var queue = new Queue<int>();
            //先把根节点索引0入队
            queue.Enqueue(0);

            //只有队列中有值就一直处理
            while (queue.Count > 0)
            {
                //出列，取出第一个节点索引
                var currentIndex = queue.Dequeue();
                //打印第一个节点值
                Console.Write(_array[currentIndex]);

                //获取左子节点索引
                int leftChildIndex = CalcLeftChildIndex(currentIndex);
                // 如果左子节点存在，将其索引加入队列
                if (leftChildIndex >= 0)
                {
                    queue.Enqueue(leftChildIndex);
                }

                //获取右子节点索引
                int rightChildIndex = CalcRightChildIndex(currentIndex);
                // 如果右子节点存在，将其索引加入队列
                if (rightChildIndex >= 0)
                {
                    queue.Enqueue(rightChildIndex);
                }
            }
        }
    }

    public class MyselfTreeNode<T>
    {
        //数据域
        public T Data { get; set; }

        //左子节点
        public MyselfTreeNode<T> Left { get; set; }

        //右子节点
        public MyselfTreeNode<T> Right { get; set; }

        public MyselfTreeNode(T data)
        {
            Data = data;
            Left = null;
            Right = null;
        }
    }

    public class MyselfTreeLinkedList<T>
    {
        //根节点
        private MyselfTreeNode<T> _root;

        //树节点数量
        private int _count;

        //初始化树根节点
        public MyselfTreeLinkedList<T> InitRoot(T root)
        {
            _root = new MyselfTreeNode<T>(root);
            //树节点数量加1
            _count++;
            //返回树
            return this;
        }

        //获取树节点数量
        public int Count
        {
            get
            {
                return _count;
            }
        }

        //获取根节点
        public MyselfTreeNode<T> Root
        {
            get
            {
                return _root;
            }
        }

        //通过指定节点添加或更新左子节点值
        public void AddOrUpdateLeftChild(MyselfTreeNode<T> parent, T left)
        {
            if (parent.Left != null)
            {
                //更节点值
                parent.Left.Data = left;
                return;
            }

            //添加节点
            parent.Left = new MyselfTreeNode<T>(left);
            //节点数量加1
            _count++;
        }

        //通过指定节点添加或更新右子节点元素
        public void AddOrUpdateRightChild(MyselfTreeNode<T> parent, T right)
        {
            if (parent.Right != null)
            {
                //更节点值
                parent.Right.Data = right;
                return;
            }

            //添加节点
            parent.Right = new MyselfTreeNode<T>(right);
            //节点数量加1
            _count++;
        }

        //通过指定节点删除节点及其后代节点
        public void Remove(MyselfTreeNode<T> node)
        {
            if (node.Left != null)
            {
                //递归删除左子节点的所有后代
                Remove(node.Left);
            }

            if (node.Right != null)
            {
                //递归删除右子节点的所有后代
                Remove(node.Right);
            }

            //删除节点
            node.Data = default;
            //节点数量减1
            _count--;
        }

        //前序遍历
        public void PreOrderTraversal(MyselfTreeNode<T> node)
        {
            //打印
            Console.Write(node.Data);

            if (node.Left != null)
            {
                //打印左子树
                PreOrderTraversal(node.Left);
            }

            if (node.Right != null)
            {
                //打印右子树
                PreOrderTraversal(node.Right);
            }
        }

        //中序遍历
        public void InOrderTraversal(MyselfTreeNode<T> node)
        {
            if (node.Left != null)
            {
                //打印左子树
                InOrderTraversal(node.Left);
            }

            //打印
            Console.Write(node.Data);

            if (node.Right != null)
            {
                //打印右子树
                InOrderTraversal(node.Right);
            }
        }

        //后序遍历
        public void PostOrderTraversal(MyselfTreeNode<T> node)
        {
            if (node.Left != null)
            {
                //打印左子树
                PostOrderTraversal(node.Left);
            }

            if (node.Right != null)
            {
                //打印右子树
                PostOrderTraversal(node.Right);
            }

            //打印
            Console.Write(node.Data);
        }

        //层次遍历
        public void LevelOrderTraversal()
        {
            //创建一个队列用于层次遍历
            Queue<MyselfTreeNode<T>> queue = new Queue<MyselfTreeNode<T>>();
            //先把根节点入队
            queue.Enqueue(_root);

            //只有队列中有值就一直处理
            while (queue.Count > 0)
            {
                //出列，取出第一个节点
                var node = queue.Dequeue();
                //打印第一个节点值
                Console.Write(node.Data);

                // 如果左子节点存在将其加入队列
                if (node.Left != null)
                {
                    queue.Enqueue(node.Left);
                }

                // 如果右子节点存在将其加入队列
                if (node.Right != null)
                {
                    queue.Enqueue(node.Right);
                }
            }
        }
    }

}
