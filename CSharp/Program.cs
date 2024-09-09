using BenchmarkDotNet.Running;

namespace CSharp
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            _ = BenchmarkRunner.Run<BytesToHexStringBenchmark>();

        }
    }
}
