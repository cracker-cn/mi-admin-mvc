using Mi.Core.Attributes;
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

        [AuthorizeCode("System:Function:Page")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, AuthorizeCode("System:Function:Query")]
        public async Task<MessageModel> GetFunctionList(FunctionSearch search)
        {
            return await _functionService.GetFunctionListAsync(search);
        }

        [HttpPost, AuthorizeCode("System:Function:AddOrUpdate")]
        public async Task<MessageModel> AddOrUpdateFunction([FromBody] FunctionOperation operation)
            => await _functionService.AddOrUpdateFunctionAsync(operation);

        [HttpPost, AuthorizeCode("System:Function:Remove")]
        public async Task<MessageModel> RemoveFunction([FromForm] IList<long> ids)
            => await _functionService.RemoveFunctionAsync(ids);

        [HttpPost, AuthorizeCode("System:Function:GetFunctionTree")]
        public IList<TreeOption> GetFunctionTree() => _functionService.GetFunctionTree();
    }
}