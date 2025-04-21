using robot_controller_api.Persistence;
using Microsoft.OpenApi.Models;
using System.Reflection;
using robot_controller_api.Authentication;
using Microsoft.AspNetCore.Authentication; // Needed for AuthenticationSchemeOptions
using System.Security.Claims; // Needed for ClaimTypes
using Microsoft.EntityFrameworkCore; // Needed for DbContext

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Register the MVC controllers
builder.Services.AddEndpointsApiExplorer(); // Register the endpoint explorer for API documentation
builder.Services.AddScoped<RobotCommandDataAccess>(); // Register the RobotCommandDataAccess service
builder.Services.AddScoped<MapDataAccess>(); // Register the MapDataAccess service
builder.Services.AddScoped<UserDataAccess>();


// Add this line to register UserDataAccess as a DbContext
builder.Services.AddDbContext<UserDataAccess>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Remove the manual constructor override:
builder.Services.AddScoped<RobotCommandDataAccess>(); 
builder.Services.AddScoped<MapDataAccess>(); 

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(
        "BasicAuthentication", options => { });
builder.Services.AddAuthorization(options =>
{
options.AddPolicy("AdminOnly", policy =>
policy.RequireClaim(ClaimTypes.Role, "Admin"));
options.AddPolicy("UserOnly", policy =>
policy.RequireClaim(ClaimTypes.Role, "Admin", "User"));
});

//builder.Services.AddAuthorization(); // Add default authorization services

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Robot Controller API", // Title of the API
        Description = "New backend service that provides resources for the Moon robot simulator.", // Description of the API
        Contact = new OpenApiContact
        {
            Name = "Tanmay Kataria", // Contact name
            Email = "tanamy4841.be23@chitkara.edu.in", // Contact email
        },
    });

    // Enable XML comments for Swagger documentation
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // Get the XML documentation file name
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename)); // Include XML comments in Swagger
});

// Build the web application.
var app = builder.Build();

// Enables Swagger only in the development environment.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger middleware
    app.UseSwaggerUI(setup => setup.InjectStylesheet("/styles/theme-flattop.css")); // Customize Swagger UI with a stylesheet
}

app.UseStaticFiles();  // Enable serving static files (CSS, images, etc.)
app.UseRouting();

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();  // Enable authorization middleware

app.MapControllers();


// Run the application.
app.Run();
