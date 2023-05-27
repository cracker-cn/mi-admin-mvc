using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mi.IRepository.BASE;
using Mi.Repository.BASE;
using Mi.Repository.DB;

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
