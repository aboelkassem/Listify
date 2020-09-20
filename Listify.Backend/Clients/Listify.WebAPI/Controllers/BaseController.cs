using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Listify.DAL;
using Listify.Lib.VMs;
using Listify.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Listify.WebAPI.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private readonly IListifyDAL _dal;
        private readonly IListifyService _service;

        public BaseController(IListifyDAL dal, IListifyService service)
        {
            _dal = dal;
            _service = service;
        }

        protected virtual async Task<Guid> GetUserIdAsync()
        {
            try
            {
                var aspNetUserId = HttpContext.User.Claims.ToList().First(s => s.Type == "sub").Value;
                var applicationUser = await _dal.ReadApplicationUserAsync(aspNetUserId);
                return applicationUser.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Guid.Empty;
        }

        protected virtual async Task<ApplicationUserVM> GetUserAsync()
        {
            try
            {
                var aspNetUserId = HttpContext.User.Claims.ToList().First(s => s.Type == "sub").Value;
                return await _dal.ReadApplicationUserAsync(aspNetUserId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
