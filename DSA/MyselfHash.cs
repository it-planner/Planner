using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DSA
{
    public class MyselfHashChaining<TKey, TValue>
    {
        //存储散列表的记录
        private class Entry
        {
            //键
            public TKey Key;
            //值
            public TValue Value;
            //下一个节点
            public Entry Next;

            public Entry(TKey key, TValue value)
            {
                Key = key;
                Value = value;
                Next = null;
            }
        }

        //散列桶数组
        private Entry[] _buckets;
        //桶的数量
        private int _size;
        //元素数量
        private int _count;

        //初始化指定容量的散列表
        public MyselfHashChaining<TKey, TValue> Init(int capacity = 16)
        {
            //桶数量
            _size = capacity;
            //初始化桶数组
            _buckets = new Entry[capacity];
            _count = 0;
            return this;
        }

        //元素数量
        public int Count
        {
            get
            {
                return _count;
            }
        }

        //插入键值
        public void Insert(TKey key, TValue value)
        {
            //负载因子达到 0.75 触发重新散列
            if (_count >= _size * 0.75)
            {
                Rehash();
            }

            //计算key的散列桶索引
            var index = CalcBucketIndex(key);
            //新建一条散列表记录
            var newEntry = new Entry(key, value);

            //判断key所在桶索引位置是否为空
            if (_buckets[index] == null)
            {
                //如果为空，则直接存储再此桶索引位置
                _buckets[index] = newEntry;
            }
            else
            {
                //如果不为空，则存储在此桶里的链表上
                //取出此桶中的记录即链表的头节点
                var current = _buckets[index];
                //遍历链表
                while (true)
                {
                    //如果链表中存在相同的key，则更新其value
                    if (current.Key.Equals(key))
                    {
                        //更新值
                        current.Value = value;
                        return;
                    }

                    //如果当前节点没有后续节点，则停止遍历链表
                    if (current.Next == null)
                    {
                        break;
                    }

                    //如果当前节点有后续节点，则继续遍历链表后续节点
                    current = current.Next;
                }

                //如果链表中不存在相同的key
                //则把新的散列表记录添加到链表尾部
                current.Next = newEntry;
            }

            //元素数量加1
            _count++;
        }

        //计算key的散列桶索引
        private int CalcBucketIndex(TKey key)
        {
            //使用取模法计算索引，使用绝对值防止负数索引
            return Math.Abs(key.GetHashCode() % _size);
        }

        //根据key删除记录
        public void Remove(TKey key)
        {
            //计算key的散列桶索引
            var index = CalcBucketIndex(key);
            //取出key所在桶索引位置的记录即链表的头节点
            var current = _buckets[index];
            //用于暂存上一个节点
            Entry previous = null;

            //遍历链表
            while (current != null)
            {
                //如果链表中存在相同的key，则删除
                if (current.Key.Equals(key))
                {
                    if (previous == null)
                    {
                        //删除头节点
                        _buckets[index] = current.Next;
                    }
                    else
                    {
                        //删除中间节点
                        previous.Next = current.Next;
                    }

                    //元素数量减1
                    _count--;
                    return;
                }

                //当前节点赋值给上一个节点变量
                previous = current;
                //继续遍历链表后续节点
                current = current.Next;
            }

            //如果未找到key则报错
            throw new KeyNotFoundException($"未找到key");
        }

        //根据key查找value
        public TValue Find(TKey key)
        {
            //计算key的散列桶索引
            var index = CalcBucketIndex(key);
            //取出key所在桶索引位置的记录即链表的头节点
            var current = _buckets[index];

            //遍历链表
            while (current != null)
            {
                //如果链表中存在相同的key，则返回value
                if (current.Key.Equals(key))
                {
                    return current.Value;
                }

                //如果当前节点有后续节点，则继续遍历链表后续节点
                current = current.Next;
            }

            //如果未找到key则报错
            throw new KeyNotFoundException($"未找到key");
        }

        //获取所有键
        public TKey[] GetKeys()
        {
            //初始化所有key数组
            var keys = new TKey[_count];
            var index = 0;

            //遍历散列桶
            for (var i = 0; i < _size; i++)
            {
                //获取每个桶链表头节点
                var current = _buckets[i];
                //遍历链表
                while (current != null)
                {
                    //收集键
                    keys[index++] = current.Key;
                    //继续遍历链表后续节点
                    current = current.Next;
                }
            }

            //返回所有键的数组
            return keys;
        }

        //获取所有值
        public TValue[] GetValues()
        {
            //初始化所有value数组
            var values = new TValue[_count];
            var index = 0;

            //遍历散列桶
            for (var i = 0; i < _size; i++)
            {
                //获取每个桶链表头节点
                var current = _buckets[i];
                //遍历链表
                while (current != null)
                {
                    //收集值
                    values[index++] = current.Value;
                    //继续遍历链表后续节点
                    current = current.Next;
                }
            }

            //返回所有值的数组
            return values;
        }

        //再散列
        public void Rehash()
        {
            //扩展2倍大小
            var newSize = _size * 2;
            //更新桶数量
            _size = newSize;
            //初始化元素个数
            _count = 0;
            //暂存老的散列表数组
            var oldBuckets = _buckets;
            //初始化新的散列表数组
            _buckets = new Entry[newSize];

            //遍历老的散列桶
            for (var i = 0; i < oldBuckets.Length; i++)
            {
                //获取老的散列桶的每个桶链表头节点
                var current = oldBuckets[i];
                //遍历链表
                while (current != null)
                {
                    //调用插入方法
                    Insert(current.Key, current.Value);

                    //暂存下一个节点
                    var next = current.Next;
                    if (next == null)
                    {
                        break;
                    }

                    //继续处理下一个节点
                    current = next;
                }
            }
        }

        public void Traverse(Action<TKey, TValue> action)
        {
            foreach (var bucket in _buckets)
            {
                Entry current = bucket;
                while (current != null)
                {
                    action(current.Key, current.Value);
                    current = current.Next;
                }
            }
        }
    }

    public class MyselfHashOpenAddressing<TKey, TValue>
    {
        //存储散列表
        private struct Entry
        {
            //键
            public TKey Key;
            //值
            public TValue Value;
            //用于标记该位置是否被占用
            public bool IsActive;
        }

        //散列表数组
        private Entry[] _array;
        //散列表的大小
        private int _size;
        //元素数量
        private int _count;

        //初始化指定容量的散列表
        public MyselfHashOpenAddressing<TKey, TValue> Init(int capacity = 16)
        {
            //散列表的大小
            _size = capacity;
            //初始化散列表数组
            _array = new Entry[capacity];
            _count = 0;
            return this;
        }

        //元素数量
        public int Count
        {
            get
            {
                return _count;
            }
        }

        //插入键值
        public void Insert(TKey key, TValue value)
        {
            //负载因子达到 0.75 触发重新散列
            if (_count >= _size * 0.75)
            {
                Rehash();
            }

            //计算key的散列表索引
            var index = CalcIndex(key);
            //遍历散列表，当位置为非占用状态则结束探测
            while (_array[index].IsActive)
            {
                //如果散列表中存在相同的key，则更新其value
                if (_array[index].Key.Equals(key))
                {
                    _array[index].Value = value;
                    return;
                }

                //否则，使用线性探测法，继续探测下一个元素
                index = (index + 1) % _size;
            }

            //在非占用位置处添加新元素
            _array[index] = new Entry
            {
                Key = key,
                Value = value,
                IsActive = true
            };

            //元素数量加1
            _count++;
        }

        //计算key的散列表索引
        private int CalcIndex(TKey key)
        {
            //使用取模法计算索引，使用绝对值防止负数索引
            return Math.Abs(key.GetHashCode() % _size);
        }

        //根据key删除元素
        public void Remove(TKey key)
        {
            //计算key的散列表索引
            var index = CalcIndex(key);
            //遍历散列表，当位置为非占用状态则结束探测
            while (_array[index].IsActive)
            {
                //如果散列表中存在相同的key，则标记为非占用状态
                if (_array[index].Key.Equals(key))
                {
                    _array[index].IsActive = false;
                    //元素数量减1
                    _count--;
                    return;
                }

                //否则，使用线性探测法，继续探测下一个元素
                index = (index + 1) % _size;
            }

            //如果未找到key则报错
            throw new KeyNotFoundException($"未找到key");
        }

        //根据key查找value
        public TValue Find(TKey key)
        {
            //计算key的散列表索引
            int index = CalcIndex(key);
            while (_array[index].IsActive)
            {
                //如果散列表中存在相同的key，则返回value
                if (_array[index].Key.Equals(key))
                {
                    return _array[index].Value;
                }

                //否则，使用线性探测法，继续探测下一个元素
                index = (index + 1) % _size;
            }

            //如果未找到key则报错
            throw new KeyNotFoundException($"未找到key");
        }

        //获取所有键
        public IEnumerable<TKey> GetKeys()
        {
            //遍历散列表
            for (var i = 0; i < _size; i++)
            {
                //收集所有占用状态的键
                if (_array[i].IsActive)
                {
                    yield return _array[i].Key;
                }
            }
        }

        //获取所有值
        public IEnumerable<TValue> GetValues()
        {
            //遍历散列表
            for (var i = 0; i < _size; i++)
            {
                //收集所有占用状态的值
                if (_array[i].IsActive)
                {
                    yield return _array[i].Value;
                }
            }
        }

        //再散列
        public void Rehash()
        {
            //扩展2倍大小
            var newSize = _size * 2;
            //暂存老的散列表数组
            var oldArray = _array;
            //初始化新的散列表数组
            _array = new Entry[newSize];
            //更新散列表大小
            _size = newSize;
            //初始化元素个数
            _count = 0;

            //遍历老的散列表数组
            foreach (var entry in oldArray)
            {
                if (entry.IsActive)
                {
                    //如果是占用状态
                    //则重新插入到新的散列表数组中
                    Insert(entry.Key, entry.Value);
                }
            }
        }

        public void Traverse()
        {
            //遍历散列表
            for (var i = 0; i < _size; i++)
            {
                Console.WriteLine($"{i}-{_array[i].Key}-{_array[i].Value}-{_array[i].IsActive}");
            }
        }
    }
}
