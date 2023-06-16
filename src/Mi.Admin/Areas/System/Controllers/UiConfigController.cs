using Mi.Core.Attributes;
using Mi.Core.Models;
using Mi.IService.Public;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.System.Controllers
{
    [Area("System")]
    [Authorize]
    public class UiConfigController : Controller
    {
        private readonly IPublicService _publicService;

        public UiConfigController(IPublicService publicService)
        {
            _publicService = publicService;
        }

        [AuthorizeCode("Sytem:UiConfig")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost,AuthorizeCode("System:SetConfig")]
        public async Task<MessageModel> SetUiConfig([FromBody] SysConfigModel operation) => await _publicService.SetUiConfigAsync(operation);

        [HttpPost, AuthorizeCode("System:GetConfig")]
        public async Task<MessageModel<SysConfigModel>> GetUiConfig() => await _publicService.GetUiConfigAsync();
    }
}
