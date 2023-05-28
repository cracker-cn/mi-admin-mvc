namespace Mi.Repository.System
{
    public class RoleRepository : RepositoryBase<SysRole>, IRoleRepository, IScoped
    {
        public RoleRepository(MIDB db) : base(db)
        {
        }
    }
}