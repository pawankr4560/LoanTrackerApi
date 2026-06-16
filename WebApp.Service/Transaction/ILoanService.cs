using WebApp.Model.Transaction;

namespace WebApp.Service.Transaction
{
    public interface ILoanService
    {
        Task<bool> AddLoan(LoanRequestModel model);
        Task<bool> DeleteLoan(int id);
        Task<LoanDto?> GetLoanById(int id);
        Task<List<LoanDto>> LoanList();
        Task<bool> UpdateLoan(LoanRequestModel model);
    }
}