using Auth.Service.Endpoints;
using Auth.Service.Infrastructure.Data.EntityFramework;
using Auth.Service.Services;
using ECommerce.Shared.Observability;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSqlServerDatastore(builder.Configuration);
builder.Services.RegisterTokenService(builder.Configuration);

Log.Logger = SerilogConfigStartupExtension.AddLogger("auth");
builder.Host.UseSerilog();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MigrateDatabase();
}

app.RegisterEndpoints();
app.UseHttpsRedirection();

await app.RunAsync();
