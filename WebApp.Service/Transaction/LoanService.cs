using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Model.Transaction;
using WebApp.Service.Message;

namespace WebApp.Service.Transaction
{
    public class LoanService : ILoanService
    {
        private readonly WebAppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;

        public LoanService(
            WebAppDbContext dbContext,
            IMapper mapper,
            IMessageService messageService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _messageService =messageService;
        }

        #region Get All

        public async Task<List<LoanDto>> LoanList()
        {
            var loans = await (
            from loan in _dbContext.Loan
            join user in _dbContext.Users
                on loan.UserId equals user.Id
            where !loan.IsDeleted
            select new LoanDto
            {
                Id = loan.Id,
                Active = loan.Active,
                UserName = $"{user.FirstName} {user.LastName}",
                LoanAmount = loan.LoanAmount,
                LoanNumber = loan.LoanNumber,
                Rate = loan.Rate,
                EMI = loan.EMI,
                Tenure = loan.Tenure,
                CreatedDateTime = DateTime.Now,
            }
        ).ToListAsync();
            return loans;
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
            // 1. Check Existing Loan FIRST before mapping (saves memory/processing)
            var existingLoan = await _dbContext.Loan
                .FirstOrDefaultAsync(x => x.LoanNumber == model.LoanNumber);

            if (existingLoan != null)
                throw new Exception("This Loan number already exists.");

            // 2. Map model to entity
            var entity = _mapper.Map<Data.Entity.Loan>(model);

            entity.IsDeleted = false;
            entity.F_Created_Date_Time = DateTime.UtcNow;
            entity.F_Updated_Date_Time = DateTime.UtcNow;
            entity.F_User_Index_Update = DateTime.UtcNow;

            // 3. Save to Database
            await _dbContext.Loan.AddAsync(entity);
            var isSavedSuccessfully = await _dbContext.SaveChangesAsync() > 0;

            // 4. Send Confirmation Message only if DB save succeeded
            if (isSavedSuccessfully)
            {
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(x => x.Id == model.UserId);

                if (user != null && !string.IsNullOrEmpty(user.PhoneNumber))
                {
                    await _messageService.SendLoanSmsAsync(
                        user.PhoneNumber,
                        entity.LoanNumber,
                        entity.LoanAmount.ToString()
                    );
                }
            }

            return isSavedSuccessfully;
        }

        public async Task<CreateLoanDTO> GetLoanNumber()
        {
            var currentYear = DateTime.UtcNow.Year;

            var lastLoanNumber = await _dbContext.Loan
                .Where(x => !x.IsDeleted &&
                            x.LoanNumber.StartsWith($"{currentYear}-GKFIN-"))
                .OrderByDescending(x => x.Id)
                .Select(x => x.LoanNumber)
                .FirstOrDefaultAsync();

            int nextSequence = 1;

            if (!string.IsNullOrWhiteSpace(lastLoanNumber))
            {
                var sequencePart = lastLoanNumber.Split('-').Last();

                if (int.TryParse(sequencePart, out int sequence))
                {
                    nextSequence = sequence + 1;
                }
            }

            var customers = await _dbContext.Users
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.FirstName)
                .Select(x => new CustomerDropdownDto
                {
                    Id = x.Id,
                    CustomerName = $"{x.FirstName} {x.LastName}"
                })
                .ToListAsync();

            return new CreateLoanDTO
            {
                LoanNumber = $"{currentYear}-GKFIN-{nextSequence:D5}",
                CustomerList = customers
            };
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
            loan.EMI = model.EMI;
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
