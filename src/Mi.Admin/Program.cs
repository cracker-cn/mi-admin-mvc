using Mi.Admin.WebComponent;
using Mi.Admin.WebComponent.Filter;
using Mi.Admin.WebComponent.Middleware;
using Mi.Core.DB;
using Mi.Core.Hubs;
using Mi.Core.Models.UI;
using Mi.Core.Service;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Serilog日志模板
/// </summary>
string serilogOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
string serilogPath = builder.Environment.WebRootPath + "\\log\\.txt";

builder.Services.AddControllersWithViews(opt =>
{
	opt.Filters.Add<GlobalExceptionFilter>();
	opt.Filters.Add<ParameterValidationFilterAttribute>();
}).AddJsonOptions(opt =>
{
	opt.JsonSerializerOptions.Converters.Add(new LongConverter());
	opt.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
});

builder.Services.AddAuthentication()
	.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => builder.Configuration.Bind("CookieSettings", options));

builder.Services.AddRequiredService();
//日志
builder.Host.UseSerilog((context, logger) =>
{
	logger.Enrich.FromLogContext();
	logger.WriteTo.Console(theme: AnsiConsoleTheme.Literate);
	logger.WriteTo.Async(a => a.File(serilogPath, rollingInterval: RollingInterval.Day, outputTemplate: serilogOutputTemplate));
});
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, FuncAuthorizationMiddleware>();

//UI配置
var uiConfig = builder.Configuration.GetSection("AdminUI");
builder.Services.Configure<PaConfigModel>(uiConfig);

//EnvironmentHandler.cs
builder.Services.Configure<EnvironmentHandler>(x =>
{
	x.WebRootPath = builder.Environment.WebRootPath;
});

builder.Services.AddSignalR();
var app = builder.Build();
DotNetService.Initialization(builder.Services);

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();

//var defaultFilesOptions = new DefaultFilesOptions();
//defaultFilesOptions.DefaultFileNames.Clear();
//defaultFilesOptions.DefaultFileNames.Add("index.html");
//app.UseDefaultFiles(defaultFilesOptions);

var contentTypeProvider = new FileExtensionContentTypeProvider();
contentTypeProvider.Mappings.Add(".less", "text/css");
contentTypeProvider.Mappings.Add(".yml", "text/html");
var options = new StaticFileOptions()
{
	ServeUnknownFileTypes = true,
	ContentTypeProvider = contentTypeProvider
};
app.UseStaticFiles(options);

app.UseRouting();

app.UseAuthentication();
app.AddCustomerMiddleware();
app.UseAuthorization();

app.MapControllerRoute(
	name: "Area",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<NoticeHub>("/noticeHub");
app.Run();