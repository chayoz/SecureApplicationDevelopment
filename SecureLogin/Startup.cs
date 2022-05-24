using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecureLogin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureLogin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            //Use this function to create roles if missing, run it once
            //CreateRoles(serviceProvider).Wait();

            //Use this function to create users and assign them roles if missing, run it once
            //CreateAccounts(serviceProvider).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var role = new IdentityRole();
            role.Name = "Admin";
            await RoleManager.CreateAsync(role);

            role = new IdentityRole();
            role.Name = "Member";
            await RoleManager.CreateAsync(role);
        }

        private async Task CreateAccounts(IServiceProvider serviceProvider)
        {
            //initializing custom roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var user = new IdentityUser();
            user.UserName = "test@gmail.com";
            user.Email = "test@gmail.com";
            user.EmailConfirmed = true;
            string userPWD = "Asdqwe123!@#";

            var user2 = new IdentityUser();
            user.UserName = "admin@gmail.com";
            user.Email = "admin@gmail.com";
            user.EmailConfirmed = true;
            string userPWD2 = "Admin123!@#";



            IdentityResult chkUser = await UserManager.CreateAsync(user, userPWD);
            IdentityResult chkUser2 = await UserManager.CreateAsync(user2, userPWD2);

            if (chkUser.Succeeded && chkUser2.Succeeded)
            {
                var result = await UserManager.AddToRoleAsync(user, "Member");
                var result2 = await UserManager.AddToRoleAsync(user2, "Admin");
            }
        }
    }
}
