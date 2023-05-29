using Dapper;

using Mi.IRepository.BASE;
using Mi.IRepository.System.QueryModels;

namespace Mi.IRepository.System
{
    public interface IDictRepository : IRepositoryBase<SysDict>
    {
    }
}