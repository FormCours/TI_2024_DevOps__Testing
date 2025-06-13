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
            if (quantity < 0) throw new NegativeQuantityCartException(product, quantity);
            if (quantity == 0) return false;

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
            CartItem? cartItem = _cartItems.SingleOrDefault(item => item.Product.Id == product.Id);

            if (cartItem is null) {
                throw new ProductNotFoundCartException(product);
            }

            if (quantity == 0) {
                return Remove(product);
            }
            
            cartItem.Quantity = quantity;
            return true;
        }

        public bool Remove(Product product)
        {
            int deletedItemCount = _cartItems.RemoveAll(item => product.Id == item.Product.Id);
            return deletedItemCount == 1;
        }

        public double GetTotalPrice()
        {
            double totalPrice = _cartItems.Select(ci => ci.Product.Price * ci.Quantity).Sum();
            return Math.Round(totalPrice, 2);
        }

        public double GetTotalPriceTTC()
        {
            double grandTotal = 0;
            foreach (CartItem item in _cartItems)
            {
                double priceHT = Math.Round(item.Product.Price * item.Quantity, 2, MidpointRounding.AwayFromZero);
                double VAT = Math.Round(priceHT * (item.Product.Vat == Product.VatEnum.FOOD ? 0.06 : 0.21), 2, MidpointRounding.AwayFromZero);
                double priceTTC = priceHT + VAT;
                grandTotal += priceTTC;
            }
            return Math.Round(grandTotal, 2, MidpointRounding.AwayFromZero);
        }
    }
}
