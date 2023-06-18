using Mi.IRepository.BASE;

namespace Mi.IRepository.System
{
    public interface IRoleRepository : IRepositoryBase<SysRole>
    {
        /// <summary>
        /// 使用角色次数，统计未删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> UsedRoleCountAsync(long id);
    }
}