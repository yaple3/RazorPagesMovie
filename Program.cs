using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;
namespace RazorPagesMovie
{ 
    public class program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<RazorPagesMovieContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("RazorPagesMovieContext") ?? throw new InvalidOperationException("Connection string 'RazorPagesMovieContext' not found.")));

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                SeedData.Initialize(services);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }

    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RazorPagesMovieContext(
                           serviceProvider.GetRequiredService<DbContextOptions<RazorPagesMovieContext>>()))
            {
                if(context == null || context.Movie == null)
                {
                    throw new ArgumentNullException("Null RazorPagesMovieContext");
                }

                // Look for any movies.
                if (context.Movie.Any())
                {
                    return;   // DB has been seeded
                }

                context.Movie.AddRange(
                                   new Movie
                                   {
                                       Title = "When Harry Met Sally",
                                       ReleaseDate = DateTime.Parse("1989-2-12"),
                                       Genre = "Romantic Comedy",
                                       Price = 7.99M,
                                      // Rating = "R"
                                   },

                                                  new Movie
                                                  {
                                                      Title = "Ghostbusters ",
                                                      ReleaseDate = DateTime.Parse("1984-3-13"),
                                                      Genre = "Comedy",
                                                      Price = 8.99M,
                                                     // Rating = "R"
                                                  },

                                                                 new Movie
                                                                 {
                                                                     Title = "Ghostbusters 2",
                                                                     ReleaseDate = DateTime.Parse("1986-2-23"),
                                                                     Genre = "Comedy",
                                                                     Price = 9.99M,
                                                                   //  Rating = "R"
                                                                 },

                                                                                new Movie
                                                                                {
                                                                                    Title = "Rio Bravo",
                                                                                    ReleaseDate = DateTime.Parse("1959-4-15"),
                                                                                    Genre = "Western",
                                                                                    Price = 3.99M,
                                                                                 //   Rating = "R"
                                                                                }
                                                                                           );
                context.SaveChanges();
            }
        }
    }   
}
