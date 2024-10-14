using System.Reflection;
using System.Runtime.InteropServices;

namespace DSA
{
    public class MyselfStackArray<T>
    {
        //存放栈元素
        private T[] _array;
        //栈容量
        private int _capacity;
        //栈顶索引，为-1表示空栈
        private int _top;

        //初始化栈为指定容量
        public MyselfStackArray<T> Init(int capacity)
        {
            //初始化栈容量为capacity
            _capacity = capacity;
            //初始化指定长度数组用于存放栈元素
            _array = new T[_capacity];
            //初始化为空栈
            _top = -1;

            //返回栈
            return this;
        }

        //栈容量
        public int Capacity
        {
            get
            {
                return _capacity;
            }
        }

        //栈长度
        public int Length
        {
            get
            {
                //栈长度等于栈顶元素加1
                return _top + 1;
            }
        }

        //获取栈顶元素
        public T Top
        {
            get
            {
                if (IsEmpty())
                {
                    //空栈，不可以进行获取栈顶元素操作
                    throw new InvalidOperationException("空栈");
                }

                return _array[_top];
            }
        }

        //是否空栈
        public bool IsEmpty()
        {
            //栈顶索引小于0表示空栈
            return _top < 0;
        }

        //是否满栈
        public bool IsFull()
        {
            //栈顶索引等于容量大小表示满栈
            return _top == _capacity - 1;
        }

        //入栈
        public void Push(T value)
        {
            if (IsFull())
            {
                //栈顶索引大于等于容量大小减1，表明已经满栈，不可以进行入栈操作
                throw new InvalidOperationException("满栈");
            }

            //栈顶索引先向后移动1位，然后再存放栈顶元素
            _array[++_top] = value;
        }

        //出栈
        public T Pop()
        {
            if (IsEmpty())
            {
                //栈顶索引小于0表示空栈，不可以进行出栈操作
                throw new InvalidOperationException("空栈");
            }

            //返回栈顶元素后，栈顶索引向前移动1位
            return _array[_top--];
        }


    }

    public class MyselfStackNode<T>
    {
        //数据域
        public T Data;
        //指针域，即下一个节点
        public MyselfStackNode<T> Next;

        public MyselfStackNode(T data)
        {
            Data = data;
            Next = null;
        }
    }

    public class MyselfStackLinkedList<T>
    {
        //栈顶节点即首元节点
        private MyselfStackNode<T> _top;
        //栈长度
        private int _length;

        //初始化栈
        public MyselfStackLinkedList<T> Init()
        {
            //初始化栈顶节点为空
            _top = null;
            //初始化栈长度为0
            _length = 0;

            //返回栈
            return this;
        }

        //栈长度
        public int Length
        {
            get
            {
                return _length;
            }
        }

        //获取栈顶元素
        public T Top
        {
            get
            {
                if (IsEmpty())
                {
                    //空栈，不可以进行获取栈顶元素操作
                    throw new InvalidOperationException("空栈");
                }

                //返回首元节点数据域
                return _top.Data;
            }
        }

        //是否空栈
        public bool IsEmpty()
        {
            //栈顶节点为null表示空栈
            return _top == null;
        }

        //入栈
        public void Push(T value)
        {
            //创建新的栈顶节点
            var node = new MyselfStackNode<T>(value);
            //将老的栈顶节点赋值给新节点的指针域
            node.Next = _top;
            //把栈顶节点变更为新创建的节点
            _top = node;
            //栈长度加1
            _length++;
        }

        //出栈
        public T Pop()
        {
            if (IsEmpty())
            {
                //空栈，不可以进行出栈操作
                throw new InvalidOperationException("空栈");
            }
            //获取栈顶节点数据
            var data = _top.Data;
            //把栈顶节点变更为原栈顶节点对应的下一个节点
            _top = _top.Next;
            //栈长度减1
            _length--;
            //返回栈顶数据
            return data;
        }


    }

}
