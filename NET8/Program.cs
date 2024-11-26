using System;
using System.Runtime.CompilerServices;

namespace NET8
{
    public interface IBufferProcessor
    {
        void ProcessBuffer();
    }

    public ref struct BufferProcessor 
    {
        private Span<byte> buffer;

        public BufferProcessor(Span<byte> buffer)
        {
            this.buffer = buffer;
        }

        public void ProcessBuffer()
        {
            // 处理 buffer
        }

       

    }

    internal class Program
    {
        //ref int Process(ref int x)
        //{
        //    return ref x;
        //}

        ////在异步方法中使用ref
        //async Task RefInAsync()
        //{
        //    int value = 0;
        //    await Task.Delay(0);
        //    ref int local = ref Process(ref value);
        //}

        ////在异步方法中使用ref
        //IEnumerable<int> RefInIterator(int[] array)
        //{
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        Span<int> span = array.AsSpan();
        //        yield return span[0];
        //    }
        //}


        static void Main()
        {
            Console.ReadKey();
        }

        //public interface IInterface { }

        //public ref struct RefStructInterfaces : IInterface
        //{
        //}


        //public class Disallow
        //{
        //    public static void Process<T>(T data) 
        //    {
        //    }

        //    public void Example()
        //    {
        //        Span<int> span = new();
        //        Process(span); 
        //    }
        //}
        public class ImplicitIndex
        {
            public int[] Numbers { get; set; } = new int[5];
        }
        private static void IndexImplicit()
        {
            var implicitIndex = new ImplicitIndex()
            {
                Numbers =
                {
                    //[^1] = 5,
                    //[^2] = 4,
                    //[^3] = 3,
                    //[^4] = 2,
                    //[^5] = 1
                }
            };

        }
    }
}
