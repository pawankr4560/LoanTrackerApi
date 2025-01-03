using System.Security.Claims;
using WebApp.Data.Entity;
using WebApp.Model.Auth;

namespace WebApp.Service.Auth
{
    public interface IAuthService
    {
        Task<bool> SignUpUser(SignUpRequestModel model);
        Task<string> Login(LoginRequestModel model);
        Task<UserAddressResponseModel> GetAddress(string address);
        string GetToken(List<Claim> claims);
        Task<List<User>> UserList();
    }
}