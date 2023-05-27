using Mi.Core.Models.Paging;

namespace Mi.IService.System
{
    public interface IUserService
    {
        Task<MessageModel> GetUserListAsync();
    }
}