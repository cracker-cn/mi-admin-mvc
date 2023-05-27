using System.Diagnostics.CodeAnalysis;

namespace Mi.IRepository.System.QueryModels
{
	public class UserItem
	{
		public long UserId { get; set; }

		[NotNull]
		public string? UserName { get; set; }

		public List<string>? RoleNames { get; set; }
	}
}