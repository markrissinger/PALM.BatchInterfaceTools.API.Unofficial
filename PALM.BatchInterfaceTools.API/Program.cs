using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PALM.BatchInterfaceTools.API.Helpers.Handlers;
using PALM.BatchInterfaceTools.API.Helpers.Mappers;
using PALM.BatchInterfaceTools.API.Infrastructure.Data;
using PALM.BatchInterfaceTools.API.Infrastructure.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Automapper
builder.Services.AddAutoMapper(cfg => cfg.MapperConfiguration());

// Exception Handling
builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Services.AddDbContext<AzureSQLContext>(options =>
{
    var x = File.ReadAllText(@"C:\Users\18502\source\repos\pbitconnstr.txt");
#if DEBUG
    options.UseAzureSql(x);
#else
    options.UseAzureSql(builder.Configuration.GetConnectionString("AzureSQLContext"));
#endif
});
builder.Services.AddScoped<RunHistoryRepository>();

var app = builder.Build();

// Must be listed before the Swagger
app.UseCors(cfg =>
{
    cfg.AllowAnyHeader();
    cfg.AllowAnyMethod();
    cfg.AllowAnyOrigin();
    cfg.WithExposedHeaders("Content-Disposition");
});

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler(_ => { });

app.Run();