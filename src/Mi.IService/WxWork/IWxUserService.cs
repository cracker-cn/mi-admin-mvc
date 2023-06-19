using Mi.IService.WxWork.Models.Result;

namespace Mi.IService.WxWork
{
    public interface IWxUserService
    {
        Task<MessageModel<WxUserItem>> GetDeptMemberListAsync(long deptId);


    }
}
