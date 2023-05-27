using Mi.IRepository.System.QueryModels;
using Mi.Toolkit.Extension;

using Microsoft.EntityFrameworkCore;

namespace Mi.Repository.System
{
	public class UserRepository : RepositoryBase<SysUser>, IUserRepository
	{
		public UserRepository(MIDB db) : base(db)
		{
		}

		public async Task<PagingModel<UserItem>> QueryListAsync(PagingSearchModel model, string? userName)
		{
			var exp = ExpressionCreator.New<SysUser>()
				.AndIf(!string.IsNullOrEmpty(userName), x => x.UserName.Contains(userName!));

			var total = await DB.Set<SysUser>().CountAsync();
			var list = await (from a in DB.Set<SysUser>()
							  join b in DB.Set<SysUserRole>() on a.Id equals b.UserId into b1
							  from b2 in b1.DefaultIfEmpty()
							  join c in DB.Set<SysRole>() on b2.RoleId equals c.Id into c1
							  from c2 in c1.DefaultIfEmpty()
							  group new { a, c2 } by new { a.Id, a.UserName } into g
							  select new UserItem
							  {
								  UserName = g.Key.UserName,
								  UserId = g.Key.Id,
								  RoleNames = g.Select(x => x.c2.RoleName).ToList()
							  }).Skip((model.Page - 1) * model.Size).Take(model.Size).ToListAsync();

			return new PagingModel<UserItem>(total, list);
		}
	}
}