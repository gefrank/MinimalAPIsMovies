using FluentValidation;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Repositories;


namespace MinimalAPIsMovies.Validations
{
    public class CreateGenreDTOValidator: AbstractValidator<CreateGenreDTO>
    {
        public CreateGenreDTOValidator(IGenresRepository genresRepository, IHttpContextAccessor httpContextAccessor)
        {
            var routeValueId = httpContextAccessor.HttpContext!.Request.RouteValues["id"];
            var id = 0;

            // 1. Type Check: It checks if the variable routeValueId is of type string.
            // 2. Pattern Matching: If routeValueId is indeed a string, it assigns the value of routeValueId to a new variable routeValueIdString.
            if (routeValueId is string routeValueIdString)
            {
                int.TryParse(routeValueIdString, out id);
            }

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationUtilities.NonEmptyMessage)
                .MaximumLength(150).WithMessage(ValidationUtilities.MaximumLengthMessage)
                .Must(ValidationUtilities.FirstLetterIsUppercase).WithMessage(ValidationUtilities.FirstLetterIsUppercaseMessage)
                .MustAsync(async(name, _) =>
                {
                    var exists = await genresRepository.Exists(id: 0, name);
                    return !exists;
                }).WithMessage(x => $"A genre with the name {x.Name} already exists");
        }

    }
}

