using Mi.Core.Models.WxWork;
using Mi.Core.Others;
using Mi.IService.WxWork;
using Mi.IService.WxWork.Models;
using Mi.IService.WxWork.Models.Result;

namespace Mi.Application.WxWork
{
    public class WxWorkDeptService : IDepartmentService, IScoped
    {
        private readonly WxWorkRequest _request;
        private readonly WxWorkConfig _config;
        private readonly MessageModel _msg;

        public WxWorkDeptService(WxWorkRequest request, WxWorkConfig config, MessageModel msg)
        {
            _request = request;
            _config = config;
            _msg = msg;
        }

        public async Task<MessageModel> AddOrUpdateDeptAsync(DepartmentOperation operation)
        {
            var url = "department/" + (operation.dept_id > 0 ? "update" : "create");
            var result = await _request.SendAsync<WxWorkApiResponseBase, DepartmentOperation>(url, _config.wx_work_contact_list_secret, HttpMethod.Post, operation);
            return result.Succeed() ? _msg.Success() : _msg.Fail();
        }

        public async Task<MessageModel<IList<DepartmentItem>>> GetDeptListAsync(DepartmentSearch search)
        {
            var result = await _request.SendAsync<WxWorkDept>("user/simplelist", _config.wx_work_contact_list_secret, HttpMethod.Post);
            var list = new List<DepartmentItem>();
            if (result.Succeed() && result.department != null)
            {
                var allDepts = result.department;
                list = allDepts.Select(x => new DepartmentItem
                {
                    Id = x.id,
                    ParentId = x.parentid,
                    Name = x.name,
                    NameEn = x.name,
                    Leader = string.Join(',', x.department_leader),
                    Children = GetChildren(x.id, allDepts)
                }).ToList();
            }
            return _msg.Success().As<IList<DepartmentItem>>(list);
        }

        public async Task<MessageModel> RemoveDeptAsync(IList<long> ids)
        {
            var id = ids.FirstOrDefault();
            var result = await _request.GetAsync<WxWorkApiResponseBase>("department/delete", _config.wx_work_contact_list_secret, "id=" + id);
            return result.Succeed() ? _msg.Success() : _msg.Fail();
        }

        private IList<DepartmentItem> GetChildren(int parentId, IList<Department> departments)
        {
            var list = departments.Where(x => x.parentid == parentId).ToList();
            return list.Select(x => new DepartmentItem
            {
                Id = x.id,
                ParentId = x.parentid,
                Name = x.name,
                NameEn = x.name,
                Leader = string.Join(',', x.department_leader),
                Children = GetChildren(x.id, departments)
            }).ToList();
        }
    }
}
