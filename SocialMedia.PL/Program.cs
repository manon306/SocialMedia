using Hangfire;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SocialMedia.BLL.Mapper;
using SocialMedia.BLL.Service.Abstraction;
using SocialMedia.BLL.Service.Implementation;
using SocialMedia.DAL.DataBase;
using SocialMedia.DAL.Entity;
using SocialMedia.DAL.REPO.Abstraction;
using SocialMedia.DAL.REPO.IMPLEMENTATION;
using SocialMedia.PL.Language;
using System.Globalization;

namespace SocialMedia.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //connection string configuration
            var connectionString = builder.Configuration.GetConnectionString("defaultConnection");
            //Auto Mapper Configuration
            builder.Services.AddDbContext<SocialMediaDbContext>(options =>
            options.UseSqlServer(connectionString));
            builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));
            //dependancy injection
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IPostsRepo, PostsRepo>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ICommentRepo, CommentRepo>();
           builder.Services.AddScoped<IJobsService, JobsService>();
			     builder.Services.AddScoped<IJobsRepo, JobsRepo>();


            //Hangfire
          // Hangfire (disabled unless packages and config are added)
          var enableHangfire = false;
          bool canConnectToSql = false;
          try
          {
            using var sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            canConnectToSql = true;
          }
          catch
          {
            canConnectToSql = false;
          }
            builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
            builder.Services.AddHangfireServer();
            




            // Add services to the container.
            builder.Services.AddControllersWithViews()
                // Localization configuration
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(Resource));
                }); ;

            
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }






            /////////////////////////////////////////////////////////////Middleware///////////////////////////////////////////////////////
            // Localization configuration middleware
            var supportedCultures = new[] {
                      new CultureInfo("ar-EG"),
                      new CultureInfo("en-US"),
                };
            
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                RequestCultureProviders = new List<IRequestCultureProvider>
                {
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider()
                }
            });

            // Hangfire dashboard middleware
            app.UseHangfireDashboard("/SocialMedia");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Post}/{action=Index}/{id?}");

            using (var scope = app.Services.CreateScope())
            {
                var postService = scope.ServiceProvider.GetRequiredService<IPostService>();
                postService.UseHangfire();
            }
            if (app.Environment.IsDevelopment())
            {
              using (var scope = app.Services.CreateScope())
              {
                var db = scope.ServiceProvider.GetRequiredService<SocialMediaDbContext>();
                if (!db.Jobs.Any())
                {
                  db.Jobs.AddRange(
                    new Job("Junior .NET Developer", "Contoso Ltd", "Cairo, EG", "Build and maintain ASP.NET Core apps."),
                    new Job("Frontend Engineer", "Fabrikam", "Remote", "React/TypeScript UI development."),
                    new Job("SQL Server DBA", "Northwind Traders", "Alexandria, EG", "Manage SQL Server instances and backups."),
                    new Job("Backend Engineer", "Adventure Works", "Giza, EG", "C# microservices and APIs.")
                  );
                  db.SaveChanges();
                }
              }
            }
// if (enableHangfire && canConnectToSql) { /* register Hangfire services */ }

			// Hangfire dashboard middleware
			// if (enableHangfire && canConnectToSql) app.UseHangfireDashboard("/SocialMedia");

			// if (enableHangfire && canConnectToSql) { /* schedule recurring jobs */ }

			// Seed Jobs data in Development if empty

            app.Run();
        }
    }
}
