using Mi.Entity.System;
using Mi.IService.System.Models.Result;

namespace Mi.IService.System
{
    public interface IDictService
    {
        Task<string> GetValueAsync(string key);

        Task<bool> SetValueAsync(string key, string value);

        Task<bool> UpdateAsync(Dictionary<string, string> keyValues);

        Task<IList<Option>> GetOptionsAsync(string key);

        List<SysDict> GetAll();

        #region Admin_UI

        Task<MessageModel<PagingModel<SysDictItem>>> GetDictListAsync(DictSearch search);

        Task<MessageModel> AddOrUpdateDictAsync(DictOperation operation);

        Task<MessageModel> RemoveDictAsync(IList<string> ids);

        Task<MessageModel<SysDict>> GetAsync(long id);

        Task<List<Option>> GetParentListAsync();

        #endregion Admin_UI
    }
}