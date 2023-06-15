using Mi.IRepository.BASE;

namespace Mi.IRepository.System
{
    public interface IPermissionRepository
    {
        Task<List<SysRole>> QueryUserRolesAsync(long userId);

        IRepositoryBase<SysUserRole> UserRoleRepo { get; }
    }
}