namespace Mi.Core.Models.Paging
{
    public class PagingModel<T> where T : new()
    {
        public IList<T>? Rows { get; set; }

        public int Total { get; set; }

        public PagingModel()
        { }

        public PagingModel(int total, IList<T>? rows)
        {
            Total = total;
            Rows = rows;
        }
    }
}
