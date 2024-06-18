using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.Helpers;
using Talabat.Core.Repositories.Contract;
using Talabat.DataAcces.Data;
using Talabat.DataAcces;
using Talabat.APIS.Errors;

namespace Talabat.APIS.Extesions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApp1icationServices(this IServiceCollection services)
        {


            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddAutoMapper(typeof(MappingProfiles));

            // handle Validation Errors
            services.Configure<ApiBehaviorOptions>(option =>
            {

                option.InvalidModelStateResponseFactory = (ActionContext) =>
                {
                    var errors = ActionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                              .SelectMany(P => P.Value.Errors)
                                              .Select(E => E.ErrorMessage)
                                              .ToArray();
                    var validationErrorReponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationErrorReponse);
                };

            });

            return services;
        }
    }
}
