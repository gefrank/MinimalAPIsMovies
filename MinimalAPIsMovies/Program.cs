// allows us to configure our application and define our endpoints
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies;
using MinimalAPIsMovies.Endpoints;
using MinimalAPIsMovies.Entities;
using MinimalAPIsMovies.Migrations;
using MinimalAPIsMovies.Repositories;
using MinimalAPIsMovies.Services;
using System.Runtime.CompilerServices;


var builder = WebApplication.CreateBuilder(args);

// Services Zone - BEGIN

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add services to the container, so that they can be used in the application
// CORS (Cross-Origin Resource Sharing) is a security mechanism that controls how web
// browsers handle requests from one domain (origin) to another.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configuration =>
    {
        configuration.WithOrigins(builder.Configuration["AllowedOrigins"]!)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });

    options.AddPolicy("free", configuration =>
    {
        configuration.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });

});

// Activate output caching
builder.Services.AddOutputCache();

// Set up the OpenAPI/Swagger generator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Always reference the interface and not the implementation
// This is an example of the dependency injection pattern, depend on abstractions, not on concretions 

// This line registers the GenresRepository class as the implementation for the IGenresRepository interface with a scoped lifetime.
// This means that for each HTTP request, a new instance of GenresRepository will be created and used for the duration of that request.
// This is particularly useful for services that interact with a database context, as it ensures that each request gets its own instance
// of the repository, avoiding potential issues with shared state across requests.
// AddScoped creates a new instance of the repository for each request
builder.Services.AddScoped<IGenresRepository, GenresRepository>(); 
builder.Services.AddScoped<IActorsRepository, ActorsRepository>();

// AddTransient creates a new instance of the service each time it is requested
builder.Services.AddTransient<IFileStorage, LocalFileStorage>(); // Register the InAppStorage class as the implementation for the IFileStorage interface
builder.Services.AddHttpContextAccessor(); // So IFileStorage can access the HttpContext via injection


builder.Services.AddAutoMapper(typeof(Program)); // Add AutoMapper to the services collection and look for conigurations automatically

// Services Zone - END

var app = builder.Build();

// Middleware Zone - BEGIN

// Enable middleware to serve generated Swagger as a JSON endpoint
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles(); // Enable static files to be served

// Apply CORS before any other middleware so that it will be available to the endpoints defined below
app.UseCors();

// Apply output caching
app.UseOutputCache();

// Define the endpoints

app.MapGet("/", () => "Hello World!"); // No caching for this endpoint

// MapGroup: Define the genres endpoints, this is a group of endpoints that all start with /genres, and share the same base path
// this makes it easier to manage the endpoints and keep them organized
// MapGenres: This is an extension method that maps the endpoints for the Genres resource
// defines in a single place all the endpoints for the Genres resource
// Also Swagger will separate the endpoints in the documentation
app.MapGroup("/genres").MapGenres();
app.MapGroup("/actors").MapActors();


// Middleware Zone - END

app.Run();


