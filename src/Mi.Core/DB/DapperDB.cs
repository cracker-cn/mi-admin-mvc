using Dapper;

using Mi.Core.Toolkit.Helper;

using Microsoft.Data.Sqlite;

namespace Mi.Core.DB
{
	public static class DapperDB
	{
		public static List<T> Query<T>(string sql, object? param = null)
		{
			using (var conn = new SqliteConnection(DBConfig.ConnectionString))
			{
				return conn.Query<T>(sql, param).ToList();
			}
		}

		public static T? QueryFirstOrDefault<T>(string sql, object? param = null)
		{
			using (var conn = new SqliteConnection(DBConfig.ConnectionString))
			{
				return conn.QueryFirstOrDefault<T>(sql, param);
			}
		}

		public async static Task<List<T>> QueryAsync<T>(string sql, object? param = null)
		{
			using (var conn = new SqliteConnection(DBConfig.ConnectionString))
			{
				return (await conn.QueryAsync<T>(sql, param)).ToList();
			}
		}

		public async static Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null)
		{
			using (var conn = new SqliteConnection(DBConfig.ConnectionString))
			{
				return await conn.QueryFirstOrDefaultAsync<T>(sql, param);
			}
		}

		public static bool Exist(string sql, object? param = null)
		{
			using (var conn = new SqliteConnection(DBConfig.ConnectionString))
			{
				return conn.ExecuteScalar<int>(DBHelper.GetCount(sql), param) > 0;
			}
		}

		public async static Task<bool> ExistAsync(string sql, object? param = null)
		{
			using (var conn = new SqliteConnection(DBConfig.ConnectionString))
			{
				return await conn.ExecuteScalarAsync<int>(DBHelper.GetCount(sql), param) > 0;
			}
		}

		public static bool Execute(string sql, object? param = null)
		{
			using (var conn = new SqliteConnection(DBConfig.ConnectionString))
			{
				return conn.Execute(sql, param) > 0;
			}
		}

		public async static Task<bool> ExecuteAsync(string sql, object? param = null)
		{
			using (var conn = new SqliteConnection(DBConfig.ConnectionString))
			{
				return await conn.ExecuteAsync(sql, param) > 0;
			}
		}
	}
}
