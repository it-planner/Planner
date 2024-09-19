using BenchmarkDotNet.Running;

namespace CSharp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ValueReferenceExample.Run();
            _ = BenchmarkRunner.Run<BytesToHexStringBenchmark>();

        }
    }
}
