using Microsoft.Extensions.Caching.Distributed;
using WebApi.Models;

namespace WebApi.Services;

public class ProductService : BaseService, IProductService
{
    public ProductService(IApiDbContext dbContext, IDistributedCache redisDistributedCache, IHttpContextAccessor httpContextAccessor) : base(dbContext, redisDistributedCache, httpContextAccessor)
    {

    }

    public Product Add(Product product)
    {
        ValidateProduct(product);
        using (var transaction = dbContext.BeginTransaction())
        {
            dbContext.Products.Add(product);
            dbContext.SaveDataChanges();
            transaction.Commit();
        }
        return product;
    }

    public void Delete(Guid id)
    {
        var product = dbContext.Products.FirstOrDefault(x => x.Id == id && !x.DeletedDate.HasValue);
        if (product == null)
        {
            throw new StarbuxValidationException("Product is not exist");
        }

        using (var transaction = dbContext.BeginTransaction())
        {
            product.DeletedDate = DateTime.UtcNow;
            dbContext.SaveDataChanges();
            transaction.Commit();
        }
    }

    public Product? Get(Guid id)
    {
        return dbContext.Products.FirstOrDefault(x => x.Id == id && !x.DeletedDate.HasValue);
    }

    public List<Models.Product> GetAll()
    {
        return dbContext.Products.Where(x => !x.DeletedDate.HasValue).OrderBy(x => x.Name).ToList();
    }

    public Product Update(Guid id, Product product)
    {
        ValidateProduct(product);
        var currentProduct = Get(id);
        if (currentProduct == null)
        {
            throw new StarbuxValidationException("Product is not exist");
        }
        using (var transaction = dbContext.BeginTransaction())
        {
            currentProduct.Name = product.Name;
            currentProduct.Price = product.Price;
            currentProduct.ProductType = product.ProductType;
            dbContext.Products.Update(currentProduct);
            dbContext.SaveDataChanges();
            transaction.Commit();
        }

        return currentProduct;
    }

    private void ValidateProduct(Product product)
    {
        if (product.Price <= 0m)
        {
            throw new StarbuxValidationException("Price cannot be equal or less than zero");
        }
    }
}
