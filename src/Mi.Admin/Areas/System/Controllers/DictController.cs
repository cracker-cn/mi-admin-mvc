using Mi.Core.Attributes;
using Mi.Core.CommonOption;
using Mi.Core.Models;
using Mi.Core.Models.Paging;
using Mi.IService.System;
using Mi.IService.System.Models;
using Mi.IService.System.Models.Result;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.System.Controllers
{
    [Area("System")]
    [Authorize]
    public class DictController : Controller
    {
        private readonly IDictService _dictService;

        public DictController(IDictService dictService)
        {
            _dictService = dictService;
        }

        [AuthorizeCode("System:Dict:Page")]
        public IActionResult Index()
        {
            return View();
        }

        [AuthorizeCode("System:Dict:AddOrUpdate")]
        public async Task<IActionResult> Edit(long id)
        {
            ViewBag.Options = _dictService.GetAll();
            return View((await _dictService.GetAsync(id)).Result);
        }

        [HttpPost, AuthorizeCode("System:Dict:Query")]
        public async Task<MessageModel<PagingModel<DictItem>>> GetDictList([FromBody] DictSearch search)
            => await _dictService.GetDictListAsync(search);

        [HttpPost, AuthorizeCode("System:Dict:AddOrUpdate")]
        public async Task<MessageModel> AddOrUpdateDict([FromBody] DictOperation operation)
            => await _dictService.AddOrUpdateDictAsync(operation);

        [HttpPost, AuthorizeCode("System:Dict:Remove")]
        public async Task<MessageModel> RemoveDict(IList<string> ids)
            => await _dictService.RemoveDictAsync(ids);

        [HttpPost, AuthorizeCode("System:Dict:GetParentList")]
        public async Task<List<Option>> GetParentList()
            => await _dictService.GetParentListAsync();
    }
}