using Mi.Core.DB;
using Mi.Entity.BASE;
using Mi.IRepository.BASE;
using Mi.Repository.BASE;
using Mi.Repository.DB;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Mi.Repository.Extension
{
	public static class DBExtension
	{
		/// <summary>
		/// 单表仓储
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static IRepositoryBase<T> GetRepository<T>(this MIDB db) where T : class, new()
		{
			return new RepositoryBase<T>(db);
		}
	}
}