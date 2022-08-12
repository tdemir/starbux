using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using WebApi.Models;
using WebApi.Services;

namespace WebApi
{
    public static class AppExtensionMethods
    {
        public static void LoadLogConfiguration(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((ctx, lc) =>
                {
                    lc.MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .WriteTo.PostgreSQL(
                            connectionString: ctx.Configuration.GetConnectionString("LogDbConnection"),
                            tableName: "logs",
                            schemaName: "public",
                            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug,
                            needAutoCreateTable: true
                        );
                });
        }

        public static void LoadHealthChecks(this WebApplicationBuilder builder)
        {
            IConfiguration _configuration = builder.Configuration;

            builder.Services
                .AddHealthChecks()

                .AddNpgSql(npgsqlConnectionString: _configuration.GetConnectionString("DefaultConnection"),
                    name: "Db Check",
                    failureStatus: HealthStatus.Unhealthy | HealthStatus.Degraded,
                    tags: new string[] { "db" })

                .AddRedis(
                    redisConnectionString: _configuration.GetConnectionString("RedisConnection"),
                    name: "Cache Check",
                    failureStatus: HealthStatus.Unhealthy | HealthStatus.Degraded,
                    tags: new string[] { "cache" });
        }

        public static void LoadServiceMethods(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(action =>
                    {
                        action.Configuration = configuration.GetConnectionString("RedisConnection");
                    });


            #region EFCore settings

            services.AddEntityFrameworkNpgsql().AddDbContext<ApiDbContext>(options => options.SetDbContextOptionsBuilder(configuration), ServiceLifetime.Scoped);

            services.AddScoped<IApiDbContext>(provider => provider.GetService<ApiDbContext>());

            #endregion

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                //options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

            services.AddHttpContextAccessor();

            #region ServiceInjections

            services.AddScoped<ITokenService, TokenService>();

            foreach (Type mytype in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                     .Where(mytype => mytype.BaseType == typeof(Services.BaseService)))
            {
                var _interfaceName = "I" + mytype.Name;
                var _interfaceType = mytype.GetInterface(_interfaceName);
                if (_interfaceType == null)
                {
                    continue;
                }

                services.AddScoped(_interfaceType, mytype);
            }

            #endregion
        }

        public static string MD5Encryption(this string encryptionText)
        {
            var md5 = MD5.Create();
            byte[] array = Encoding.UTF8.GetBytes(encryptionText);
            array = md5.ComputeHash(array);

            var sb = new StringBuilder();
            foreach (byte ba in array)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }
            return sb.ToString();
        }

        public static string GetBearerToken(this HttpContext context)
        {
            try
            {
                var authorizationHeader = context.Request.Headers[Constants.Authorization];
                if (authorizationHeader != Microsoft.Extensions.Primitives.StringValues.Empty)
                {
                    return authorizationHeader.Single().Split(" ").Last();
                }
            }
            catch
            {

            }
            return string.Empty;
        }


        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static byte[] SerializeToByteArray<T>(this T obj)
        {
            if (obj == null)
                return null;

            var str = System.Text.Json.JsonSerializer.Serialize(obj);
            return System.Text.Encoding.UTF8.GetBytes(str);
        }

        public static T DerializeFromByteArray<T>(this byte[] data)
        {
            if (data == null)
                return default(T);

            var str = System.Text.Encoding.UTF8.GetString(data);
            var rval = JsonSerializer.Deserialize<T>(str);
            if (rval == null)
            {
                return default(T);
            }
            return rval;
        }

        public static string GetTokenCacheKey(this string token)
        {
            var cacheKey = string.Concat(Constants.Redis.ValidAccessToken,
                                                           Constants.Redis.Splitter,
                                                           token.Base64Encode());
            return cacheKey;
        }
    }
}