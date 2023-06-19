using Mi.IService.System.Models.Result;

namespace Mi.Application.WxWork
{
    public class WxWorkDeptService : IDepartmentService, IScoped
    {
        public Task<MessageModel> AddOrUpdateDeptAsync(DepartmentOperation operation)
        {
            throw new NotImplementedException();
        }

        public Task<MessageModel<IList<DepartmentItem>>> GetDeptListAsync(DepartmentSearch search)
        {
            throw new NotImplementedException();
        }

        public Task<MessageModel> RemoveDeptAsync(IList<long> ids)
        {
            throw new NotImplementedException();
        }
    }
}
