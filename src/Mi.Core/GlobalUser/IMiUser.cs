namespace Mi.Core.GlobalUser
{
    public interface IMiUser
    {
        long UserId { get; }
        string UserName { get; }
        bool IsSuperAdmin { get; }
        IList<long> FuncIds { get; }
    }
}