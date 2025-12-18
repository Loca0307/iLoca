using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Api.Data;
using Api.Repositories;
using Api.Services;

// Create the builder
var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Add OpenAPI/Swagger support
builder.Services.AddOpenApi();

// --- CORS configuration ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
        policy.WithOrigins("http://localhost:5173") // set the ports to listen to (e.g. react)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// --- Dependency Injection ---
// DB layer
builder.Services.AddScoped<IDbContext, DbContext>();

// Client layer
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();

// Login layer
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();

// Transaction layer
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

// Build the app
var app = builder.Build();

// Serve static files from wwwroot
app.UseStaticFiles();

// Use HTTPS redirection
app.UseHttpsRedirection();

//CORS
app.UseRouting();                // Sets up routing for the request
app.UseCors("DefaultCorsPolicy"); // Applies the CORS policy
app.UseAuthorization();           // Applies authorization middleware

// Map controllers
app.MapControllers();

// Default route redirects to static index.html
app.MapGet("/", () => Results.Redirect("/html/index.html"));

// Run the app
app.Run();

