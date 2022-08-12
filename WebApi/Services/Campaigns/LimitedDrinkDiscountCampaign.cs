
namespace WebApi.Services.Campaigns
{
    public class LimitedDrinkDiscountCampaign : ICampaign
    {
        private const int DRINK_COUNT = 3;

        public bool IsEligible { get; private set; } = false;
        public decimal DiscountAmount { get; private set; } = 0m;
        public decimal TotalPrice { get; private set; } = 0m;

        public void Calculate(List<RequestResponseModels.CartItemDto> cartItems, Dictionary<Guid, decimal> productPriceDict)
        {
            IsEligible = false;
            DiscountAmount = 0m;
            TotalPrice = 0m;

            var drinkListPrices = new List<decimal>();
            if (cartItems != null)
            {
                var drinks = cartItems.Where(x => x != null &&
                                                    x.ProductType == Enums.EProductType.Drink)
                                        .ToList();

                foreach (var drinkItem in drinks)
                {
                    var drinkTotalPrice = productPriceDict[drinkItem.ProductId];

                    var toppings = cartItems.Where(x => x != null &&
                                                        x.ProductType == Enums.EProductType.Topping &&
                                                        x.ParentId == drinkItem.CartItemId)
                                            .ToList();
                    if (toppings.Any())
                    {
                        drinkTotalPrice += toppings.Sum(x => productPriceDict[x.ProductId]);
                    }

                    drinkListPrices.Add(drinkTotalPrice);
                }
            }

            if (drinkListPrices.Any())
            {
                TotalPrice = drinkListPrices.Sum();
                IsEligible = drinkListPrices.Count >= DRINK_COUNT;
                if (IsEligible)
                {
                    DiscountAmount = drinkListPrices.Min();
                }
            }
        }
    }
}