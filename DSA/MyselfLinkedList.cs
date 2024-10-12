using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace DSA
{
    public struct MyselfLinkedListNode
    {
        //数据域
        public string Data { get; set; }
        //指针域
        public IntPtr Next { get; set; }
    }

    public sealed class MyselfLinkedList : IDisposable
    {
        //申请内存起始位置指针
        private IntPtr _head;
        //链表长度
        private int _length;

        //初始化链表，声明头指针，并创建头节点
        public MyselfLinkedListNode Init()
        {
            //计算节点的大小
            var size = Marshal.SizeOf(typeof(MyselfLinkedListNode));
            //分配指定字节数的内存空间
            _head = Marshal.AllocHGlobal(size);

            //创建头节点
            var node = new MyselfLinkedListNode
            {
                Data = null,
                Next = IntPtr.Zero
            };

            //将节点实例写入分配的内存
            Marshal.StructureToPtr(node, _head, false);
            //链表长度加1
            _length++;

            //返回头节点
            return node;
        }

        //链表长度
        public int Length
        {
            get
            {
                return _length;
            }
        }

        //头节点
        public MyselfLinkedListNode? HeadNode
        {
            get
            {
                if (_head == IntPtr.Zero)
                {
                    return null;
                }

                return GetNode(_head);
            }
        }

        //获取节点
        private MyselfLinkedListNode GetNode(IntPtr pointer)
        {
            // 从分配的内存读取实例
            return Marshal.PtrToStructure<MyselfLinkedListNode>(pointer);
        }


        //在指定节点后插入新节点
        public MyselfLinkedListNode InsertAfter(MyselfLinkedListNode node, string value)
        {
            //获指定取节点对应指针
            var pointer = GetPointer(node);
            //如果指针不为空才处理
            if (pointer != IntPtr.Zero)
            {
                //以新值创建一个节点
                var (newPointer, newNode) = CreateNode(value);
                //把新节点的下一个节点指针指向指定节点的下一个节点
                newNode.Next = node.Next;
                //把指定节点的下一个节点指针指向新节点
                node.Next = newPointer;
                //更新修改后的节点
                Marshal.StructureToPtr(newNode, newPointer, false);
                Marshal.StructureToPtr(node, pointer, false);
                //链表长度加1
                _length++;
                return newNode;
            }

            return default;
        }

        //获取节点对应指针
        private IntPtr GetPointer(MyselfLinkedListNode node)
        {
            //从头指针开始查找
            var currentPointer = _head;
            //如果当前指针为空则停止查找
            while (currentPointer != IntPtr.Zero)
            {
                //获取当前指针对应的节点
                var currentNode = GetNode(currentPointer);

                //如果当前节点数据域和指针域与要查找的节点相同则返回当前节点指针
                if (currentNode.Data == node.Data && currentNode.Next == node.Next)
                {
                    return currentPointer;
                }

                //否则查找下一个节点
                currentPointer = currentNode.Next;
            }

            return IntPtr.Zero;
        }

        //创建节点
        private (IntPtr Pointer, MyselfLinkedListNode Node) CreateNode(string value)
        {
            //计算大小
            var size = Marshal.SizeOf(typeof(MyselfLinkedListNode));
            //分配指定字节数的内存空间
            var pointer = Marshal.AllocHGlobal(size);

            //创建实例并设置值
            var node = new MyselfLinkedListNode
            {
                Data = value,
                Next = IntPtr.Zero
            };

            //将实例写入分配的内存
            Marshal.StructureToPtr(node, pointer, false);

            //返回节点指针和节点
            return (pointer, node);
        }

        //根据数据查找节点
        public MyselfLinkedListNode Find(string value)
        {
            //从头指针开始查找
            var pointer = _head;
            //如果当前指针为空则停止查找
            while (pointer != IntPtr.Zero)
            {
                //获取当前指针对应的节点
                var node = GetNode(pointer);

                //如果当前节点数据域和要查找值相同则返回当前节点
                if (node.Data == value)
                {
                    return node;
                }

                //否则查找下一个节点
                pointer = node.Next;
            }

            return default;
        }

        //更新节点数据
        public void Update(MyselfLinkedListNode node, string value)
        {
            //获取节点对应指针
            var pointer = GetPointer(node);
            //当指针不为空，则更新节点数据
            if (pointer != IntPtr.Zero)
            {
                //修改数据
                node.Data = value;
                //将数据写入分配的内存，完成数据更新
                Marshal.StructureToPtr(node, pointer, false);
            }
        }

        //移除节点
        public void Remove(MyselfLinkedListNode node)
        {
            //从头指针开始查找
            var currentPointer = _head;
            //获取当前节点
            var currentNode = GetNode(_head);

            //查找节点对应的指针
            var pointer = GetPointer(node);
            while (true)
            {
                if (currentNode.Next == IntPtr.Zero)
                {
                    //指针为空则返回
                    return;
                }
                else if (currentNode.Next == pointer)
                {
                    //把要删除节点的上一个节点对应的下一个节点指向要删除节点的下一个节点
                    currentNode.Next = node.Next;
                    //手动释放被删除节点对应的内存
                    Marshal.FreeHGlobal(pointer);
                    //更新要删除节点的上一个节点
                    Marshal.StructureToPtr(currentNode, currentPointer, false);
                    //链表长度减1
                    _length--;
                    break;
                }
                else
                {
                    //查找下一个节点
                    currentPointer = currentNode.Next;
                    currentNode = GetNode(currentPointer);
                }
            }
        }

        //销毁链表
        public void Destroy()
        {
            var pointer = _head;
            while (pointer != IntPtr.Zero)
            {
                var value = GetNode(pointer);
                Marshal.FreeHGlobal(pointer);
                _length--;
                pointer = value.Next;
            }

            _head = IntPtr.Zero;
        }

        public void Dispose()
        {
            Destroy();
        }
    }
}
