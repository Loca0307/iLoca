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

// Add Razor Pages support for Server Side Rendering
//builder.Services.AddRazorPages();

// Add services to the builder
builder.Services.AddOpenApi();

// For now, register all layers here
builder.Services.AddScoped<IDbContext, DbContext>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();

var app = builder.Build();

// Serve static files (wwwroot/index.html)
app.UseStaticFiles();


// Enable controller routing 
app.MapControllers();

// Enable Razor Pages routing for Server Side Rendering
//app.MapRazorPages();


// DEFAULT ROUTE - redirect to static HTML "index.html" from the "wwwroot/html" folder
app.MapGet("/", () => Results.Redirect("/html/index.html"));


app.Run();


