using CSRedis;

namespace Redis
{
    internal static class CSRedisRedisHelper
    {
        public static void Run()
        {
            Console.WriteLine($"CSRedisRedis 使用示例");
            // 创建 CSRedisClient 实例
            var redis = new CSRedisClient("127.0.0.1:6379");
            //设置键值对
            var setResult = redis.Set("key1", "value1");
            Console.WriteLine($"设置键值对key1/value1操作结果：{setResult}");
            //获取键对应的值
            var value = redis.Get("key1");
            Console.WriteLine($"获取键key1对应的值为：{value}");
            // 删除键
            var delResult = redis.Del("key1");
            Console.WriteLine($"删除键key1操作结果：{delResult}");
            //检查键是否存在
            var exists = redis.Exists("key1");
            Console.WriteLine($"键key1是否存在: {exists}");
        }
    }
}
