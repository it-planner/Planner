using CSRedis;

namespace Redis.RedisExtension
{
    public class RedisService : IRedisService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="redisClient"></param>
        public RedisService(CSRedisClient redisClient)
        {
            Client = redisClient;
        }

        /// <summary>
        /// CSRedis
        /// </summary>
        /// <returns></returns>
        public CSRedisClient Client { get; }

        #region 自定义方法

        /// <summary>
        /// 迭代哈希表中的键值对
        /// </summary>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return Client.Get<T>(key);
        }

        /// <summary>
        /// 获取迭代哈希表中的所有键值对
        /// </summary>
        /// <returns></returns>
        public Task<T> GetAsync<T>(string key)
        {
            return Client.GetAsync<T>(key);
        }
        #endregion

    }
}
