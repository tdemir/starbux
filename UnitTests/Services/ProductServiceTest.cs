using WebApi.Helpers.Exceptions;

namespace UnitTests.Services
{
    public class ProductServiceTest : BaseServiceTest
    {
        IProductService productService;
        private IApiDbContext dbContext;
        public ProductServiceTest() : base()
        {
            dbContext = GetDbContext();
            productService = new ProductService(dbContext, mockRedisDistributedCache.Object, mockHttpContextAccessor.Object);
        }

        private Product ExampleProduct()
        {
            var p = new Product();
            p.Id = Guid.Parse("72b20590-86c7-4f40-a2e7-ff3539e12f5e");
            p.Name = "test";
            p.Price = 5;
            p.ProductType = WebApi.Enums.EProductType.Drink;
            return p;
        }

        [Fact]
        public void AddingProduct_ShouldWork()
        {
            //Arrange
            var p = ExampleProduct();

            //Act
            var result = productService.Add(p);

            //Assert
            Assert.Equal(p.Id, result.Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void WhenPriceLowerOrEqualThanZero_Should_Throw_Exception(decimal price)
        {
            //Arrange
            var p = ExampleProduct();
            p.Price = price;

            //Act & Assert
            Assert.Throws<StarbuxValidationException>(() => productService.Add(p));
        }

        [Fact]
        public void DeletingProduct_ShouldWork()
        {
            //Arrange
            var p = ExampleProduct();
            var deletedProductId = p.Id;

            //Act
            productService.Add(p);
            productService.Delete(deletedProductId);

            //Assert
            Assert.True(true);
        }

        [Fact]
        public void DeletingNotExistProduct_ShouldThrow_Exception()
        {
            //Arrange
            var deletedProductId = Guid.Parse("277ec5a9-cd0b-4032-90a4-59b081a74ad3");
            var p = ExampleProduct();

            //Act & Assert
            productService.Add(p);
            Assert.Throws<StarbuxValidationException>(() => productService.Delete(deletedProductId));
        }

        [Fact]
        public void GettingProduct_ShouldWork()
        {
            //Arrange
            var p = ExampleProduct();
            var productId = p.Id;

            //Act
            productService.Add(p);
            var result = productService.Get(productId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
        }

        [Fact]
        public void GettingNotExistProduct_ShouldReturn_Null()
        {
            //Arrange
            var productId = Guid.Parse("277ec5a9-cd0b-4032-90a4-59b081a74ad3");

            //Act
            var result = productService.Get(productId);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GettingAllProducts_ShouldWork()
        {
            //Arrange

            //Act
            var result = productService.GetAll();

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(0, result.Count);
        }

        [Fact]
        public void UpdateProduct_ShouldWork()
        {
            //Arrange
            var productId = Guid.Parse("18be78cd-49f7-4a5c-bb7c-b86e9e125d51");
            var p1 = productService.Get(productId);
            var name = p1.Name;
            var price = p1.Price;

            var p2 = new Product();
            p2.Id = productId;
            p2.Name = "new product name";
            p2.Price = 50;

            //Act
            var result = productService.Update(p2.Id, p2);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(name, p2.Name);
            Assert.NotEqual(price, p2.Price);
        }

        [Fact]
        public void UpdateProduct_ShouldThrowException_When_Product_IsNot_Exist()
        {
            //Arrange
            var p1 = ExampleProduct();
            p1.Id = Guid.Parse("bc850c0d-db51-4bb6-94da-4d57596ac168");

            //Act & Assert
            Assert.Throws<StarbuxValidationException>(() => productService.Update(p1.Id, p1));
        }
    }
}