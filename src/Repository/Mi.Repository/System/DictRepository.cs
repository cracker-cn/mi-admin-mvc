using System.Linq.Expressions;

using Dapper;

using Mi.Core.Models;
using Mi.Repository.Extension;

namespace Mi.Repository.System
{
    public class DictRepository : RepositoryBase<SysDict>, IDictRepository, IScoped
    {
        public DictRepository(MIDB db) : base(db)
        {
        }
    }
}