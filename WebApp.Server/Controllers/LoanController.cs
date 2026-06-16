using Microsoft.AspNetCore.Mvc;
using WebApp.Model.Transaction;
using WebApp.Service.Transaction;

namespace WebApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _loanService.LoanList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var loan = await _loanService.GetLoanById(id);

            return loan == null ? NotFound() : Ok(loan);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LoanRequestModel model)
        {
            var result = await _loanService.AddLoan(model);

            return result ? Ok() : BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update(LoanRequestModel model)
        {
            var result = await _loanService.UpdateLoan(model);

            return result ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _loanService.DeleteLoan(id);

            return result ? Ok() : NotFound();
        }
    }
}
