using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Data.Entity;
using WebApp.Model.Auth;

namespace WebApp.Service.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly WebAppDbContext _dbContext;
        private readonly IMapper _mapper;

        public AuthService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            WebAppDbContext dbContext,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> SignUpUserAsync(SignUpRequestModel model)
        {
            try
            {
                if (await _dbContext.Users.AnyAsync(u => u.Email == model.Email))
                {
                    throw new Exception("User is already exists.");
                }
                var user = _mapper.Map<User>(model);
                user.CreatedOn = DateTime.UtcNow;
                user.IsActive = true;
                user.IsDeleted = false;
                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, "User");
                await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return true;
            }
            catch (Exception) { throw; }
        }
    }
}
