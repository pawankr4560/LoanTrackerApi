using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
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
        private readonly HttpClient _httpClient;

        public AuthService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            WebAppDbContext dbContext,
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            IMapper mapper,
            HttpClient httpClient)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<bool> SignUpUser(SignUpRequestModel model)
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
                user.UserName = model.Email;
                user.EmailConfirmed = true;
                user.Id = Guid.NewGuid().ToString();
                var res = await _userManager.CreateAsync(user,model.Password);
                await _userManager.AddToRoleAsync(user, "User");
                await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return true;
            }
            catch (Exception) { throw; }
        }

        public async Task<string> Login(LoginRequestModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                if (user is null)
                    throw new Exception("User not exist.");

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (result.Succeeded)
                    return await CreateToken(user);
                throw new Exception("Invalid email or password.");
            }
            catch (Exception) { throw; }
        }

        public async Task<string> CreateToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
               new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
               new Claim("Email", user.Email),
               new Claim("Id", user.Id),
               new Claim("FirstName", user.FirstName ?? string.Empty),
               new Claim("LastName", user.LastName ?? string.Empty),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            var identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("zxcvldjhytpsmngvzwjsetveuydededexw_@jfdsfs=__"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "https://localhost:7176/",
                "https://localhost:7176/",
                identity.Claims,
                signingCredentials: creds
            );

            user.LoginCount += 1;
            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new JwtSecurityTokenHandler().WriteToken(token); 
        }

        public async Task<UserAddressResponseModel> GetAddress(string address)
        {
            try
            {
                string url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={Uri.EscapeDataString(address)}&key={"AIzaSyCKoMQs - ZZgHFUNglLpdGlpsXiD2JrXjhE"}&types=establishment";
               
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<UserAddressResponseModel>(res);
                }
                else
                {
                    throw new HttpRequestException($"Error calling Google Places API: {response.StatusCode}");
                }
            }
            catch (Exception) { throw; }
        }
    }
}
