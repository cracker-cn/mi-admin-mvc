using Mi.Core.Attributes;
using Mi.Core.Models;
using Mi.Core.Models.Paging;
using Mi.Entity.System;
using Mi.IService.System;
using Mi.IService.System.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.Workspace.Controllers
{
    [Area("Workspace")]
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [AuthorizeCode("Workspace:Message:Page")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, AuthorizeCode("Workspace:Message:Readed")]
        public async Task<MessageModel> Readed(IList<long> ids) => await _messageService.ReadedAsync(ids);

        [HttpPost, AuthorizeCode("Workspace:Message:Query")]
        public async Task<MessageModel<PagingModel<SysMessage>>> GetMessageList([FromBody] MessageSearch search) => await _messageService.GetMessageListAsync(search);
    }
}
