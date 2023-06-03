using Mi.Core.Factory;
using Mi.Core.GlobalUser;
using Mi.Core.Models;
using Mi.Core.Service;
using Mi.Repository.DB;
using Mi.Core.Toolkit.Extension;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Mi.Core.Extension
{
    public static class ServiceRegisterExtension
    {
        public static void AddRequiredService(this IServiceCollection service)
        {
            service.AddDbContext<MIDB>(opt =>
            {
                opt.UseSqlite(DotNetService.Get<IConfiguration>().GetConnectionString("Sqlite"))
                .EnableSensitiveDataLogging();
            });
            service.AutoInject();
            service.AddSingleton<MessageModel>();
            service.AddHttpContextAccessor();
            service.AddMemoryCache();
            service.AddSimpleCaptcha(builder =>
            {
                builder.UseMemoryStore();
                builder.AddConfiguration(opt =>
                {
                    opt.CodeLength = 4;
                    opt.ExpiryTime = TimeSpan.FromMinutes(2);
                });
            });
            service.AddScoped<IMiUser, MiUser>();
            service.AddScoped<CreatorFactory>();
            service.AddScoped<CaptchaFactory>();
        }
    }
}