using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Listify.Domain.Lib.Entities;
using Listify.Paths;
using Microsoft.AspNetCore.Mvc;

namespace Listify.WebAPI.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {

        }

        //public async Task<ApplicationUser> GetAsync()
        //{
        //    var accessToken = Request.Headers.First(s => s.Key == "Authorization").Value[0].Substring(7);
        //    //var userInfoClient = new IdentityModel.Client.UserInfoClient();
        //    var client = new HttpClient();

        //    var disco = await client.GetDiscoveryDocumentAsync(Globals.IDENTITY_SERVER_AUTHORITY_URL);
        //    var response = await client.GetUserInfoAsync(new UserInfoRequest
        //    {
        //        Address = disco.UserInfoEndpoint,
        //        Token = accessToken
        //    });

        //    return new ApplicationUser
        //    {
        //        AspNetUserId = response.Claims.ToList().First(s => s.Type == "preferred_username").Value,
        //        Username = response.Claims.ToList().First(s => s.Type == "name").Value
        //    };
        //}
    }
}
