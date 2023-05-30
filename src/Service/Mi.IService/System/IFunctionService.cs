using Mi.Entity.System;
using Mi.Entity.System.Enum;

namespace Mi.IService.System
{
    public interface IFunctionService
    {
        Task<MessageModel> AddOrUpdateFunctionAsync(FunctionOperation operation);

        EnumTreeNode CheckFunctionNode(SysFunction node);
    }
}