using Mi.Core.CommonOption;
using Mi.Core.Models;
using Mi.IService.System;
using Mi.IService.System.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.System.Controllers
{
    [Area("System")]
    [Authorize]
    public class FunctionController : Controller
    {
        private readonly IFunctionService _functionService;

        public FunctionController(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(long id)
        {
            return View(await _functionService.GetAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetFunctionList(FunctionSearch search)
        {
            var list = await _functionService.GetFunctionListAsync(search);
            return Ok(new { code = 0, msg = "success", data = list });
        }

        [HttpPost]
        public async Task<MessageModel> AddOrUpdateFunction([FromBody] FunctionOperation operation)
            => await _functionService.AddOrUpdateFunctionAsync(operation);

        [HttpPost]
        public async Task<MessageModel> RemoveFunction([FromForm] IList<long> ids)
            => await _functionService.RemoveFunctionAsync(ids);
    }
}