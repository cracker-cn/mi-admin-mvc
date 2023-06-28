using Mi.Admin.WebComponent;
using Mi.Admin.WebComponent.Filter;
using Mi.Admin.WebComponent.Middleware;
using Mi.Core.Hubs;
using Mi.Core.Models.UI;
using Mi.Core.Service;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.StaticFiles;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(opt =>
{
    opt.Filters.Add<GlobalExceptionFilter>();
    opt.Filters.Add<GlobalActionFilterAttribute>();
}).AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new LongConverter());
    opt.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
});

builder.Services.AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => builder.Configuration.Bind("CookieSettings", options));

builder.Services.AddRequiredService();
//»’÷æ
builder.Host.UseSerilog((context, logger) =>
{
    var serilogOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
    var serilogPath = builder.Environment.WebRootPath + "\\exception\\.txt";
    logger.Enrich.FromLogContext();
    logger.WriteTo.Console(theme: AnsiConsoleTheme.Literate);
    logger.WriteTo.Async(a => a.File(serilogPath, restrictedToMinimumLevel: LogEventLevel.Error, rollingInterval: RollingInterval.Day, outputTemplate: serilogOutputTemplate));
});
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, FuncAuthorizationMiddleware>();

//UI≈‰÷√
var uiConfig = builder.Configuration.GetSection("AdminUI");
builder.Services.Configure<PaConfigModel>(uiConfig);
//EnvironmentHandler.cs
EnvironmentHandler.WebRootPath = builder.Environment.WebRootPath;

builder.Services.AddSignalR();
builder.Services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
.Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);
var app = builder.Build();
DotNetService.Initialization(builder.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

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