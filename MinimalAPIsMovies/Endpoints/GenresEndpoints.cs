using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.Entities;
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
            group.MapGet("/{id:int}", GetById);
            group.MapPost("/", Create);
            group.MapPut("/{id:int}", Update);
            group.MapDelete("/{id:int}", Delete);
            return group;   
        }

        static async Task<Ok<List<Genre>>> GetGenres(IGenresRepository repository)
        {
            var genres = await repository.GetAll();
            // TypedResults is a helper class that simplifies the return of typed results, and work better with swagger
            return TypedResults.Ok(genres);
        }

        /// Results can either be Ok with the Genre or NotFound
        static async Task<Results<Ok<Genre>, NotFound>> GetById(int id, IGenresRepository repository)
        {
            var genre = await repository.GetById(id);
            if (genre is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(genre);
        }

        static async Task<Created<Genre>> Create(Genre genre, IGenresRepository repository,
            IOutputCacheStore outputCacheStore)
        {
            var id = await repository.Create(genre);
            await outputCacheStore.EvictByTagAsync("genres-get", default); // Evict the cache for the "genres-get" tag, forcing the next request to re-fetch the data
            return TypedResults.Created($"/genres/{id}", genre);
        }

        static async Task<Results<NotFound, NoContent>> Update(int id, Genre genre, IGenresRepository repository,
            IOutputCacheStore outputCacheStore)
        {
            var exists = await repository.Exists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }
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
