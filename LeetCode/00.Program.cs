using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System.Collections.Concurrent;
using System.Reflection;
using System.Xml.Linq;

namespace LeetCode
{
    internal partial class Program
    {
        public static int LengthOfLongestSubstring(string s)
        {
            if (s.Length == 0)
            {
                return 0;
            }

            if (s.Length == 1)
            {
                return 1;
            }

            var ss = s.AsSpan();
            var len = 1;
            var index = 1;
            var startIndex = 0;
            int target = 1;
            ReadOnlySpan<char> res = ss.Slice(startIndex, len);
            while (len < ss.Length && index < ss.Length)
            {
                var fristIndex = res.IndexOf(ss[index]);
                if (fristIndex == -1)
                {
                    res = ss.Slice(startIndex, res.Length + 1);
                    if (target <= res.Length)
                    {
                        target = res.Length;
                    }
                    len++;
                }
                else
                {
                    fristIndex = fristIndex + index - res.Length ;
                    startIndex = fristIndex + 1;
                    res = ss.Slice(startIndex, index-fristIndex);
                    if (target <= res.Length)
                    {
                        target = res.Length;
                    }
                }
                index++;
            }

            return target;
        }

        static void Main(string[] args)
        {
            var s = "aabaab!bb";
            var dd = LengthOfLongestSubstring(s);
            Console.WriteLine(dd);
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
