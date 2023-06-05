using Mi.Core.Toolkit.API;
using Mi.IRepository.BASE;
using Mi.Repository.Extension;

using Microsoft.EntityFrameworkCore;

namespace Mi.Repository.System
{
    public class PermissionRepository : IPermissionRepository, IScoped
    {
        private readonly MIDB _db;

        public PermissionRepository(MIDB db)
        {
            _db = db;
        }

        public IRepositoryBase<SysUserRole> UserRoleRepo => _db.GetRepository<SysUserRole>();

        public async Task<List<SysRole>> QueryUserRolesAsync(long userId)
        {
            var list = await _db.Set<SysRole>().Join(_db.Set<SysUserRole>().Where(b => b.UserId == userId)
                , a => a.Id, b => b.RoleId, (a, b) => a).ToListAsync();

            return list;
        }
    }
}