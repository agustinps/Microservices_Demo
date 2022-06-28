using MediatR;
using Microsoft.EntityFrameworkCore;
using ms.rabbitmq.Consumers;
using ms.rabbitmq.Middlewares;
using ms.rabbitmq.Producers;
using ms.storage.api.Consumers;
using ms.storage.application.Commands;
using ms.storage.application.Mappers;
using ms.storage.domain.Interfaces;
using ms.storage.infrastructure.Data;
using ms.storage.infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(typeof(IProducer), typeof(EventProducer));
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConn"));
});
builder.Services.AddAutoMapper(typeof(ProductMapperProfile).Assembly);
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddMediatR(typeof(CreateProductCommand).Assembly);

builder.Services.AddSingleton(typeof(IConsumer), typeof(ProductConsumer));

builder.Services.AddHostedService<UseRabbitConsumer>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
