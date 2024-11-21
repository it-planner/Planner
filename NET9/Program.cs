using System;
using System.Collections;
using System.Collections.Generic;

namespace NET9
{
    internal class Program
    {
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


        static void Main()
        {

            Console.ReadKey();
        }

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
