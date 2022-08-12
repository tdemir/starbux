using WebApi.Enums;
using WebApi.Helpers.Attributes;

namespace WebApi.Models;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid? ParentOrderItemId { get; set; }
    public Guid ProductId { get; set; }

    [DecimalPrecision(10, 2)]
    public decimal Price { get; set; }
    public EProductType ProductType { get; set; }
}
