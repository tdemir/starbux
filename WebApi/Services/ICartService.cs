using WebApi.RequestResponseModels;

namespace WebApi.Services;

public interface ICartService
{
    Task<CartDto> Get(List<Models.Product> products = null);
    Task Save(CartDto cartDto);
    Task AddItem(Models.Product product, Guid? parentCartItemId);
    Task RemoveCartItem(Guid cartItemId);

    Task Pay(List<Models.Product> products);
}


