using WebApi.Enums;
using WebApi.RequestResponseModels;

namespace WebApi.Mappings
{
    public static class ProductMapping
    {
        public static Models.Product ToProduct(this ProductDto productDto)
        {
            if (productDto == null)
            {
                return default;
            }
            var p = new Models.Product();
            if (productDto.Id.HasValue)
            {
                p.Id = productDto.Id.Value;
            }
            p.Name = productDto.Name;
            p.Price = productDto.Price;
            if (!Enum.TryParse<EProductType>(productDto.ProductType, true, out EProductType _productTypeResult))
            {
                throw new StarbuxValidationException("Invalid product type");
            }
            p.ProductType = _productTypeResult;
            return p;
        }

        public static List<Models.Product> ToProducts(this List<ProductDto> productDtos)
        {
            if (productDtos == null)
            {
                return default;
            }
            return productDtos.Select(x => x.ToProduct()).ToList();
        }

        public static ProductDto ToProductDto(this Models.Product product)
        {
            if (product == null)
            {
                return default;
            }
            var p = new ProductDto();
            p.Id = product.Id;
            p.Name = product.Name;
            p.Price = product.Price;
            p.ProductType = product.ProductType.ToString();
            return p;
        }
        public static List<ProductDto> ToProductDtos(this List<Models.Product> products)
        {
            if (products == null)
            {
                return default;
            }
            return products.Select(x => x.ToProductDto()).ToList();
        }
    }
}