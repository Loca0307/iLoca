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
// builder.Services.AddRazorPages();

// Add services to the builder
builder.Services.AddOpenApi();

// Configure CORS from configuration
// and defines which frontend URLS can call the API
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? new[] { "https://localhost:5027" };
builder.Services.AddCors(options =>
{
	options.AddPolicy("DefaultCorsPolicy", policy =>
		policy.WithOrigins(allowedOrigins)
			  .AllowAnyHeader()
			  .AllowAnyMethod());
});

// Register the layers to be used in the controllers
// DB ACCESS
builder.Services.AddScoped<IDbContext, DbContext>();

// CLIENT LAYERS
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();

// LOGIN LAYERS
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();

// TRANSACTION LAYERS
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();




var app = builder.Build();

// Serve static files (wwwroot/index.html)
app.UseStaticFiles();

// Enforce HTTPS and HSTS in non-development environments
if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
}

// Activate HTTPS site serving
app.UseHttpsRedirection();

// Add a few secure response headers
app.Use(async (context, next) =>
{
	context.Response.Headers["X-Content-Type-Options"] = "nosniff";
	context.Response.Headers["X-Frame-Options"] = "DENY";
	context.Response.Headers["Referrer-Policy"] = "no-referrer";
	context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
	// Minimal CSP - adjust for your static assets and trusted CDNs
	context.Response.Headers["Content-Security-Policy"] = "default-src 'self'";
	await next();
});

// Routing, CORS and Authorization middleware
app.UseRouting();
app.UseCors("DefaultCorsPolicy");
app.UseAuthorization();

// Enable controller routing 
app.MapControllers();

// Enable Razor Pages routing for Server Side Rendering
//app.MapRazorPages();


// DEFAULT ROUTE - redirect to static HTML "index.html" from the "wwwroot/html" folder
app.MapGet("/", () => Results.Redirect("/html/index.html"));


app.Run();


