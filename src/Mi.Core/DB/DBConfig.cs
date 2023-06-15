using Mi.Core.Service;

using Microsoft.Extensions.Configuration;

namespace Mi.Core.DB
{
	public static class DBConfig
	{
		public readonly static string ConnectionString = DotNetService.Get<IConfiguration>().GetConnectionString("Sqlite") ?? "";
	}
}
