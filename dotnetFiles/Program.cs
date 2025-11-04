using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

using Api.DBContext;
using Api.Repositories;
using Api.Services;




// App builder creation
var builder = WebApplication.CreateBuilder(args);

// add controllers to the builder
builder.Services.AddControllers();

// Add services to the builder
builder.Services.AddOpenApi();

// Register DB context, repositories, services
builder.Services.AddScoped<AppDBContext>();        
builder.Services.AddScoped<ClientRepository>();    
builder.Services.AddScoped<ClientService>();         

var app = builder.Build();

// Serve static files (wwwroot/index.html)
app.UseStaticFiles();


// Enable controller routing 
app.MapControllers();

///////////////////////////
///  ROUTING MANAGING  ///
///////////////////////////


// Default route
app.MapGet("/", () => Results.Redirect("/index.html"));


// to redirect the default route to the one managed by the clientController 
//app.MapGet("/", () => Results.Redirect("/api/client"));


app.Run();

// Models
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

