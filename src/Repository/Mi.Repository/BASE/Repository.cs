using Dapper;

namespace Mi.Repository.BASE
{
    public class Repository<T> where T : class, new()
    {
        private readonly MIDB _db;

        public Repository(MIDB db)
        {
            _db = db;
        }

        public async Task<PagingModel<T>> GetPagingAsync(PagingSearchModel model, string sql, object? param = default)
        {
            var result = new PagingModel<T>();
            result.Total = await _db.Connection.ExecuteScalarAsync<int>(DBHelper.GetCount(sql), param);
            result.Rows = (await _db.Connection.QueryAsync<T>(DBHelper.GetPaging(model.Page, model.Size, sql), param)).ToList();
            return result;
        }

        public async Task<List<T>> GetListAsync(string sql,object? param = default)
        {
            return (await _db.Connection.QueryAsync<T>(sql,param)).ToList();
        }
    }
}