using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Listify.WebAPI.Hubs;
using Listify.Domain.CodeFirst;
using Listify.Domain.BLL;
using Listify.DAL;
using Listify.Paths;
using Listify.BLL;
using Listify.BLL.Polls;

namespace Listify.WebAPI
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>();

            services.AddSignalR();

            services.AddCors(options => options.AddPolicy("SignalRCorsPolicy", builder => 
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowCredentials()
                .WithOrigins(Globals.ANGULAR_WEBAPP_URL);
            }));

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options => 
                {
                    options.Authority = Globals.IDENTITY_SERVER_AUTHORITY_URL;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "ListifyWebAPI";
                });

            services.AddControllers();

            //services.AddTransient<IPathsListify, PathsListify>();
            services.AddTransient<IListifyServices, ListifyServices>();

            services.AddSingleton(AutoMap.CreateAutoMapper());

            var serviceProvider = services.BuildServiceProvider();
            var listifyService = serviceProvider.GetService<IListifyServices>();

            IPingPoll pingPoll = new PingPoll(listifyService);
            pingPoll.Start(10000);
            services.AddSingleton(pingPoll);
            //services.AddSingleton<IBasePoll<PingPoll>, BasePoll<PingPoll>>();
            //services.AddSingleton<IPingPoll, PingPoll>();

            ICurrencyPoll currencyPoll = new CurrencyPoll(listifyService);
            currencyPoll.Start(5000);
            services.AddSingleton(currencyPoll);
            //services.AddSingleton<ICurrencyPoll, CurrencyPoll>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseCors("SignalRCorsPolicy");

            app.UseSignalR(routes =>
            {
                routes.MapHub<ListifyHub>("/listifyHub");
                //routes.MapHub<ChatHub>("/chatHub");
                routes.MapHub<RoomHub>("/roomHub");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
