using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.DataAcces.Identity
{
    public static class AppldentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Nada_Assem",
                    Email = "nadaassem81@gmail.com" ,
                    PhoneNumber = "01100432184" ,
                    UserName = "nada_assem"
                };
                await userManager.CreateAsync(user , "Nada@81");
            }
        }
    }
}
