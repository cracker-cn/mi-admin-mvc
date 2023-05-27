namespace Mi.Toolkit.Helper
{
    public class DBHelper
    {
        public static string GetCount(string sql)
        {
            return $"select count(*) from ({sql}) countTable;";
        }

        public static string GetPaging(int page, int size, string sql, string? orderBy = default)
        {
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                sql += $" order by {orderBy} ";
            }
            return $"select * from ({sql}) mergeTable limit {(page - 1) * size},{size};";
        }
    }
}