using Demo_Testing.BLL.CustomExceptions;
using Demo_Testing.BLL.Models;
using Demo_Testing.BLL.Services;

namespace Demo_Testing.BLL.Test.Services
{
    public class CartServiceTest
    {
        [Fact]
        public void Add_SingleProduct_ServiceContaintOneProduct()
        {
            // Arrange
            // -> Préparation des éléments du test
            Product p1 = new Product(1, "Pomme", 0.35, Product.VatEnum.FOOD);
            int p1_quantity = 1;

            int expected_quantity = 1;
            Product expected_product = new Product(1, "Pomme", 0.35, Product.VatEnum.FOOD);

            // Action
            // -> Execution du code à tester
            CartService cartService = new CartService();
            cartService.Add(p1, p1_quantity);
            List<CartItem> actual_cart = cartService.CartItems.ToList();

            // Assert
            // -> Analyse du resultat
            Assert.Single(actual_cart);
            Assert.Equivalent(expected_product, actual_cart[0].Product);
            Assert.Equal(expected_quantity, actual_cart[0].Quantity);
        }

        [Fact]
        public void Add_TwoSameProduct_ServiceContainsOneProduct()
        {
            // Arrange
            Product product = new Product(2, "Fraise", 5, Product.VatEnum.FOOD);
            int p1_quantity = 10;
            int p2_quantity = 1;

            Product expected_product = new Product(2, "Fraise", 5, Product.VatEnum.FOOD);
            int expected_quantity = 11;

            // Action
            CartService cartService = new CartService();
            cartService.Add(product, p1_quantity);
            cartService.Add(product, p2_quantity);
            List<CartItem> actual_cart = cartService.CartItems.ToList();

            // Assert
            Assert.Single(actual_cart);
            Assert.Equivalent(expected_product, actual_cart[0].Product);
            Assert.Equal(expected_quantity, actual_cart[0].Quantity);
        }

        [Fact]
        public void Add_NegativeQuantity_ThrowException()
        {
            // Arrange
            Product product = new Product(3, "Casier de tomate", 49.95, Product.VatEnum.FOOD);
            int bad_quantity = -2;
            string expected_message_exception = "Negative quantity in cart";

            // Action + Assert
            CartService cartService = new CartService();

            // - L'exception est déclanché ?
            var actual_exception = Assert.Throws<NegativeQuantityCartException>(() =>
            {
                cartService.Add(product, bad_quantity);
            });

            // - Le contenu de l'exception
            Assert.Equal(expected_message_exception, actual_exception.Message);
            Assert.Equal(bad_quantity, actual_exception.Quantity);
            Assert.Equivalent(product, actual_exception.Product);
        }

        [Fact]
        public void Remove_SingleProduct_ServiceNotContainsProduct()
        {
            // Arrange
            CartService cartService = new CartService();
            Product product = new Product(1, "boite de mouchoir", 99.99, Product.VatEnum.NO_FOOD);
            cartService.Add(product, 8);
            Product remove_product = new Product(1, "boite de mouchoir", 99.99, Product.VatEnum.NO_FOOD);
            
            // Action
            bool actual_result = cartService.Remove(remove_product);
            List<CartItem> actual_cartItems = cartService.CartItems.ToList();

            // Assert
            Assert.Empty(actual_cartItems);
            Assert.True(actual_result);
        }
    }
}
