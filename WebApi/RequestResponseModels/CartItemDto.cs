using WebApi.Enums;

namespace WebApi.RequestResponseModels;

public class CartItemDto
{
    public Guid CartItemId { get; set; }
    public Guid? ParentId { get; set; }
    public Guid ProductId { get; set; }
    public EProductType ProductType { get; set; }
}
