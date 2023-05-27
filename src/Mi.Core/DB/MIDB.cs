using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;

using Mi.Core.DB;

using Microsoft.EntityFrameworkCore;

namespace Mi.Repository.DB
{
	public class MIDB : DbContext
	{
		public MIDB()
		{ }

		public MIDB(DbContextOptions<MIDB> contextOptions) : base(contextOptions)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source=D:\\github\\mi-admin-mvc\\db\\mi.db");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			#region System

			modelBuilder.Entity<SysDict>();
			modelBuilder.Entity<SysFunction>();
			modelBuilder.Entity<SysFunctionGroup>();
			modelBuilder.Entity<SysRole>();
			modelBuilder.Entity<SysRoleFunction>();
			modelBuilder.Entity<SysUser>();
			modelBuilder.Entity<SysUserRole>();

			#endregion System
		}

		/// <summary>
		/// Db连接对象
		/// </summary>
		public IDbConnection Connection
		{
			get
			{
				return base.Database.GetDbConnection();
			}
		}

		public bool Edit<T>(long id, Action<Updater<T>> lambda) where T : class, new()
		{
			var prop = typeof(T).GetProperties().FirstOrDefault(x => x.GetCustomAttribute<KeyAttribute>() != null);
			var model = base.Set<T>().Find(Convert.ChangeType(id, prop!.PropertyType));
			if (model == null)
				return false;

			using (var updator = new Updater<T>())
			{
				lambda(updator);
				var keypair = updator.Explain();
				var value = keypair.Value;

				var entry = base.Entry(model);
				entry.State = EntityState.Unchanged;

				foreach (var item in keypair.Key)
				{
					if (prop.Name == item.ToString()) continue;
					entry.Property(item.ToString()).IsModified = true;
				}

				return 0 < base.SaveChanges();
			}
		}

		public async Task<bool> EditAsync<T>(long id, Action<Updater<T>> lambda) where T : class, new()
		{
			var prop = typeof(T).GetProperties().FirstOrDefault(x => x.GetCustomAttribute<KeyAttribute>() != null);
			var model = await base.Set<T>().FindAsync(Convert.ChangeType(id, prop!.PropertyType));
			if (model == null)
				return false;

			using (var updator = new Updater<T>())
			{
				lambda(updator);
				var keypair = updator.Explain();
				var value = keypair.Value;

				var entry = base.Entry(model);
				entry.State = EntityState.Unchanged;

				foreach (var item in keypair.Key)
				{
					if (prop.Name == item.ToString()) continue;
					entry.Property(item.ToString()).IsModified = true;
				}

				return 0 < await base.SaveChangesAsync();
			}
		}

		public T Get<T>(object id) where T : class, new()
		{
			var prop = typeof(T).GetProperties().FirstOrDefault(x => x.GetCustomAttribute<KeyAttribute>() != null);
			return base.Set<T>().Find(Convert.ChangeType(id, prop!.PropertyType)) ?? new T();
		}

		public async Task<T> GetAsync<T>(object id) where T : class, new()
		{
			var prop = typeof(T).GetProperties().FirstOrDefault(x => x.GetCustomAttribute<KeyAttribute>() != null);
			return await base.FindAsync<T>(Convert.ChangeType(id, prop!.PropertyType)) ?? new T();
		}
	}
}