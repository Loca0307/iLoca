using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Api.Data;
using Api.Repositories;
using Api.Services;
using Api.Modules.VidaLoca;

var builder = WebApplication.CreateBuilder(args);

// Add controllers to the builder
builder.Services.AddControllers();

builder.Services.AddOpenApi();

// --- CORS configuration ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("VidaLocaCors", policy =>
        // set the ports to listen (e.g react)
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:5174").AllowAnyHeader().AllowAnyMethod()); 


    options.AddPolicy("DefaultCorsPolicy", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// --- Dependency Injection ---
// DB layer
builder.Services.AddScoped<IDbContext, DbContext>();


// Module registration 
builder.Services.VidaLocaModule(builder.Configuration);


var app = builder.Build();

// Serve static files from wwwroot

app.UseStaticFiles();

// CORS, Routing and Authorization
app.UseRouting();
app.UseCors("VidaLocaCors");
app.UseAuthorization();

// Map controllers

app.MapControllers();


// Default route redirects to static index.html
app.MapGet("/", () => Results.Redirect("/html/index.html"));


app.Run();

