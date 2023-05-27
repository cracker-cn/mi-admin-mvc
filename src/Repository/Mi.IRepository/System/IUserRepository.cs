using Mi.IRepository.BASE;
using Mi.IRepository.System.QueryModels;
using Mi.Toolkit.ICore;

namespace Mi.IRepository.System
{
	public interface IUserRepository : IRepositoryBase<SysUser>
	{
		Task<PagingModel<UserItem>> QueryListAsync(PagingSearchModel model,string? userName);
	}
}