using Mi.Entity.System;

namespace Mi.IService.System
{
    public interface ILogService
    {
        Task<bool> WriteLogAsync(string userName, bool succeed, string operationInfo);

        Task<MessageModel<PagingModel<SysLoginLog>>> GetLoginLogListAsync(LoginLogSearch search);
    }
}
