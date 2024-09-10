
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIS.Errors;
using Talabat.APIS.Extesions;
using Talabat.APIS.Helpers;
using Talabat.APIS.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.DataAcces;
using Talabat.DataAcces.Data;
using Talabat.DataAcces.Identity;
using Talabat.Service;

namespace Talabat.APIS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services
            // Add services to the container.

            builder.Services.AddControllers();

            // Extension Method
            builder.Services.AddSwaggerServices();

            builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            // Extension Method
            builder.Services.AddApp1icationServices();

            // Security Services
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();

           
            var app = builder.Build();

            #endregion

            #region Create_Database
            using var Scope = app.Services.CreateScope();
                var services = Scope.ServiceProvider;
            //ASK CLR for Creating Object from DbContext Explicitly
                var _Identity = services.GetRequiredService<AppIdentityDbContext>();
                var _dbContext = services.GetRequiredService<StoreContext>();

                var LoggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    // Update-Database
                     await _dbContext.Database.MigrateAsync();
                     await StoreContextSeed.SeedAsync(_dbContext);
                     await _Identity.Database.MigrateAsync();

                    // seed user in Identity database
                    var user_manger = services.GetRequiredService<UserManager<AppUser>>();
                    AppldentityDbContextSeed.SeedUserAsync(user_manger);

                }
                catch (Exception ex)
                {
                    var log = LoggerFactory.CreateLogger<Program>();
                    log.LogError(ex, "The Error Has Occurred During Apply The Migration");
                }
            #endregion


            #region Configure Kestrel Middlewares

            //Adding the middleware that server exception handling 
            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Extension Method
                app.UseSwaggerMiddleWares();
            }

            app.UseStatusCodePagesWithRedirects("/errors/{0}");
            app.UseHttpsRedirection();

            app.UseStaticFiles();


            app.UseAuthorization();
            app.MapControllers();

            #endregion


            app.Run();
        }
    }
}
