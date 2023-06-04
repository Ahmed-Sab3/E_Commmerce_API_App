﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any()) 
            {
                var user = new AppUser()
                {
                    DisplayName="Ahmed Nasr",
                    Email = "ahmed.nasr@linkdev.com",
                    UserName="ahmed.nasr",
                    PhoneNumber="01203647035"

                };
                await userManager.CreateAsync(user,"Pa$$w0rd");    
            
            }
        
        }

    }
}
