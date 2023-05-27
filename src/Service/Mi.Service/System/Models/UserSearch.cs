using Mi.Core.Models.Paging;

namespace Mi.Service.System.Models
{
	public class UserSearch : PagingSearchModel
	{
		public string? UserName { get; set; }
	}
}