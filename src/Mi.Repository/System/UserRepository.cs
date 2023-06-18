using Dapper;

using Mi.Core.Toolkit.API;
using Mi.Repository.Extension;

namespace Mi.Repository.System
{
    public class UserRepository : RepositoryBase<SysUser>, IUserRepository, IScoped
    {
        public UserRepository(MIDB db) : base(db)
        {
        }

        public async Task<PagingModel<UserItem>> QueryListAsync(PagingSearchModel model, string? userName)
        {
            var sql = @"SELECT
	                    u.Id UserId,
	                    u.UserName,
	                    u.CreatedOn,
	                    u.IsEnabled,
	                    u.NickName,
	                    u.Sex,
	                    u.Signature,
	                    u.Avatar,
	                    GROUP_CONCAT( r.RoleName ) RoleNameString
                    FROM
	                    SysUser u
	                    LEFT JOIN SysUserRole ur ON u.Id = ur.UserId
	                    LEFT JOIN SysRole r ON ur.RoleId = r.Id 
                    where u.IsDeleted = 0 ";
            var parameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(userName))
            {
                parameters.Add("@userName", "%" + userName + "%");
                sql += " and u.UserName like @userName";
            }
            sql += " GROUP BY u.Id,u.UserName ";

            return await DB.QueryPageAsync<UserItem>(model.Page, model.Size, sql, parameters, " u.CreatedOn desc");
        }
    }
}