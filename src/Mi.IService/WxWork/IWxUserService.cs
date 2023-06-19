using Mi.IService.WxWork.Models.Result;

namespace Mi.IService.WxWork
{
    public interface IWxUserService
    {
        Task<MessageModel<IList<WxUserItem>>> GetDeptMemberListAsync(long deptId);
    }
}
