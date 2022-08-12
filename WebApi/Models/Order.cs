using WebApi.Helpers.Attributes;

namespace WebApi.Models;

public class Order : BaseEntity
{
    public Guid UserId { get; set; }

    [DecimalPrecision(10, 2)]
    public decimal Price { get; set; } = 0m;

    [DecimalPrecision(10, 2)]
    public decimal DiscountAmount { get; set; } = 0m;

    [DecimalPrecision(10, 2)]
    public decimal NetPrice { get; set; } = 0m;
}