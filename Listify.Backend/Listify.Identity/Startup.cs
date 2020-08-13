using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Listify.Identity.Models;
using Listify.Identity.Data;

namespace Listify.Identity
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddAspNetIdentity<ApplicationUser>()
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 300;
                });

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                var filename = Path.Combine(Environment.WebRootPath, "cert.pfx");

                if (!File.Exists(filename))
                {
                    throw new FileNotFoundException("Signing Certificate is missing!");
                }

                var cert = new X509Certificate2(filename, Constants.CERTIFICATE_PASSWORD);
                builder.AddSigningCredential(cert);
            }

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "258367043031-4hp1ir4e7hk1u7vhn7jipg5ei2qt3r1u.apps.googleusercontent.com";
                    options.ClientSecret = "I-bl612AAYV0JRLks_f-Rl2J";
                })
                .AddTwitch(options =>
                {
                    options.ClientId = "um50fja3x21e2a8208u7lyau8ihv0t";
                    options.ClientSecret = "snnza9njqez85i0a575x9enfsvzkur";
                })
                .AddFacebook(options =>
                {
                    options.ClientId = "221800619181076";
                    options.ClientSecret = "7d22dcb3d22b19c6057629521f2f80b0";
                })
                //.addtwitter(options =>
                //{
                //    options.consumerkey = "";
                //    options.consumersecret = "";
                //})
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = "d3182534-3010-41d9-8709-bcafcfc71c53";
                    options.ClientSecret = "CuU448A1~h1rt3Y.WeoiC-~rzBXI-2cb-K";
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //app.UseCors();
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
