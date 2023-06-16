using System.Linq.Expressions;

using Dapper;

using Mi.Entity.BASE;
using Mi.IRepository.BASE;

using Microsoft.EntityFrameworkCore;

namespace Mi.Repository.Extension
{
	public static class DBExtension
	{
		/// <summary>
		/// 单表仓储
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static IRepositoryBase<T> GetRepository<T>(this MIDB db) where T : EntityBase, new()
		{
			return new RepositoryBase<T>(db);
		}

		public static PagingModel<T> QueryPage<T>(this MIDB db, int page, int size, Expression<Func<T, bool>>? exp = null) where T : class, new()
		{
			exp ??= x => true;
			var result = new PagingModel<T>();
			result.Total = db.Set<T>().Count(exp);
			result.Rows = db.Set<T>().Skip((page - 1) * size).Take(size).ToList();
			return result;
		}

		public static PagingModel<T> QueryPage<T>(this MIDB db, int page, int size, string sql, DynamicParameters parameters, string? orderBy = null) where T : class, new()
		{
			var result = new PagingModel<T>();
			result.Total = db.Connection.ExecuteScalar<int>(DBHelper.GetCount(sql), parameters);
			result.Rows = db.Connection.Query<T>(DBHelper.GetPaging(page, size, sql, orderBy), parameters).ToList();
			return result;
		}

		public static async Task<PagingModel<T>> QueryPageAsync<T>(this MIDB db, int page, int size, Expression<Func<T, bool>>? exp = null) where T : class, new()
		{
			exp ??= x => true;
			var result = new PagingModel<T>();
			result.Total = await db.Set<T>().CountAsync(exp);
			result.Rows = await db.Set<T>().Where(exp).Skip((page - 1) * size).Take(size).ToListAsync();
			return result;
		}

		public static async Task<PagingModel<T>> QueryPageAsync<T, TKey>(this MIDB db, int page, int size,Expression<Func<T, bool>>? exp = null, bool asc = true, params Expression<Func<T, TKey>>[] keySelectors) where T : class, new()
		{
			exp ??= x => true;
			var result = new PagingModel<T>();
			result.Total = await db.Set<T>().CountAsync(exp);
			if(asc) result.Rows = await db.Set<T>().Where(exp).OrderBy(keySelectors[0]).Skip((page - 1) * size).Take(size).ToListAsync();
			else result.Rows = await db.Set<T>().Where(exp).OrderByDescending(keySelectors[0]).Skip((page - 1) * size).Take(size).ToListAsync();
			return result;
		}

		public static async Task<PagingModel<T>> QueryPageAsync<T>(this MIDB db, int page, int size, string sql, DynamicParameters parameters, string? orderBy = null) where T : class, new()
		{
			var result = new PagingModel<T>();
			result.Total = await db.Connection.ExecuteScalarAsync<int>(DBHelper.GetCount(sql), parameters);
			result.Rows = (await db.Connection.QueryAsync<T>(DBHelper.GetPaging(page, size, sql, orderBy), parameters)).ToList();
			return result;
		}
	}
}