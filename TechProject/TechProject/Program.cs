using Microsoft.EntityFrameworkCore;
using Tech.DataAccess.Repository.IRepository;
using Tech.DataAccess.Repository;
using TechProject.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Tech.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using TechProject.Models;

namespace TechProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
              .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath =$"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

             builder.Services.AddRazorPages();
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork >();
            builder.Services.AddScoped<IEmailSender,EmailSender>();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false; 
                options.Password.RequiredLength = 6; 
                options.Password.RequireNonAlphanumeric = false; 
                options.Password.RequireUppercase = false; 
                options.Password.RequireLowercase = false; 
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                CreateAdminUser(services).Wait();
            }
            app.Run();
        }
        private static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string adminRole = "Admin";
                string adminEmail = "adminUser@gmail.com";
                string adminPassword = "Admin@123456";

               
                if (!await context.Roles.AnyAsync(r => r.Name == adminRole))
                {
                    await roleManager.CreateAsync(new IdentityRole(adminRole));
                }

                
                if (!await context.Users.AnyAsync(u => u.Email == adminEmail))
                {
                    var hasher = new PasswordHasher<ApplicationUser>();

                    var adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                        City = "AdminCity",
                        BirthDate = new DateTime(1990, 1, 1), 
                        PasswordHash = hasher.HashPassword(null, adminPassword)
                    };

                    context.Users.Add(adminUser);
                    await context.SaveChangesAsync();

                   
                    var adminUserRole = new IdentityUserRole<string>
                    {
                        UserId = adminUser.Id,
                        RoleId = context.Roles.First(r => r.Name == adminRole).Id
                    };

                    context.UserRoles.Add(adminUserRole);
                    await context.SaveChangesAsync();
                }
            }
        }

    }

}
