using Microsoft.Extensions.Caching.Distributed;
using WebApi.Models;

namespace WebApi.Services
{
    public abstract class BaseService
    {
        protected readonly IApiDbContext dbContext;
        protected readonly IDistributedCache redisDistributedCache;
        protected readonly IHttpContextAccessor httpContextAccessor;

        public BaseService(IApiDbContext dbContext, IDistributedCache redisDistributedCache, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.redisDistributedCache = redisDistributedCache;
            this.httpContextAccessor = httpContextAccessor;
        }


        public Guid GetUserId()
        {
            var _userIdStr = GetClaimData(System.Security.Claims.ClaimTypes.Sid);
            if (Guid.TryParse(_userIdStr, out Guid _userId))
            {
                return _userId;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// Returns data, according to your input
        /// </summary>
        /// <param name="claimType"> example: System.Security.Claims.ClaimTypes.Sid </param>
        /// <returns></returns>
        private string GetClaimData(string claimType)
        {
            var _claimsIdentity = httpContextAccessor.HttpContext?.User?.Identity as System.Security.Claims.ClaimsIdentity;
            if (_claimsIdentity != null && _claimsIdentity.Claims != null)
            {
                var userClaim = _claimsIdentity.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.Sid).FirstOrDefault();
                if (userClaim != null)
                {
                    return userClaim.Value;
                }
            }
            return null;
        }
    }
}