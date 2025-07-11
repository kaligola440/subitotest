using Microsoft.EntityFrameworkCore;
using PurchaseCartService.Data;
using PurchaseCartService.Services;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// 1. Percorso dinamico e cross-platform del file cart.db
var dataPath = Path.Combine(builder.Environment.ContentRootPath, "data");
Directory.CreateDirectory(dataPath); // crea la cartella se manca
var dbPath = Path.Combine(dataPath, "cart.db");

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite($"Data Source={dbPath}"));

// 2. Registriamo OrderService
builder.Services.AddScoped<OrderService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.MapControllers();

// Log utile per debug path del DB
app.Logger.LogInformation("Usando database SQLite: {Path}", dbPath);

app.Run("http://0.0.0.0:9090");
