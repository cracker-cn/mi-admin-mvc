using Mi.Core.Attributes;
using Mi.Core.Models;
using Mi.Core.Models.WxWork;
using Mi.IService.System;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.WxWork.Controllers
{
    [Area("WxWork")]
    [Authorize]
    public class ConfigController : Controller
    {
        private readonly IDictService _dictService;
        private readonly WxConfig _config;

        public ConfigController(IDictService dictService, WxConfig config)
        {
            _dictService = dictService;
            _config = config;
        }

        [AuthorizeCode("WxWork:GetConfig")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, AuthorizeCode("WxWork:SetConfig")]
        public async Task<MessageModel> SetConfig([FromBody] Dictionary<string, string> operation) => await _dictService.SetAsync(operation);

        [HttpPost, AuthorizeCode("WxWork:GetConfig")]
        public MessageModel GetConfig() => new MessageModel<WxConfig>(_config);
    }
}
