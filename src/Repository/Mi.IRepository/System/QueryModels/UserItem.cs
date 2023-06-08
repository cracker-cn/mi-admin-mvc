namespace Mi.IRepository.System.QueryModels
{
    public class UserItem
    {
        public long UserId { get; set; }

        [NotNull]
        public string? UserName { get; set; }

        public string? RoleNameString { get; set; }

        public DateTime CreatedOn { get; set; }

        public IList<string>? RoleNames { get; set; }

        public int IsEnabled { get; set; }
    }
}