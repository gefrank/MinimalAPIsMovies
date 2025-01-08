using FluentValidation;
using MinimalAPIsMovies.DTOs;

namespace MinimalAPIsMovies.Filters
{
    public class GenresValidationFilter: IEndpointFilter
    {
        /// <summary>
        /// This method is called when the endpoint is executed. It is used to validate the CreateGenreDTO object        
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            // gets the instance of the CreateGenreDTOValidator class
            var validator = context.HttpContext.RequestServices.GetService<IValidator<CreateGenreDTO>>();

            if (validator is null)
            {
                return await next(context);
            }

            var obj = context.Arguments.OfType<CreateGenreDTO>().FirstOrDefault();

            if (obj is null)
            {
                return Results.Problem("The object to validation could not be found");
            }

            var validationResult = await validator.ValidateAsync(obj);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            return await next(context);
        }
    }
}
