namespace Talabat.APIs.Extensions
{
    public static class AddSwaggerServices
    {

        public static IServiceCollection UseSwagger(this IServiceCollection services) 
        {

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;        
        }

        public static WebApplication UseSwaggerMiddleWares(this WebApplication app) 
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;

        }


    }
}
