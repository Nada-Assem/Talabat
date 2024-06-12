
using Microsoft.EntityFrameworkCore;
using Talabat.APIS.Helpers;
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


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();


            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
