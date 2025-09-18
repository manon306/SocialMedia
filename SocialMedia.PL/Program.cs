using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SocialMedia.BLL.Config;
using SocialMedia.BLL.Mapper;
using SocialMedia.BLL.Service.Abstraction;
using SocialMedia.BLL.Service.Chat;
using SocialMedia.BLL.Service.Implementation;
using SocialMedia.Configuration;
using SocialMedia.DAL.DataBase;
using SocialMedia.DAL.Entity;
using SocialMedia.DAL.REPO.Abstraction;
using SocialMedia.DAL.REPO.IMPLEMENTATION;
using SocialMedia.PL.Configuration;
using SocialMedia.PL.Hubs;
using SocialMedia.PL.Language;
using Stripe;
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

            // DbContext
            builder.Services.AddDbContext<SocialMediaDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Identity configuration
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Name;
                options.ClaimsIdentity.EmailClaimType = ClaimTypes.Email;
            })
            .AddEntityFrameworkStores<SocialMediaDbContext>()
            .AddDefaultTokenProviders();

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

            // AutoMapper
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

            // AI service
            builder.Services.AddSingleton<AiService>();

            // Stripe integration
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            // SignalR for chat
            builder.Services.AddSignalR();
            builder.Services.AddScoped<IChatService, ChatService>();

            // Hangfire
            try
            {
                using var sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
            }
            catch
            {
                // If SQL is down, we still want the app to boot
            }
            builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
            builder.Services.AddHangfireServer();

            // Google Maps API config
            builder.Services.Configure<GoogleMapsConfig>(builder.Configuration.GetSection("GoogleMaps"));

            // Localization + MVC
            builder.Services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(Resource));
                });

            // CORS for SignalR / APIs
            builder.Services.AddCors(o =>
            {
                o.AddPolicy("Default", p => p
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowed(_ => true)); // TODO: restrict in production
            });

            var app = builder.Build();

            // Middleware pipeline
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
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                RequestCultureProviders = new List<Microsoft.AspNetCore.Localization.IRequestCultureProvider>
                {
                    new Microsoft.AspNetCore.Localization.QueryStringRequestCultureProvider(),
                    new Microsoft.AspNetCore.Localization.CookieRequestCultureProvider()
                }
            });

            app.UseHangfireDashboard("/SocialMedia");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("Default");
            app.UseAuthentication();
            app.UseAuthorization();

            // Controllers + Razor Pages
            app.MapControllers();
            app.MapRazorPages();

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