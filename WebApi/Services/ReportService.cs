using Microsoft.Extensions.Caching.Distributed;
using WebApi.Models;
using WebApi.RequestResponseModels;

namespace WebApi.Services;

public class ReportService : BaseService, IReportService
{
    public ReportService(IApiDbContext dbContext, IDistributedCache redisDistributedCache, IHttpContextAccessor httpContextAccessor) : base(dbContext, redisDistributedCache, httpContextAccessor)
    {
    }

    public List<TotalOrderAmountByCustomerDto> GetTotalOrderAmountByCustomer()
    {
        var groupedOrders = dbContext.Orders
                                    .Where(x => !x.DeletedDate.HasValue)
                                    .GroupBy(x => x.UserId)
                                    .Select(g => new { UserId = (Guid?)g.Key, OrderCount = (int?)g.Count() });

        //left join
        var data = from u in dbContext.Users
                   join o in groupedOrders on u.Id equals o.UserId
                   into Details
                   from defaultVal in Details.DefaultIfEmpty()
                   select new TotalOrderAmountByCustomerDto
                   {
                       OrderCount = defaultVal == null ? 0 : defaultVal.OrderCount.GetValueOrDefault(0),
                       CustomerId = u.Id,
                       Email = u.Email
                   };


        return data.OrderBy(x => x.Email).ToList();
    }

    public List<MostUsedToppingsForEachDrink> GetMostUsedToppingsForEachDrink()
    {
        var allProducts = dbContext.Products.ToList();

        var drinkToppings = from d in dbContext.Products
                            join o in dbContext.OrderItems on d.Id equals o.ProductId
                            join os in dbContext.OrderItems on o.Id equals os.ParentOrderItemId
                            where d.ProductType == Enums.EProductType.Drink
                            select new
                            {
                                DrinkProductId = d.Id,
                                ToppingProductId = os.ProductId
                            };

        var reportData = (from d in drinkToppings
                          group d by new { d.DrinkProductId, d.ToppingProductId } into g
                          select new MostUsedToppingsForEachDrink
                          {
                              DrinkId = g.Key.DrinkProductId,
                              ToppingId = g.Key.ToppingProductId,
                              Count = g.Count()
                          }).ToList();

        var drinkProducts = allProducts.Where(x => x.ProductType == Enums.EProductType.Drink);
        var toppingProducts = allProducts.Where(x => x.ProductType == Enums.EProductType.Topping);

        foreach (var drinkProduct in drinkProducts)
        {
            foreach (var toppingProduct in toppingProducts)
            {
                var item = reportData.FirstOrDefault(x => x.DrinkId == drinkProduct.Id && x.ToppingId == toppingProduct.Id);
                if (item == null)
                {
                    reportData.Add(new MostUsedToppingsForEachDrink
                    {
                        DrinkId = drinkProduct.Id,
                        DrinkName = drinkProduct.Name,
                        ToppingId = toppingProduct.Id,
                        ToppingName = toppingProduct.Name,
                        Count = 0
                    });
                    continue;
                }
                item.DrinkName = drinkProduct.Name;
                item.ToppingName = toppingProduct.Name;
            }
        }

        return reportData.OrderBy(x => x.DrinkName)
                            .ThenByDescending(x => x.Count)
                            .ThenBy(x => x.ToppingName)
                            .ToList();
    }
}
