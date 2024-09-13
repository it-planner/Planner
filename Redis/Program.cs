
using BeetleX.Redis;
using Microsoft.Extensions.DependencyInjection;
using NewLife.Caching;
using Redis.RedisExtension;
using ServiceStack.Redis;
using StackExchange.Redis;
using static ServiceStack.Diagnostics.Events;


namespace Redis
{
    internal class Program
    {
        static async Task Main(string[] args)
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


