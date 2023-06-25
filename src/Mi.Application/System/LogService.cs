using Mi.Core.Factory;
using Mi.Core.Service;
using Mi.IRepository.BASE;
using Mi.Repository.BASE;

using Microsoft.AspNetCore.Http;

namespace Mi.Application.System
{
    public class LogService : ILogService, IScoped
    {
        private readonly HttpContext httpContext;
        private readonly CreatorFactory _creator;
        private readonly MiHeader _header;
        private readonly IMiUser _miUser;

        public LogService(IHttpContextAccessor httpContextAccessor, CreatorFactory creator, MiHeader header, IMiUser miUser)
        {
            httpContext = httpContextAccessor.HttpContext;
            _creator = creator;
            _header = header;
            _miUser = miUser;
        }

        public async Task<MessageModel<PagingModel<SysLoginLog>>> GetLoginLogListAsync(LoginLogSearch search)
        {
            var repo = DotNetService.Get<IRepositoryBase<SysLoginLog>>();
            var exp = ExpressionCreator.New<SysLoginLog>()
                .AndIf(!string.IsNullOrEmpty(search.UserName), x => x.UserName.Contains(search.UserName!))
                .AndIf(search.Succeed == 1, x => x.Status == 1)
                .AndIf(search.Succeed == 2, x => x.Status == 0);
            var list = await repo.QueryPageAsync(search.Page, search.Size, x => x.CreatedOn, exp, false);
            return new MessageModel<PagingModel<SysLoginLog>>(list);
        }

        public async Task<bool> WriteLogAsync(string url, string? param, string? actionFullName, string? contentType = null, bool succeed = true, string? exception = null)
        {
            var repo = DotNetService.Get<IRepositoryBase<SysLog>>();
            var log = _creator.NewEntity<SysLog>();
            log.RequestUrl = url;
            log.RequestParams = param;
            log.ActionFullName = actionFullName;
            log.ContentType = contentType;
            log.Succeed = succeed ? 1: 0;
            log.Exception = exception;
            log.UserId = _miUser.UserId;
            log.UserName = _miUser.UserName;
            return await repo.AddAsync(log);
        }

        public async Task<bool> WriteLoginLogAsync(string userName, bool succeed, string operationInfo)
        {
            var model = _creator.NewEntity<SysLoginLog>();
            var repo = DotNetService.Get<IRepositoryBase<SysLoginLog>>();
            model.UserName = userName;
            model.OperationInfo = operationInfo;
            model.Status = succeed ? 1 : 0;
            model.IpAddress = _header.Ip;
            model.RegionInfo = _header.Region;
            model.Browser = _header.Browser;
            model.System = _header.System;
            return await repo.AddAsync(model);
        }
    }
}
