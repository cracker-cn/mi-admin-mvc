using System.Linq.Expressions;

using Dapper;

using Mi.Core.DB;
using Mi.Entity.BASE;

namespace Mi.IRepository.BASE
{
    public interface IRepositoryBase<T> where T : EntityBase, new()
    {
        #region 新增

        bool Add(T model);

        object AddReturnId(T model);

        T AddReturnEntity(T model);

        Task<bool> AddAsync(T model);

        Task<object> AddReturnIdAsync(T model);

        Task<T> AddReturnEntityAsync(T model);

        Task<bool> AddManyAsync(IList<T> models);

        #endregion 新增

        #region 删除

        bool Delete(T model);

        bool Delete(object id);

        Task<bool> DeleteAsync(T model);

        Task<bool> DeleteAsync(object id);

        Task<bool> DeleteManyAsync(IList<T> models);

        #endregion 删除

        #region 更新

        bool Update(T model);

        Task<bool> UpdateAsync(T model);

        bool Update(object id, Action<Updater<T>> action);

        Task<bool> UpdateAsync(object id, Action<Updater<T>> action);

        Task<bool> UpdateManyAsync(IList<T> models);

        int Execute(string sql, object? param = default);

        Task<int> ExecuteAsync(string sql, object? param = default);

        TField ExecuteScalar<TField>(string sql, object? param = default);

        Task<TField> ExecuteScalarAsync<TField>(string sql, object? param = default);

        #endregion 更新

        #region 查询

        T Get(object id);

        T Get(Expression<Func<T, bool>> exp);

        IList<T> GetAll();

        IList<T> GetAll(Expression<Func<T, bool>> exp);

        Task<T> GetAsync(object id);

        Task<T> GetAsync(Expression<Func<T, bool>> exp);

        Task<IList<T>> GetAllAsync();

        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> exp);

        PagingModel<T> QueryPage(int page, int size, Expression<Func<T, bool>>? exp = default);

        Task<PagingModel<T>> QueryPageAsync(int page, int size, Expression<Func<T, bool>>? exp = default);

        PagingModel<T> QueryPage(int page, int size, string sql, DynamicParameters parameters, string? orderBy = default);

        Task<PagingModel<T>> QueryPageAsync(int page, int size, string sql, DynamicParameters parameters, string? orderBy = default);

        #endregion 查询
    }
}