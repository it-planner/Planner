using StackExchange.Redis;

namespace Redis
{
    internal static class StackExchangeRedisHelper
    {
        public static void Run()
        {
            Console.WriteLine($"StackExchange.Redis 使用示例");
            // 创建 ConnectionMultiplexer 实例
            using var connection = ConnectionMultiplexer.Connect("127.0.0.1:6379");
            //获取 Redis 数据库实例
            var redis = connection.GetDatabase();
            //设置键值对
            var setResult = redis.StringSet("key1", "value1");
            Console.WriteLine($"设置键值对key1/value1操作结果：{setResult}");
            //获取键对应的值
            var value = redis.StringGet("key1");
            Console.WriteLine($"获取键key1对应的值为：{value}");
            // 删除键
            var delResult = redis.KeyDelete("key1");
            Console.WriteLine($"删除键key1操作结果：{delResult}");
            //检查键是否存在
            var exists = redis.KeyExists("key1");
            Console.WriteLine($"键key1是否存在: {exists}");
        }
    }
}
