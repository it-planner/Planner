using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis.RedisExtension
{
    internal class RedisTest
    {
        public static void Run()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddRedisClientSetup();

            var redisService = services.BuildServiceProvider().GetService<IRedisService>();

            var setResult = redisService.Client.Set("key1", "value1");
            Console.WriteLine($"redisService.Client.Set(\"key1\",\"value1\")执行结果：{setResult}");
            var value = redisService.Get<string>("key1");
            Console.WriteLine($"redisService.Get<string>(\"key1\")执行结果：{value}");
            redisService.Client.Del("key1");
        }
    }
}
