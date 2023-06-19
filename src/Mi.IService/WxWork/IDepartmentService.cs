using Mi.IService.WxWork.Models;
using Mi.IService.WxWork.Models.Result;

namespace Mi.IService.WxWork
{
    public interface IDepartmentService
    {
        Task<MessageModel<IList<DepartmentItem>>> GetDeptListAsync(DepartmentSearch search);

        Task<MessageModel> AddOrUpdateDeptAsync(DepartmentOperation operation);

        Task<MessageModel> RemoveDeptAsync(IList<long> ids);
    }
}
