using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
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
using SocialMedia.PL.Hubs;
using System.Globalization;
using System.Security.Claims;
using Stripe;
using SocialMedia.BLL.Config;

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
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<SocialMediaDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Name;
                options.ClaimsIdentity.EmailClaimType = ClaimTypes.Email;
            });

            // Authentication: Google & Facebook
            builder.Services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                    googleOptions.Scope.Add("email");
                    googleOptions.Scope.Add("profile");
                    googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                    googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                    googleOptions.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                    googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
                })
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
                    facebookOptions.Fields.Add("name");
                    facebookOptions.Fields.Add("email");
                    facebookOptions.Fields.Add("first_name");
                    facebookOptions.Fields.Add("last_name");
                    facebookOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                    facebookOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                    facebookOptions.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
                    facebookOptions.ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
                });

            // DbContext & AutoMapper
            builder.Services.AddDbContext<SocialMediaDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

            // Dependency Injection
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IPostsRepo, PostsRepo>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ICommentRepo, CommentRepo>();
            builder.Services.AddScoped<IReplyService, ReplyService>();
            builder.Services.AddScoped<IReplyRepo, ReplyRepo>();
            builder.Services.AddScoped<IJobsService, JobsService>();
            builder.Services.AddScoped<IJobsRepo, JobsRepo>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();

            // Stripe integration
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            // AI service
            builder.Services.AddSingleton<AiService>();

            // SignalR for chat
            builder.Services.AddSignalR();

            // Hangfire
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

            // MVC + Localization
            builder.Services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(Resource));
                });

            var app = builder.Build();

            // Configure middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

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

            app.UseHangfireDashboard("/SocialMedia");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // SignalR hub endpoint
            app.MapHub<ChatHub>("/hubs/chat");

            // Default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Post}/{action=Index}/{id?}");

            // Hangfire jobs
            using (var scope = app.Services.CreateScope())
            {
                var postService = scope.ServiceProvider.GetRequiredService<IPostService>();
                postService.UseHangfire();
            }

            app.Run();
        }
    }
}