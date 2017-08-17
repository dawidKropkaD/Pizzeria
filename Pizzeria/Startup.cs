using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pizzeria.Data;
using Pizzeria.Models;
using Pizzeria.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Pizzeria.Models.Tables;

namespace Pizzeria
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.CookieHttpOnly = true;
            });


            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("en-AU"),
                new CultureInfo("en-GB"),
                new CultureInfo("en"),
                new CultureInfo("es-ES"),
                new CultureInfo("es-MX"),
                new CultureInfo("es"),
                new CultureInfo("fr-FR"),
                new CultureInfo("fr"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseSession();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            await CreateRoles(serviceProvider);
            await CreateUsers(serviceProvider, app.ApplicationServices.GetService<ApplicationDbContext>(),
                "admin@example.pl", "admin@example.pl", "Admin", "123456789", 
                new Address { City="Warszawa", Street = "Bonifraterska", HouseNumber = "6" });
            await CreateUsers(serviceProvider, app.ApplicationServices.GetService<ApplicationDbContext>(),
                "employee@example.pl", "employee@example.pl", "Employee", "987654321",
                new Address { City = "Warszawa", Street = "al. Solidarności", HouseNumber = "86", FlatNumber = "88" });
            await CreateUsers(serviceProvider, app.ApplicationServices.GetService<ApplicationDbContext>(),
                "member@example.pl", "member@example.pl", "Member", "741236985",
                new Address { City = "Warszawa", Street = "Żelazna", HouseNumber = "32" });

            SeedData.InitializeMenu(serviceProvider);
            SeedData.InitializeAdditionalComponents(serviceProvider);
            SeedData.InitializePromotion(serviceProvider);
        }

        /// <summary>
        /// Source: https://gooroo.io/GoorooTHINK/Article/17333/Custom-user-roles-and-rolebased-authorization-in-ASPNET-core/28352#.WXNv3YjyiUl
        /// </summary>
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "Employee", "Member" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }


        private async Task CreateUsers(IServiceProvider serviceProvider, ApplicationDbContext context,
            string name, string email, string role, string phone, Address address)
        {
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var user = new ApplicationUser
            {
                UserName = name,
                Email = email,
                PhoneNumber = phone
            };

            string userPWD = "qwerty";
            var _user = await UserManager.FindByEmailAsync(user.Email);

            if (_user == null)
            {
                var createUser = await UserManager.CreateAsync(user, userPWD);
                if (createUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user, role);

                    UserDb userDb = new UserDb
                    {
                        AspNetUserId = user.Id,
                        LoyaltyPoints = 0,
                        MoneyPrize = 0,
                        City = address.City,
                        Street = address.Street,
                        HouseNumber = address.HouseNumber,
                        FlatNumber = address.FlatNumber
                    };
                    context.Add(userDb);
                    context.SaveChanges();
                }
            }
        }
    }
}
