using BlazorExampleWeb.Components;
using CoreLibrary.DbCore;
using log4net.Config;
using log4net;
using System.Reflection;

namespace BlazorExampleWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            DbCoreConfig.SetConfig(conf =>
            {
                conf.SetDbConnection(
                    connection => connection.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!
                    );
            });

            var logRepository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo(CoreLibrary.Logging.Logger.LogConfigPath));
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
