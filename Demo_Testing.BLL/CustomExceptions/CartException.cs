using Demo_Testing.BLL.Models;

namespace Demo_Testing.BLL.CustomExceptions
{
    public class CartException : Exception
    {
        public CartException(string? message) : base(message)
        {
        }
    }

    public class NegativeQuantityCartException : CartException
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public NegativeQuantityCartException(Product product, int quantity)
            : base("Negative quantity in cart")
        {
            Product = product;
            Quantity = quantity;
        }
    }
}
