using BeetleX.Redis;

namespace Redis
{
    internal static class BeetleXRedisHelper
    {
        public static async Task RunAsync()
        {
            Console.WriteLine($"BeetleX.Redis 使用示例");
            // 创建 CSRedisClient 实例
            RedisDB redis = new RedisDB(0)
            {
                DataFormater = new JsonFormater()
            };
            //添加写主机
            redis.Host.AddWriteHost("127.0.0.1", 6379);
            //添加读主机
            redis.Host.AddReadHost("127.0.0.1", 6379);
            //设置键值对
            var setResult = await redis.Set("key1", "value1");
            Console.WriteLine($"设置键值对key1/value1操作结果：{setResult}");
            //获取键对应的值
            var value = await redis.Get<string>("key1");
            Console.WriteLine($"获取键key1对应的值为：{value}");
            // 删除键
            var delResult = await redis.Del("key1");
            Console.WriteLine($"删除键key1操作结果：{delResult}");
            //检查键是否存在
            var exists = await redis.Exists("key1");
            Console.WriteLine($"键key1是否存在: {exists}");
        }
    }
}
