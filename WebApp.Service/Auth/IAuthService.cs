using WebApp.Model.Auth;

namespace WebApp.Service.Auth
{
    public interface IAuthService
    {
        Task<bool> SignUpUser(SignUpRequestModel model);
        Task<string> Login(LoginRequestModel model);
    }
}