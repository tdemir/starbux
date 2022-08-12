
namespace UnitTests.Services;

public class ReportServiceTest : BaseServiceTest
{

    private ReportService reportService;
    private IApiDbContext dbContext;
    public ReportServiceTest() : base()
    {
        dbContext = GetDbContext();
        reportService = new ReportService(dbContext, mockRedisDistributedCache.Object, mockHttpContextAccessor.Object);

    }


    [Fact]
    public async void GetTotalOrderAmountByCustomer_ShouldWork()
    {
        //Arrange
        var users = dbContext.Users.ToArray();
        var allProducts = dbContext.Products.ToList();
        var productsDrink = allProducts.Where(x => x.ProductType == WebApi.Enums.EProductType.Drink).ToArray();
        var productsTopping = allProducts.Where(x => x.ProductType == WebApi.Enums.EProductType.Topping).ToArray();

        var order1 = new Order()
        {
            UserId = users[0].Id
        };
        order1.UserId = users[0].Id;
        var order1OrderItem1 = new OrderItem()
        {
            OrderId = order1.Id,
            ProductId = productsDrink[0].Id,
            ProductType = WebApi.Enums.EProductType.Drink
        };
        var order1OrderItem2 = new OrderItem()
        {
            OrderId = order1.Id,
            ParentOrderItemId = order1OrderItem1.Id,
            ProductId = productsTopping[0].Id,
            ProductType = WebApi.Enums.EProductType.Topping
        };

        dbContext.Orders.Add(order1);
        dbContext.OrderItems.Add(order1OrderItem1);
        dbContext.OrderItems.Add(order1OrderItem2);
        await dbContext.SaveChangesAsync();

        //Act
        var result = reportService.GetTotalOrderAmountByCustomer();

        //Assert
        Assert.Equal(users.Length, result.Count);
        foreach (var item in result)
        {
            var expectedOrderCount = item.CustomerId == users[0].Id ? 1 : 0;
            Assert.Equal(expectedOrderCount, item.OrderCount);
        }

    }

    [Fact]
    public async void GetMostUsedToppingsForEachDrink_ShouldWork()
    {
        //Arrange
        var users = dbContext.Users.ToArray();
        var allProducts = dbContext.Products.ToList();
        var productsDrink = allProducts.Where(x => x.ProductType == WebApi.Enums.EProductType.Drink).ToArray();
        var productsTopping = allProducts.Where(x => x.ProductType == WebApi.Enums.EProductType.Topping).ToArray();

        var order1 = new Order()
        {
            UserId = users[0].Id
        };
        order1.UserId = users[0].Id;
        var order1OrderItem1 = new OrderItem()
        {
            OrderId = order1.Id,
            ProductId = productsDrink[0].Id,
            ProductType = WebApi.Enums.EProductType.Drink
        };
        var order1OrderItem2 = new OrderItem()
        {
            OrderId = order1.Id,
            ParentOrderItemId = order1OrderItem1.Id,
            ProductId = productsTopping[0].Id,
            ProductType = WebApi.Enums.EProductType.Topping
        };

        dbContext.Orders.Add(order1);
        dbContext.OrderItems.Add(order1OrderItem1);
        dbContext.OrderItems.Add(order1OrderItem2);
        await dbContext.SaveChangesAsync();

        //Act
        var result = reportService.GetMostUsedToppingsForEachDrink();

        //Assert
        Assert.Equal(productsDrink.Length * productsTopping.Length, result.Count);
        foreach (var item in result)
        {
            var expectedCount = (item.DrinkId == order1OrderItem1.ProductId && item.ToppingId == order1OrderItem2.ProductId) ? 1 : 0;
            Assert.Equal(expectedCount, item.Count);
        }

    }


    // public void ToPrettyDate_ShouldArgumentNullException_WhenCultureIsNull()
    // {
    //     var result = Record.Exception(() => DateTime.Now.ToPrettyDate(null));
    //     Assert.NotNull(result);
    //     var exception = Assert.IsType<ArgumentNullException>(result);
    //     var actual = exception.ParamName;
    //     const string expected = "culture";
    //     Assert.Equal(expected, actual);
    // }
}
