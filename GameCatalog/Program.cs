using Microsoft.EntityFrameworkCore;
using GameCatalog.Data;
using GameCatalog.Controllers;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<GameCatalogContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GameCatalogContext") ?? throw new InvalidOperationException("Connection string 'GameCatalogContext' not found.")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Game Catalog API");
        options.RoutePrefix = string.Empty; // Swagger UI at the app's root
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
