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
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

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
// IMoviesRepository is registered with the MoviesRepository class as the implementation
// This approach allows you to replace the implementation of an interface in the DI container without changing
// the dependent code, promoting loose coupling and flexibility.
builder.Services.AddScoped<IMoviesRepository, MoviesRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
builder.Services.AddScoped<IErrorsRepository, ErrorsRepository>();

// AddTransient creates a new instance of the service each time it is requested
builder.Services.AddTransient<IFileStorage, LocalFileStorage>(); // Register the InAppStorage class as the implementation for the IFileStorage interface
builder.Services.AddHttpContextAccessor(); // So IFileStorage can access the HttpContext via injection


builder.Services.AddAutoMapper(typeof(Program)); // Add AutoMapper to the services collection and look for conigurations automatically

// AddValidatorsFromAssemblyContaining will scan the assembly containing the Program class for classes
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Allows us to customize the response of the application when an exception is thrown
builder.Services.AddProblemDetails();

// Services Zone - END

var app = builder.Build();

// Middleware Zone - BEGIN

// Enable middleware to serve generated Swagger as a JSON endpoint
app.UseSwagger();
app.UseSwaggerUI();

// Enable exception handling middleware, this goes along with the AddProblemDetails service
app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.Run(async context =>
{
    // Log Error to DB
    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
    var exception = exceptionHandlerFeature?.Error!;

    var error = new Error
    {
        ErrorMessage = exception.Message,
        StackTrace = exception.StackTrace,
        Date = DateTime.Now
    };

    // RequestServices is a property of HttpContext that provides access to the service container
    var repository = context.RequestServices.GetRequiredService<IErrorsRepository>();
    await repository.Create(error);

    await Results.BadRequest(new {type= "error", message = "an unexpected exception has occurred", status=500 }).ExecuteAsync(context);
})); 
app.UseStatusCodePages(); // Enable status code pages middleware, this will return a page with the status code

app.UseStaticFiles(); // Enable static files to be served

// Apply CORS before any other middleware so that it will be available to the endpoints defined below
app.UseCors();

// Apply output caching
app.UseOutputCache();

// Define the endpoints

app.MapGet("/", () => "Hello World!"); // No caching for this endpoint
app.MapGet("/error", () =>
{
    throw new Exception("This is an example exception");
});

// MapGroup: Define the genres endpoints, this is a group of endpoints that all start with /genres, and share the same base path
// this makes it easier to manage the endpoints and keep them organized
// MapGenres: This is an extension method that maps the endpoints for the Genres resource
// defines in a single place all the endpoints for the Genres resource
// Also Swagger will separate the endpoints in the documentation
app.MapGroup("/genres").MapGenres();
app.MapGroup("/actors").MapActors();
app.MapGroup("/movies").MapMovies();
// movieId will be passed as a parameter to the comments endpoints
app.MapGroup("/movie/{movieId:int}/comments").MapComments();


// Middleware Zone - END

app.Run();


