using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

using Dapper;

using Mi.Core.DB;
using Mi.Core.Models.Paging;
using Mi.IRepository.BASE;
using Mi.Repository.DB;

using Microsoft.EntityFrameworkCore;

namespace Mi.Repository.BASE
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class, new()
    {
        protected readonly MIDB DB;

        public RepositoryBase(MIDB db)
        {
            DB = db;
        }

        public bool Add(T model)
        {
            DB.Add(model);
            return 0 < DB.SaveChanges();
        }

        public async Task<bool> AddAsync(T model)
        {
            await DB.AddAsync(model);
            return 0 < await DB.SaveChangesAsync();
        }

        public async Task<bool> AddManyAsync(IList<T> models)
        {
            await DB.AddRangeAsync(models);
            return models.Count == await DB.SaveChangesAsync();
        }

        public T AddReturnEntity(T model)
        {
            DB.Add(model);
            DB.SaveChanges();
            DB.Entry(model);
            return model;
        }

        public async Task<T> AddReturnEntityAsync(T model)
        {
            await DB.AddAsync(model);
            await DB.SaveChangesAsync();
            DB.Entry(model);
            return model;
        }

        public object AddReturnId(T model)
        {
            var data = AddReturnEntity(model);
            var prop = typeof(T).GetProperties().FirstOrDefault(x => x.GetCustomAttribute<KeyAttribute>() != null);
            return prop!.GetValue(data)!;
        }

        public async Task<object> AddReturnIdAsync(T model)
        {
            var data = await AddReturnEntityAsync(model);
            var prop = typeof(T).GetProperties().FirstOrDefault(x => x.GetCustomAttribute<KeyAttribute>() != null);
            return prop!.GetValue(data)!;
        }

        public bool Delete(T model)
        {
            DB.Remove(model);
            return 0 < DB.SaveChanges();
        }

        public bool Delete(object id)
        {
            var model = Get(id);
            return Delete(model);
        }

        public async Task<bool> DeleteAsync(T model)
        {
            DB.Remove(model);
            return 0 < await DB.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(object id)
        {
            var model = await GetAsync(id);
            DB.Remove(model);
            return 0 < await DB.SaveChangesAsync();
        }

        public async Task<bool> DeleteManyAsync(IList<T> models)
        {
            DB.RemoveRange(models);
            return models.Count == await DB.SaveChangesAsync();
        }

        public T Get(object id)
        {
            return DB.Get<T>(id);
        }

        public T Get(Expression<Func<T, bool>> exp)
        {
            return DB.Set<T>().FirstOrDefault(exp) ?? new T();
        }

        public IList<T> GetAll()
        {
            return DB.Set<T>().ToList();
        }

        public IList<T> GetAll(Expression<Func<T, bool>> exp)
        {
            return DB.Set<T>().Where(exp).ToList();
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await DB.Set<T>().ToListAsync();
        }

        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> exp)
        {
            return await DB.Set<T>().Where(exp).ToListAsync();
        }

        public Task<T> GetAsync(object id)
        {
            return DB.GetAsync<T>(id);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> exp)
        {
            return await DB.Set<T>().FirstOrDefaultAsync(exp) ?? new T();
        }

        public PagingModel<T> QueryPage(int page, int size, Expression<Func<T, bool>>? exp = null)
        {
            exp ??= x => true;
            var result = new PagingModel<T>();
            result.Total = DB.Set<T>().Count(exp);
            result.Rows = DB.Set<T>().Skip((page - 1) * size).Take(size).ToList();
            return result;
        }

        public PagingModel<T> QueryPage(int page, int size, string sql, DynamicParameters parameters, string? orderBy = null)
        {
            var result = new PagingModel<T>();
            result.Total = DB.Connection.ExecuteScalar<int>(DBHelper.GetCount(sql), parameters);
            result.Rows = DB.Connection.Query<T>(DBHelper.GetPaging(page, size, sql, orderBy), parameters).ToList();
            return result;
        }

        public async Task<PagingModel<T>> QueryPageAsync(int page, int size, Expression<Func<T, bool>>? exp = null)
        {
            exp ??= x => true;
            var result = new PagingModel<T>();
            result.Total = await DB.Set<T>().CountAsync(exp);
            result.Rows = await DB.Set<T>().Skip((page - 1) * size).Take(size).ToListAsync();
            return result;
        }

        public async Task<PagingModel<T>> QueryPageAsync(int page, int size, string sql, DynamicParameters parameters, string? orderBy = null)
        {
            var result = new PagingModel<T>();
            result.Total = await DB.Connection.ExecuteScalarAsync<int>(DBHelper.GetCount(sql), parameters);
            result.Rows = (await DB.Connection.QueryAsync<T>(DBHelper.GetPaging(page, size, sql, orderBy), parameters)).ToList();
            return result;
        }

        public bool Update(T model)
        {
            DB.Update(model);
            return 0 < DB.SaveChanges();
        }

		public bool Update(object id, Action<Updater<T>> action)
		{
            return DB.Edit((long)id,action);
		}

		public async Task<bool> UpdateAsync(T model)
        {
            DB.Update(model);
            return 0 < await DB.SaveChangesAsync();
        }

		public Task<bool> UpdateAsync(object id, Action<Updater<T>> action)
		{
			return DB.EditAsync((long)id, action);
		}

		public async Task<bool> UpdateManyAsync(IList<T> models)
        {
            DB.UpdateRange(models);
            return models.Count == await DB.SaveChangesAsync();
        }
    }
}