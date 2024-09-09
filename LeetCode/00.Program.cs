using BenchmarkDotNet.Running;
using System.Collections.Concurrent;
using System.Reflection;

namespace LeetCode
{
    internal partial class Program
    {
        private static void Main(string[] args)
        {
            string? input;

            do
            {
                Console.Write("请输入题目序号： ('exit'推出): ");

                input = Console.ReadLine();

                if (input != "exit" && !string.IsNullOrWhiteSpace(input))
                {
                    var type = GetType(input);
                    if (type != null)
                    {
                        _ = BenchmarkRunner.Run(type);
                    }
                }

            } while (input != "exit");
        }
    }


    internal partial class Program
    {
        private static readonly ConcurrentDictionary<string, Type> concurrentDictionary = new();

        private static void Init()
        {
            if (!concurrentDictionary.IsEmpty)
            {
                return;
            }

            var types = Assembly.Load("LeetCode").GetTypes().Where(k =>
               k.IsClass
               && typeof(IBenchmark).IsAssignableFrom(k)
               && !k.IsAbstract
               && !k.IsGenericType);

            foreach (var type in types)
            {
                concurrentDictionary.TryAdd(type.Name, type);
            }
        }

        public static Type? GetType(string name)
        {
            Init();

            var type = concurrentDictionary.FirstOrDefault(v => v.Key.Contains(name)).Value;
            if (type == null)
            {
                return null;
            }

            return type;
        }
    }
}
