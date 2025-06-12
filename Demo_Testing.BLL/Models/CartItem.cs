namespace Demo_Testing.BLL.Models
{
    public class CartItem
    {
        public required Product Product { get; set; }
        public required int Quantity { get; set; }
    }
}
