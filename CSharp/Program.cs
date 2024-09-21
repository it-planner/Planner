using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;
using Perfolizer.Metrology;
using System.Globalization;

namespace CSharp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var config = ManualConfig.Create(DefaultConfig.Instance)
                         .WithOptions(ConfigOptions.DisableOptimizationsValidator);
            //.WithSummaryStyle(new SummaryStyle(cultureInfo: CultureInfo.InvariantCulture,
            //                                   printUnitsInHeader: false,
            //                                   sizeUnit: null,
            //                                   timeUnit: null,
            //                                   textColumnJustification: SummaryTable.SummaryTableColumn.TextJustification.Right));
            _ = BenchmarkRunner.Run<DeepCopyBenchmark>(config);
        }
    }
}
