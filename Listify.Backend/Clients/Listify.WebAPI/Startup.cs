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
using Listify.Services;

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
                // this lambda determines whether user consent for non-essential cookies is needed for a given request
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>();

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });

            services.AddSignalR().AddNewtonsoftJsonProtocol();

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
                    options.ApiName = "listifyWebAPI";
                    options.SaveToken = true;
                });

            services.AddControllers().AddNewtonsoftJson();

            //services.AddTransient<IPathsListify, PathsListify>();
            services.AddTransient<IListifyDAL, ListifyDAL>();
            services.AddTransient<IListifyService, ListifyService>();

            services.AddSingleton(AutoMap.CreateAutoMapper());

            var serviceProvider = services.BuildServiceProvider();
            var listifyDAL = serviceProvider.GetService<IListifyDAL>();

            IPingPoll pingPoll = new PingPoll(listifyDAL);
            pingPoll.Start(45000);
            services.AddSingleton(pingPoll);
            //services.AddScoped<IPingPoll, PingPoll>();

            ICurrencyPoll currencyPoll = new CurrencyPoll(listifyDAL);
            currencyPoll.Start(30000);
            services.AddSingleton(currencyPoll);
            //services.AddScoped<ICurrencyPoll, CurrencyPoll>();

            //IRoomsOnlinePool roomsOnline = new RoomsOnlinePool(listifyDAL);
            //roomsOnline.Start(45000);
            //services.AddSingleton(roomsOnline);
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

            app.UseCors("SignalRCorsPolicy");

            app.UseSignalR(routes =>
            {
                routes.MapHub<ListifyHub>("/listifyHub");
                //routes.MapHub<ChatHub>("/chatHub");
                routes.MapHub<RoomHub>("/roomHub");
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
