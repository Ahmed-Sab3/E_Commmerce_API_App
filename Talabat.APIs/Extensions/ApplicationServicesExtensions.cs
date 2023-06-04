using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {

            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IUntiOfWork, UnitOfWork>();

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

             //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            services.AddAutoMapper(typeof(MappingProfiles));

            services.Configure<ApiBehaviorOptions>(options =>
            {

                options.InvalidModelStateResponseFactory = (actioncontext) =>
                {
                    var errors = actioncontext.ModelState.Where(p => p.Value.Errors.Count > 0)
                                                          .SelectMany(p => p.Value.Errors)
                                                          .Select(E => E.ErrorMessage)
                                                          .ToArray();

                    var validationerrorresponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationerrorresponse);
                };


            });
            return services;

        }

    }
}
