using WebApi.RequestResponseModels;

namespace WebApi.Services;

public interface IUserService
{
    Task<UserLoginResponse> Login(UserLoginRequest model);
    Task Logout();
}
