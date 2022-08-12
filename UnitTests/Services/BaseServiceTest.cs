using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi;
using WebApi.Enums;

namespace UnitTests.Services;

public abstract class BaseServiceTest
{
    protected Mock<IDistributedCache> mockRedisDistributedCache;
    protected Mock<IHttpContextAccessor> mockHttpContextAccessor;


    public BaseServiceTest()
    {
        SetupCacheObject();
        SetupHttpContextAccessor();
    }

    protected IConfiguration GetConfiguration()
    {
        var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:Key","ae0cd136565342bbad59c41b25189c87"},
            {"Jwt:Issuer","StarbuxApp"},
            {"Jwt:Audience","StarbuxAppAudience"},
            {"Jwt:TokenExpiryInMinutes","60"},
            {"Jwt:RefreshTokenExpiryInMinutes","120"}
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        return configuration;
    }

    private void SetupCacheObject()
    {
        mockRedisDistributedCache = new Mock<IDistributedCache>();



        mockRedisDistributedCache.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as byte[]);
        mockRedisDistributedCache.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>())).Verifiable();
        mockRedisDistributedCache.Setup(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();

    }

    private void SetupHttpContextAccessor()
    {
        var _userId = "d20733b1-fe94-482c-bd80-d82f7d8d0ef3";

        mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        var claims = new List<System.Security.Claims.Claim>();
        claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Sid, _userId));

        var claimIdentity = new List<System.Security.Claims.ClaimsIdentity>();
        claimIdentity.Add(new System.Security.Claims.ClaimsIdentity(claims));

        var context = new DefaultHttpContext();
        context.User = new System.Security.Claims.ClaimsPrincipal(claimIdentity);

        context.Request.Headers[Constants.Authorization] = "RandomToken" + DateTime.UtcNow.Ticks.ToString();
        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

    }

    protected IApiDbContext GetDbContext()
    {
        var dbName = "Starbux_" + Guid.NewGuid().ToString();
        var dbContextOptions = new DbContextOptionsBuilder<ApiDbContext>()
                                .UseInMemoryDatabase(dbName)
                                // don't raise the error warning us that the in memory db doesn't support transactions
                                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                                .Options;

        var db = new ApiDbContext(dbContextOptions);

        if (db.Users.Any())
        {
            return db;
        }

        var encryptedText = "123".MD5Encryption();
        var createdDate = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        db.Users.AddRange(new User
        {
            Id = Guid.Parse("35062e26-cd41-4123-b824-ea06b0f19596"),
            Email = "admin@test.com",
            Password = encryptedText,
            IsAdmin = true,
            CreatedDate = createdDate
        }, new User
        {
            Id = Guid.Parse("d20733b1-fe94-482c-bd80-d82f7d8d0ef3"),
            Email = "user1@test.com",
            Password = encryptedText,
            IsAdmin = false,
            CreatedDate = createdDate
        }, new User
        {
            Id = Guid.Parse("ed8f558e-1ebb-418c-a1bb-db953ba90c58"),
            Email = "user2@test.com",
            Password = encryptedText,
            IsAdmin = false,
            CreatedDate = createdDate
        }, new User
        {
            Id = Guid.Parse("f2e6040a-df19-4cec-b929-7e2c5c0e53fe"),
            Email = "user3@test.com",
            Password = encryptedText,
            IsAdmin = false,
            CreatedDate = createdDate
        });

        db.Products.AddRange(new Product
        {
            Id = Guid.Parse("e49854e8-8b95-47ce-a622-d676e94b3e75"),
            Name = "Black Coffee",
            Price = 4m,
            ProductType = EProductType.Drink,
            CreatedDate = createdDate
        }, new Product
        {
            Id = Guid.Parse("f5ccffc4-1b21-4581-a71d-96bc70791255"),
            Name = "Latte",
            Price = 5m,
            ProductType = EProductType.Drink,
            CreatedDate = createdDate
        }, new Product
        {
            Id = Guid.Parse("1be28499-751c-4d31-9b90-7307c5448031"),
            Name = "Mocha",
            Price = 6m,
            ProductType = EProductType.Drink,
            CreatedDate = createdDate
        }, new Product
        {
            Id = Guid.Parse("0e7ca561-a147-460c-98dd-c22556019836"),
            Name = "Tea",
            Price = 3m,
            ProductType = EProductType.Drink,
            CreatedDate = createdDate
        });

        db.Products.AddRange(new Product
        {
            Id = Guid.Parse("ffcdafd8-5906-4414-a364-0d7f1936920b"),
            Name = "Milk",
            Price = 2m,
            ProductType = EProductType.Topping,
            CreatedDate = createdDate
        }, new Product
        {
            Id = Guid.Parse("a3af147b-2e43-498a-9f7a-020f98ceb0a4"),
            Name = "Hazelnut syrup",
            Price = 3m,
            ProductType = EProductType.Topping,
            CreatedDate = createdDate
        }, new Product
        {
            Id = Guid.Parse("a44fcd45-1d18-467c-af9a-21de7194d243"),
            Name = "Chocolate sauce",
            Price = 5m,
            ProductType = EProductType.Topping,
            CreatedDate = createdDate
        }, new Product
        {
            Id = Guid.Parse("18be78cd-49f7-4a5c-bb7c-b86e9e125d51"),
            Name = "Lemon",
            Price = 2m,
            ProductType = EProductType.Topping,
            CreatedDate = createdDate
        });
        db.SaveChanges();

        return db;
    }

}
