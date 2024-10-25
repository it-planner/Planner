namespace DSA
{
    //小根堆
    public class MyselfMinHeap
    {
        //存放堆元素
        private int[] _array;
        //数组尾元素索引
        private int _tailIndex;

        //初始化堆为指定容量
        public MyselfMinHeap Init(int capacity)
        {
            //初始化指定长度数组用于存放堆元素
            _array = new int[capacity];
            //初始化数组尾元素索引为-1，表示空数组
            _tailIndex = -1;
            //返回堆
            return this;
        }

        //堆容量
        public int Capacity
        {
            get
            {
                return _array.Length;
            }
        }

        //堆元素数量
        public int Count
        {
            get
            {
                //数组尾元素索引加1即为堆元素数量
                return _tailIndex + 1;
            }
        }


        //获取堆顶元素，即最小元素
        public int Top
        {
            get
            {
                if (IsEmpty())
                {
                    //空堆，不可以进行获取最小堆元素操作
                    throw new InvalidOperationException("空堆");
                }

                return _array[0];
            }
        }


        //检查堆是否为空
        public bool IsEmpty()
        {
            return _tailIndex == -1;
        }

        //检查堆是否为满堆
        public bool IsFull()
        {
            //数组末尾元素索引等于数组容量减1表示满堆
            return _tailIndex == _array.Length - 1;
        }

        //入堆 向堆中添加一个元素
        public void Push(int data)
        {
            if (IsFull())
            {
                //满堆，不可以进行入添加元素操作
                throw new InvalidOperationException("满堆");
            }

            //新元素索引
            var index = _tailIndex + 1;

            //在数组末尾添加新元素
            _array[index] = data;

            //向上调整堆，以保持堆的性质
            SiftUp(index);

            //尾元素索引向后前进1位
            _tailIndex++;
        }

        //向上调整堆，以保持堆的性质
        private void SiftUp(int index)
        {
            //一直调整到堆顶为止
            while (index > 0)
            {
                //计算父节点元素索引
                var parentIndex = (index - 1) / 2;
                //如果当前元素大于等于其父节点元素，则结束调整
                if (_array[index] >= _array[parentIndex])
                {
                    break;
                }

                //否则，交换两个元素
                (_array[parentIndex], _array[index]) = (_array[index], _array[parentIndex]);

                //更新当前索引为父节点元素索引继续调整
                index = parentIndex;
            }
        }

        //出堆 删除并返回堆中最小元素
        public int Pop()
        {
            if (IsEmpty())
            {
                //空堆，不可以进行删除并返回堆中最小元素操作
                throw new InvalidOperationException("空堆");
            }

            //取出数组第一个元素即最小元素
            var min = _array[0];
            //将数组末尾元素赋值给第一个元素
            _array[0] = _array[_tailIndex];

            //将数组末尾元素设为默认值
            _array[_tailIndex] = 0;
            //将数组末尾元素索引向前移动1位
            _tailIndex--;

            //向下调整堆，以保持堆的性质
            SiftDown(0);

            //返回最小元素
            return min;
        }

        //向下调整堆，以保持堆的性质
        private void SiftDown(int index)
        {
            while (index <= _tailIndex)
            {
                //定义较小值索引变量，用于存放比较当前元素及其左右子节点元素中最小元素
                var minIndex = index;

                //计算右子节点索引
                var rightChildIndex = 2 * index + 2;
                //如果存在右子节点，则比较其与当前元素，保留值较小的索引
                if (rightChildIndex <= _tailIndex && _array[rightChildIndex] < _array[minIndex])
                {
                    minIndex = rightChildIndex;
                }

                //计算左子节点索引
                var leftChildIndex = 2 * index + 1;
                //如果存在左子节点，则比较其与较小值元素，保留值较小的索引
                if (leftChildIndex <= _tailIndex && _array[leftChildIndex] < _array[minIndex])
                {
                    minIndex = leftChildIndex;
                }

                //如果当前元素就是最小的，则停止调整
                if (minIndex == index)
                {
                    break;
                }

                //否则，交换当前元素和较小元素
                (_array[minIndex], _array[index]) = (_array[index], _array[minIndex]);

                //更新索引为较小值索引，继续调整
                index = minIndex;
            }
        }

        //堆化，即把一个无序数组堆化成小根堆
        public void Heapify(int[] array)
        {
            if (array == null || _array.Length < array.Length)
            {
                throw new InvalidOperationException("无效数组");
            }

            //将数组复制到堆中
            Array.Copy(array, _array, array.Length);

            //更新尾元素索引
            _tailIndex = array.Length - 1;

            //从最后一个非叶子节点开始向下调整堆
            for (int i = (array.Length / 2) - 1; i >= 0; i--)
            {
                SiftDown(i);
            }
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
    }
}
