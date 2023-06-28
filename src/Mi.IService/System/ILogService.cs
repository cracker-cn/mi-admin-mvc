using Mi.Entity.System;

namespace Mi.IService.System
{
    public interface ILogService
    {
        Task<bool> WriteLoginLogAsync(string userName, bool succeed, string operationInfo);

        Task<bool> WriteLogAsync(string url, string? param, string? actionFullName, string? uniqueId = default, string? contentType = default, bool succeed = true, string? exception = default);

        Task<MessageModel<PagingModel<SysLoginLog>>> GetLoginLogListAsync(LoginLogSearch search);

        Task<bool> SetExceptionAsync(string uniqueId,string errorMsg);
    }
}