using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace DSA
{
    public class MyselfQueueArray<T>
    {
        //存放队列元素
        private T[] _array;
        //队列容量
        private int _capacity;
        //队尾索引，为-1表示空队列
        private int _tail;

        //初始化队列为指定容量
        public MyselfQueueArray<T> Init(int capacity)
        {
            //初始化队列容量为capacity
            _capacity = capacity;
            //初始化指定长度数组用于存放队列元素
            _array = new T[_capacity];
            _tail = -1;

            //返回队列
            return this;
        }

        //队列容量
        public int Capacity
        {
            get
            {
                return _capacity;
            }
        }

        //队列长度
        public int Length
        {
            get
            {
                if (IsEmpty())
                {
                    return 0;
                }

                //队列长度等于队尾索引加1
                return _tail + 1;
            }
        }

        //获取队头元素
        public T Head
        {
            get
            {
                if (IsEmpty())
                {
                    //空队列，不可以进行获取队头元素操作
                    throw new InvalidOperationException("空队列");
                }

                return _array[0];
            }
        }

        //获取队尾元素
        public T Tail
        {
            get
            {
                if (IsEmpty())
                {
                    //空队列，不可以进行获取队头元素操作
                    throw new InvalidOperationException("空队列");
                }

                return _array[_tail];
            }
        }

        //是否空队列
        public bool IsEmpty()
        {
            //队尾索引小于0表示空队列
            return _tail < 0;
        }

        //是否满队列
        public bool IsFull()
        {
            //队头索引等于容量大小减1表示满队列
            return _tail == _capacity - 1;
        }

        //入队
        public void Enqueue(T value)
        {
            if (IsFull())
            {
                //满队列，不可以进行入队列操作
                throw new InvalidOperationException("满队列");
            }

            //队尾索引向后移动1位
            _tail++;
            //给队尾元素赋值新值
            _array[_tail] = value;

        }

        //出队
        public T Dequeue()
        {
            if (IsEmpty())
            {
                //空队列，不可以进行出队列操作
                throw new InvalidOperationException("空队列");
            }

            //取出队头元素
            var value = _array[0];
            //对头元素重置为默认值
            _array[0] = default;

            //队头元素后面所有元素都向队头移动一位
            for (int i = 0; i < _tail; i++)
            {
                _array[i] = _array[i + 1];
            }

            //队尾元素重置为默认值
            _array[_tail] = default;
            //队尾索引向队头方向移动一位
            _tail--;

            //返回队头元素
            return value;
        }
    }

    public class MyselfQueueNode<T>
    {
        //数据域
        public T Data;
        //指针域，即下一个节点
        public MyselfQueueNode<T> Next;

        public MyselfQueueNode(T data)
        {
            Data = data;
            Next = null;
        }
    }

    public class MyselfQueueLinkedList<T>
    {
        //队头节点即首元节点
        private MyselfQueueNode<T> _head;
        //队尾节点即尾节点
        private MyselfQueueNode<T> _tail;
        //队列长度
        private int _length;

        //初始化队列
        public MyselfQueueLinkedList<T> Init()
        {
            //初始化队头节点为空
            _head = null;
            //初始化队尾节点为空
            _tail = null;
            //初始化队列长度为0
            _length = 0;

            //返回队列
            return this;
        }

        //队列长度
        public int Length
        {
            get
            {
                return _length;
            }
        }

        //获取队头元素
        public T Head
        {
            get
            {
                if (IsEmpty())
                {
                    //空队列，不可以进行获取队头元素操作
                    throw new InvalidOperationException("空队列");
                }

                //返回队头节点数据域
                return _head.Data;
            }
        }

        //获取队尾元素
        public T Tail
        {
            get
            {
                if (IsEmpty())
                {
                    //空队列，不可以进行获取队尾元素操作
                    throw new InvalidOperationException("空队列");
                }

                //返回队尾节点数据域
                return _tail.Data;
            }
        }

        //是否空队列
        public bool IsEmpty()
        {
            //队头节点为null和队尾节点都为空表示空队列
            return _head == null && _tail == null;
        }

        //入队
        public void Enqueue(T value)
        {
            //创建新的队尾节点
            var node = new MyselfQueueNode<T>(value);
            //如果队尾节点不为空，则把新的队尾节点连接到尾节点后面
            if (_tail != null)
            {
                _tail.Next = node;
            }

            //队尾节点变更为新的队尾节点
            _tail = node;
            //如果队头节点为空，则为其赋值为队尾节点
            if (_head == null)
            {
                _head = _tail;
            }
            //队列长度加1
            _length++;
        }

        //出队
        public T Dequeue()
        {
            if (IsEmpty())
            {
                //空队列，不可以进行出队列操作
                throw new InvalidOperationException("空队列");
            }
            //获取队头节点数据
            var data = _head.Data;
            //把队头节点变更为原队头节点对应的下一个节点
            _head = _head.Next;
            //如果队列为空，表明为空队列，同时更新队尾为空
            if (_head == null)
            {
                _tail = null; 
            }
            //队列长度减1
            _length--;
            //返回队头节点数据
            return data;
        }


    }

}
