using Microsoft.Extensions.DependencyInjection;
using PALM.DeveloperTools.API.Helpers.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Automapper
builder.Services.AddAutoMapper(cfg => cfg.MapperConfiguration());


//builder.Services.AddScoped<IPOHeaderDetails, POHeaderDetails>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(cfg =>
{
    cfg.AllowAnyHeader();
    cfg.AllowAnyMethod();
    cfg.AllowAnyOrigin();
});

//app.UseAuthorization();

app.MapControllers();

app.Run();