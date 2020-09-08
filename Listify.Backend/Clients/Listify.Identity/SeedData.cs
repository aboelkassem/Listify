using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Listify.Identity.Models;
using Listify.Identity.Data;

namespace Listify.Identity
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Seeding database...");

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
                
                EnsureSeedData(scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>());

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var admin = userMgr.FindByNameAsync("aboelkassem").Result;

                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = "aboelkassem",
                        Email = "mohamedabdelrahman972@gmail.com"
                    };

                    var result = userMgr.CreateAsync(admin, "asdklnAsdfio3w4895!").Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(admin, new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, "Mohamed Abdelrahman"),
                        new Claim(JwtClaimTypes.GivenName, "aboelkassem"),
                        new Claim(JwtClaimTypes.FamilyName, "hanfy"),
                        new Claim(JwtClaimTypes.Email, "mohamedabdelrahman972@gmail.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true"),
                        new Claim(JwtClaimTypes.WebSite, "https://www.Listify.com"),
                        new Claim(JwtClaimTypes.Role, "admin"),
                    }).Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    Console.WriteLine("Admin created");
                }
                else
                {
                    Console.WriteLine("Admin already exists");
                }
            }

            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            foreach (var client in Config.GetClients().ToList())
            {
                if (!context.Clients.Any(s => s.ClientId == client.ClientId))
                {
                    context.Clients.Add(client.ToEntity());
                }
            }
            context.SaveChanges();

            foreach (var identityResource in Config.GetIdentityResources().ToList())
            {
                if (!context.IdentityResources.Any(s => s.Name == identityResource.Name))
                {
                    context.IdentityResources.Add(identityResource.ToEntity());
                }
            }
            context.SaveChanges();

            foreach (var apiResource in Config.GetApiResources().ToList())
            {
                if (!context.ApiResources.Any(s => s.Name == apiResource.Name))
                {
                    context.ApiResources.Add(apiResource.ToEntity());
                }
            }

            context.SaveChanges();
        }
    }
}
