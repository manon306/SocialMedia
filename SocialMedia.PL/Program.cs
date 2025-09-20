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
using System.Globalization;
using System.Security.Claims;

namespace SocialMedia.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Connection string
            var connectionString = builder.Configuration.GetConnectionString("defaultConnection");

            // Identity configuration
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<SocialMediaDbContext>()
                .AddDefaultTokenProviders();

            // Authorization configuration
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserOrAdmin", policy => policy.RequireRole("User", "Admin"));
            });

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Name;
                options.ClaimsIdentity.EmailClaimType = ClaimTypes.Email;
            });

            // Authentication: Google & Facebook (commented out for now)
            builder.Services.AddAuthentication();

            // DbContext & AutoMapper
            builder.Services.AddDbContext<SocialMediaDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

            // Dependency Injection
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IPostsRepo, PostsRepo>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ICommentRepo, CommentRepo>();
            // builder.Services.AddScoped<IReplyService, ReplyService>();
            // builder.Services.AddScoped<IReplyRepo, ReplyRepo>();
            builder.Services.AddScoped<IJobsService, JobsService>();
            builder.Services.AddScoped<IJobsRepo, JobsRepo>();
            // builder.Services.AddScoped<IMessageService, MessageService>();
            // builder.Services.AddScoped<IMessageRepository, MessageRepository>();

            // Stripe integration (commented out for now)
            // builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            // StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            // AI service (commented out for now)
            // builder.Services.AddSingleton<AiService>();

            // SignalR for chat
            builder.Services.AddSignalR();

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


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // SignalR hub endpoint (commented out for now)
            // app.MapHub<ChatHub>("/hubs/chat");

            // Default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Post}/{action=Index}/{id?}");


            // Seed admin user
            using (var scope = app.Services.CreateScope())
            {
                await SeedAdminUser(scope.ServiceProvider);
            }

            app.Run();
        }

        private static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Create roles if they don't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Create admin user if it doesn't exist
            var adminEmail = "admin@socialmedia.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Name = "System Administrator",
                    Role = "Admin"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}