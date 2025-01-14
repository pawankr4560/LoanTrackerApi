using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using WebApp.Model.Common;
using WebApp.Service.Stripe;

namespace WebApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StripeController : ControllerBase
    {
        private readonly IStripeService _stripeService;

        public StripeController(IStripeService stripeService)
        {
            _stripeService = stripeService;
        }

        [HttpPost("Customer")]
        public async Task<IActionResult> CreateCustomer(string name, string email)
        {
            try
            {
                var result = await _stripeService.CreateCustomer(name, email);
                return Ok(new ApiResponse(true, null, result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(false, ex.Message, null));
            }
        }

        [HttpGet("CreateProduct")]
        public async Task<IActionResult> CreateProduct(double amount)
        {
            try
            {
                var result = await _stripeService.CreateProduct(amount);
                return Ok(new ApiResponse(true, null, result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(false, ex.Message, null));
            }
        }
    }
}
