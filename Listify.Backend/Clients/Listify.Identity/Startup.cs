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
using Listify.Paths;

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
                    options.ClientId = Globals.GOOGLE_ATHENTICATION_CLIENT_ID;
                    options.ClientSecret = Globals.GOOGLE_ATHENTICATION_CLIENT_SECRET;
                })
                .AddTwitch(options =>
                {
                    options.ClientId = Globals.TWITCH_ATHENTICATION_CLIENT_ID;
                    options.ClientSecret = Globals.TWITCH_ATHENTICATION_CLIENT_SECRET;
                })
                .AddFacebook(options =>
                {
                    options.ClientId = Globals.FACEBOOK_ATHENTICATION_CLIENT_ID;
                    options.ClientSecret = Globals.FACEBOOK_ATHENTICATION_CLIENT_SECRET;
                })
                //.addtwitter(options =>
                //{
                //    options.consumerkey = Globals.TWITTER_ATHENTICATION_CLIENT_ID;
                //    options.consumersecret = Globals.TWITTER_ATHENTICATION_CLIENT_SECRET;
                //})
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = Globals.MICROSOFT_ATHENTICATION_CLIENT_ID;
                    options.ClientSecret = Globals.MICROSOFT_ATHENTICATION_CLIENT_SECRET;
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
