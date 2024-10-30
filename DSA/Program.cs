using System.Text.Json;

namespace DSA
{
    internal class Program
    {
        static void Main(string[] args)
        {

            MyselfHashChaining<int, string> hashTable = new MyselfHashChaining<int, string>();
            hashTable.Init(10);
            hashTable.Insert(1, "One");
            hashTable.Insert(2, "Two");
            hashTable.Insert(21, "Eleven"); // 可能发生冲突
            //hashTable.Traverse();

            Console.WriteLine(hashTable.Find(1)); // 输出: One
            Console.WriteLine(hashTable.Find(21)); // 输出: Eleven

            hashTable.Remove(1); // 删除键 1

            //hashTable.Traverse();
            try
            {
                Console.WriteLine(hashTable.Find(1)); // 尝试查找已删除的键
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine(e.Message); // 输出: Key not found: 1
            }

            // 遍历并输出所有键
            foreach (var key in hashTable.GetKeys())
            {
                Console.WriteLine(key);
            }

            // 遍历并输出所有值
            foreach (var value in hashTable.GetValues())
            {
                Console.WriteLine(value);
            }

            hashTable.Insert(1, "Two3");
            //hashTable.Traverse();
            hashTable.Insert(4, "Two4");
            //hashTable.Traverse();
            hashTable.Insert(5, "Two5");
            //hashTable.Traverse();
            hashTable.Insert(6, "Two6");
            //hashTable.Traverse();
            hashTable.Insert(7, "Two7");
            //hashTable.Traverse();
            hashTable.Insert(8, "Two8");
            //hashTable.Traverse();
            hashTable.Insert(21, "Two21");
            //hashTable.Traverse();
            hashTable.Insert(10, "Two10");
            //hashTable.Traverse();
            hashTable.Insert(21, "Two10");


            //var minHeap = new MyselfMinHeap();
            //minHeap.Init(8);
            //minHeap.Push(7);
            //Console.WriteLine();
            //minHeap.LevelOrderTraversal();
            //minHeap.Push(6);
            //Console.WriteLine();
            //minHeap.LevelOrderTraversal();
            //minHeap.Push(5);
            //Console.WriteLine();
            //minHeap.LevelOrderTraversal();
            //minHeap.Push(4);
            //Console.WriteLine();
            //minHeap.LevelOrderTraversal();
            //minHeap.Push(3);
            //Console.WriteLine();
            //minHeap.LevelOrderTraversal();
            //minHeap.Push(2);
            //Console.WriteLine();
            //minHeap.LevelOrderTraversal();
            //minHeap.Push(1);
            //Console.WriteLine();
            //minHeap.LevelOrderTraversal();
            //minHeap.Pop();
            //Console.WriteLine();
            //minHeap.LevelOrderTraversal();
            //minHeap.Pop();
            //Console.WriteLine();
            //minHeap.LevelOrderTraversal();
            //minHeap.Pop();
            //Console.WriteLine();
            //minHeap.LevelOrderTraversal();
            //minHeap.Pop();
            //Console.WriteLine();
            //minHeap.LevelOrderTraversal();
            //minHeap.Pop();
            //Console.WriteLine();
            //minHeap.LevelOrderTraversal();
            //minHeap.Pop();
            //Console.WriteLine();
            //minHeap.LevelOrderTraversal();
            //minHeap.Pop();
            //Console.WriteLine();
            //minHeap.LevelOrderTraversal();
            //minHeap.Heapify(new int[8] { 8, 7, 6, 5, 4, 3, 2, 1 });
            //Console.WriteLine();
            //minHeap.LevelOrderTraversal();


            //var tree=new MyselfTreeLinkedList<int>();
            //tree.InitRoot(1);
            //tree.AddOrUpdateLeftChild(tree.Root, 2);
            //tree.AddOrUpdateRightChild(tree.Root, 3);
            //tree.AddOrUpdateLeftChild(tree.Root.Left, 4);
            //tree.AddOrUpdateRightChild(tree.Root.Left, 5);
            //tree.AddOrUpdateLeftChild(tree.Root.Right, 6);
            //tree.AddOrUpdateRightChild(tree.Root.Right, 7);
            //Console.WriteLine();
            //tree.PreOrderTraversal(tree.Root);
            //Console.WriteLine();
            //tree.InOrderTraversal(tree.Root);
            //Console.WriteLine();
            //tree.PostOrderTraversal(tree.Root);
            //Console.WriteLine();
            //tree.LevelOrderTraversal();
            //tree.Remove(tree.Root.Left);
            //Console.WriteLine();
            //tree.LevelOrderTraversal();
            //var tree = new MyselfTreeArray<int>();
            //tree.Init(7);
            //tree.AddOrUpdate(0, 1);
            //tree.AddOrUpdateLeftChild(1, 2);
            //tree.AddOrUpdateRightChild(1, 3);
            //tree.AddOrUpdateLeftChild(2, 4);
            //tree.AddOrUpdateRightChild(2, 5);
            //tree.AddOrUpdateLeftChild(3, 6);
            //tree.AddOrUpdateRightChild(3, 7);
            //Console.WriteLine();
            //tree.PreOrderTraversal();
            //Console.WriteLine();
            //tree.InOrderTraversal();
            //Console.WriteLine();
            //tree.PostOrderTraversal();
            //Console.WriteLine();
            //tree.LevelOrderTraversal();
            //tree.Remove(2);
            //Console.WriteLine();
            //tree.LevelOrderTraversal();

            Console.WriteLine("Hello, World!");
        }
    }
}
