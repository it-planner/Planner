using BenchmarkDotNet.Running;

namespace CSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
          
            _ = BenchmarkRunner.Run<BytesToHexStringBenchmark>();
            
        }
    }
}
