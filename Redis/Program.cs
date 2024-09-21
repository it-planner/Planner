using Redis.RedisExtension;


namespace Redis
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            RedisTest.Run();
            await BeetleXRedisHelper.RunAsync();
            NewLifeRedisHelper.Run();
            FreeRedisHelper.Run();
            CSRedisRedisHelper.Run();
            StackExchangeRedisHelper.Run();
            ServiceStackRedisHelper.Run();

            Console.ReadKey();
        }
    }
}


