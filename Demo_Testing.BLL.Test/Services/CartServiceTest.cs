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
            CartService cartService = new CartService();

            // Action + Assert

            // - L'exception est déclenchée ?
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

        [Fact]
        public void Add_WithQuantityZero_ServiceContainsNoProduct()
        {
            // Arrange
            CartService cartService = new CartService();
            Product product = new Product(1, "PetRock 2.0", 10.50, Product.VatEnum.NO_FOOD);
            int productQuantity = 0;

            // Action
            bool actual_result = cartService.Add(product, productQuantity);
            List<CartItem> actual_cartItems = cartService.CartItems.ToList();

            // Assert
            Assert.False(actual_result);
            Assert.Empty(actual_cartItems);

        }

        [Fact]
        public void Remove_NonExistingProduct_ReturnFalse()
        {
            // Arrange
            CartService cartService = new CartService();
            Product product = new Product(1, "Quantum banana", 0.99, Product.VatEnum.FOOD);

            // Action
            bool isSuccess = cartService.Remove(product);

            // Assert
            Assert.False(isSuccess);
        }

        [Fact]
        public void UpdateQuantity_ExistingProduct_CartContainsProductWithNewQuantity()
        {
            // Arrange
            CartService cartService = new CartService();
            Product initialProduct = new Product(1, "Moon Cookie", 16.99, Product.VatEnum.FOOD);
            int initialQuantity = 4;
            cartService.Add(initialProduct, initialQuantity);

            Product updatedProduct = new Product(1, "Moon Cookie", 16.99, Product.VatEnum.FOOD);
            int updatedQuantity = 42;
            int expectedQuantity = 42;

            // Action
            bool actual_result = cartService.ModifyQuantity(updatedProduct, updatedQuantity);
            List<CartItem> actual_items = cartService.CartItems.ToList();

            // Assert
            Assert.True(actual_result);
            Assert.Equal(expectedQuantity, actual_items[0].Quantity);
        }

        [Fact]
        public void UpdateQuantity_ExistingProduct_RemoveWhenQuantityEqualZero()
        {
            // Arrange
            CartService cartService = new CartService();
            Product initialProduct = new Product(1, "Moon Cookie 2", 16.99, Product.VatEnum.FOOD);
            int initialQuantity = 4;
            cartService.Add(initialProduct, initialQuantity);

            Product updatedProduct = new Product(1, "Moon Cookie 2", 16.99, Product.VatEnum.FOOD);
            int updatedQuantity = 0;

            // Action
            bool actual_result = cartService.ModifyQuantity(updatedProduct, updatedQuantity);
            List<CartItem> actual_items = cartService.CartItems.ToList();

            // Assert
            Assert.True(actual_result);
            Assert.Empty(actual_items);
        }

        [Fact]
        public void UpdateQuantity_NonExistingProduct_ThrowProductNotFoundCartException()
        {
            // Arrange
            CartService cartService = new CartService();
            string expected_message_exception = "The product \"Banana\" is not found !";

            Product updatedProduct = new Product(1, "Banana", 16.99, Product.VatEnum.FOOD);
            int updatedQuantity = 12;

            // Action + Assert
            var actual_exception = Assert.Throws<ProductNotFoundCartException>(() =>
            {
                cartService.ModifyQuantity(updatedProduct, updatedQuantity);
            });

            Assert.Equal(expected_message_exception, actual_exception.Message);
        }
    }
}
