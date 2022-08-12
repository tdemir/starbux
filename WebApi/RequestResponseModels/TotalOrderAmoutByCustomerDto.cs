namespace WebApi.RequestResponseModels;

public class TotalOrderAmountByCustomerDto
{
    public int OrderCount { get; set; }
    public Guid CustomerId { get; set; }
    public string Email { get; set; }
}
