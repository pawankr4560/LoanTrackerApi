using WebApp.Model.Auth;

namespace WebApp.Service.Auth
{
    public interface IAuthService
    {
        Task<bool> SignUpUserAsync(SignUpRequestModel model);
    }
}