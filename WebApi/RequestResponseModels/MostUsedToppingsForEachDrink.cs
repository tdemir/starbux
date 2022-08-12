namespace WebApi.RequestResponseModels;

public class MostUsedToppingsForEachDrink
{
    public Guid DrinkId { get; set; }
    public Guid ToppingId { get; set; }
    public int Count { get; set; }
    public string DrinkName { get; set; }
    public string ToppingName { get; set; }


}