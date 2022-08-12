

namespace WebApi.Services.Campaigns
{
    public interface ICampaign
    {
        bool IsEligible { get; }
        decimal DiscountAmount { get; }

        decimal TotalPrice { get; }

        //void Calculate(List<BasketDrink> drinks);

        void Calculate(List<RequestResponseModels.CartItemDto> cartItems, Dictionary<Guid, decimal> productPriceDict);
    }
}