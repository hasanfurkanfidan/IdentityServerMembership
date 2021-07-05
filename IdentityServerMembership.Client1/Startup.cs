using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerMembership.Client1
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
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookie1";
                options.DefaultChallengeScheme = "oidc";



            }).AddCookie("Cookie1").AddOpenIdConnect("oidc", opt =>
            {
                opt.SignInScheme = "Cookie1";
                opt.Authority = "https://localhost:5001";
                opt.ClientId = "Client1-Mvc";
                opt.ClientSecret = "secret";
                opt.ResponseType = "code id_token";
                opt.GetClaimsFromUserInfoEndpoint = true;
                opt.SaveTokens = true;
                opt.Scope.Add("api1.read");
                opt.Scope.Add("offline_access");
                opt.Scope.Add("CountryAndCity");
                opt.ClaimActions.MapUniqueJsonKey("country", "country");
                opt.ClaimActions.MapUniqueJsonKey("city", "city");
                opt.Scope.Add("Roles");
                opt.ClaimActions.MapUniqueJsonKey("role", "role");
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    RoleClaimType = "role"
                };

            });
            services.AddControllersWithViews();
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

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Product}/{action=Index}/{id?}");
            });
        }
    }
}
