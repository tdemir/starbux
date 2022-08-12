using Microsoft.Extensions.Caching.Distributed;
using WebApi.Models;
using WebApi.RequestResponseModels;

namespace WebApi.Services;

public class CartService : BaseService, ICartService
{
    private readonly Guid UserId;
    private readonly string CacheCartKey;
    public CartService(IApiDbContext dbContext, IDistributedCache redisDistributedCache, IHttpContextAccessor httpContextAccessor) : base(dbContext, redisDistributedCache, httpContextAccessor)
    {
        UserId = GetUserId();
        CacheCartKey = string.Concat(Constants.Redis.Cart, Constants.Redis.Splitter, UserId);
    }

    /// <summary>
    /// Returns cart data. If you want to calculated price, you should add products
    /// </summary>
    /// <param name="products"></param>
    /// <returns></returns>
    public async Task<CartDto> Get(List<Models.Product> products = null)
    {
        var cartDataByteArray = await redisDistributedCache.GetAsync(CacheCartKey);
        var cartData = cartDataByteArray.DerializeFromByteArray<CartDto>();
        if (cartData == null)
        {
            cartData = new CartDto();
            cartData.UserId = UserId;
        }
        if (products != null)
        {
            cartData.Calculate(products);
        }
        return cartData;
    }

    public async Task Save(CartDto cartDto)
    {
        if (cartDto == null)
        {
            await redisDistributedCache.RemoveAsync(CacheCartKey);
            return;
        }
        await redisDistributedCache.SetAsync(CacheCartKey, cartDto.SerializeToByteArray(), new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.UtcNow.AddYears(1)
        });
    }

    public async Task AddItem(Models.Product product, Guid? parentCartItemId)
    {
        if (product == null)
        {
            throw new StarbuxValidationException("Product is not exist");
        }
        var cart = await this.Get();
        cart.AddItem(Guid.NewGuid(), product.Id, product.ProductType, parentCartItemId);
        await this.Save(cart);
    }

    public async Task RemoveCartItem(Guid cartItemId)
    {
        var cart = await this.Get();
        cart.RemoveItem(cartItemId);
        await this.Save(cart);
    }

    public async Task Pay(List<Models.Product> products)
    {
        var cart = await this.Get(products);
        if (!cart.CartItems.Any())
        {
            throw new StarbuxValidationException("You should have at least 1 drink in your cart");
        }


        using (var transaction = dbContext.BeginTransaction())
        {
            var order = new Models.Order();
            order.DiscountAmount = cart.DiscountAmount;
            order.NetPrice = cart.NetPrice;
            order.Price = cart.TotalPrice;
            order.UserId = cart.UserId;

            dbContext.Orders.Add(order);

            foreach (var cartItem in cart.CartItems)
            {
                var orderItem = new Models.OrderItem();
                orderItem.Id = cartItem.CartItemId;
                orderItem.OrderId = order.Id;
                orderItem.ParentOrderItemId = cartItem.ParentId;
                orderItem.Price = products.First(x => x.Id == cartItem.ProductId).Price;
                orderItem.ProductId = cartItem.ProductId;
                orderItem.ProductType = cartItem.ProductType;

                dbContext.OrderItems.Add(orderItem);
            }
            await dbContext.SaveChangesAsync();
            transaction.Commit();

            await this.Save(null);
        }
    }
}


