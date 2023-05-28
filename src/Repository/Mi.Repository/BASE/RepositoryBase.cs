using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

using Dapper;

using Mi.Core.DB;
using Mi.Entity.BASE;
using Mi.Entity.Field;
using Mi.IRepository.BASE;
using Mi.Repository.Extension;

using Microsoft.EntityFrameworkCore;

namespace Mi.Repository.BASE
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase, new()
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
            return DB.QueryPage<T>(page, size, exp);
        }

        public PagingModel<T> QueryPage(int page, int size, string sql, DynamicParameters parameters, string? orderBy = null)
        {
            return DB.QueryPage<T>(page, size, sql, parameters, orderBy);
        }

        public Task<PagingModel<T>> QueryPageAsync(int page, int size, Expression<Func<T, bool>>? exp = null)
        {
            return DB.QueryPageAsync<T>(page, size, exp);
        }

        public Task<PagingModel<T>> QueryPageAsync(int page, int size, string sql, DynamicParameters parameters, string? orderBy = null)
        {
            return DB.QueryPageAsync<T>(page, size, sql, parameters, orderBy);
        }

        public bool Update(T model)
        {
            DB.Update(model);
            return 0 < DB.SaveChanges();
        }

        public bool Update(object id, Action<Updater<T>> action)
        {
            return DB.Edit((long)id, action);
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