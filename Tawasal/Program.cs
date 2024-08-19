using Microsoft.EntityFrameworkCore;
using Tawasal.Contexts;
using Tawasal.Models;
using Tawasal.Repositories;
using Tawasal.Repositories.IRepositories;
using Tawasal.Services;
using Tawasal.Services.IServices;


namespace Tawasal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("SqlServer");
                //var connectionString = builder.Configuration.GetConnectionString("PostgreSQL");

                options.UseSqlServer(connectionString);
                //.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                //options.UseNpgsql(connectionString);
            });

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(
                options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequiredUniqueChars = 3;

                    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    //options.Lockout.MaxFailedAccessAttempts = 5;
                    //options.Lockout.AllowedForNewUsers = true;
                }).AddEntityFrameworkStores<ApplicationContext>();

            //Custom Service
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();

            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<IProfileRepository, ProfileRepository>();

            builder.Services.AddScoped<IFeedService, FeedService>();
            builder.Services.AddScoped<IFeedRepository, FeedRepository>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Feed}/{action=TimeLine}/{id?}");

            app.Run();
        }
    }
}
