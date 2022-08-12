using WebApi.RequestResponseModels;

namespace WebApi.Services;

public interface IReportService
{
    List<TotalOrderAmountByCustomerDto> GetTotalOrderAmountByCustomer();
    List<MostUsedToppingsForEachDrink> GetMostUsedToppingsForEachDrink();
}
