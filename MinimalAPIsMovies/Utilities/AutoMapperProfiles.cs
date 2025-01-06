using AutoMapper;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        /// <summary>
        /// This class contains the profiles for the AutoMapper configuration.
        /// </summary>
        public AutoMapperProfiles()
        {
            CreateMap<Genre, GenreDTO>();
            CreateMap<CreateGenreDTO, Genre>();

            CreateMap<Actor, ActorDTO>();
            // This is to ignore the Picture property when mapping from CreateActorDTO to Actor
            CreateMap<CreateActorDTO, Actor>().ForMember(x=>x.Picture, options => options.Ignore());
        }
    }
}
