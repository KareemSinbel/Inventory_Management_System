using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(
                options=> options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
            );

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();


            builder.Services.AddScoped<IAccountManagerRepo, AccountManagerRepo>();
            builder.Services.AddScoped<IHomeRepo, HomeRepo>();
            builder.Services.AddScoped<IGenericRepo<Supplier>, SupplierRepo>();
            builder.Services.AddScoped<IGenericRepo<Product>, ProductRepo>();
            builder.Services.AddScoped<IGenericRepo<Category>, CategoryRepo>();
            builder.Services.AddScoped<IGenericRepo<Employee>, EmployeeRepo>();
            builder.Services.AddScoped<IFactoryRepository, FactoryRepository>();


            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(options => options.LoginPath = "/Account/Login");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");


            using(var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                
                DataManager.RolesSeeding(services).GetAwaiter().GetResult();
                DataManager.AdminSeeding(services).GetAwaiter().GetResult();
            }

            app.Run();
        }
    }
}
