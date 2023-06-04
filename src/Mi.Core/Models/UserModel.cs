namespace Mi.Core.Models
{
    public class UserModel
    {
        public bool IsSuperAdmin { get; set; }

        public long UserId { get; set; }
        public string? UserName { get; set; }
    }
}