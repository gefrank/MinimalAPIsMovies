using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Entities;
using MinimalAPIsMovies.Repositories;
using MinimalAPIsMovies.Services;

namespace MinimalAPIsMovies.Endpoints
{
    public static class ActorsEndpoints
    {
        private readonly static string container = "actors";

        public static RouteGroupBuilder MapActors(this RouteGroupBuilder group)
        {
            // DisableAntiforgery is used to disable the antiforgery token validation for this endpoint, so that it works from a form
            group.MapPost("/", Create).DisableAntiforgery();
            return group;
        }

        static async Task<Created<ActorDTO>> Create([FromForm] CreateActorDTO createActorDTO,
                IActorsRepository repository, IOutputCacheStore outputCacheStore, 
                IMapper mapper, IFileStorage fileStorage)
        {
            var actor = mapper.Map<Actor>(createActorDTO);
            if (createActorDTO.Picture is not null)
            {
               var url = await fileStorage.Store(container, createActorDTO.Picture);
                actor.Picture = url;
            }


            var id = await repository.Create(actor);
            await outputCacheStore.EvictByTagAsync("actors-get", default);
            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Created($"/actors/{id}", actorDTO);
        }

    }
}
