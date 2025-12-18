using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Api.Data;
using Api.Repositories;
using Api.Services;
using Api.Modules.BankuumTubo;

// Create the builder
var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Add OpenAPI/Swagger support
builder.Services.AddOpenApi();

// --- CORS configuration ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("BankuumCors", policy =>
        // set the ports to listen (e.g react)
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173").AllowAnyHeader().AllowAnyMethod()); 


    options.AddPolicy("DefaultCorsPolicy", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// --- Dependency Injection ---
// DB layer
builder.Services.AddScoped<IDbContext, DbContext>();

// Module registration 
builder.Services.BankuumTuboModule(builder.Configuration);

var app = builder.Build();

// Serve static files from wwwroot
app.UseStaticFiles();

// Use HTTPS redirection
//app.UseHttpsRedirection();

// CORS, Routing and Authorization
app.UseRouting();
app.UseCors("DefaultCorsPolicy");
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Default route redirects to static index.html
app.MapGet("/", () => Results.Redirect("/html/index.html"));

// Run the app
app.Run();

