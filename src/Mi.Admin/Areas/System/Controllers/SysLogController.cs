using Mi.Core.Models.Paging;
using Mi.Core.Models;
using Mi.Entity.System;

using Microsoft.AspNetCore.Mvc;
using Mi.IService.System.Models;
using Mi.Core.Attributes;
using Mi.IService.System;

namespace Mi.Admin.Areas.System.Controllers
{
    [Area("System")]
    public class SysLogController : Controller
    {
        private readonly ILogService _logService;

        public SysLogController(ILogService logService)
        {
            _logService = logService;
        }

        [AuthorizeCode("System:LoginLog:Page")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, AuthorizeCode("System:LoginLog:Query")]
        public async Task<MessageModel<PagingModel<SysLoginLog>>> GetLoginLogList([FromBody] LoginLogSearch search)
        {
            return await _logService.GetLoginLogListAsync(search);
        }
    }
}
