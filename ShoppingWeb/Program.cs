using CoreLibrary.DbCore;
using CoreLibrary.Logging;
using log4net;
using log4net.Config;
using log4net.Util;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

DbCoreConfig.SetConfig(conf =>
{
    conf.SetDbConnection(
        connection => connection.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        );
});

var logRepository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo(Logger.LogConfigPath));

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
