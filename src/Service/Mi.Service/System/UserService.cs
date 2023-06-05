using Mi.Core.GlobalUser;
using Mi.Core.Toolkit.API;
using Mi.Entity.System;
using Mi.Core.Toolkit.Helper;

namespace Mi.Service.System
{
    public class UserService : IUserService, IScoped
    {
        private readonly IUserRepository _userRepository;
        private readonly MessageModel _message;
        private readonly IMiUser _miUser;

        public UserService(IUserRepository userRepository, MessageModel message
            , IMiUser miUser)
        {
            _userRepository = userRepository;
            _message = message;
            _miUser = miUser;
        }

        public async Task<MessageModel<string>> AddUserAsync(string userName)
        {
            var user = new SysUser { UserName = userName, Id = IdHelper.SnowflakeId() };
            user.PasswordSalt = EncryptionHelper.GetPasswordSalt();
            var password = StringHelper.GetRandomString(6);
            user.Password = EncryptionHelper.GenEncodingPassword(password, user.PasswordSalt);

            var flag = await _userRepository.AddAsync(user);

            return new MessageModel<string>(flag, flag ? "操作成功" : "操作失败", password);
        }

        public async Task<MessageModel<SysUser>> GetUserAsync(long userId)
        {
            return new MessageModel<SysUser>(true, "查询成功", await _userRepository.GetAsync(userId));
        }

        public async Task<MessageModel<PagingModel<UserItem>>> GetUserListAsync(UserSearch search)
        {
            var pageModel = await _userRepository.QueryListAsync(search, search.UserName);

            foreach (var item in pageModel.Rows!)
            {
                if (!string.IsNullOrEmpty(item.RoleNameString))
                {
                    item.RoleNames = item.RoleNameString.Split(',');
                }
            }
            return new MessageModel<PagingModel<UserItem>>(true, "查询成功", pageModel);
        }

        public async Task<MessageModel> RemoveUserAsync(long userId)
        {
            var flag = await _userRepository.UpdateAsync(userId, node => node
                .Set(x => x.IsDeleted, 1)
                .Set(x => x.ModifiedBy, _miUser.UserId)
                .Set(x => x.ModifiedOn, TimeHelper.LocalTime()));

            return flag ? _message.Success() : _message.Fail();
        }

        public async Task<MessageModel<string>> UpdatePasswordAsync(long userId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user.Id <= 0) return new MessageModel<string>(false, "用户不存在", "");

            var password = StringHelper.GetRandomString(6);
            var flag = await _userRepository.UpdateAsync(userId, node => node
                .Set(x => x.Password, EncryptionHelper.GenEncodingPassword(password, user.PasswordSalt))
                .Set(x => x.ModifiedBy, _miUser.UserId)
                .Set(x => x.ModifiedOn, TimeHelper.LocalTime()));

            return new MessageModel<string>(flag, flag ? "操作成功" : "操作失败", password);
        }
    }
}