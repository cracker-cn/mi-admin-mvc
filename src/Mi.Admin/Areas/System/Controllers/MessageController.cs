using Mi.Core.Attributes;
using Mi.Core.Models;
using Mi.Core.Models.Paging;
using Mi.Entity.System;
using Mi.IService.System;
using Mi.IService.System.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.System.Controllers
{
	[Area("System")]
	[Authorize]
	public class MessageController : Controller
	{
		private readonly IMessageService _messageService;
		public MessageController(IMessageService messageService)
		{
			_messageService = messageService;
		}

		[AuthorizeCode("System:Message:Page")]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost, AuthorizeCode("System:Message:Readed")]
		public async Task<MessageModel> Readed(long msgId) => await _messageService.ReadedAsync(msgId);

		[HttpPost, AuthorizeCode("System:Message:Query")]
		public async Task<MessageModel<PagingModel<SysMessage>>> GetMessageList([FromBody] MessageSearch search) => await _messageService.GetMessageListAsync(search);
	}
}
