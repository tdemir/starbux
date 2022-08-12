using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.RequestResponseModels;
using WebApi.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers;

public class CartController : BaseController
{
    private readonly IProductService productService;
    private readonly ICartService cartService;

    public CartController(IConfiguration configuration, IProductService productService, ICartService cartService) : base(configuration)
    {
        this.productService = productService;
        this.cartService = cartService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var products = productService.GetAll();
        var cart = await cartService.Get(products);
        return Ok(cart);
    }

    [HttpPost]
    public async Task<IActionResult> Add(CartAddItemDto model)
    {
        var product = productService.Get(model.ProductId);
        await cartService.AddItem(product, model.ParentCartItemId);
        return Ok();
    }

    [HttpDelete("{cartItemId}")]
    public async Task<IActionResult> Remove(Guid cartItemId)
    {
        await cartService.RemoveCartItem(cartItemId);
        return Ok();
    }

    [HttpPost("Pay")]
    public async Task<IActionResult> Pay()
    {
        var products = productService.GetAll();
        await cartService.Pay(products);
        return Ok();
    }
}


