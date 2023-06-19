using Mi.Core.Attributes;
using Mi.Core.Models;
using Mi.Core.Models.WxWork;
using Mi.IService.Public;
using Mi.IService.System;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.System.Controllers
{
    [Area("System")]
    [Authorize]
    public class UiConfigController : Controller
    {
        private readonly IPublicService _publicService;
        private readonly IDictService _dictService;

        public UiConfigController(IPublicService publicService, IDictService dictService)
        {
            _publicService = publicService;
            _dictService = dictService;
        }

        [AuthorizeCode("System:UiConfig")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, AuthorizeCode("System:SetConfig")]
        public async Task<MessageModel> SetUiConfig([FromBody] Dictionary<string, string> operation) => await _dictService.SetAsync(operation);

        [HttpPost, AuthorizeCode("System:GetConfig")]
        public async Task<MessageModel<SysConfigModel>> GetUiConfig() => await _publicService.GetUiConfigAsync();
    }
}
