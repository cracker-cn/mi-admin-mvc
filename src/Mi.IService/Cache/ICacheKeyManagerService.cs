﻿namespace Mi.IService.Cache
{
    public interface ICacheKeyManagerService
    {
        Task<MessageModel<IList<string>>> GetAllKeysAsync(string? vague, int cacheType = 1);

        Task<MessageModel> RemoveKeyAsync(string key);

        Task<MessageModel<string>> GetDataAsync(string key);
    }
}
