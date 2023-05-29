using Mi.IService.System.Models.Result;

namespace Mi.IService.System
{
    public interface IDictService
    {
        Task<string> GetValueAsync(string key);

        Task<bool> SetValueAsync(string key, string value);

        Task<bool> UpdateAsync(Dictionary<string, string> keyValues);

        Task<IList<Option>> GetOptionsAsync(string key);

        #region Admin_UI

        Task<MessageModel<PagingModel<SysDictItem>>> GetDictListAsync(DictSearch search);

        Task<MessageModel> AddOrUpdateDictAsync(DictOperation operation);

        Task<MessageModel> RemoveDictAsync(IList<string> ids);

        #endregion Admin_UI
    }
}