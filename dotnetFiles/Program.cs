using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

using Api.Data;
using Api.Repositories;
using Api.Services;




// App builder creation
var builder = WebApplication.CreateBuilder(args);

// add controllers to the builder
builder.Services.AddControllers();

// Add services to the builder
builder.Services.AddOpenApi();

// Register DB context, repositories, services
builder.Services.AddScoped<DbContext>();        
builder.Services.AddScoped<ClientRepository>();    
builder.Services.AddScoped<ClientService>();         

var app = builder.Build();

// Serve static files (wwwroot/index.html)
app.UseStaticFiles();


// Enable controller routing 
app.MapControllers();


// DEFAULT ROUTE
app.MapGet("/", () => Results.Redirect("/index.html"));


app.Run();


