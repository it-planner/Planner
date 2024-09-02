using System.Runtime.InteropServices;

namespace CSharp
{
    public class MemoryLayout
    {
        [StructLayout(LayoutKind.Sequential)]

        public struct OriginalLayout
        {
            public long LongField1;
            public short ShortField;
            public byte ByteField1;

        }

        public static void Run()
        {
            Console.WriteLine($"OriginalLayout LongField1 偏移量: {Marshal.OffsetOf(typeof(OriginalLayout), "LongField1")} ");
            Console.WriteLine($"OriginalLayout ShortField 偏移量: {Marshal.OffsetOf(typeof(OriginalLayout), "ShortField")} ");
            Console.WriteLine($"OriginalLayout ByteField1 偏移量: {Marshal.OffsetOf(typeof(OriginalLayout), "ByteField1")} ");
            Console.WriteLine($"OriginalLayout 总大小: {Marshal.SizeOf(typeof(OriginalLayout))} bytes");
            Console.ReadKey();
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OriginalChangeLayout
        {
            public short ShortField;
            public long LongField1;
            public byte ByteField1;
        }

        public static void RunChange()
        {
            Console.WriteLine($"OriginalLayout ShortField 偏移量: {Marshal.OffsetOf(typeof(OriginalChangeLayout), "ShortField")} ");
            Console.WriteLine($"OriginalLayout LongField1 偏移量: {Marshal.OffsetOf(typeof(OriginalChangeLayout), "LongField1")} ");
            Console.WriteLine($"OriginalLayout ByteField1 偏移量: {Marshal.OffsetOf(typeof(OriginalChangeLayout), "ByteField1")} ");
            Console.WriteLine($"OriginalLayout 总大小: {Marshal.SizeOf(typeof(OriginalChangeLayout))} bytes");
            Console.ReadKey();
        }
    }
}
