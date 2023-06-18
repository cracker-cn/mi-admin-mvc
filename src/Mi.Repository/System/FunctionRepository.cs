using Mi.Core.Toolkit.API;

namespace Mi.Repository.System
{
    public class FunctionRepository : RepositoryBase<SysFunction>, IFunctionRepository, IScoped
    {
        public FunctionRepository(MIDB db) : base(db)
        {
        }
    }
}