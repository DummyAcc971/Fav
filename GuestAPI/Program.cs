using GuestAPI.Repositories;
using GuestAPI.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add Controllers
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache(); // Fix for DistributedSessionStore
builder.Services.AddSession(); // Enables session tracking for guest users
builder.Services.AddHttpContextAccessor(); // Fix for GuestStockRepository

// ✅ Register services & repositories for dependency injection
builder.Services.AddScoped<IGuestStockService, GuestStockService>();
builder.Services.AddScoped<IGuestStockRepository, GuestStockRepository>();
builder.Services.AddScoped<IGuestStockSymbolRepository, GuestStockSymbolRepository>();
builder.Services.AddScoped<IGuestStockSymbolService, GuestStockSymbolService>();

builder.Services.AddScoped<IGuestStockGraphRepository, GuestStockGraphRepository>();
builder.Services.AddScoped<IGuestStockGraphService, GuestStockGraphService>();
builder.Services.AddHttpClient<GuestStockGraphRepository>();


// ✅ Load API key from configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// ✅ Enable CORS for Angular & other frontends
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// ✅ Enable Swagger documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GuestAPI",
        Version = "v1",
        Description = "API for fetching stock data for guest users"
    });
});

var app = builder.Build();

// ✅ Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GuestAPI v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseRouting();
app.UseSession(); // Enables session tracking for guest search limits
app.UseAuthorization();
app.MapControllers();
app.Run();
