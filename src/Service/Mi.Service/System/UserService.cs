using Mi.Core.Models.Paging;
using Mi.IRepository.System;
using Mi.IRepository.System.QueryModels;
using Mi.Service.System.Models;

namespace Mi.Service.System
{
	public class UserService
	{
		private readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<MessageModel<PagingModel<UserItem>>> GetUserListAsync(UserSearch search)
		{
			var pageModel = await _userRepository.QueryListAsync(search, search.UserName);

			return new MessageModel<PagingModel<UserItem>>(true, "查询成功", pageModel);
		}
	}
}