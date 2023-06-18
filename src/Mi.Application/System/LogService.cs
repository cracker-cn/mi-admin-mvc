using Mi.Core.Factory;
using Mi.Core.Service;
using Mi.IRepository.BASE;

using Microsoft.AspNetCore.Http;

namespace Mi.Application.System
{
    public class LogService : ILogService, IScoped
    {
        private readonly HttpContext httpContext;
        private readonly CreatorFactory _creator;
        private readonly MiHeader _header;

        public LogService(IHttpContextAccessor httpContextAccessor, CreatorFactory creator, MiHeader header)
        {
            httpContext = httpContextAccessor.HttpContext;
            _creator = creator;
            _header = header;
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

        public async Task<bool> WriteLogAsync(string userName, bool succeed, string operationInfo)
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
            await repo.AddAsync(model);
            return true;
        }
    }
}
