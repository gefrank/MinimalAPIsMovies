using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MinimalAPIsMovies.DTOs;

namespace MinimalAPIsMovies.Utilities
{
    public static class SwaggerExtentions
    {
        // This makes this an extension method for ITBuilder
        public static TBuilder AddPaginationParameters<TBuilder>(this TBuilder builder) 
                        where TBuilder : IEndpointConventionBuilder
        {
            return builder.WithOpenApi(options =>
            {
                // Customizing the metadata for the GetAll endpoint, this will allow you to use 
                // these parameters in swagger
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "page",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Default = new OpenApiInteger(PaginationDTO.pageInitialValue)
                    }
                });

                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "RecordsPerPage",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Default = new OpenApiInteger(PaginationDTO.recordsPerPageInitialValue)
                    }
                });

                return options;
            });
        }

    }
}
