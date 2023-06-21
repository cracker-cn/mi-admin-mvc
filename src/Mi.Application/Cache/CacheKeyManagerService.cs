﻿using Mi.Core.Factory;
using Mi.IService.Cache;

using Newtonsoft.Json;

namespace Mi.Application.Cache
{
    public class CacheKeyManagerService : ICacheKeyManagerService, IScoped
    {
        private readonly MemoryCacheFactory _cache;
        private readonly MessageModel _msg;

        public CacheKeyManagerService(MemoryCacheFactory cache, MessageModel msg)
        {
            _cache = cache;
            _msg = msg;
        }

        public Task<MessageModel<IList<Option>>> GetAllKeysAsync(string? vague, int cacheType = 1)
        {
            var list = new List<Option>();
            if (cacheType == 1)
            {
                list = _cache.GetCacheKeys().OrderBy(x=>x).Select(x=>new Option{ Name = x}).ToList();
                if (!string.IsNullOrEmpty(vague))
                {
                    list = list.Where(x => x.Name!.Contains(vague)).ToList();
                }
            }
            return Task.FromResult(new MessageModel<IList<Option>>(list));
        }

        public Task<MessageModel<string>> GetDataAsync(string key)
        {
            var str = string.Empty;
            if (_cache.Exists(key))
            {
                str = JsonConvert.SerializeObject(_cache.Get<dynamic>(key));
            }
            return Task.FromResult(new MessageModel<string>(str));
        }

        public Task<MessageModel> RemoveKeyAsync(string key)
        {
            _cache.Remove(key);
            return Task.FromResult(_msg.Success());
        }
    }
}