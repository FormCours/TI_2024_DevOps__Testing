using Demo_Testing.BLL.Models;

namespace Demo_Testing.BLL.Interfaces
{
    public interface ICartService
    {
        IEnumerable<CartItem> CartItems { get; }

        bool Add(Product product, int quantity);
        bool Remove(Product product);
        bool ModifyQuantity(Product product, int quantity);
        double GetTotalPrice();
        double GetTotalPriceTTC();
    }
}
