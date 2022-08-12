namespace WebApi.RequestResponseModels
{
    public class CartAddItemDto
    {
        public Guid ProductId { get; set; }
        public Guid? ParentCartItemId { get; set; }
    }
}