﻿using Mi.Core.Models;
using Mi.Core.Models.Paging;
using Mi.IService.System;
using Mi.IService.System.Models;
using Mi.IService.System.Models.Result;

using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.System.Controllers
{
    [Area("System")]
    public class DictController : Controller
    {
        private readonly IDictService _dictService;

        public DictController(IDictService dictService)
        {
            _dictService = dictService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(long id)
        {
            return View((await _dictService.GetAsync(id)).Result);
        }

        [HttpPost]
        public async Task<MessageModel<PagingModel<SysDictItem>>> GetDictList([FromBody] DictSearch search)
            => await _dictService.GetDictListAsync(search);

        [HttpPost]
        public async Task<MessageModel> AddOrUpdateDict([FromBody] DictOperation operation)
            => await _dictService.AddOrUpdateDictAsync(operation);

        [HttpPost]
        public async Task<MessageModel> RemoveDict(IList<string> ids)
            => await _dictService.RemoveDictAsync(ids);
    }
}