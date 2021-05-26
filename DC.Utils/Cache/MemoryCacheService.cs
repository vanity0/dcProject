
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace DC.Utils
{
    /// <summary>
    /// 内存缓存
    /// </summary>
    public class MemoryCacheService : ICacheService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IMemoryCache _memoryCache;
        /// <summary>
        /// 
        /// </summary>
        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 
        /// </summary>
        public long Del(params string[] key)
        {
            foreach(var k in key)
            {
                _memoryCache.Remove(k);
            }
            return key.Length;
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task<long> DelAsync(params string[] key)
        {
            foreach (var k in key)
            {
                _memoryCache.Remove(k);
            }

            return await Task.FromResult(key.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task<long> DelByPatternAsync(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return default;

            pattern = Regex.Replace(pattern, @"\{.*\}", "(.*)");

            var keys = GetAllKeys().Where(k => Regex.IsMatch(k, pattern));
            
            if(keys != null && keys.Any())
            {
                return await DelAsync(keys.ToArray());
            }

            return default;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Exists(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        /// <summary>
        /// 
        /// </summary>
        public async  Task<bool> ExistsAsync(string key)
        {
            return await Task.FromResult(_memoryCache.TryGetValue(key, out _));
        }

        /// <summary>
        /// 
        /// </summary>
        public string Get(string key)
        {
            return _memoryCache.Get(key)?.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }
        /// <summary>
        /// 
        /// </summary>
        public async Task<string> GetAsync(string key)
        {
            return await Task.FromResult(Get(key));
        }
        /// <summary>
        /// 
        /// </summary>
        public async  Task<T> GetAsync<T>(string key)
        {
            return await Task.FromResult(Get<T>(key));
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Set(string key, object value)
        {
            _memoryCache.Set(key, value);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Set(string key, object value, TimeSpan expire)
        {
            _memoryCache.Set(key, value, expire);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        public async Task<bool> SetAsync(string key, object value)
        {
            Set(key, value);
            return await Task.FromResult(true);
        }
        /// <summary>
        /// 
        /// </summary>
        public async Task<bool> SetAsync(string key, object value, TimeSpan expire)
        {
            Set(key, value, expire);
            return await Task.FromResult(true);
        }
        /// <summary>
        /// 
        /// </summary>
        private List<string> GetAllKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _memoryCache.GetType().GetField("_entries", flags).GetValue(_memoryCache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }
    }
}
