using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Validation;
using IdentityServerMembership.AuthServer.Models;
using IdentityServerMembership.AuthServer.Seed;
using IdentityServerMembership.AuthServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityServerMembership.AuthServer
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

            services.AddDbContext<CustomDbContext>(option =>
            {
                option.UseSqlServer("Data Source=localhost\\SQLEXPRESS01;Database=CustomIdentityLearning;Integrated Security=true;");
            });
            services.AddScoped<ICustomUserRepository, CustomUserRepository>();
            services.AddScoped<IdentityServer4.Services.IProfileService, CustomProfileService>();

            var assemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddConfigurationStore(opt =>
                {
                    opt.ConfigureDbContext = c => c.UseSqlServer("Data Source=localhost\\SQLEXPRESS01;Database=CustomIdentityLearning;Integrated Security=true;", sqlopt =>
                    {
                        sqlopt.MigrationsAssembly(assemblyName);
                    });
                })
                .AddOperationalStore(opt =>
                {
                    opt.ConfigureDbContext = c => c.UseSqlServer("Data Source=localhost\\SQLEXPRESS01;Database=CustomIdentityLearning;Integrated Security=true;", sqlopt =>
                    {
                        sqlopt.MigrationsAssembly(assemblyName);
                    });
                })
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryApiScopes(Config.GetApiScopes()).AddInMemoryClients(Config.GetClients())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                //.AddTestUsers(Config.GetUsers())
                .AddDeveloperSigningCredential()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                ;

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ConfigurationDbContext context)
        {
            IdentityServerSeedData.Seed(context);
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

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
