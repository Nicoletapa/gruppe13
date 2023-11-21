using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nøsted.Data;
using Nøsted.Models;


public class Program
{
    public static async Task Main(string[] args)
    {

                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                 var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                 builder.Services.AddDbContext<ApplicationDbContext>(options =>
                 {
                    options.UseMySql(connectionString, 
                        ServerVersion.AutoDetect(connectionString));
                    
                    if (builder.Environment.IsDevelopment())
                    {
                        options.EnableSensitiveDataLogging(); // Enable sensitive data logging in development
                    }
                 });

                builder.Services.AddDatabaseDeveloperPageExceptionFilter();


                builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

                
                builder.Services.AddControllersWithViews();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseMigrationsEndPoint();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.MapRazorPages();
                app.UseRouting();

                app.UseAuthorization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");


                using (var scope = app.Services.CreateScope())
                {
                    var roleManager =
                        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    var roles = new[] { "Admin", "Mekaniker", "Kunde", "Elektro", "Hydraulikker" };

                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                            await roleManager.CreateAsync(new IdentityRole(role));
                    }

                }

                using (var scope = app.Services.CreateScope())
                {
                    var userManager = 
                        scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    
                    string email = "admin@admin.com";
                    string password = "Test1234,";

                    if (await userManager.FindByEmailAsync(email) == null)
                    {
                        var user = new IdentityUser();
                        user.UserName = email;
                        user.Email = email;
                        

                        await userManager.CreateAsync(user, password);
                        var result = await userManager.AddToRoleAsync(user, "Admin");
                       
                    }
                }
                
             

                app.Run();

    }
    

}