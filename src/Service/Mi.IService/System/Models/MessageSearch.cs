namespace Mi.IService.System.Models
{
	public class MessageSearch : PagingSearchModel
	{
		public string? Title { get; set; }
		public string? WriteTime { get; set; }
	}
}
