using System.Data;

using Mi.Core.DB;
using Mi.Entity.BASE;

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

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Data Source=D:\\github\\mi-admin-mvc\\db\\mi.db");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region System

            modelBuilder.Entity<SysDict>().HasQueryFilter(x => x.IsDeleted == 0);
            modelBuilder.Entity<SysFunction>().HasQueryFilter(x => x.IsDeleted == 0);
            modelBuilder.Entity<SysRole>().HasQueryFilter(x => x.IsDeleted == 0);
            modelBuilder.Entity<SysRoleFunction>().HasQueryFilter(x => x.IsDeleted == 0);
            modelBuilder.Entity<SysUser>().HasQueryFilter(x => x.IsDeleted == 0);
            modelBuilder.Entity<SysUserRole>().HasQueryFilter(x => x.IsDeleted == 0);
            modelBuilder.Entity<SysMessage>().HasQueryFilter(x => x.IsDeleted == 0);

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

        public bool Edit<T>(long id, Action<Updater<T>> lambda) where T : EntityBase, new()
        {
            SetNoTracking<T>();
            using (var updator = new Updater<T>())
            {
                lambda(updator);
                var keypair = updator.Explain();
                var value = keypair.Value;
                value.Id = id;

                var entry = base.Entry(value);
                entry.State = EntityState.Unchanged;

                foreach (var item in keypair.Key)
                {
                    if (nameof(value.Id) == item) continue;
                    entry.Property(item).IsModified = true;
                }

                return 0 < base.SaveChanges();
            }
        }

        public async Task<bool> EditAsync<T>(long id, Action<Updater<T>> lambda) where T : EntityBase, new()
        {
            SetNoTracking<T>();
            using (var updator = new Updater<T>())
            {
                lambda(updator);
                var keypair = updator.Explain();
                var value = keypair.Value;
                value.Id = id;

                var entry = base.Entry(value);
                entry.State = EntityState.Unchanged;

                foreach (var item in keypair.Key)
                {
                    if (nameof(value.Id) == item) continue;
                    entry.Property(item).IsModified = true;
                }

                return 0 < await base.SaveChangesAsync();
            }
        }

        public void SetNoTracking<T>() where T : EntityBase, new()
        {
            var ens = base.ChangeTracker.Entries<T>();
            foreach (var item in ens)
            {
                item.State = EntityState.Detached;
            }
        }

        public T Get<T>(object id) where T : EntityBase, new()
        {
            return base.Set<T>().Find((long)id) ?? new T();
        }

        public async Task<T> GetAsync<T>(object id) where T : EntityBase, new()
        {
            return await base.Set<T>().FindAsync((long)id) ?? new T();
        }
    }
}