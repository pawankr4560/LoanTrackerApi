using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.V2;
using WebApp.Model.Common;
using WebApp.Model.Order;

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

        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout(CheckoutRequestModel model)
        {
            try
            {
                var result = await _stripeService.CreateCheckout(model.Amount,model.CustomerId,model.Email,model.CardId);
                return Ok(new ApiResponse(true, null, result));
            }
            catch (Exception ex) 
            {
                return BadRequest(new ApiResponse(false, ex.Message, null));
            }
        }

        [HttpPost("CreateCard")]
        public async Task<IActionResult> CreateCard(CardRequestModel model)
        {
            try
            {
                var result = await _stripeService.CreateCard(model);
                return Ok(new ApiResponse(true, null, result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(false, ex.Message, null));
            }
        }

        [HttpDelete("DeleteCard")]
        public async Task<IActionResult> RemoveCard(string cardId)
        {
            try
            {
                var result = await _stripeService.DeleteCard(cardId);
                return Ok(new ApiResponse(true, null, result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(false, ex.Message, null));
            }
        }
    }
}
