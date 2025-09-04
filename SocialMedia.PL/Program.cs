using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
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

            // Connection string
            var connectionString = builder.Configuration.GetConnectionString("defaultConnection");


            builder.Services.AddDbContext<SocialMediaDbContext>(options =>
                options.UseSqlServer(connectionString));


            // Register Repositories & Services
            builder.Services.AddScoped<IuserRepo, UserRepo>();
            builder.Services.AddScoped<IUserSerives, UserSerives>();

            builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

            // MVC + Localization
            builder.Services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(Resource));
                });

            //  Identity + Authentication BEFORE Build()
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


           

            // Google Auth
            builder.Services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                })
                // Facebook Auth
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
                });





            var app = builder.Build();

            // Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Localization middleware
            var supportedCultures = new[]
            {
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); 
            app.UseAuthorization();

            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


            app.Run();
        }
    }
}
