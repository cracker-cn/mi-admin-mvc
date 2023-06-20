using System.ComponentModel.DataAnnotations;

using Mi.Core.Attributes;
using Mi.Core.CommonOption;
using Mi.Core.Models;
using Mi.IService.Cache;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.Development.Controllers
{
    [Area("Development")]
    [Authorize]
    public class CacheKeyManagerController : Controller
    {
        private readonly ICacheKeyManagerService _keyService;

        public CacheKeyManagerController(ICacheKeyManagerService keyService)
        {
            _keyService = keyService;
        }

        [AuthorizeCode("Development:CacheKey:Page")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeCode("Development:CacheKey:Query")]
        public async Task<MessageModel<IList<Option>>> GetAllKeys(string? vague, int cacheType) => await _keyService.GetAllKeysAsync(vague);

        [HttpPost]
        [AuthorizeCode("Development:CacheKey:Remove")]
        public async Task<MessageModel> RemoveKey([Required(ErrorMessage = "key不能为空")] string key) => await _keyService.RemoveKeyAsync(key);

        [HttpPost]
        [AuthorizeCode("Development:CacheKey:GetData")]
        public async Task<MessageModel<string>> GetData([Required(ErrorMessage = "key不能为空")] string key) => await _keyService.GetDataAsync(key);
    }
}
