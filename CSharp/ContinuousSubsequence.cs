using Newtonsoft.Json;

namespace CSharp
{
    public class ContinuousSubsequence
    {
        public static bool IsSubsequenceJoin(IEnumerable<string> main, IEnumerable<string> sub)
        {
            var mainString = string.Join(",", main);
            var subString = string.Join(",", sub);
            return mainString.Contains(subString);
        }

        public static bool IsSubsequenceSerialize(IEnumerable<string> main, IEnumerable<string> sub)
        {
            var mainString = JsonConvert.SerializeObject(main).TrimStart('[').TrimEnd(']');
            var subString = JsonConvert.SerializeObject(sub).TrimStart('[').TrimEnd(']');
            return mainString.Contains(subString);
        }

        public static bool IsSubsequenceSlidingWindow<T>(IEnumerable<T> main, IEnumerable<T> sub)
        {
            var mainLength = main.Count();
            var subLength = sub.Count();
            for (int i = 0; i < mainLength - subLength + 1; i++)
            {
                var expect = main.Skip(i).Take(subLength);
                if (expect.SequenceEqual(sub))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsSubsequenceSlidingWindowAsSpan<T>(IEnumerable<T> main, IEnumerable<T> sub)
        {
            var mainSpan = main.ToArray().AsSpan();
            var subSpan = sub.ToArray().AsSpan();
            var mainLength = mainSpan.Length;
            var subLength = subSpan.Length;
            for (int i = 0; i < mainLength - subLength + 1; i++)
            {
                var expect = mainSpan.Slice(i, subLength);
                if (expect.SequenceEqual(subSpan))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsSubsequence<T>(IEnumerable<T> main, IEnumerable<T> sub) where T : IEquatable<T>?
        {
            var mainSpan = main.ToArray().AsSpan();
            //var mainSpan = CollectionsMarshal.AsSpan(main.ToList());
            var subSpan = sub.ToArray().AsSpan();
            return mainSpan.IndexOf(subSpan) > -1;
        }

        public static void Run()
        {
            string[] mainJoin = ["a", "b", "c", "d", "e"];
            string[] subJoin = ["d", "e"];


            bool isSubsequenceJoin = ContinuousSubsequence.IsSubsequenceJoin(mainJoin, subJoin);

            Console.WriteLine("IsSubsequenceJoin 方法: ");
            Console.WriteLine("");
            Console.WriteLine("数组 main [\"a\", \"b\", \"c\", \"d,e\"] 序列化后: " + string.Join(",", mainJoin));
            Console.WriteLine("");
            Console.WriteLine("数组 sub [\"d\", \"e\"] 序列化后: " + string.Join(",", subJoin));
            Console.WriteLine("");
            Console.WriteLine("mainString.Contains(subString) 结果: " + isSubsequenceJoin);

            string[] mainSerialize = ["a", "b", "c", "d,\"e"];
            string[] subSerialize = ["e"];

            var isSubsequenceSerialize = ContinuousSubsequence.IsSubsequenceSerialize(mainSerialize, subSerialize);

            Console.WriteLine("IsSubsequenceSerialize 方法: ");
            Console.WriteLine("");
            Console.WriteLine("数组 main [\"a\", \"b\", \"c\", \"d,\\\"e\"] 序列化后: " + JsonConvert.SerializeObject(mainSerialize).TrimStart('[').TrimEnd(']'));
            Console.WriteLine("");
            Console.WriteLine("数组 sub [\"e\"] 序列化后: " + JsonConvert.SerializeObject(subSerialize).TrimStart('[').TrimEnd(']'));
            Console.WriteLine("");
            Console.WriteLine("mainString.Contains(subString) 结果: " + isSubsequenceSerialize);

            Console.ReadKey();
        }
    }
}
