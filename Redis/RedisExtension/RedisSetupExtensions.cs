using CSRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Redis.RedisExtension
{
    public static class RedisSetupExtensions
    {
        /// <summary>
        /// Redis客户端启动项
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisClientSetup(this IServiceCollection services)
        {
            services.AddSingleton<CSRedisClient>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var setting = configuration["RedisConnectionString"];
                return new CSRedisClient(setting);
            });

            services.AddSingleton<IRedisService, RedisService>();
            return services;
        }

        /// <summary>
        /// Redis客户端启动项
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisClientSetup(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<CSRedisClient>(serviceProvider =>
            {
                return new CSRedisClient(connectionString);
            });

            services.AddSingleton<IRedisService, RedisService>();
            return services;
        }
    }
}
