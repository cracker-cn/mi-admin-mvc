using Mi.Core.Models.Paging;

namespace Mi.IService.System.Models
{
	public class UserSearch : PagingSearchModel
	{
		public string? UserName { get; set; }
	}
}