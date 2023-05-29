using Mi.Core.GlobalUser;
using Mi.Core.Models;
using Mi.Core.Service;
using Mi.Repository.DB;
using Mi.Toolkit.Extension;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mi.Core.Extension
{
    public static class ServiceRegisterExtension
    {
        public static void AddRequiredService(this IServiceCollection service)
        {
            service.AddDbContext<MIDB>(opt =>
            {
                opt.UseSqlite(DotNetService.Get<IConfiguration>().GetConnectionString("Sqlite")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            service.AutoInject();
            service.AddSingleton<MessageModel>();
            service.AddHttpContextAccessor();
            service.AddMemoryCache();
            service.AddScoped<IMiUser,MiUser>();
        }
    }
}