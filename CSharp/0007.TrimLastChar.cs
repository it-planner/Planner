using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Text;
using System.Text.RegularExpressions;

namespace CSharp
{
    public class TrimLastChar
    {
        public static void Run()
        {
            Check(StringSubstring);
            Check(StringRangeOperator);
            Check(StringRemove);
            Check(StringCreate);

            Check(StringBuilderAppend);
            Check(StringBuilderLength);

            Check(ArrayFor);
            Check(ArrayString);
            Check(ArrayResize);
            Check(ArrayCopyTo);

            Check(LinqTake);
            Check(LinqSkipLast);
            Check(LinqRangeSelect);

            Check(LinqStringConcat);
            Check(LinqStringJoin);

            Check(Span);
            Check(Memory);
            Check(ArraySegment);

            Check(RegexReplace);
            Check(RegexMatch);
        }

        public static void Check(Func<string, string> func)
        {
            var source = "abcde";
            var result = func(source);
            if (result != "abcd")
            {
                throw new Exception("not ==");
            }
        }



        #region 字符串方式 

        public static string StringSubstring(string source)
        {
            return source.Substring(0, source.Length - 1);
        }

        public static string StringRangeOperator(string source)
        {
            return source[..^1];
        }

        public static string StringRemove(string source)
        {
            return source.Remove(source.Length - 1);
        }

        public static string StringCreate(string source)
        {
            return string.Create(source.Length - 1, source, (span, state) =>
            {
                for (var i = 0; i < state.Length - 1; i++)
                {
                    span[i] = state[i];
                }
            });
        }

        #endregion

        #region StringBuilder方式

        public static string StringBuilderAppend(string source)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < source.Length - 1; i++)
            {
                sb.Append(source[i]);
            }
            return sb.ToString();
        }

        public static string StringBuilderLength(string source)
        {
            var sb = new StringBuilder(source);
            sb.Length--;
            return sb.ToString();
        }

        #endregion

        #region Array方式

        public static string ArrayFor(string source)
        {
            var chars = new char[source.Length - 1];
            for (var i = 0; i < chars.Length; i++)
            {
                chars[i] = source[i];
            }
            return new string(chars);
        }

        public static string ArrayResize(string source)
        {
            var chars = source.ToCharArray();
            Array.Resize(ref chars, chars.Length - 1);
            return new string(chars);
        }

        public static string ArrayCopyTo(string source)
        {
            var chars = new char[source.Length - 1];
            source.CopyTo(0, chars, 0, chars.Length);
            return new string(chars);
        }

        public static string ArrayString(string source)
        {
            var chars = source.ToCharArray();
            return new string(chars, 0, chars.Length - 1);
        }

        #endregion

        #region Linq方式

        public static string LinqTake(string source)
        {
            return new string(source.Take(source.Length - 1).ToArray());
        }

        public static string LinqSkipLast(string source)
        {
            return new string(source.SkipLast(1).ToArray());
        }

        public static string LinqRangeSelect(string source)
        {
            return new string(Enumerable.Range(0, source.Length - 1).Select(i => source[i]).ToArray());
        }

        #endregion

        #region Linq + String方式

        public static string LinqStringConcat(string source)
        {
            return string.Concat(source.SkipLast(1));
        }

        public static string LinqStringJoin(string source)
        {
            return string.Join("", source.SkipLast(1));
        }

        #endregion
        #region 数据视图方式

        public static string Span(string source)
        {
            var span = source.AsSpan(0, source.Length - 1);
            return new string(span);
        }

        public static string Memory(string source)
        {
            var memory = source.AsMemory(0, source.Length - 1);
            return new string(memory.Span);
        }

        public static string ArraySegment(string source)
        {
            var segment = new ArraySegment<char>(source.ToCharArray(), 0, source.Length - 1);
            return new string(segment.Array, segment.Offset, segment.Count);
        }

        #endregion

        #region 正则表达式方式

        public static string RegexReplace(string source)
        {
            return Regex.Replace(source, ".$", "");
        }

        public static string RegexMatch(string source)
        {
            var match = Regex.Match(source, @"^(.*).$");
            return match.Groups[1].Value;
        }

        #endregion
    }

    [MemoryDiagnoser]
    public class TrimLastCharBenchmark
    {
        /// <summary>
        /// 测试次数
        /// </summary>
        private const int TestNumber = 10000;

        /// <summary>
        /// 数组长度
        /// </summary>
        private readonly int[] ArrayLengths = [100, 1000, 10000];

        private readonly Dictionary<int, string[]> Datas = [];


        [GlobalSetup]
        public void Setup()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789~!@#$%^&*()_+{}|:\" <>?-= []\\;',./' "; // 可选择的字符集
            foreach (var al in ArrayLengths)
            {
                var list = new List<string>();
                for (var i = 0; i < al; i++)
                {
                    var sb = new StringBuilder(al);
                    for (int j = 0; j < al; j++)
                    {
                        int index = random.Next(chars.Length);
                        sb.Append(chars[index]);
                    }
                    list.Add(sb.ToString());
                }

                var models = list.ToArray();
                Datas.Add(al, models);
            }
        }

        [Benchmark]
        public void StringSubstring_100()
        {
            var res = Datas[100];
            Handle(res, models => TrimLastChar.StringSubstring(models));
        }
        [Benchmark]
        public void StringRangeOperator_100()
        {
            var res = Datas[100];
            Handle(res, models => TrimLastChar.StringRangeOperator(models));
        }
        [Benchmark]
        public void StringRemove_100()
        {
            var res = Datas[100];
            Handle(res, models => TrimLastChar.StringRemove(models));
        }
        [Benchmark]
        public void StringCreate_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = TrimLastChar.StringCreate(models));
        }
        [Benchmark]
        public void StringBuilderAppend_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = TrimLastChar.StringBuilderAppend(models));
        }
        [Benchmark]
        public void StringBuilderLength_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = TrimLastChar.StringBuilderLength(models));
        }
        [Benchmark]
        public void ArrayFor_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = TrimLastChar.ArrayFor(models));
        }
        [Benchmark]
        public void ArrayString_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = TrimLastChar.ArrayString(models));
        }
        [Benchmark]
        public void ArrayResize_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = TrimLastChar.ArrayResize(models));
        }
        [Benchmark]
        public void ArrayCopyTo_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = TrimLastChar.ArrayCopyTo(models));
        }
        [Benchmark]
        public void LinqTake_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = TrimLastChar.LinqTake(models));
        }
        [Benchmark]
        public void LinqSkipLast_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = TrimLastChar.LinqSkipLast(models));
        }
        [Benchmark]
        public void LinqRangeSelect_100()
        {
            var res = Datas[100];
            Handle(res, models => TrimLastChar.LinqRangeSelect(models));
        }
        [Benchmark]
        public void LinqStringConcat_100()
        {
            var res = Datas[100];
            Handle(res, models => TrimLastChar.LinqStringConcat(models));
        }
        [Benchmark]
        public void LinqStringJoin_100()
        {
            var res = Datas[100];
            Handle(res, models => TrimLastChar.LinqStringJoin(models));
        }
        [Benchmark]
        public void Span_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = TrimLastChar.Span(models));
        }
        [Benchmark]
        public void Memory_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = TrimLastChar.Memory(models));
        }
        [Benchmark]
        public void ArraySegment_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = TrimLastChar.ArraySegment(models));
        }
        [Benchmark]
        public void RegexReplace_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = TrimLastChar.RegexReplace(models));
        }
        [Benchmark]
        public void RegexMatch_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = TrimLastChar.RegexMatch(models));
        }
        [Benchmark]
        public void StringSubstring_1000()
        {
            var res = Datas[1000];
            Handle(res, models => TrimLastChar.StringSubstring(models));
        }
        [Benchmark]
        public void StringRangeOperator_1000()
        {
            var res = Datas[1000];
            Handle(res, models => TrimLastChar.StringRangeOperator(models));
        }
        [Benchmark]
        public void StringRemove_1000()
        {
            var res = Datas[1000];
            Handle(res, models => TrimLastChar.StringRemove(models));
        }
        [Benchmark]
        public void StringCreate_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = TrimLastChar.StringCreate(models));
        }
        [Benchmark]
        public void StringBuilderAppend_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = TrimLastChar.StringBuilderAppend(models));
        }
        [Benchmark]
        public void StringBuilderLength_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = TrimLastChar.StringBuilderLength(models));
        }
        [Benchmark]
        public void ArrayFor_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = TrimLastChar.ArrayFor(models));
        }
        [Benchmark]
        public void ArrayString_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = TrimLastChar.ArrayString(models));
        }
        [Benchmark]
        public void ArrayResize_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = TrimLastChar.ArrayResize(models));
        }
        [Benchmark]
        public void ArrayCopyTo_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = TrimLastChar.ArrayCopyTo(models));
        }
        [Benchmark]
        public void LinqTake_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = TrimLastChar.LinqTake(models));
        }
        [Benchmark]
        public void LinqSkipLast_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = TrimLastChar.LinqSkipLast(models));
        }
        [Benchmark]
        public void LinqRangeSelect_1000()
        {
            var res = Datas[1000];
            Handle(res, models => TrimLastChar.LinqRangeSelect(models));
        }
        [Benchmark]
        public void LinqStringConcat_1000()
        {
            var res = Datas[1000];
            Handle(res, models => TrimLastChar.LinqStringConcat(models));
        }
        [Benchmark]
        public void LinqStringJoin_1000()
        {
            var res = Datas[1000];
            Handle(res, models => TrimLastChar.LinqStringJoin(models));
        }
        [Benchmark]
        public void Span_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = TrimLastChar.Span(models));
        }
        [Benchmark]
        public void Memory_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = TrimLastChar.Memory(models));
        }
        [Benchmark]
        public void ArraySegment_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = TrimLastChar.ArraySegment(models));
        }
        [Benchmark]
        public void RegexReplace_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = TrimLastChar.RegexReplace(models));
        }
        [Benchmark]
        public void RegexMatch_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = TrimLastChar.RegexMatch(models));
        }
        [Benchmark]
        public void StringSubstring_10000()
        {
            var res = Datas[10000];
            Handle(res, models => TrimLastChar.StringSubstring(models));
        }
        [Benchmark]
        public void StringRangeOperator_10000()
        {
            var res = Datas[10000];
            Handle(res, models => TrimLastChar.StringRangeOperator(models));
        }
        [Benchmark]
        public void StringRemove_10000()
        {
            var res = Datas[10000];
            Handle(res, models => TrimLastChar.StringRemove(models));
        }
        [Benchmark]
        public void StringCreate_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = TrimLastChar.StringCreate(models));
        }
        [Benchmark]
        public void StringBuilderAppend_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = TrimLastChar.StringBuilderAppend(models));
        }
        [Benchmark]
        public void StringBuilderLength_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = TrimLastChar.StringBuilderLength(models));
        }
        [Benchmark]
        public void ArrayFor_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = TrimLastChar.ArrayFor(models));
        }
        [Benchmark]
        public void ArrayString_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = TrimLastChar.ArrayString(models));
        }
        [Benchmark]
        public void ArrayResize_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = TrimLastChar.ArrayResize(models));
        }
        [Benchmark]
        public void ArrayCopyTo_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = TrimLastChar.ArrayCopyTo(models));
        }
        [Benchmark]
        public void LinqTake_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = TrimLastChar.LinqTake(models));
        }
        [Benchmark]
        public void LinqSkipLast_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = TrimLastChar.LinqSkipLast(models));
        }
        [Benchmark]
        public void LinqRangeSelect_10000()
        {
            var res = Datas[10000];
            Handle(res, models => TrimLastChar.LinqRangeSelect(models));
        }
        [Benchmark]
        public void LinqStringConcat_10000()
        {
            var res = Datas[10000];
            Handle(res, models => TrimLastChar.LinqStringConcat(models));
        }
        [Benchmark]
        public void LinqStringJoin_10000()
        {
            var res = Datas[10000];
            Handle(res, models => TrimLastChar.LinqStringJoin(models));
        }
        [Benchmark]
        public void Span_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = TrimLastChar.Span(models));
        }
        [Benchmark]
        public void Memory_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = TrimLastChar.Memory(models));
        }
        [Benchmark]
        public void ArraySegment_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = TrimLastChar.ArraySegment(models));
        }
        [Benchmark]
        public void RegexReplace_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = TrimLastChar.RegexReplace(models));
        }
        [Benchmark]
        public void RegexMatch_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = TrimLastChar.RegexMatch(models));
        }

        private static void Handle(string[] models, Func<string, string> func)
        {
            foreach (var model in models)
            {
                func(model);
            }
        }
    }
}
