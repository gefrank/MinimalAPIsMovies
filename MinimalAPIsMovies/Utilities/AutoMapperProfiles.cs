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

            CreateMap<Movie, MovieDTO>()
                .ForMember(x => x.Genres, entity => 
                    entity.MapFrom(p => p.GenresMovies
                    .Select(gm => new GenreDTO
                    { 
                        Id = gm.GenreId, 
                        Name = gm.Genre.Name 
                    })))
                .ForMember(x => x.Actors, entity => 
                    entity.MapFrom(p => p.ActorsMovies
                    .Select(am => new ActorMovieDTO 
                    {
                        Id = am.ActorId, 
                        Name = am.Actor.Name, 
                        Character = am.Character 
                    })));

            CreateMap<CreateMovieDTO, Movie>().ForMember(x => x.Poster, options => options.Ignore());

            CreateMap<Comment, CommentDTO>();
            CreateMap<CreateCommentDTO, Comment>();

            CreateMap<AssignActorMovieDTO, ActorMovie>();
        }
    }
}
