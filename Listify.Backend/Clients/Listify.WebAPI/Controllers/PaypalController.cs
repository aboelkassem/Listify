using System;
using System.Threading.Tasks;
using Listify.DAL;
using Listify.Lib.Requests;
using Listify.Lib.VMs;
using Listify.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Listify.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class PaypalController : BaseController
    {
        private readonly IListifyDAL _dal;
        private readonly IListifyService _service;

        public PaypalController(IListifyDAL dal, IListifyService service) : base(dal, service)
        {
            _dal = dal;
            _service = service;
        }

        [Route("RequestPayment")]
        [HttpPost]
        public async Task<IActionResult> RequestPayment([FromBody]PaypalPaymentRequest request)
        {
            try
            {
                request.Order.OrderId = request.OrderID;
                request.Order.PayerId = request.PayerID;

                var userId = await GetUserIdAsync();

                var purchaseVM = await _dal.CreatePurchaseAsync(request.Order, userId);
                if (purchaseVM != null)
                {
                    var purchaseConfirmed = new PurchaseConfirmedUpdateRequest
                    {
                        Purchase = purchaseVM,
                        WasChargeAccepted = true
                    };
                    return Ok(purchaseConfirmed);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }
    }
}
