using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace NET9
{

    public class Student
    {
        public string Name { get; set; }

        public string Grade { get; set; }

        public int Age { get; set; }


        public void CountByExample()
        {
            var students = new List<Student>
            {
                new Student { Name = "小明", Age = 10 },
                new Student { Name = "小红", Age = 12 },
                new Student { Name = "小华", Age = 10 },
                new Student { Name = "小亮", Age = 11 }
            };

            //统计不同年龄有多少人，两个版本实现

            //.NET 9 之前
            var group = students.GroupBy(x => x.Age);
            foreach (var item in group)
            {
                Console.WriteLine($"年龄为：{item.Key}，有：{item.Count()} 人。");
            }


            //.NET 9
            foreach (var student in students.CountBy(c => c.Age))
            {
                Console.WriteLine($"年龄为：{student.Key}，有：{student.Value} 人。");
            }
        }

        public void AggregateByExample()
        {
            var students = new List<Student>
            {
                new Student { Name = "小明", Grade = "一班", Age = 10 },
                new Student { Name = "小红", Grade = "二班", Age = 12 },
                new Student { Name = "小华", Grade = "一班", Age = 10 },
                new Student { Name = "小亮", Grade = "二班", Age = 11 }
            };

            //统计每个班级各自学生总年龄，两个版本实现

            //.NET 9 之前
            var old = students
               .GroupBy(stu => stu.Grade)
               .ToDictionary(group => group.Key, group => group.Sum(stu => stu.Age))
               .AsEnumerable();
            foreach (var item in old)
            {
                Console.WriteLine($"班级：{item.Key}，总年龄：{item.Value} 。");
            }

            //.NET 9
            foreach (var group in students.AggregateBy(c => c.Grade, 0, (acc, stu) => acc + stu.Age))
            {
                Console.WriteLine($"班级：{group.Key}，总年龄：{group.Value} 。");
            }
        }
    }




    public partial class PartialExamples
    {
        //部分属性
        public partial int Capacity { get; set; }

        //部分索引器
        public partial string this[int index] { get; set; }

        //部分方法
        public partial string? TryGetItemAt(int index);
    }

    public partial class PartialExamples
    {
        private List<string> _items = ["one", "two", "three", "four", "five"];

        //部分属性
        public partial int Capacity
        {
            get => _items.Count;
            set
            {
                if ((value != _items.Count) && (value >= 0))
                {
                    _items.Capacity = value;
                }
            }
        }

        //部分索引器
        public partial string this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        //部分方法
        public partial string? TryGetItemAt(int index)
        {
            if (index < _items.Count)
            {
                return _items[index];
            }
            return null;
        }
    }

    public interface IInterface { }

    public ref struct RefStructInterface : IInterface
    {
        public void Example()
        {
            RefStructInterface rsi = new();
            //IInterface result = (IInterface)rsi;
        }
    }



    public interface IInterface1 { }

    public struct RefStructInterface1 : IInterface1
    {
        public void Example()
        {
            RefStructInterface1 span = new();
            IInterface1 d = span;
        }
    }

    ////.NET 9 之前
    //public class Disallow
    //{
    //    public static void Process<T>(T data)
    //    {
    //    }

    //    public void Example()
    //    {
    //        Span<int> span = new();
    //        Process(span);
    //    }
    //}

    //.NET 9
    public class Allow
    {
        public static void Process<T>(T data) where T : allows ref struct
        {
        }

        public void Example()
        {
            Span<int> span = new();
            Process(span);
        }
    }
    public ref struct MyRefStruct
    {
        public int Value;
    }

    internal class Program
    {
        ref int Process(ref int x)
        {
            return ref x;
        }

        //在迭代器中使用ref
        IEnumerable<int> RefInIterator(int[] array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                ref var v = ref Process(ref array[i]);
                yield return v;
            }
        }

        //在异步方法中使用ref
        async Task RefInAsync()
        {
            var value = 0;
            await Task.Delay(0);
            ref var local = ref Process(ref value);
        }

        async Task WhenEachAsync()
        {
            //生成100个随机时间完成的任务列表
            var tasks = Enumerable.Range(1, 100)
                           .Select(async i =>
                           {
                               await Task.Delay(new Random().Next(1000, 5000));
                               return $"任务 {i} 完成";
                           })
                           .ToList();

            //.NET 9 之前
            while (tasks.Count > 0)
            {
                var completedTask = await Task.WhenAny(tasks);
                tasks.Remove(completedTask);
                Console.WriteLine(await completedTask);
            }

            //.NET 9
            await foreach (var completedTask in Task.WhenEach(tasks))
            {
                Console.WriteLine(await completedTask);
            }
        }

        //public async Task Main()
        //{
        //    Span<int> numbers = stackalloc int[100];  // `ref struct`，栈分配
        //                                              // 此时 `numbers` 是一个有效的 `ref struct`

        //    await Task.Delay(1000);  // 异步操作

        //    // 此时 `numbers` 变量已经跨越了异步操作的边界，这会导致编译错误
        //    Console.WriteLine(numbers[0]); // Error: Cannot use `ref struct` variable after `await`
        //}
        static void Main()
        {
            var dd = new ConcurrentDictionary<string,string>();
            TimeSpan timeSpan2 = TimeSpan.FromSeconds(seconds: 101, milliseconds: 832);
            Console.WriteLine($"timeSpan2 = {timeSpan2}");
            // timeSpan2 = 00:01:41.8320000
            Console.ReadKey();
        }

        private static void Json()
        {
            //var options = new JsonSerializerOptions
            //{
            //    WriteIndented = true,
            //    IndentCharacter = '\t',
            //    IndentSize = 2,
            //    //处理中文乱码
            //    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            //};

            //var json = JsonSerializer.Serialize(
            //    new Student { Name = "小明", Grade = "一班", Age = 10 },
            //    options
            //);

            //var json = JsonSerializer.Serialize(
            //    new Student { Name = "xiaoming", Grade = "yinianji", Age = 10 },
            //    JsonSerializerOptions.Web
            //);
            //Console.WriteLine(json);
        }

        // .NET 9 之前
        public class LockExampleNET9Before
        {
            private readonly object _lock = new();

            public void Print()
            {
                lock (_lock)
                {
                    Console.WriteLine("我们是老的锁");
                }
            }
        }

        // .NET 9
        public class LockExampleNET9
        {
            private readonly Lock _lock = new();

            public void Print()
            {
                lock (_lock)
                {
                    Console.WriteLine("我们是 .NET 9 新锁");
                }
            }

            public async Task LogMessageAsync(string message)
            {
                using (_lock.EnterScope())
                {
                    Console.WriteLine("我们是 .NET 9 新锁，可以和using一起使用");
                }
            }
        }

        public static void PrintNumbers(params int[] numbers) { }

        public static void PrintNumbers(params ArrayList numbers) { }

        public static void PrintNumbers(params List<string> numbers) { }

        public static void PrintNumbers(params HashSet<int> numbers) { }

        public static void PrintNumbers(params SortedSet<int> numbers) { }

        public static void PrintNumbers(params IList<int> numbers) { }

        public static void PrintNumbers(params ICollection<int> numbers) { }

        public static void PrintNumbers(params IEnumerable<int> numbers) { }

        public static void PrintNumbers(params Span<int> numbers) { }

        public static void PrintNumbers(params ReadOnlySpan<int> numbers) { }



        private static void UUID()
        {
            // v4 UUID
            var guid_v4 = Guid.NewGuid();

            // v7 UUID
            var guid_v7 = Guid.CreateVersion7();

            // v7 UUID with timestamp
            var guid_v7_time = Guid.CreateVersion7(TimeProvider.System.GetLocalNow());
        }

        public class ImplicitIndex
        {
            public int[] Numbers { get; set; } = new int[5];
        }

        private static void IndexImplicit()
        {
            var implicitIndex = new ImplicitIndex()
            {
                Numbers =
                {
                    [^1] = 5,
                    [^2] = 4,
                    [^3] = 3,
                    [^4] = 2,
                    [^5] = 1
                }
            };
        }

        private static void Escape()
        {
            var colorRed = "\x1b[31m红色文本\x1b[0m";
            Console.WriteLine(colorRed);
            Console.WriteLine();
            Console.WriteLine();
            var invisible = "\u001b看不见，\x1b看不见";
            Console.WriteLine(invisible);
            Console.WriteLine();
            Console.WriteLine();
            var invisible1 = "\u001b1看不见，\x1b1看不见";
            Console.WriteLine(invisible1);
            Console.WriteLine();
            Console.WriteLine();
            var invisible2 = "\e看不见，\e1看不见";
            Console.WriteLine(invisible2);
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
