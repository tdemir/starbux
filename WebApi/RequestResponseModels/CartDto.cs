using WebApi.Enums;

namespace WebApi.RequestResponseModels;

public class CartDto
{
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; } = 0m;
    public decimal DiscountAmount { get; set; } = 0m;

    public decimal NetPrice
    {
        get
        {
            return TotalPrice - DiscountAmount;
        }
    }

    public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();

    public void AddItem(Guid cartItemId, Guid productId, EProductType productType, Guid? parentId = null)
    {
        if (CartItems.Any(x => x.CartItemId == cartItemId))
        {
            throw new StarbuxValidationException("This item has already added");
        }
        if (productType == EProductType.Drink)
        {
            if (parentId.HasValue)
            {
                throw new StarbuxValidationException("You cannot assing parentId for Drinks");
            }

            CartItems.Add(new CartItemDto
            {
                CartItemId = cartItemId,
                ParentId = null,
                ProductId = productId,
                ProductType = productType
            });
        }
        else if (productType == EProductType.Topping)
        {
            if (!parentId.HasValue)
            {
                throw new StarbuxValidationException("You should assing parentId for Toppings");
            }
            var parentCartItem = CartItems.FirstOrDefault(x => x.CartItemId == parentId.Value);
            if (parentCartItem == null)
            {
                throw new StarbuxValidationException("Parent Item couldn't found for Toppings");
            }

            CartItems.Add(new CartItemDto
            {
                CartItemId = cartItemId,
                ParentId = parentId,
                ProductId = productId,
                ProductType = productType
            });
        }
        else
        {
            throw new StarbuxValidationException("Invalid product type");
        }
    }

    public void RemoveItem(Guid cartItemId)
    {
        CartItems.RemoveAll(x => x.CartItemId == cartItemId || x.ParentId == cartItemId);
    }

    public void Calculate(List<Models.Product> products)
    {
        var productPriceDictionary = GetProductPriceDictionary(products);

        var campaignList = new List<Services.Campaigns.ICampaign>();
        campaignList.Add(new Services.Campaigns.LimitedDrinkDiscountCampaign());
        campaignList.Add(new Services.Campaigns.TotalPriceDiscountCampaign());

        foreach (var c in campaignList)
        {
            c.Calculate(this.CartItems, productPriceDictionary);
        }

        var _campaign = campaignList.First();//randomly get one campaing, to access totalPrice value
        var appropriateCampaigns = campaignList.Where(x => x.IsEligible).ToList();

        TotalPrice = _campaign.TotalPrice;
        DiscountAmount = appropriateCampaigns.Any() ? appropriateCampaigns.Max(x => x.DiscountAmount) : 0m;
    }

    private Dictionary<Guid, decimal> GetProductPriceDictionary(List<Models.Product> products)
    {
        var productPriceDictionary = new Dictionary<Guid, decimal>();
        foreach (var item in products)
        {
            if (productPriceDictionary.ContainsKey(item.Id))
            {
                productPriceDictionary[item.Id] = item.Price;
                continue;
            }
            productPriceDictionary.Add(item.Id, item.Price);
        }
        return productPriceDictionary;
    }
}
