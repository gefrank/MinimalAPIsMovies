using AutoMapper;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Endpoints;
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
        }
    }
}
