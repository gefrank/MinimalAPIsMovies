using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Entities;
using MinimalAPIsMovies.Filters;
using MinimalAPIsMovies.Repositories;
using System.Runtime.CompilerServices;

namespace MinimalAPIsMovies.Endpoints
{

    /// <summary>
    /// This class contains the endpoints for the Genres resource.
    /// </summary>
    public static class GenresEndpoints
    {
        // this is an extension method that is used to map the endpoints to the GenresEndpoints class
        public static RouteGroupBuilder MapGenres(this RouteGroupBuilder group) 
        {
            group.MapGet("/", GetGenres)
                .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)).Tag("genres-get")); // Tells this endpoint to cache the response for 15 seconds, and tag it with "genres-get" for easy eviction
                //.RequireAuthorization(); 
            // TestFilter is a custom filter that we created to test the endpoint filters
            //group.MapGet("/{id:int}", GetById).AddEndpointFilter<TestFilter>();
            group.MapGet("/{id:int}", GetById);

            group.MapPost("/", Create).AddEndpointFilter<ValidationFilter<CreateGenreDTO>>().RequireAuthorization("isadmin");
            group.MapPut("/{id:int}", Update).AddEndpointFilter<ValidationFilter<CreateGenreDTO>>().RequireAuthorization("isadmin");
            group.MapDelete("/{id:int}", Delete).RequireAuthorization("isadmin");
            return group;   
        }

        static async Task<Ok<List<GenreDTO>>> GetGenres(IGenresRepository repository, IMapper mapper, ILoggerFactory loggerFactory)
        {
            var type = typeof(GenresEndpoints);
            var logger = loggerFactory.CreateLogger(type.FullName!);
            logger.LogTrace("This is a trace message");
            logger.LogInformation("Getting the list of genres");

            var genres = await repository.GetAll();
            // AutoMapper is a library that simplifies the mapping of objects from one type to another
            var genresDTO = mapper.Map<List<GenreDTO>>(genres);
            // TypedResults is a helper class that simplifies the return of typed results, and work better with swagger
            return TypedResults.Ok(genresDTO);
        }

        /// Results can either be Ok with the Genre or NotFound
        static async Task<Results<Ok<GenreDTO>, NotFound>> GetById(int id, IGenresRepository repository, IMapper mapper)
        {
            var genre = await repository.GetById(id);
            if (genre is null)
            {
                return TypedResults.NotFound();
            }

            var genreDTO = mapper.Map<GenreDTO>(genre);

            return TypedResults.Ok(genreDTO);
        }

        static async Task<Created<GenreDTO>> Create(CreateGenreDTO createGenreDTO,
            IGenresRepository repository,
            IOutputCacheStore outputCacheStore, 
            IMapper mapper)
        {
            var genre = mapper.Map<Genre>(createGenreDTO);

            var id = await repository.Create(genre);
            await outputCacheStore.EvictByTagAsync("genres-get", default); // Evict the cache for the "genres-get" tag, forcing the next request to re-fetch the data

            var genreDTO = mapper.Map<GenreDTO>(genre);

            return TypedResults.Created($"/genres/{id}", genreDTO);
        }

        static async Task<Results<NotFound, NoContent>> Update(int id, CreateGenreDTO createGenreDTO, IGenresRepository repository,
            IOutputCacheStore outputCacheStore, 
            IMapper mapper)
        {
            // Other way to do this, but you need to inject the validator in the endpoint
            //var validationResult = await validator.ValidateAsync(createGenreDTO);
            //if (!validationResult.IsValid)
            //{
            //    return TypedResults.ValidationProblem(validationResult.ToDictionary());
            //};

            var exists = await repository.Exists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

            var genre = mapper.Map<Genre>(createGenreDTO);
            genre.Id = id;

            await repository.Update(genre);
            await outputCacheStore.EvictByTagAsync("genres-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent>> Delete(int id, IGenresRepository repository,
            IOutputCacheStore outputCacheStore)
        {
            var exists = await repository.Exists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }
            await repository.Delete(id);
            await outputCacheStore.EvictByTagAsync("genres-get", default);
            return TypedResults.NoContent();
        }

    }
}
