using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SocialMedia.BLL.Mapper;
using SocialMedia.BLL.Service.Implementation;
using SocialMedia.DAL.DataBase;
using SocialMedia.DAL.Entity;
using SocialMedia.DAL.REPO.Abstraction;
using SocialMedia.DAL.REPO.IMPLEMENTATION;
using SocialMedia.PL.Language;
using System.Globalization;
using System.Security.Claims;

namespace SocialMedia.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Connection string
            var connectionString = builder.Configuration.GetConnectionString("defaultConnection");

            // Identity configuration
            builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddEntityFrameworkStores<SocialMediaDbContext>()
                            .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/Login";
                });

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<SocialMediaDbContext>()
            .AddDefaultTokenProviders();



            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Name;
                options.ClaimsIdentity.EmailClaimType = ClaimTypes.Email;
            });

            // Authentication: External providers disabled on this branch (Google/Facebook packages not referenced)

            // DbContext & AutoMapper
            builder.Services.AddDbContext<SocialMediaDbContext>(options =>
            options.UseSqlServer(connectionString));
            builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));
            // Dependency Injection
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IPostsRepo, PostsRepo>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ICommentRepo, CommentRepo>();
            builder.Services.AddScoped<IJobsService, JobsService>();
            builder.Services.AddScoped<IJobsRepo, JobsRepo>();
            // The following DI registrations are disabled on this branch because their types are missing
            // builder.Services.AddScoped<IReplyService, ReplyService>();
            // builder.Services.AddScoped<IReplyRepo, ReplyRepo>();
            // builder.Services.AddScoped<IUserProfileRepo, UserProfileRepo>();
            // builder.Services.AddScoped<IUserProfileService, UserProfileService>();
            // builder.Services.AddScoped<IConnectionRepo, ConnectionRepo>();
            // builder.Services.AddScoped<IConnectionSerives, ConnectionSerives>();
            // builder.Services.AddScoped<IReactService, ReactService>();
            // builder.Services.AddScoped<IReactRepo, ReactRepo>();
            // builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomClaimsPrincipalFactory>();


            //AI INTEGRATION SERVICE
            builder.Services.AddHttpClient();
            //builder.Services.AddHttpClient<AiService>();
            // إضافة خدمات HttpClient
            //builder.Services.AddHttpClient<AiService>(client =>
            //{
            //    client.BaseAddress = new Uri("https://api-inference.huggingface.co/");
            //    client.DefaultRequestHeaders.Authorization =
            //        new System.Net.Http.Headers.AuthenticationHeaderValue(
            //            "Bearer", "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");
            //    client.Timeout = TimeSpan.FromSeconds(30);
            //});
            //Hangfire
            // Hangfire (disabled unless packages and config are added)
            var enableHangfire = builder.Configuration.GetValue<bool>("EnableHangfire");
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
            
            if (enableHangfire && canConnectToSql)
            {
                builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
                builder.Services.AddHangfireServer();
            }
            
            // MVC + Localization




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

            //Hangfire dashboard middleware
            if (enableHangfire && canConnectToSql)
            {
                app.UseHangfireDashboard("/SocialMedia");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Hangfire dashboard middleware
            // if (enableHangfire && canConnectToSql) app.UseHangfireDashboard("/SocialMedia");


            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Hangfire Dashboard
            //app.UseHangfireDashboard("/SocialMedia");

            // Default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Post}/{action=Index}/{id?}");

            // Hangfire jobs
            if (enableHangfire && canConnectToSql)
            {
                using (var scope = app.Services.CreateScope())
                {
                    var postService = scope.ServiceProvider.GetRequiredService<IPostService>();
                    postService.UseHangfire();
                }
            }

            // Seed Jobs data in Development
                //if (app.Environment.IsDevelopment())
                //{
                //    var db = scope.ServiceProvider.GetRequiredService<SocialMediaDbContext>();
                //    if (!db.Jobs.Any())
                //    {
                //        db.Jobs.AddRange(
                //            new Job("Junior .NET Developer", "Contoso Ltd", "Cairo, EG", "Build and maintain ASP.NET Core apps."),
                //            new Job("Frontend Engineer", "Fabrikam", "Remote", "React/TypeScript UI development."),
                //            new Job("SQL Server DBA", "Northwind Traders", "Alexandria, EG", "Manage SQL Server instances and backups."),
                //            new Job("Backend Engineer", "Adventure Works", "Giza, EG", "C# microservices and APIs.")
                //        );
                //        db.SaveChanges();
                //    }
                //}
            //if (app.Environment.IsDevelopment())
            //{
            //    using (var scope = app.Services.CreateScope())
            //    {
            //        var db = scope.ServiceProvider.GetRequiredService<SocialMediaDbContext>();
            //        if (!db.Jobs.Any())
            //        {
            //            db.Jobs.AddRange(
            //              new Job("Junior .NET Developer", "Contoso Ltd", "Cairo, EG", "Build and maintain ASP.NET Core apps."),
            //              new Job("Frontend Engineer", "Fabrikam", "Remote", "React/TypeScript UI development."),
            //              new Job("SQL Server DBA", "Northwind Traders", "Alexandria, EG", "Manage SQL Server instances and backups."),
            //              new Job("Backend Engineer", "Adventure Works", "Giza, EG", "C# microservices and APIs.")
            //            );
            //            db.SaveChanges();
            //        }
            //    }
            //}
// if (enableHangfire && canConnectToSql) { /* register Hangfire services */ }


			// Hangfire dashboard middleware
			// if (enableHangfire && canConnectToSql) app.UseHangfireDashboard("/SocialMedia");

            // if (enableHangfire && canConnectToSql) { /* schedule recurring jobs */ }

            // Seed Jobs data in Development if empty

            app.Run();
        }
    }
}
