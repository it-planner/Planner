using System.Runtime.InteropServices;

namespace DSA
{

    public sealed class MyselfArray : IDisposable
    {
        //申请内存起始位置指针
        private IntPtr _pointer;
        //数组长度
        private int _length;

        //初始化数组为指定长度，并元素设置默认值0
        public MyselfArray Init(int capacity)
        {
            //初始化数组长度为capacity
            _length = capacity;
            //分配指定字节数的内存空间
            _pointer = Marshal.AllocHGlobal(capacity * sizeof(int));
            //初始化数组元素
            for (int i = 0; i < _length; i++)
            {
                //初始化每个元素为0
                Marshal.WriteInt32(_pointer + i * sizeof(int), 0);
            }

            //返回数组
            return this;
        }

        //数组长度
        public int Length
        {
            get
            {
                return _length;
            }
        }

        //根据索引获取元素
        public int Get(int index)
        {
            //索引小于0 或者索引大于数组长度-1 则报错
            if (index < 0 || index > _length - 1) throw new IndexOutOfRangeException();
            //读取指定索引元素值
            return Marshal.ReadInt32(_pointer + index * sizeof(int));
        }

        //根据索引设置元素
        public void Set(int index, int value)
        {
            //索引小于0 或者索引大于数组长度-1 则报错
            if (index < 0 || index > _length - 1) throw new IndexOutOfRangeException();
            //根据索引设置元素值
            Marshal.WriteInt32(_pointer + index * sizeof(int), value);
        }

        //根据索引插入元素
        public void Insert(int index, int value)
        {
            //索引小于0 或者索引大于数组长度-1 则报错
            if (index < 0 || index > _length - 1) throw new IndexOutOfRangeException();

            //获取索引处的值
            var v = Get(index);
            //如果索引处无值
            if (v == 0)
            {
                //直接在索引处插入新元素并返回
                Set(index, value);
                return;
            }

            //定义空位置索引
            var nullIndex = -1;
            //检查插入位置之后是否有空位
            for (int i = index + 1; i < _length; i++)
            {
                //有空位
                if (Get(i) == 0)
                {
                    //记录空位置处索引，并结束检查
                    nullIndex = i;
                    break;
                }
            }

            //如果没找到空位，则报错
            if (nullIndex == -1)
            {
                throw new InvalidOperationException("没有可用的空位用于插入。");
            }

            //从插入位置到空位之前的元素向后移动一位
            for (int i = nullIndex; i > index; i--)
            {
                Set(i, Get(i - 1));
            }

            //在指定索引处插入新元素
            Set(index, value);
        }

        //根据索引移除元素
        public void Remove(int index)
        {
            //索引小于0 或者索引大于数组长度-1 则报错
            if (index < 0 || index > _length - 1) throw new IndexOutOfRangeException();

            //后面的元素（除了最后一个元素）向前移动一位
            for (int i = index; i < _length - 1; i++)
            {
                Set(i, Get(i + 1));
            }

            //最后一位设为默认值0
            Set(_length - 1, 0);
        }

        public void Dispose()
        {
            if (_pointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_pointer);
                _pointer = IntPtr.Zero;
            }
        }
    }
}
