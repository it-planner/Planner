using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CSharp
{
    public class BytesToHexString
    {
        public static string ToHexStringStringBuilder(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        public static string ToHexStringBitConverter(byte[] bytes)
        {
            return BitConverter.ToString(bytes);
        }

        public static string ToHexStringConvert(byte[] bytes)
        {
            return Convert.ToHexString(bytes);
        }

        public static string ToHexStringBitOperation(byte[] bytes)
        {
            char[] hexChars = new char[bytes.Length * 2];
            const string hex = "0123456789ABCDEF";

            for (int i = 0; i < bytes.Length; i++)
            {
                hexChars[i * 2] = hex[bytes[i] >> 4];
                hexChars[i * 2 + 1] = hex[bytes[i] & 0x0F];
            }

            return new string(hexChars);
        }

        public static unsafe string ToHexStringUnsafe(byte[] bytes)
        {
            const string hex = "0123456789ABCDEF";
            var hexChars = new char[bytes.Length * 2];

            fixed (byte* bytePtr = bytes)
            {
                fixed (char* charPtr = hexChars)
                {
                    byte* source = bytePtr;
                    char* dest = charPtr;

                    for (int i = 0; i < bytes.Length; i++)
                    {
                        byte b = source[i];
                        dest[i * 2] = hex[b >> 4];
                        dest[i * 2 + 1] = hex[b & 0x0F];
                    }
                }
            }

            return new string(hexChars);
        }
    }


    [MemoryDiagnoser]
    public class BytesToHexStringBenchmark
    {
        /// <summary>
        /// 测试次数
        /// </summary>
        private const int TestNumber = 10000;

        /// <summary>
        /// 数组长度
        /// </summary>
        private readonly int[] ArrayLengths = [100, 1000, 10000];

        /// <summary>
        /// 数据
        /// </summary>
        private readonly Dictionary<int, byte[,]> Datas = [];

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random();

            foreach (var al in ArrayLengths)
            {
                var nu = new byte[TestNumber, al];
                for (var j = 0; j < TestNumber; j++)
                {
                    for (var i = 0; i < al; i++)
                    {
                        nu[j, i] = (byte)random.Next(0, 255);
                    }
                }

                Datas.Add(al, nu);
            }
        }

        [Benchmark]
        public void ToHexStringStringBuilder_10000_100()
        {
            var res = Datas[ArrayLengths[0]];
            Handle(res, bs => _ = BytesToHexString.ToHexStringStringBuilder(bs));
        }


        [Benchmark]
        public void ToHexStringBitConverter_10000_100()
        {
            var res = Datas[ArrayLengths[0]];
            Handle(res, bs => _ = BytesToHexString.ToHexStringBitConverter(bs));
        }

        [Benchmark]
        public void ToHexStringConvert_10000_100()
        {
            var res = Datas[ArrayLengths[0]];
            Handle(res, bs => _ = BytesToHexString.ToHexStringConvert(bs));
        }

        [Benchmark]
        public void ToHexStringBitOperation_10000_100()
        {
            var res = Datas[ArrayLengths[0]];
            Handle(res, bs => _ = BytesToHexString.ToHexStringBitOperation(bs));
        }

        [Benchmark]
        public void ToHexStringUnsafe_10000_100()
        {
            var res = Datas[ArrayLengths[0]];
            Handle(res, bs => _ = BytesToHexString.ToHexStringUnsafe(bs));
        }

        [Benchmark]
        public void ToHexStringStringBuilder_10000_1000()
        {
            var res = Datas[ArrayLengths[1]];
            Handle(res, bs => _ = BytesToHexString.ToHexStringStringBuilder(bs));
        }


        [Benchmark]
        public void ToHexStringBitConverter_10000_1000()
        {
            var res = Datas[ArrayLengths[1]];
            Handle(res, bs => _ = BytesToHexString.ToHexStringBitConverter(bs));
        }

        [Benchmark]
        public void ToHexStringConvert_10000_1000()
        {
            var res = Datas[ArrayLengths[1]];
            Handle(res, bs => _ = BytesToHexString.ToHexStringConvert(bs));
        }

        [Benchmark]
        public void ToHexStringBitOperation_10000_1000()
        {
            var res = Datas[ArrayLengths[1]];
            Handle(res, bs => _ = BytesToHexString.ToHexStringBitOperation(bs));
        }

        [Benchmark]
        public void ToHexStringUnsafe_10000_1000()
        {
            var res = Datas[ArrayLengths[1]];
            Handle(res, bs => _ = BytesToHexString.ToHexStringUnsafe(bs));
        }

        [Benchmark]
        public void ToHexStringStringBuilder_10000_10000()
        {
            var res = Datas[ArrayLengths[2]];
            Handle(res, bs => _ = BytesToHexString.ToHexStringStringBuilder(bs));
        }

        [Benchmark]
        public void ToHexStringBitConverter_10000_10000()
        {
            var res = Datas[ArrayLengths[2]];
            Handle(res, bs => _ = BytesToHexString.ToHexStringBitConverter(bs));
        }

        [Benchmark]
        public void ToHexStringConvert_10000_10000()
        {
            var res = Datas[ArrayLengths[2]];
            Handle(res, bs => _ = BytesToHexString.ToHexStringConvert(bs));
        }
        [Benchmark]
        public void ToHexStringBitOperation_10000_10000()
        {
            var res = Datas[ArrayLengths[2]];
            Handle(res, bs => _ = BytesToHexString.ToHexStringBitOperation(bs));
        }

        [Benchmark]
        public void ToHexStringUnsafe_10000_10000()
        {
            var res = Datas[ArrayLengths[2]];
            Handle(res, bs => _ = BytesToHexString.ToHexStringUnsafe(bs));
        }

        private static void Handle(byte[,] res, Func<byte[], string> func)
        {
            int colCount = res.GetLength(1);
            for (int rowIndex = 0; rowIndex < res.GetLength(0); rowIndex++)
            {
                byte[] row = new byte[colCount];

                for (int colIndex = 0; colIndex < colCount; colIndex++)
                {
                    row[colIndex] = res[rowIndex, colIndex];
                }

                func(row);
            }
        }
    }
}
