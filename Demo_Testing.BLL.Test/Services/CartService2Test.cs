using Demo_Testing.BLL.Models;
using Demo_Testing.BLL.Services;

namespace Demo_Testing.BLL.Test.Services
{
    public class CartService2Test
    {
        private readonly CartService _cartService;
        private readonly int _initialProductCount;
        public CartService2Test()
        {
            Console.WriteLine("CartService2Test");
            _cartService = new CartService();
            _cartService.Add(new Product(1, "Pomme", 2.5, Product.VatEnum.FOOD), 5);
            _cartService.Add(new Product(2, "Fraise", 6, Product.VatEnum.FOOD), 1);
            _cartService.Add(new Product(4, "Pespi Max", 1.1, Product.VatEnum.FOOD), 2);
            _cartService.Add(new Product(5, "Coca Zero", 1.0, Product.VatEnum.FOOD), 3);
            _cartService.Add(new Product(6, "Boite de mouchoir", 41.99, Product.VatEnum.NO_FOOD), 5);
            _initialProductCount = 5;
        }

        [Fact]
        public void Add_OneNewProduct_CartServiceContainsNewProduct()
        {
            Console.WriteLine("Add_OneNewProduct_CartServiceContainsNewProduct");
            // Arrange
            int productId = 9;
            Product productToAdd = new Product(productId, "Banane explosive", 4.99, Product.VatEnum.NO_FOOD);
            int productQuantity = 11;

            int expectedProductCount = _initialProductCount + 1;
            Product expectedProductAdded = new Product(productId, "Banane explosive", 4.99, Product.VatEnum.NO_FOOD);
            int expectedProductQuantity = 11;

            // Action
            bool isAdded = _cartService.Add(productToAdd, productQuantity);
            List<CartItem> actualItems = _cartService.CartItems.ToList();
            CartItem actualAddItem = actualItems.Single(i => i.Product.Id == productId);

            // Assert
            Assert.True(isAdded);
            Assert.Equal(expectedProductCount, actualItems.Count);
            Assert.Equal(expectedProductQuantity, actualAddItem.Quantity);
            Assert.Equivalent(expectedProductAdded, actualAddItem.Product);
        }

        [Fact]
        public void Add_OneExistingProduct_CartServiceUpdateQuantity()
        {
            Console.WriteLine("Add_OneExistingProduct_CartServiceUpdateQuantity");

            // Arrange
            int productId = 2;
            Product productToAdd = new Product(productId, "Fraise", 6, Product.VatEnum.FOOD);
            int productQuantity = 2;

            int expectedProductCount = _initialProductCount;
            Product expectedProductAdded = new Product(productId, "Fraise", 6, Product.VatEnum.FOOD);
            int expectedProductQuantity = 3;

            // Action
            bool isAdded = _cartService.Add(productToAdd, productQuantity);
            List<CartItem> actualItems = _cartService.CartItems.ToList();
            CartItem actualAddItem = actualItems.Single(i => i.Product.Id == productId);

            // Assert
            Assert.True(isAdded);
            Assert.Equal(expectedProductCount, actualItems.Count);
            Assert.Equal(expectedProductQuantity, actualAddItem.Quantity);
            Assert.Equivalent(expectedProductAdded, actualAddItem.Product);
        }
    }
}
