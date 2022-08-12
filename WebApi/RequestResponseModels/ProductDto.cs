using System.ComponentModel.DataAnnotations;
using WebApi.Helpers.Attributes;

namespace WebApi.RequestResponseModels;

public class ProductDto
{
    public Guid? Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; }

    [Range(0.01, 100000000, ErrorMessage = "Price must be greater than zero !")]
    public decimal Price { get; set; }

    [ProductTypeEnumValidation]
    public string ProductType { get; set; }

}
