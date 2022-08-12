using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using WebApi.Models;
using WebApi.RequestResponseModels;

namespace WebApi.Services;

public class UserService : BaseService, IUserService
{
    private readonly IConfiguration configuration;
    private readonly ITokenService tokenService;
    public UserService(IConfiguration configuration, ITokenService tokenService, IApiDbContext dbContext, IDistributedCache redisDistributedCache, IHttpContextAccessor httpContextAccessor) : base(dbContext, redisDistributedCache, httpContextAccessor)
    {
        this.configuration = configuration;
        this.tokenService = tokenService;
    }

    private int GetTokenExpiryMin
    {
        get
        {
            return configuration.GetValue<int>("Jwt:TokenExpiryInMinutes");
        }
    }

    public async Task<UserLoginResponse> Login(UserLoginRequest model)
    {
        var currentDate = DateTime.UtcNow;
        var loginResponse = new UserLoginResponse();
        loginResponse.TokenExpireDate = currentDate.AddMinutes(GetTokenExpiryMin);

        var encryptedPassword = model.Password.MD5Encryption();

        var user = dbContext.Users.SingleOrDefault(x => x.Email == model.Email &&
                                                       x.Password == encryptedPassword &&
                                                       !x.DeletedDate.HasValue);
        if (user == null)
        {
            throw new StarbuxValidationException("User not found");
        }
        loginResponse.Token = tokenService.GenerateJWT(user, loginResponse.TokenExpireDate);

        using (var transaction = dbContext.BeginTransaction())
        {
            try
            {

                user.LastLoginDate = currentDate;

                var oldUserLogins = dbContext.UserLogins.Where(x => !x.DeletedDate.HasValue && x.TokenExpireDate >= currentDate && x.UserId == user.Id).ToList();
                foreach (var oldUserLogin in oldUserLogins)
                {
                    oldUserLogin.DeletedDate = currentDate;
                    await CacheRemoveUserData(oldUserLogin.Token);
                }

                var ul = new Models.UserLogin();
                ul.CreatedDate = currentDate;
                ul.Token = loginResponse.Token;
                ul.TokenExpireDate = loginResponse.TokenExpireDate;
                ul.UserId = user.Id;

                dbContext.UserLogins.Add(ul);

                await dbContext.SaveChangesAsync();

                await CacheAddUserLogin(loginResponse.Token, user.Id, loginResponse.TokenExpireDate);

                transaction.Commit();
            }
            catch (System.Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        return loginResponse;
    }

    private Task CacheAddUserLogin(string token, Guid userId, DateTime expireDate)
    {
        return redisDistributedCache.SetAsync(token.GetTokenCacheKey(), Encoding.UTF8.GetBytes(userId.ToString()), new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
        {
            AbsoluteExpiration = expireDate
        });
    }

    private Task CacheRemoveUserData(string token)
    {
        return redisDistributedCache.RemoveAsync(token.GetTokenCacheKey());
    }

    public async Task Logout()
    {
        var token = this.httpContextAccessor?.HttpContext?.GetBearerToken();
        if (string.IsNullOrEmpty(token))
        {
            throw new StarbuxValidationException("Token not found");
        }

        var currentDate = DateTime.UtcNow;

        using (var transaction = dbContext.BeginTransaction())
        {
            var userlogin = dbContext.UserLogins.FirstOrDefault(x => !x.DeletedDate.HasValue && x.TokenExpireDate >= currentDate && x.Token == token);
            if (userlogin != null)
            {
                userlogin.DeletedDate = currentDate;
                await dbContext.SaveChangesAsync();
            }
            await CacheRemoveUserData(token);

            transaction.Commit();
        }



    }
}
