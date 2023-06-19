using Mi.IService.System.Models.Result;

namespace Mi.IService.System
{
    public interface IDepartmentService
    {
        Task<MessageModel<IList<DepartmentItem>>> GetDeptListAsync(DepartmentSearch search);

        Task<MessageModel> AddOrUpdateDeptAsync(DepartmentOperation operation);

        Task<MessageModel> RemoveDeptAsync(IList<long> ids);
    }
}
