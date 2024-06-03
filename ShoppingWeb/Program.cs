using CoreLibrary;
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
LogLog.InternalDebugging = true;
try
{
    var logRepository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
    XmlConfigurator.Configure(logRepository, new FileInfo(Logger.LogConfigPath));
    Console.WriteLine("log4net configuration loaded successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error configuring log4net: {ex.Message}");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
