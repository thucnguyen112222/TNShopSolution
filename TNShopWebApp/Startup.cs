using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using TNShopSolution.ViewModels.System.Users;
using TNShopWebApp.Service;

namespace TNShopWebApp
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
            services.AddHttpClient();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = ("/admin/login/index");
                    options.AccessDeniedPath = ("/admin/user/forbidden");

                });
            services.AddControllersWithViews().AddFluentValidation(
                v => v.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());
            services.AddRazorPages();
            services.AddSession(options=> {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddDbContext<TNShopSulotion.Data.EntityFramework.TNShopdbContext>();
            services.AddDbContext<TNShopSulotion.Data.EntityFramework.TNShopdbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("TNShop")));
            services.AddTransient<IUserApiClient, UserApiClient>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            IMvcBuilder builder = services.AddRazorPages();
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT");
            if (environment == Environments.Development)
            {
                builder.AddRazorRuntimeCompilation();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
