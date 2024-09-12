
using BeetleX.Redis;
using NewLife.Caching;
using ServiceStack.Redis;
using StackExchange.Redis;
using static ServiceStack.Diagnostics.Events;


namespace Redis
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
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


