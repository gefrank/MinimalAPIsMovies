
using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using MinimalAPIsMovies.Repositories;

namespace MinimalAPIsMovies.Filters
{
    public class TestFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, 
                                              EndpointFilterDelegate next)
        {
            // This is the code that will execute before the GetById endpoint
            // This will get the parameters of the GetById endpoint

            var param1 = context.Arguments.OfType<int>().FirstOrDefault(); // This will get the first parameter of type int
            var param2 = context.Arguments.OfType<IGenresRepository>().FirstOrDefault();
            var param3 = context.Arguments.OfType<IMapper>().FirstOrDefault();

            var result = await next(context);
            // This is the code that will execute after the GetById endpoint
            return result;
        }
    }
}
