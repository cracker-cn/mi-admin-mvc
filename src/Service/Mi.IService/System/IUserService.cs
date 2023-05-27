using Mi.Core.Models.Paging;
using Mi.IRepository.System.QueryModels;
using Mi.IService.System.Models;

namespace Mi.IService.System
{
    public interface IUserService
    {
		Task<MessageModel<PagingModel<UserItem>>> GetUserListAsync(UserSearch search);

	}
}