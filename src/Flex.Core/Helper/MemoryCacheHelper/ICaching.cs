
namespace Flex.Core.Helper.MemoryCacheHelper
{
    /// <summary>
    /// 简单的缓存接口，只有查询和添加，以后会进行扩展
    /// </summary>
    public interface ICaching
    {
        object Get(string cacheKey);

        void Set(string cacheKey, object cacheValue, int timeSpan);

        void Remove(string cacheKey);
        bool Exist(string cacheKey);
        void Set(string cacheKey, object cacheValue, TimeSpan timeSpan);
    }
}
