using Demo_Testing.BLL.CustomExceptions;
using Demo_Testing.BLL.Interfaces;
using Demo_Testing.BLL.Models;

namespace Demo_Testing.BLL.Services
{
    public class CartService : ICartService
    {
        private readonly List<CartItem> _cartItems = [];

        public IEnumerable<CartItem> CartItems 
        {
            get { return _cartItems.AsReadOnly(); }
        }

        public bool Add(Product product, int quantity)
        {
            if(quantity < 0)
            {
                throw new NegativeQuantityCartException(product, quantity);
            }

            CartItem? cart = _cartItems.SingleOrDefault(ci => ci.Product.Id == product.Id);
            if (cart is not null)
            {
                cart.Quantity += quantity;
                return true;
            }

            _cartItems.Add(new CartItem
            {
                Product = product,
                Quantity = quantity
            });

            return true;
        }

        public bool ModifyQuantity(Product product, int quantity)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Product product)
        {
            throw new NotImplementedException();
        }

        public double GetTotalPrice()
        {
            throw new NotImplementedException();
        }

        public double GetTotalPriceTTC()
        {
            throw new NotImplementedException();
        }
    }
}
