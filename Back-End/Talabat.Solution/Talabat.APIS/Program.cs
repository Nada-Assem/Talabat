
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIS.Errors;
using Talabat.APIS.Helpers;
using Talabat.APIS.Middlewares;
using Talabat.Core.Repositories.Contract;
using Talabat.DataAcces;
using Talabat.DataAcces.Data;

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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddAutoMapper(typeof(MappingProfiles));

            // handle Validation Errors
            builder.Services.Configure<ApiBehaviorOptions>(option =>
            {
                
                option.InvalidModelStateResponseFactory = (ActionContext) =>
                {
                    var errors = ActionContext.ModelState.Where(P=> P.Value.Errors.Count() > 0 ) 
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

            var app = builder.Build();

            #endregion

            #region Create_Database
            using var Scope = app.Services.CreateScope();
                var services = Scope.ServiceProvider;
                //ASK CLR for Creating Object from DbContext Explicitly
                var _dbContext = services.GetRequiredService<StoreContext>();

                var LoggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    // Update-Database
                    await _dbContext.Database.MigrateAsync();
                     await StoreContextSeed.SeedAsync(_dbContext);

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
                app.UseSwagger();
                app.UseSwaggerUI();
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
