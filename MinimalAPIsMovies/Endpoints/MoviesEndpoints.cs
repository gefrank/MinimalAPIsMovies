﻿using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Entities;
using MinimalAPIsMovies.Filters;
using MinimalAPIsMovies.Repositories;
using MinimalAPIsMovies.Services;
using MinimalAPIsMovies.Utilities;

namespace MinimalAPIsMovies.Endpoints
{
    public static class MoviesEndpoints
    {
        // Creates a movie folder locally
        private readonly static string container = "movies";

        public static RouteGroupBuilder MapMovies(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("movies-get")).AddPaginationParameters();
            group.MapGet("/{id:int}", GetById);
            group.MapPost("/filter", FilterMovies).AddMoviesFilterParameters();

            group.MapPost("/", Create).DisableAntiforgery().AddEndpointFilter<ValidationFilter<CreateMovieDTO>>().RequireAuthorization("isadmin").WithOpenApi();
            group.MapPut("/{id:int}", Update).DisableAntiforgery().AddEndpointFilter<ValidationFilter<CreateMovieDTO>>().RequireAuthorization("isadmin").WithOpenApi();
            group.MapDelete("/{id:int}", Delete).RequireAuthorization("isadmin");
            group.MapPost("/{id:int}/assignGenres", AssignGenres).RequireAuthorization("isadmin");
            group.MapPost("/{id:int}/assignActors", AssignActors).RequireAuthorization("isadmin");
            return group;
        }

        static async Task<Ok<List<MovieDTO>>> GetAll(IMoviesRepository repository, IMapper mapper, PaginationDTO pagination)
        {            
            var movies = await repository.GetAll(pagination);
            var moviesDTO = mapper.Map<List<MovieDTO>>(movies);
            return TypedResults.Ok(moviesDTO);
        }

        static async Task<Results<Ok<MovieDTO>, NotFound>> GetById(int id, IMoviesRepository repository, IMapper mapper)
        {
            var movie = await repository.GetById(id);
            if (movie is null)
            {
                return TypedResults.NotFound();
            }

            var movieDTO = mapper.Map<MovieDTO>(movie);
            return TypedResults.Ok(movieDTO);
        }

        public static async Task<Created<MovieDTO>> Create([FromForm] CreateMovieDTO createMovieDTO,
                                                    IFileStorage fileStorage,
                                                    IOutputCacheStore outputCacheStore,
                                                    IMapper mapper,
                                                    IMoviesRepository repository)
        {
            var movie = mapper.Map<Movie>(createMovieDTO);

            if (createMovieDTO.Poster is not null)
            {
                var url = await fileStorage.Store(container, createMovieDTO.Poster);
                movie.Poster = url;
            }

            var id = await repository.Create(movie);
            await outputCacheStore.EvictByTagAsync("movies-get", default);
            var movieDTO = mapper.Map<MovieDTO>(movie);
            return TypedResults.Created($"$movies/{id}", movieDTO);
        }

        public static async Task<Results<NoContent, NotFound>> Update(int id, [FromForm] CreateMovieDTO createMovieDTO,
                                            IMoviesRepository repository,
                                            IFileStorage fileStorage,
                                            IOutputCacheStore outputCacheStore,
                                            IMapper mapper)
        {
            var movieDB = await repository.GetById(id);
            if (movieDB is null)
            {
                return TypedResults.NotFound();
            }

            var movieForUpdate = mapper.Map<Movie>(createMovieDTO);
            movieForUpdate.Id = id;
            movieForUpdate.Poster = movieDB.Poster;

            if (createMovieDTO.Poster is not null)
            {
                var url = await fileStorage.Edit(movieForUpdate.Poster, container, createMovieDTO.Poster);
                movieForUpdate.Poster = url;
            }

            await repository.Update(movieForUpdate);
            await outputCacheStore.EvictByTagAsync("movies-get", default);
            return TypedResults.NoContent();
        }

        public static async Task<Results<NoContent, NotFound>> Delete(int id, IMoviesRepository repository,
            IOutputCacheStore outputCacheStore, IFileStorage fileStorage)
        {
            var movieDB = await repository.GetById(id);

            if (movieDB is null)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
            await fileStorage.Delete(movieDB.Poster, container);
            await outputCacheStore.EvictByTagAsync("movies-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound, BadRequest<string>>> AssignGenres
            (int id, List<int> genresIds, 
            IMoviesRepository moviesRepository, IGenresRepository genresRepository)
        {
            if (!await moviesRepository.Exists(id))
            {
                return TypedResults.NotFound();
            }

            var existingGenres = new List<int>();

            if (genresIds.Count != 0)
            {
                existingGenres = await genresRepository.Exists(genresIds);
            }

            if (existingGenres.Count != genresIds.Count)
            {
                var nonExistingGenres = genresIds.Except(existingGenres);
                var nonExistingGenresCSV = string.Join(", ", nonExistingGenres);
                return TypedResults.BadRequest($"Genres with ids {string.Join(", ", nonExistingGenresCSV)} do not exist.");
            }

            await moviesRepository.Assign(id, genresIds);
            return TypedResults.NoContent();

        }

        static async Task<Results<NotFound, NoContent, BadRequest<string>>> AssignActors(
                                        int id, List<AssignActorMovieDTO> actorsDTO, 
                                        IMoviesRepository moviesRepository, IActorsRepository actorsRepository, IMapper mapper)
        {
            if (!await moviesRepository.Exists(id))
            {
                return TypedResults.NotFound();
            }

            var existingActors = new List<int>();
            var actorIds = actorsDTO.Select(x => x.ActorId).ToList();

            if (actorsDTO.Count != 0)
            {
                existingActors = await actorsRepository.Exists(actorIds);
            }

            if (existingActors.Count != actorsDTO.Count)
            {
                var nonExistingActors = actorIds.Except(existingActors);
                var nonExistingActorsCSV = string.Join(", ", nonExistingActors);
                return TypedResults.BadRequest($"Actors with ids {string.Join(", ", nonExistingActorsCSV)} do not exist.");
            }

            var actors = mapper.Map<List<ActorMovie>>(actorsDTO);
            await moviesRepository.Assign(id, actors);
            return TypedResults.NoContent();

        }

        static async Task<Ok<List<MovieDTO>>> FilterMovies(MoviesFilterDTO moviesFilterDTO, IMoviesRepository repository, IMapper mapper)
        {
            var movies = await repository.Filter(moviesFilterDTO);
            var moviesDTO = mapper.Map<List<MovieDTO>>(movies);
            return TypedResults.Ok(moviesDTO);
        }


    }
}
