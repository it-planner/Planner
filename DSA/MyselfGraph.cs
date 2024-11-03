using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DSA
{
    public class MyselfGraphArray<T>
    {
        //点集合
        private T[] _vertexArray { get; set; }

        //边集合
        private int[,] _edgeArray { get; set; }

        //点数量
        private int _vertexCount;

        //边数量
        private int _edgeCount { get; set; }

        //初始化
        public MyselfGraphArray<T> Init(int length)
        {
            //初始化指定长度点集合
            _vertexArray = new T[length];
            //初始化指定长度边集合
            _edgeArray = new int[length, length];
            //初始化点数量
            _vertexCount = 0;
            //初始化边数量
            _edgeCount = 0;
            return this;
        }

        //返回点数量
        public int VertexCount
        {
            get
            {
                return _vertexCount;
            }
        }

        //返回边数量
        public int EdgeCount
        {
            get
            {
                return _edgeCount;
            }
        }

        //返回指定点元素的索引   
        public int GetVertexIndex(T vertex)
        {
            if (vertex == null)
            {
                return -1;
            }

            //根据值查找索引
            return Array.IndexOf(_vertexArray, vertex);
        }

        //返回指定点索引的元素
        public T GetVertexByIndex(int index)
        {
            //如果索引非法则报错
            if (index < 0 || index > _vertexArray.Length - 1)
            {
                throw new InvalidOperationException("索引错误");
            }

            return _vertexArray[index];
        }

        //插入点
        public void InsertVertex(T vertex)
        {
            //获取点索引
            var index = GetVertexIndex(vertex);
            //如果索引大于-1说明点已存在，则直接返回
            if (index > -1)
            {
                return;
            }

            //如果点集合已满，则直接返回
            if (_vertexCount == _vertexArray.Length)
            {
                return;
            }

            //添加点元素，并且更新点数量
            _vertexArray[_vertexCount++] = vertex;
        }

        //插入边
        public void InsertEdge(T vertex1, T vertex2, int weight)
        {
            //根据点元素获取点索引
            var vertexIndex1 = GetVertexIndex(vertex1);
            //如果索引等于-1说明点不存在，则直接返回
            if (vertexIndex1 == -1)
            {
                return;
            }

            //根据点元素获取点索引
            var vertexIndex2 = GetVertexIndex(vertex2);
            //如果索引等于-1说明点不存在，则直接返回
            if (vertexIndex2 == -1)
            {
                return;
            }

            //更新两点关系，即边信息
            _edgeArray[vertexIndex1, vertexIndex2] = weight;
            //用于无向图，对于有向图则删除此句子
            _edgeArray[vertexIndex2, vertexIndex1] = weight;
            //更新边数量
            _edgeCount++;
        }

        //返回两点之间边的权值
        public int GetWeight(T vertex1, T vertex2)
        {
            //根据点元素获取点索引
            var vertexIndex1 = GetVertexIndex(vertex1);
            //如果索引等于-1说明点不存在
            if (vertexIndex1 == -1)
            {
                //如果未找到点则报错
                throw new KeyNotFoundException($"点不存在");
            }

            //根据点元素获取点索引
            var vertexIndex2 = GetVertexIndex(vertex2);
            //如果索引等于-1说明点不存在
            if (vertexIndex2 == -1)
            {
                //如果未找到点则报错
                throw new KeyNotFoundException($"点不存在");
            }

            return _edgeArray[vertexIndex1, vertexIndex2];
        }

        //深度优先遍历
        public void DFS(T startVertex)
        {
            //根据点元素获取点索引
            var startVertexIndex = GetVertexIndex(startVertex);
            //如果索引等于-1说明点不存在
            if (startVertexIndex == -1)
            {
                //如果未找到点则报错
                throw new KeyNotFoundException($"点不存在");
            }

            //定义已访问标记数组
            //因为无向图对称特性因此一维数组即可
            //如果是有向图则需要定义二维数组
            var visited = new bool[_vertexCount];
            DFSUtil(startVertexIndex, visited);
            Console.WriteLine();
        }

        //深度优先遍历
        private void DFSUtil(int index, bool[] visited)
        {
            //标记当前元素已访问过
            visited[index] = true;
            //打印点
            Console.Write(_vertexArray[index] + " ");

            //遍历查找与当前元素相邻的元素
            for (var i = 0; i < _vertexCount; i++)
            {
                //如果是相邻的元素，并且元素未被访问过
                if (_edgeArray[index, i] == 1 && !visited[i])
                {
                    //则递归调用自身方法
                    DFSUtil(i, visited);
                }
            }
        }

        //广度优先遍历
        public void BFS(T startVertex)
        {
            //根据点元素获取点索引
            var startVertexIndex = GetVertexIndex(startVertex);
            //如果索引等于-1说明点不存在
            if (startVertexIndex == -1)
            {
                //如果未找到点则报错
                throw new KeyNotFoundException($"点不存在");
            }

            //定义已访问标记数组
            //因为无向图对称特性因此一维数组即可
            //如果是有向图则需要定义二维数组
            var visited = new bool[_vertexCount];
            //使用队列实现广度优先遍历
            var queue = new Queue<int>();
            //将起点入队
            queue.Enqueue(startVertexIndex);
            //标记起点为已访问
            visited[startVertexIndex] = true;

            //遍历队列
            while (queue.Count > 0)
            {
                //出队点
                var vertexIndex = queue.Dequeue();
                //打印点
                Console.Write(_vertexArray[vertexIndex] + " ");

                //遍历查找与当前元素相邻的元素
                for (var i = 0; i < _vertexCount; i++)
                {
                    //如果是相邻的元素，并且元素未被访问过
                    if (_edgeArray[vertexIndex, i] == 1 && !visited[i])
                    {
                        //则将相邻元素索引入队
                        queue.Enqueue(i);
                        //并标记为已访问
                        visited[i] = true;
                    }
                }
            }

            Console.WriteLine();
        }

        //打印图
        public void PrintGraph()
        {
            int row = _edgeArray.GetLength(0);
            int colnum = _edgeArray.GetLength(1);

            string rowContent1 = "  ";
            for (int i = 0; i < row; i++)
            {
                rowContent1 += _vertexArray[i].ToString() + " ";
            }
            Console.WriteLine(rowContent1);
            for (int i = 0; i < row; i++)
            {
                string rowContent = _vertexArray[i].ToString() + " ";
                for (int j = 0; j < colnum; j++)
                {
                    rowContent += _edgeArray[i, j] + " ";
                }
                Console.WriteLine(rowContent);
            }
        }
    }

    public class MyselfGraphNode<T>
    {
        //数据域
        public T Data { get; set; }

        //左子节点
        public MyselfGraphNode<T> Left { get; set; }

        //右子节点
        public MyselfGraphNode<T> Right { get; set; }

        public MyselfGraphNode(T data)
        {
            Data = data;
            Left = null;
            Right = null;
        }
    }

    public class MyselfGraphLinkedList<T>
    {
        private readonly Dictionary<int, List<int>> _adjacencyList;

        public MyselfGraphLinkedList()
        {
            _adjacencyList = new Dictionary<int, List<int>>();
        }

        // 添加边
        public void AddEdge(int source, int destination)
        {
            if (!_adjacencyList.ContainsKey(source))
            {
                _adjacencyList[source] = new List<int>();
            }
            if (!_adjacencyList.ContainsKey(destination))
            {
                _adjacencyList[destination] = new List<int>();
            }

            _adjacencyList[source].Add(destination);
            _adjacencyList[destination].Add(source); // 无向图
        }

        // 打印图
        public void PrintGraph()
        {
            foreach (var vertex in _adjacencyList)
            {
                Console.Write(vertex.Key + ": ");
                foreach (var neighbor in vertex.Value)
                {
                    Console.Write(neighbor + " ");
                }
                Console.WriteLine();
            }
        }

        // 广度优先遍历
        public void BFS(int startVertex)
        {
            var visited = new HashSet<int>();
            var queue = new Queue<int>();

            visited.Add(startVertex);
            queue.Enqueue(startVertex);

            while (queue.Count > 0)
            {
                int vertex = queue.Dequeue();
                Console.Write(vertex + " ");

                foreach (var neighbor in _adjacencyList[vertex])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
            Console.WriteLine();
        }

        // 深度优先遍历
        public void DFS(int startVertex)
        {
            var visited = new HashSet<int>();
            DFSUtil(startVertex, visited);
            Console.WriteLine();
        }

        private void DFSUtil(int vertex, HashSet<int> visited)
        {
            visited.Add(vertex);
            Console.Write(vertex + " ");

            foreach (var neighbor in _adjacencyList[vertex])
            {
                if (!visited.Contains(neighbor))
                {
                    DFSUtil(neighbor, visited);
                }
            }
        }
    }

}
