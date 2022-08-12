using System.ComponentModel.DataAnnotations;
using WebApi.Enums;
using WebApi.Helpers.Attributes;

namespace WebApi.Models;

public class Product : BaseEntity
{
    [Required, MaxLength(50)]
    public string Name { get; set; }

    [DecimalPrecision(10, 2)]
    public decimal Price { get; set; }

    public EProductType ProductType { get; set; }
}
