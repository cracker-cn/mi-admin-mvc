using Mi.Core.CommonOption;
using Mi.IService.System.Models.Result;

namespace Mi.IService.System
{
    public interface IPermissionService
    {
        Task<MessageModel> SetUserRoleAsync(long userId, List<long> roleIds);

        Task<MessageModel<IList<UserRoleOption>>> GetUserRolesAsync(long userId);
    }
}