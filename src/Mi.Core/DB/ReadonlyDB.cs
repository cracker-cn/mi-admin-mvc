using Dapper;

using Microsoft.Data.Sqlite;

namespace Mi.Core.DB
{
	public static class ReadonlyDB
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
	}
}
