using IChiba.Customer.Application;
using IChiba.Customer.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ichiba Customers API",
        Version = "v1",
        Description = "API for managing customers"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Đăng ký Services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// ✅ Chỉ cần gọi AddControllers() một lần
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ichiba Customer API v1");
        c.RoutePrefix = "swagger";
    });
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseCors("AllowLocalhost");
app.UseAuthorization();

// ✅ Middleware cho Dapr
app.UseCloudEvents();

// ✅ Xóa `UseEndpoints()` và dùng `MapControllers()`
app.MapControllers();

// ✅ Dùng Dapr Subscribe
app.MapSubscribeHandler();

app.Run();
