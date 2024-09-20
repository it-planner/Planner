using BenchmarkDotNet.Running;

namespace CSharp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int[] a = [1, 3, 3, 2, 5];
            int[] b = [2, 3];
            ArrayHelper.AddRange(ref a, b);
            ArrayHelper.RemoveAll(ref a, ca => b.Contains(ca));
            var d8 = a.AddRange(b);
            var d7 = a.RemoveAll(a => b.Contains(a));
            var d3 = ArrayAddRemove.RemoveByList([1, 3, 3, 2, 5], [2, 3]);
            var d1 = ArrayAddRemove.RemoveByWhere([1, 3, 3, 2, 5], [2, 3]);
            var d2 = ArrayAddRemove.RemoveByArray([1, 3, 3, 2, 5], [2, 3]);
            var d4 = ArrayAddRemove.RemoveByForList([1, 3, 3, 2, 5], [2, 3]);
            var d5 = ArrayAddRemove.RemoveByForMarkCopy([1, 3, 3, 2, 5], [2, 3]);
            var d6 = ArrayAddRemove.RemoveByForMarkResize([1, 3, 3, 2, 5], [2, 3]);
            _ = BenchmarkRunner.Run<ArrayAddRemoveBenchmark>();
            ValueReferenceExample.Run();
            _ = BenchmarkRunner.Run<BytesToHexStringBenchmark>();

        }
    }
}
