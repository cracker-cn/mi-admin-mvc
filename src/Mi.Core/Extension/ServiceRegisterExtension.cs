using Mi.Core.DB;
using Mi.Core.Factory;
using Mi.Core.GlobalUser;
using Mi.Core.Models;
using Mi.Core.Service;
using Mi.Core.Toolkit.Extension;
using Mi.Repository.DB;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

namespace Mi.Core.Extension
{
	public static class ServiceRegisterExtension
	{
		public static void AddRequiredService(this IServiceCollection service)
		{
			service.AddDbContext<MIDB>(opt =>
			{
				opt.UseSqlite(DBConfig.ConnectionString)
				.EnableSensitiveDataLogging();
			}, ServiceLifetime.Scoped);
			service.AutoInject();
			service.AddSingleton<MessageModel>();
			service.AddHttpContextAccessor();
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
			//header
			service.AddTransient<MiHeader>(p => {
				var httpContext = p.GetRequiredService<IHttpContextAccessor>().HttpContext!;
				if(httpContext.Items.TryGetValue(MiHeader.MIHEADER, out var str))
				{
                    return JsonConvert.DeserializeObject<MiHeader>((string)str) ?? new MiHeader();
                }
				return new MiHeader();
            });
			service.AddScoped<CreatorFactory>();
			service.AddScoped<CaptchaFactory>();
			service.AddScoped<MemoryCacheFactory>();
			//==HostService==
			service.AddHostedService<SeedDataBackgroundService>();
		}
	}
}