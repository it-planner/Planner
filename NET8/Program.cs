namespace NET8
{
    internal class Program
    {
        static void Main()
        {
            Console.ReadKey();
        }

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
