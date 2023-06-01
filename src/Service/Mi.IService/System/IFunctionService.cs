using Mi.Entity.System;
using Mi.Entity.System.Enum;

namespace Mi.IService.System
{
    public interface IFunctionService
    {
        Task<MessageModel> AddOrUpdateFunctionAsync(FunctionOperation operation);

        EnumTreeNode CheckFunctionNode(SysFunction node);

        Task<IList<SysFunction>> GetFunctionListAsync(FunctionSearch search);

        IList<TreeOption> GetFunctionTree();

        Task<SysFunction> GetAsync(long id); 
    }
}