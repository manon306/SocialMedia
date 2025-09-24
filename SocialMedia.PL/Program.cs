using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.BLL.Mapper;
using SocialMedia.BLL.Service.Implementation;
using SocialMedia.DAL.DataBase;
using SocialMedia.DAL.Entity;
using SocialMedia.DAL.REPO.Abstraction;
using SocialMedia.DAL.REPO.IMPLEMENTATION;
using SocialMedia.PL.Factories;
using SocialMedia.PL.Language;
using Stripe;
using System.Globalization;
using System.Security.Claims;

namespace SocialMedia.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSignalR();

            // Connection string
            var connectionString = builder.Configuration.GetConnectionString("defaultConnection");

            // Identity configuration
            

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

            // Authentication: Google & Facebook
            builder.Services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    
                    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                    googleOptions.Scope.Add("profile");

                    googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                    googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                    googleOptions.Events.OnCreatingTicket = ctx =>
                    {
                        var email = ctx.User.GetProperty("email").GetString();
                        var name = ctx.User.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : null;

                        if (!string.IsNullOrEmpty(email) && ctx.Identity != null)
                        {
                            // الجزء قبل الـ @
                            var username = email.Split('@')[0];

                            // لو Google رجعت Name
                            if (!string.IsNullOrEmpty(name))
                            {
                                ctx.Identity.AddClaim(new Claim(ClaimTypes.Name, name));
                            }
                            else
                            {
                                ctx.Identity.AddClaim(new Claim(ClaimTypes.Name, username));
                            }
                        }

                        return Task.CompletedTask;
                    };

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
            builder.Services.AddScoped<IUserProfileRepo, UserProfileRepo>();
            builder.Services.AddScoped<IUserProfileService, UserProfileService>();
            builder.Services.AddScoped<IUserSerives, UserSerives>();
            builder.Services.AddScoped<IuserRepo, UserRepo>();
            builder.Services.AddScoped<IConnectionRepo, ConnectionRepo>();
            builder.Services.AddScoped<IConnectionSerives, ConnectionSerives>();
            builder.Services.AddScoped<IReactService, ReactService>();
            builder.Services.AddScoped<IReactRepo, ReactRepo>();
            builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomClaimsPrincipalFactory>();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddTransient<IEmailService, EmailSerives>();




            // AI service
            builder.Services.AddSingleton<AiService>();

            // SignalR for chat
            builder.Services.AddSignalR();

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




            // Add services to the container.
            builder.Services.AddControllersWithViews()
                // Localization configuration
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(Resource));
                }); ;

            builder.Services.AddHttpClient<PaymobService>(client =>
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });



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
            app.UseHangfireDashboard("/SocialMedia");

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
            // SignalR hub endpoint
            app.MapHub<ChatHub>("/chatHub");
            // Hangfire jobs
            using (var scope = app.Services.CreateScope())
            {
                var postService = scope.ServiceProvider.GetRequiredService<IPostService>();
                postService.UseHangfire();
                var services = scope.ServiceProvider;
                await RoleSeeder.SeedRolesAsync(services);
            }
            
            app.Run();
        }
    }
}
