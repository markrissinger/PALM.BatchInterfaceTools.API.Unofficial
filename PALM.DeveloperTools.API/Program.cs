using Microsoft.Extensions.DependencyInjection;
using PALM.DeveloperTools.API.Helpers.Mappers;
using PALM.InterfaceLayouts.Unofficial.Entities.InterfaceLayouts.PurchaseOrders.InboundEncumbranceLoad;
using PALM.InterfaceLayouts.Unofficial.Interfaces.PurchaseOrders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Automapper
builder.Services.AddAutoMapper(cfg => MapperConfig.MapperConfiguration());


//builder.Services.AddScoped<IPOHeaderDetails, POHeaderDetails>();

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