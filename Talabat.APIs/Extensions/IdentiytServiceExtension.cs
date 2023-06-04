﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class IdentiytServiceExtension
    {

        public static IServiceCollection AddIDentityService(this IServiceCollection services,IConfiguration configuration ) 
        {

            services.AddScoped<ITokenServic, TokenService>();


            services.AddIdentity<AppUser, IdentityRole>(options => { 
            
            })
                .AddEntityFrameworkStores<AppIdentityDbContext>();


            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
            }).
                AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters() {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:ValidIssur"],
                        ValidateAudience=true,
                        ValidAudience = configuration["JWT:ValidAudience"],
                        ValidateLifetime=true,
                        ValidateIssuerSigningKey=true,
                        IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))

                };
                });
            return services;
        }

    }
}
