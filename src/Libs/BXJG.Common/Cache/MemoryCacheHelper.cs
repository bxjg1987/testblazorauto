using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Common.Cache
{
    /// <summary>
    /// 极简内存缓存工具类，支持绝对过期和滑动过期。
    /// 默认最大缓存容量为 10000 项，可通过 ConfigureMaxItems 调整。
    /// 适合 Blazor WebAssembly 或其它受限环境。
    /// </summary>
    public static class MemoryCacheHelper
    {
        /// <summary>
        /// 缓存项内部结构，包含值、过期策略和最后访问时间
        /// </summary>
        class CacheItem
        {
            public object Value { get; set; }
            public DateTime AbsoluteExpiration { get; set; }
            public TimeSpan? SlidingExpiration { get; set; }
            public DateTime LastAccess { get; set; }

            /// <summary>
            /// 判断缓存项是否过期
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool IsExpired()
            {
                var now = DateTime.UtcNow;
                if (now > AbsoluteExpiration) return true;
                if (SlidingExpiration.HasValue && now > LastAccess.Add(SlidingExpiration.Value)) return true;
                return false;
            }
        }

        static readonly ConcurrentDictionary<object, CacheItem> dic = new ConcurrentDictionary<object, CacheItem>();
        static Timer cleanupTimer;

        // 默认最大容量
        static int maxItems = 10000;

        static MemoryCacheHelper()
        {
            // 默认每30秒清理一次过期项
            cleanupTimer = new Timer(CleanupExpiredItems, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
        }

        /// <summary>
        /// 定时清理过期项
        /// </summary>
        private static void CleanupExpiredItems(object state)
        {
            try
            {
                foreach (var kvp in dic)
                {
                    if (kvp.Value.IsExpired())
                        dic.TryRemove(kvp.Key, out _);
                }
            }
            catch
            {
                // 防止清理过程中出现异常影响正常功能
            }
        }

        /// <summary>
        /// 配置清理周期（默认30秒）
        /// </summary>
        public static void ConfigureCleanupInterval(TimeSpan interval) => cleanupTimer?.Change(interval, interval);

        /// <summary>
        /// 配置最大缓存容量（默认10000）
        /// </summary>
        public static void ConfigureMaxItems(int max)
        {
            if (max <= 0) throw new ArgumentOutOfRangeException(nameof(max));
            maxItems = max;
        }

        /// <summary>
        /// 设置缓存（超过最大容量则不缓存）
        /// </summary>
        public static void Set(object key, object value, int absoluteExpirationSeconds = 60, int? slidingExpirationSeconds = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (absoluteExpirationSeconds <= 0) throw new ArgumentOutOfRangeException(nameof(absoluteExpirationSeconds));
            if (dic.Count >= maxItems) return;

            dic[key] = CreateItem(value, absoluteExpirationSeconds, slidingExpirationSeconds);
        }

        /// <summary>
        /// 获取缓存（自动检查过期并移除）
        /// </summary>
        public static object Get(object key)
        {
            if (key == null) return null;
            if (!dic.TryGetValue(key, out var item)) return null;

            if (item.IsExpired())
            {
                dic.TryRemove(key, out _);
                return null;
            }

            if (item.SlidingExpiration.HasValue)
                item.LastAccess = DateTime.UtcNow;

            return item.Value;
        }

        /// <summary>
        /// 获取缓存并转换为指定类型
        /// </summary>
        public static T Get<T>(object key) => Get(key) is T t ? t : default;

        /// <summary>
        /// 异步版本的 GetOrSet（超过容量时不缓存）
        /// </summary>
        public static async Task<object> GetOrSetAsync(object key, Func<Task<object>> factory, int absoluteExpirationSeconds = 60, int? slidingExpirationSeconds = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            if (Contains(key)) return Get(key);

            if (dic.Count >= maxItems)
                return await factory(); // 超过容量，不缓存

            var newItem = CreateItem(await factory(), absoluteExpirationSeconds, slidingExpirationSeconds);
            var result = dic.AddOrUpdate(key, newItem, (k, old) => old.IsExpired() ? newItem : old);
            return result.Value;
        }

        /// <summary>
        /// 泛型版本的 GetOrSetAsync
        /// </summary>
        public static async Task<T> GetOrSetAsync<T>(object key, Func<Task<T>> factory, int absoluteExpirationSeconds = 60, int? slidingExpirationSeconds = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            if (Contains(key)) return Get<T>(key);

            if (dic.Count >= maxItems)
                return await factory(); // 超过容量，不缓存

            //Console.WriteLine($"正在创建缓存：{key} 当前缓存信息：{ GetStats() }");
            var newItem = CreateItem(await factory(), absoluteExpirationSeconds, slidingExpirationSeconds);
            var result = dic.AddOrUpdate(key, newItem, (k, old) => old.IsExpired() ? newItem : old);
            //Console.WriteLine($"完成创建缓存：{key} 当前缓存信息：{GetStats()}");
            return result.Value is T t ? t : default;
        }

        /// <summary>
        /// 创建缓存项
        /// </summary>
        private static CacheItem CreateItem(object value, int absoluteExpirationSeconds, int? slidingExpirationSeconds)
        {
            var now = DateTime.UtcNow;
            return new CacheItem
            {
                Value = value,
                AbsoluteExpiration = now.AddSeconds(absoluteExpirationSeconds),
                SlidingExpiration = slidingExpirationSeconds.HasValue ? TimeSpan.FromSeconds(slidingExpirationSeconds.Value) : null,
                LastAccess = now
            };
        }

        /// <summary>
        /// 检查缓存是否存在且未过期
        /// </summary>
        public static bool Contains(object key)
        {
            if (key == null) return false;
            if (!dic.TryGetValue(key, out var item)) return false;

            if (item.IsExpired())
            {
                dic.TryRemove(key, out _);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 移除指定缓存项
        /// </summary>
        public static void Remove(object key) => dic.TryRemove(key, out _);

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public static void Clear() => dic.Clear();

        /// <summary>
        /// 当前缓存项数量
        /// </summary>
        public static int Count => dic.Count;

        /// <summary>
        /// 获取缓存统计信息（总数和过期数）
        /// </summary>
        public static (int count, int expiredCount) GetStats()
        {
            int count = 0, expiredCount = 0;
            foreach (var kvp in dic)
            {
                count++;
                if (kvp.Value.IsExpired()) expiredCount++;
            }
            return (count, expiredCount);
        }

        /// <summary>
        /// 停止清理但保留缓存
        /// </summary>
        public static void StopCleanup() => cleanupTimer?.Dispose();

        /// <summary>
        /// 停止清理并清空缓存
        /// </summary>
        public static void Shutdown()
        {
            cleanupTimer?.Dispose();
            dic.Clear();
        }
    }
}
