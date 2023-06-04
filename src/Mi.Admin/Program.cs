using Mi.Admin.WebComponent.Filter;
using Mi.Admin.WebComponent.Middleware;
using Mi.Core.Service;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.StaticFiles;

using Serilog;
using Serilog.Settings.Configuration;
using Serilog.Sinks.SystemConsole.Themes;

Log.Information("Serilog Started!");

var builder = WebApplication.CreateBuilder(args);

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
//ÈÕÖ¾
builder.Host.UseSerilog((context, logger) =>
{
    logger.Enrich.FromLogContext();
    logger.WriteTo.Console(theme: AnsiConsoleTheme.Literate);
    logger.ReadFrom.Configuration(builder.Configuration, new ConfigurationReaderOptions { SectionName = "Logs" });
});
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