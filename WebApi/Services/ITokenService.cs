namespace WebApi.Services;

public interface ITokenService
{
    string GenerateJWT(Models.User user, DateTime expireDate);
}
