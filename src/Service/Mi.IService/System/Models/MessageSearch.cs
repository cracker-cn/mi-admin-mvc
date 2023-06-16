namespace Mi.IService.System.Models
{
	public class MessageSearch : PagingSearchModel
	{
        public long? No { get; set; }

        public string? Title { get; set; }

		public string? WriteTime { get; set; }

		public int? Readed { get; set; }
	}
}
