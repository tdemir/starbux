using System.Net;
using Microsoft.Extensions.Caching.Distributed;

namespace WebApi.Middlewares
{
    public class TokenManagerMiddleware
    {
        private static string[] EXCLUDED_PATHS = new string[]
        {
            "/api/user/login",
            "/api/user/test"
        };
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenManagerMiddleware> _logger;
        private readonly IDistributedCache _redisDistributedCache;

        public TokenManagerMiddleware(RequestDelegate next, ILogger<TokenManagerMiddleware> logger, IDistributedCache redisDistributedCache)
        {
            _next = next;
            _logger = logger;
            _redisDistributedCache = redisDistributedCache;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (IsExcludedPage(httpContext))
            {
                await _next(httpContext);
                return;
            }

            var _token = httpContext.GetBearerToken();
            if (string.IsNullOrWhiteSpace(_token))
            {
                await _next(httpContext);
                return;
            }

            if (await IsTokenInValidList(_token))
            {
                await _next(httpContext);
                return;
            }

            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        private bool IsExcludedPage(HttpContext context)
        {
            var _path = string.Empty;
            try
            {
                _path = context.Request.Path.Value.ToLowerInvariant();
            }
            catch
            {

            }

            if (!string.IsNullOrWhiteSpace(_path))
            {
                foreach (var _excludedPath in EXCLUDED_PATHS)
                {
                    if (_path.Length < _excludedPath.Length)
                    {
                        continue;
                    }
                    if (_path.StartsWith(_excludedPath))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private async Task<bool> IsTokenInValidList(string token)
        {
            var _data = await _redisDistributedCache.GetAsync(token.GetTokenCacheKey());
            return _data != null;
        }

    }
}