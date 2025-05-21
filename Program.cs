using JobTracker.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

const string FrontendOrigin =
    "https://jobtrackerclient-bxd7bvb0fhe7aag4.northeurope-01.azurewebsites.net";

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowFrontend", p => p
    .WithOrigins(
        "https://jobtrackerclient-bxd7bvb0fhe7aag4.northeurope-01.azurewebsites.net",
        "https://localhost:5002", // lägg till denna rad
        "http://localhost:5002"   // och denna också för HTTP fallback
    )
    .AllowAnyHeader()
    .AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();  
app.UseAuthentication();    
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "JobTracker API v1");
    c.RoutePrefix = string.Empty;
});

app.Use(async (ctx, next) =>
{
    if (ctx.Request.Headers.TryGetValue("Origin", out var origin))
        app.Logger.LogInformation("Origin: {Origin}", origin);
    await next();
});

app.MapControllers();
app.Run();
