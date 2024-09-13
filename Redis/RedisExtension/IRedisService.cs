using CSRedis;

namespace Redis.RedisExtension
{
    public interface IRedisService
    {
        /// <summary>
        /// RedisClient
        /// </summary>
        /// <returns></returns>
        CSRedisClient Client { get; }

        #region 自定义方法
        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);
        #endregion
    }
}