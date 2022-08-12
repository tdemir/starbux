namespace WebApi.Services;

public interface IProductService
{

    List<Models.Product> GetAll();

    Models.Product Add(Models.Product product);

    Models.Product? Get(Guid id);

    Models.Product Update(Guid id, Models.Product product);

    void Delete(Guid id);

}
