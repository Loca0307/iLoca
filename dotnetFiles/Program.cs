using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

using Api.DBContext;
using Api.Repositories;
using Api.Services;




// App builder creation
var builder = WebApplication.CreateBuilder(args);

// register controllers
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



/*
// WeatherForecast endpoint
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        )).ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// Test endpoint
app.MapGet("/scemo", () => "Ciao scemo");

*/

app.Run();

// Models
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

