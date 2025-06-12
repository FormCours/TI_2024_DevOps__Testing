using Demo_Testing.BLL.Interfaces;
using Demo_Testing.BLL.Models;

namespace Demo_Testing.BLL.Services
{
    public class CartService : ICartService
    {
        public IEnumerable<CartItem> CartItems => throw new NotImplementedException();

        public bool Add(Product product, int quantity)
        {
            throw new NotImplementedException();
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
