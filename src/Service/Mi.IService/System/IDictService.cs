using Mi.Entity.System;
using Mi.IService.System.Models.Result;

namespace Mi.IService.System
{
    public interface IDictService
    {
        #region Admin_UI

        List<SysDict> GetAll();

        Task<MessageModel<PagingModel<DictItem>>> GetDictListAsync(DictSearch search);

        Task<MessageModel> AddOrUpdateDictAsync(DictOperation operation);

        Task<MessageModel> RemoveDictAsync(IList<string> ids);

        Task<MessageModel<SysDict>> GetAsync(long id);

        Task<List<Option>> GetParentListAsync();

        #endregion Admin_UI
    }
}