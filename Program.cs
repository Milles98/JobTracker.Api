using JobTracker.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Flytta CORS högst upp
app.UseCors("AllowFrontend");

// Logga headers för felsökning
app.Use(async (ctx, next) =>
{
    if (ctx.Request.Method == "OPTIONS")
    {
        ctx.Response.Headers.Add("X-Debug-Message", "Handling OPTIONS request");
        app.Logger.LogInformation("Handling OPTIONS request from {Origin}", ctx.Request.Headers["Origin"]);
    }
    app.Logger.LogInformation("Response Headers: {Headers}", ctx.Response.Headers);
    await next();
});


app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "JobTracker API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();