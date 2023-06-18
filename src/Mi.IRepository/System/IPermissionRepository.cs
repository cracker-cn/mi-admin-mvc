using Mi.IRepository.BASE;

namespace Mi.IRepository.System
{
    public interface IPermissionRepository
    {
        Task<List<SysRole>> QueryUserRolesAsync(long userId);

        IRepositoryBase<SysUserRole> UserRoleRepo { get; }

        /// <summary>
        /// 获取角色名xxx，xxx的所有用户
        /// </summary>
        /// <param name="roleNames"></param>
        /// <returns></returns>
        Task<IList<long>> GetUserIdInRolesAsync(string[] roleNames);

        Task<IList<long>> GetUserIdInAuthorizationCodesAsync(string[] codes);
    }
}