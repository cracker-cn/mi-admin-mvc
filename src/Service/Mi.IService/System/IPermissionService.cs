using Mi.Core.Models.UI;
using Mi.Entity.System;
using Mi.IService.System.Models.Result;

namespace Mi.IService.System
{
    public interface IPermissionService
    {
        Task<MessageModel> SetUserRoleAsync(long userId, List<long> roleIds);

        Task<MessageModel<IList<UserRoleOption>>> GetUserRolesAsync(long userId);

        /// <summary>
        /// 获取当前用户可查看的侧边菜单
        /// </summary>
        /// <returns></returns>
        Task<List<PaMenuModel>> GetSiderMenuAsync();

        Task<MessageModel<IList<long>>> GetRoleFunctionIdsAsync(long id);

        Task<MessageModel> SetRoleFunctionsAsync(long id,IList<long> funcIds);

        Task<MessageModel> RegisterAsync(string userName, string password);

        Task<MessageModel> LoginAsync(string userName, string password, string verifyCode);

        Task LogoutAsync();

        Task<UserModel> QueryUserModelCacheAsync(string userData);
    }
}