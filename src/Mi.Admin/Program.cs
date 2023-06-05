using Mi.Admin.WebComponent.Filter;
using Mi.Admin.WebComponent.Middleware;
using Mi.Core.Service;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

Log.Information("Serilog Started!");

//string SerilogOutputTemplate = "{NewLine}时间:{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}日志等级:{Level}{NewLine}所在类:{SourceContext}{NewLine}日志信息:{Message}{NewLine}{Exception}";
string SerilogOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Serilog日志模板
/// </summary>
string serilogDebug = builder.Environment.WebRootPath + "\\log\\debug\\.log";
string serilogInfo = builder.Environment.WebRootPath + "\\log\\info\\.log";
string serilogWarn = builder.Environment.WebRootPath + "\\log\\warning\\.log";
string serilogError = builder.Environment.WebRootPath + "\\log\\error\\.log";
string serilogFatal = builder.Environment.WebRootPath + "\\log\\fatal\\.log";

builder.Services.AddControllersWithViews(opt =>
{
    opt.Filters.Add<GlobalExceptionFilter>();
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
    logger.WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Debug).WriteTo.Async(a => a.File(serilogDebug, rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate)))
                                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Information).WriteTo.Async(a => a.File(serilogInfo, rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate)))
                                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Warning).WriteTo.Async(a => a.File(serilogWarn, rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate)))
                                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Error).WriteTo.Async(a => a.File(serilogError, rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate)))
                                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Fatal).WriteTo.Async(a => a.File(serilogFatal, rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate)));

});
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, FuncAuthorizationMiddleware>();
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

app.UseAuthorization();
app.UseFetchUser();

app.MapControllerRoute(
    name: "Area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();