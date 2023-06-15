using Mi.Core.Toolkit.API;

namespace Mi.Repository.System
{
    public class DictRepository : RepositoryBase<SysDict>, IDictRepository, IScoped
    {
        public DictRepository(MIDB db) : base(db)
        {
        }
    }
}