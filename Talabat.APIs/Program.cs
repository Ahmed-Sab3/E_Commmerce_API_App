using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddleWares;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.UseSwagger();

            builder.Services.AddDbContext<StoreContext>(options =>
            {

                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(S =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);


            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));

            });

            builder.Services.AddApplicationServices();

            builder.Services.AddIDentityService(builder.Configuration);

            #endregion



            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var LoggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var dbcontext = services.GetRequiredService<StoreContext>();
                await dbcontext.Database.MigrateAsync();//Uply Migrations

                await StoreContextSeed.SeedAsync(dbcontext);

                var identitydbcontext = services.GetRequiredService<AppIdentityDbContext>();
                await identitydbcontext.Database.MigrateAsync();

                var usermanger = services.GetRequiredService<UserManager<AppUser>>();

                await AppIdentityDbContextSeed.SeedUserAsync(usermanger);


            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error occured during applyin the migration");
                
            }

            app.UseMiddleware<ExceptionMiddeleware>();         

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
             app.UseSwaggerMiddleWares();
                
            }


            app.UseStatusCodePagesWithReExecute("errors/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();



            app.MapControllers();

            app.Run();
        }
    }
}