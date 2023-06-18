using Dapper;

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

        public async Task<IList<long>> GetUserIdInAuthorizationCodesAsync(string[] codes)
        {
            var sql = @"select a.UserId from SysUserRole a join SysRoleFunction b on b.RoleId=a.RoleId 
                        join SysFunction c on c.Id=b.FunctionId and b.IsDeleted=0 and a.IsDeleted=0 and c.IsDeleted=0 and c.AuthorizationCode in @codes
                        union select Id from SysUser where IsDeleted=0 and IsSuperAdmin=1 ";
            var list = await _db.Connection.QueryAsync<long>(sql, new { codes });
            return list.Distinct().ToList();
        }

        public async Task<IList<long>> GetUserIdInRolesAsync(string[] roleNames)
        {
            var sql = "select a.UserId from SysUserRole a join SysRole b on a.RoleId=b.Id and b.IsDeleted=0 and a.IsDeleted=0 and b.RoleName in @names";
            var list = await _db.Connection.QueryAsync<long>(sql, new { name = roleNames });
            return list.Distinct().ToList();
        }

        public async Task<List<SysRole>> QueryUserRolesAsync(long userId)
        {
            var list = await _db.Set<SysRole>().Join(_db.Set<SysUserRole>().Where(b => b.UserId == userId)
                , a => a.Id, b => b.RoleId, (a, b) => a).ToListAsync();

            return list;
        }
    }
}