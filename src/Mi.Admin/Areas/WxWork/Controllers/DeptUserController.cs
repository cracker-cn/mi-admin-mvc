using Mi.Core.Attributes;
using Mi.Core.Models;
using Mi.IService.WxWork;
using Mi.IService.WxWork.Models;
using Mi.IService.WxWork.Models.Result;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.WxWork.Controllers
{
    [Area("WxWork")]
    [Authorize]
    public class DeptUserController : Controller
    {
        private readonly IWxUserService _wxUserService;
        private readonly IDepartmentService _departmentService;

        public DeptUserController(IWxUserService wxUserService, IDepartmentService departmentService)
        {
            _wxUserService = wxUserService;
            _departmentService = departmentService;
        }

        [AuthorizeCode("WxWork:DeptUser:Page")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("WxWork:DeptUser:Query")]
        public async Task<MessageModel<IList<WxUserItem>>> GetDeptMemberList(long deptId)
        {
            return await _wxUserService.GetDeptMemberListAsync(deptId);
        }

        [HttpPost, AuthorizeCode("WxWork:DeptUser:Query")]
        public async Task<MessageModel<IList<DepartmentItem>>> GetDeptList(DepartmentSearch search)
        {
            return await _departmentService.GetDeptListAsync(search);
        }
    }
}
