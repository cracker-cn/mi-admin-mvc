using Mi.Entity.System;
using Mi.IService.System.Models.Result;

namespace Mi.IService.System
{
    public interface IDictService
    {
        #region Admin_UI

        List<SysDict> GetAll();

        Task<MessageModel<PagingModel<DictItem>>> GetDictListAsync(DictSearch search);

        Task<MessageModel> AddOrUpdateDictAsync(DictOperation operation, bool addEnabled = true);

        Task<MessageModel> RemoveDictAsync(IList<string> ids);

        Task<MessageModel<SysDict>> GetAsync(long id);

        Task<List<Option>> GetParentListAsync();

        #endregion Admin_UI

        #region 公共读写方法，带缓存

        Task<T> GetAsync<T>(string parentKey) where T : class, new();

        T Get<T>(string parentKey) where T : class, new();

        Task<bool> SetAsync<T>(T model) where T : class, new();

        Task<string> GetStringAsync(string key);

        Task<bool> SetAsync(string key, string value, bool autoCreate = true);

        Task<IList<Option>> GetOptionsAsync(string parentKey);

        IList<Option> GetOptions(string parentKey);

        Task<MessageModel> SetAsync(Dictionary<string, string> dict);

        #endregion
    }
}