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

var app = builder.Build();

// Serve static files from wwwroot

app.UseStaticFiles();

// CORS, Routing and Authorization
app.UseRouting();
app.UseCors("DefaultCorsPolicy");
app.UseAuthorization();

// Map controllers

app.MapControllers();


// Default route redirects to static index.html
app.MapGet("/", () => Results.Redirect("/html/index.html"));


app.Run();

