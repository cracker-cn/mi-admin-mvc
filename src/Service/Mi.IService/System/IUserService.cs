﻿using Mi.Entity.System;

namespace Mi.IService.System
{
	public interface IUserService
	{
		Task<MessageModel<PagingModel<UserItem>>> GetUserListAsync(UserSearch search);

		Task<MessageModel<string>> AddUserAsync(string userName);

		Task<MessageModel> RemoveUserAsync(long userId);

		Task<MessageModel<string>> UpdatePasswordAsync(long userId);

		Task<MessageModel<SysUser>> GetUserAsync(long userId);

		Task<MessageModel> PassedUserAsync(long id);

		Task<IList<SysRole>> GetRolesAsync(long id);

		Task<MessageModel<UserBaseInfo>> GetUserBaseInfoAsync();

		Task<MessageModel> SetUserBaseInfoAsync(UserBaseInfo model);

		Task<MessageModel> SetPasswordAsync(string password);
	}
}