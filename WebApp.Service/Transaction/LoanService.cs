using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Model.Transaction;

namespace WebApp.Service.Transaction
{
    public class LoanService : ILoanService
    {
        private readonly WebAppDbContext _dbContext;
        private readonly IMapper _mapper;

        public LoanService(
            WebAppDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #region Get All

        public async Task<List<LoanDto>> LoanList()
        {
            return await (
       from loan in _dbContext.Loan
       join user in _dbContext.Users
           on loan.UserId.ToString() equals user.Id
       where !loan.IsDeleted
       select new LoanDto
       {
           LoanNumber = loan.LoanNumber,
           UserId = loan.UserId,
           Active = loan.Active,
           LoanAmount = loan.LoanAmount,
           Rate = loan.Rate,
           Tenure = loan.Tenure,
           UserName = user.FirstName + " " + user.LastName,
       })
       .ToListAsync();
        }

        #endregion

        #region Get By Id

        public async Task<LoanDto?> GetLoanById(int id)
        {
            var loan = await _dbContext.Loan
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            return loan == null
                ? null
                : _mapper.Map<LoanDto>(loan);
        }

        #endregion

        #region Add

        public async Task<bool> AddLoan(LoanRequestModel model)
        {
            var entity = _mapper.Map<Data.Entity.Loan>(model);

            entity.IsDeleted = false;
            entity.Active = true;
            entity.F_Created_Date_Time = DateTime.UtcNow;
            entity.F_Updated_Date_Time = DateTime.UtcNow;
            entity.F_User_Index_Update = DateTime.UtcNow;
            await _dbContext.Loan.AddAsync(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        #endregion

        #region Update

        public async Task<bool> UpdateLoan(LoanRequestModel model)
        {
            var loan = await _dbContext.Loan
                .FirstOrDefaultAsync(x => x.Id == model.Id && !x.IsDeleted);

            if (loan == null)
                return false;

            loan.LoanAmount = model.LoanAmount;
            loan.Rate = model.Rate;
            loan.Active = model.Active;
            loan.F_Updated_Date_Time = DateTime.UtcNow;

            _dbContext.Loan.Update(loan);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        #endregion

        #region Delete (Soft Delete)

        public async Task<bool> DeleteLoan(int id)
        {
            var loan = await _dbContext.Loan
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (loan == null)
            {
                return false;
            }

            loan.IsDeleted = true;
            loan.F_Updated_Date_Time = DateTime.UtcNow;

            _dbContext.Loan.Update(loan);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        #endregion
    }
}
