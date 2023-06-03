using Mi.Entity.System;
using Mi.Entity.System.Enum;
using Mi.IService.System.Models.Result;

namespace Mi.IService.System
{
    public interface IFunctionService
    {
        Task<MessageModel> AddOrUpdateFunctionAsync(FunctionOperation operation);

        EnumTreeNode CheckFunctionNode(SysFunction node);

        Task<IList<FunctionItem>> GetFunctionListAsync(FunctionSearch search);

        IList<TreeOption> GetFunctionTree();

        Task<SysFunction> GetAsync(long id);

        Task<MessageModel> RemoveFunctionAsync(IList<long> ids);

        IList<SysFunction> GetFunctionsCache();
    }
}