using FluentValidation;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Repositories;


namespace MinimalAPIsMovies.Validations
{
    public class CreateGenreDTOValidator: AbstractValidator<CreateGenreDTO>
    {
        public CreateGenreDTOValidator(IGenresRepository genresRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The field {PropertyName} is required")
                .MaximumLength(150).WithMessage("The field {PropertyName} should be less than {MaxLength} characters")
                .Must(FirstLetterIsUppercase).WithMessage("The field {PropertyName} should start with an uppercase letter")
                .MustAsync(async(name, _) =>
                {
                    var exists = await genresRepository.Exists(id: 0, name);
                    return !exists;
                }).WithMessage(x => $"A genre with the name {x.Name} already exists");
        }

        private bool FirstLetterIsUppercase(string value)
        {

            // THis isn't the objective of this validation rule
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            var firstLetter = value[0].ToString();
            return firstLetter == firstLetter.ToUpper();

        }
    }
}
