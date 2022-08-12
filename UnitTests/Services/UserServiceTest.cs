using WebApi.Helpers.Exceptions;
using WebApi.RequestResponseModels;

namespace UnitTests.Services
{
    public class UserServiceTest : BaseServiceTest
    {
        IUserService userService;
        private IApiDbContext dbContext;

        public UserServiceTest() : base()
        {
            var configuration = GetConfiguration();
            dbContext = GetDbContext();

            var mockToken = new Mock<ITokenService>();
            mockToken.Setup(x => x.GenerateJWT(It.IsAny<User>(), It.IsAny<DateTime>())).Returns("tokenData" + DateTime.UtcNow.Ticks.ToString());

            userService = new UserService(configuration, mockToken.Object, dbContext, mockRedisDistributedCache.Object, mockHttpContextAccessor.Object);
        }

        [Fact]
        public async void UserLogin_ShouldWork()
        {
            //Arrange
            var u = new UserLoginRequest();
            u.Email = "user1@test.com";
            u.Password = "123";

            //Act
            var result = await userService.Login(u);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Token);
            Assert.NotEmpty(result.Token);
        }

        [Fact]
        public async void WhenUserLogin_Should_Throw_Exception_IfUserNotExist()
        {
            //Arrange
            var u = new UserLoginRequest();
            u.Email = "aaa@test.com";
            u.Password = "123456";

            //Act
            var result = await Assert.ThrowsAsync<StarbuxValidationException>(async () => await userService.Login(u));

            //Assert
            Assert.Equal("User not found", result.Message);
        }
    }
}