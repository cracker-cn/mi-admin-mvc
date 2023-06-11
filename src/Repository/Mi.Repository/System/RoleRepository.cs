using Mi.Core.Toolkit.API;

namespace Mi.Repository.System
{
    public class RoleRepository : RepositoryBase<SysRole>, IRoleRepository, IScoped
    {
        public RoleRepository(MIDB db) : base(db)
        {
        }

        public Task<int> UsedRoleCountAsync(long id)
        {
            var sql = @"select count(*) from SysRole r
                        inner join SysUserRole rf on r.Id=rf.RoleId and rf.IsDeleted=0
                        where r.IsDeleted=0 and r.Id=@id";
            return base.ExecuteScalarAsync<int>(sql, new {id});
        }
    }
}