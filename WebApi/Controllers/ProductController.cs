using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Mappings;
using WebApi.RequestResponseModels;

namespace WebApi.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService productService;

        public ProductController(IConfiguration configuration, IProductService productService) : base(configuration)
        {
            this.productService = productService;
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var data = productService.GetAll().ToProductDtos();
            return Ok(data);
        }

        [HttpGet("{id:guid}")]
        public IActionResult Get(Guid id)
        {
            var data = productService.Get(id);
            if (data == null)
            {
                return NoContent();
            }
            return Ok(data.ToProductDto());
        }

        [Authorize(Roles = Constants.Role.Admin)]
        [HttpPost]
        public IActionResult Add(ProductDto productDto)
        {
            var product = productDto.ToProduct();
            var result = productService.Add(product).ToProductDto();
            return Ok(result);
        }

        [Authorize(Roles = Constants.Role.Admin)]
        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, ProductDto productDto)
        {
            var product = productDto.ToProduct();
            var result = productService.Update(id, product).ToProductDto();
            return Ok(result);
        }

        [Authorize(Roles = Constants.Role.Admin)]
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            productService.Delete(id);
            return Ok();
        }

    }
}