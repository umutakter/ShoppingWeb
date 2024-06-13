using CoreLibrary.Authorization.SecretKeySettings;
using CoreLibrary.DbCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
DbCoreConfig.SetConfig(conf =>
{
    conf.SetDbConnection(
        connection => connection.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!
        );
});
var secretKey = builder.Configuration["SecretKeys:SecretKey"];
AuthCoreConfig.SetConfig(conf =>
{
    conf.SetSecretKey(secretKeyConfig =>
    {
        secretKeyConfig.SecretKeys = builder.Configuration["SecretKeys:SecretKey"];
    });
});
var s2 = BaseAuthSettings.SecretKey;
CoreLibrary.MetotAnaliz.MethodAnalysis.Analyze();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
