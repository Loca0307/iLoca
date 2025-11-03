using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();

var app = builder.Build();

// Serve static files (wwwroot/index.html)
app.UseStaticFiles();

// OpenAPI (only in development)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

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

// Default route
app.MapGet("/", () => Results.Redirect("/index.html"));

// Loca: GO TO THIS SUB-URL TO SEND THE DB REQUEST
// PostgreSQL endpoint 
app.MapGet("/clients", () =>
{
    string host = "bankuumtubo-bankuumtubo.f.aivencloud.com";
    int port = 28017;
    string database = "bankuumdb";
    string username = "avnadmin";
    string password = "AVNS_u-FjY5aszGHtMkGPQmY";

    string connString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;";

    var clients = new List<Client>();

    try
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();

        // Insert a client (avoid duplicates with ON CONFLICT)
        using (var cmd = new NpgsqlCommand(
            @"INSERT INTO clients (first_name, last_name, email, phone)
              VALUES (@first, @last, @email, @phone)
              ON CONFLICT (email) DO NOTHING", conn))
        {
            cmd.Parameters.AddWithValue("first", "Federico");
            cmd.Parameters.AddWithValue("last", "Bonezzi");
            cmd.Parameters.AddWithValue("email", "bonni.smith@example.com");
            cmd.Parameters.AddWithValue("phone", "+41 79 123 45 67");

            cmd.ExecuteNonQuery();
        }

        // Select all clients
        using var cmdSelect = new NpgsqlCommand(
            "SELECT client_id, first_name, last_name, email, phone FROM clients", conn);
        using var reader = cmdSelect.ExecuteReader();
        while (reader.Read())
        {
            clients.Add(new Client
            {
                ClientId = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Email = reader.GetString(3),
                Phone = reader.GetString(4)
            });
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("PostgreSQL error: " + ex.Message);
    }

    return clients;
});

app.Run();

// Models
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

class Client
{
    public int ClientId { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
}
