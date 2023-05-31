using Mi.Core.Models.UI;
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
    }
}