

namespace WebApi.Services.Campaigns;

public class TotalPriceDiscountCampaign : ICampaign
{
    private const decimal PRICE_LEVEL = 12m;
    private const decimal DISCOUNT_RATIO = 0.25m;
    public bool IsEligible { get; private set; } = false;
    public decimal DiscountAmount { get; private set; } = 0m;
    public decimal TotalPrice { get; private set; } = 0m;


    public void Calculate(List<RequestResponseModels.CartItemDto> cartItems, Dictionary<Guid, decimal> productPriceDict)
    {
        IsEligible = false;
        DiscountAmount = 0m;
        TotalPrice = 0m;

        if (cartItems != null)
        {
            TotalPrice = cartItems.Where(x => x != null).Sum(x => productPriceDict[x.ProductId]);
        }

        IsEligible = TotalPrice > PRICE_LEVEL;
        if (IsEligible)
        {
            DiscountAmount = TotalPrice * DISCOUNT_RATIO;
        }
    }
}
