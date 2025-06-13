using Demo_Testing.BLL.CustomExceptions;
using Demo_Testing.BLL.Interfaces;
using Demo_Testing.BLL.Models;
using Demo_Testing.BLL.Services;

namespace Demo_Testing.BLL.Test.Services
{
    public class CartServiceTest
    {
        private readonly ICartService _cartService;
        public CartServiceTest()
        {
            _cartService = new CartService();
        }

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
            _cartService.Add(p1, p1_quantity);
            List<CartItem> actual_cart = _cartService.CartItems.ToList();

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
            _cartService.Add(product, p1_quantity);
            _cartService.Add(product, p2_quantity);
            List<CartItem> actual_cart = _cartService.CartItems.ToList();

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

            // - L'exception est déclenchée ?
            var actual_exception = Assert.Throws<NegativeQuantityCartException>(() =>
            {
                _cartService.Add(product, bad_quantity);
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
            Product product = new Product(1, "boite de mouchoir", 99.99, Product.VatEnum.NO_FOOD);
            _cartService.Add(product, 8);
            Product remove_product = new Product(1, "boite de mouchoir", 99.99, Product.VatEnum.NO_FOOD);

            // Action
            bool actual_result = _cartService.Remove(remove_product);
            List<CartItem> actual_cartItems = _cartService.CartItems.ToList();

            // Assert
            Assert.Empty(actual_cartItems);
            Assert.True(actual_result);
        }

        [Fact]
        public void Add_WithQuantityZero_ServiceContainsNoProduct()
        {
            // Arrange
            Product product = new Product(1, "PetRock 2.0", 10.50, Product.VatEnum.NO_FOOD);
            int productQuantity = 0;

            // Action
            bool actual_result = _cartService.Add(product, productQuantity);
            List<CartItem> actual_cartItems = _cartService.CartItems.ToList();

            // Assert
            Assert.False(actual_result);
            Assert.Empty(actual_cartItems);

        }

        [Fact]
        public void Remove_NonExistingProduct_ReturnFalse()
        {
            // Arrange
            Product product = new Product(1, "Quantum banana", 0.99, Product.VatEnum.FOOD);

            // Action
            bool isSuccess = _cartService.Remove(product);

            // Assert
            Assert.False(isSuccess);
        }

        [Fact]
        public void UpdateQuantity_ExistingProduct_CartContainsProductWithNewQuantity()
        {
            // Arrange
            Product initialProduct = new Product(1, "Moon Cookie", 16.99, Product.VatEnum.FOOD);
            int initialQuantity = 4;
            _cartService.Add(initialProduct, initialQuantity);

            Product updatedProduct = new Product(1, "Moon Cookie", 16.99, Product.VatEnum.FOOD);
            int updatedQuantity = 42;
            int expectedQuantity = 42;

            // Action
            bool actual_result = _cartService.ModifyQuantity(updatedProduct, updatedQuantity);
            List<CartItem> actual_items = _cartService.CartItems.ToList();

            // Assert
            Assert.True(actual_result);
            Assert.Equal(expectedQuantity, actual_items[0].Quantity);
        }

        [Fact]
        public void UpdateQuantity_ExistingProduct_RemoveWhenQuantityEqualZero()
        {
            // Arrange
            Product initialProduct = new Product(1, "Moon Cookie 2", 16.99, Product.VatEnum.FOOD);
            int initialQuantity = 4;
            _cartService.Add(initialProduct, initialQuantity);

            Product updatedProduct = new Product(1, "Moon Cookie 2", 16.99, Product.VatEnum.FOOD);
            int updatedQuantity = 0;

            // Action
            bool actual_result = _cartService.ModifyQuantity(updatedProduct, updatedQuantity);
            List<CartItem> actual_items = _cartService.CartItems.ToList();

            // Assert
            Assert.True(actual_result);
            Assert.Empty(actual_items);
        }

        [Fact]
        public void UpdateQuantity_NonExistingProduct_ThrowProductNotFoundCartException()
        {
            // Arrange
            string expected_message_exception = "The product \"Banana\" is not found !";

            Product updatedProduct = new Product(1, "Banana", 16.99, Product.VatEnum.FOOD);
            int updatedQuantity = 12;

            // Action + Assert
            var actual_exception = Assert.Throws<ProductNotFoundCartException>(() =>
            {
                _cartService.ModifyQuantity(updatedProduct, updatedQuantity);
            });

            Assert.Equal(expected_message_exception, actual_exception.Message);
        }

        [Theory]
        [InlineData(5, 2, 3, 7, 6, 23)]
        [InlineData(9, 22, 21, 0, 9, 61)]
        [InlineData(1_000, 1_500, 499, 1_001, 42, 4042)]
        public void Add_SingleProduct_CheckQuantityProductAfterManyAddSameProduct(
              int qty1, int qty2, int qty3, int qty4, int qty5, int expected_qty)
        {
            // Arrange
            Product product = new Product(1, "Banana", 16.99, Product.VatEnum.FOOD);

            Product expected_product = new Product(1, "Banana", 16.99, Product.VatEnum.FOOD);

            // Action
            _cartService.Add(product, qty1);
            _cartService.Add(product, qty2);
            _cartService.Add(product, qty3);
            _cartService.Add(product, qty4);
            _cartService.Add(product, qty5);

            CartItem actual_item = _cartService.CartItems.Single();
            // Assert
            Assert.Equivalent(expected_product, actual_item.Product);
            Assert.Equal(expected_qty, actual_item.Quantity);
        }

        [Theory]
        [InlineData(5, 10, 50, 65)]
        [InlineData(5.3, 1.3, 1.1, 7.7)]
        [InlineData(901.9, 102.06, 333.04, 1337)]
        public void GetTotalPrice_MultipleProductWithQuantityToOne_CheckCorrectTotalPrice(
            double price1, double price2, double price3, double expectedTotalPrice
            )
        {
            // Arrange
            Product product1 = new Product(1, "Banana plantain", price1, Product.VatEnum.FOOD);
            Product product2 = new Product(2, "Banana Cavendish", price2, Product.VatEnum.FOOD);
            Product product3 = new Product(3, "Banana pinky :3", price3, Product.VatEnum.FOOD);
            int productQuantity = 1;

            // Action
            _cartService.Add(product1, productQuantity);
            _cartService.Add(product2, productQuantity);
            _cartService.Add(product3, productQuantity);
            double actualTotalPrice = _cartService.GetTotalPrice();

            // Assert
            Assert.Equal(expectedTotalPrice, actualTotalPrice);
        }

        [Theory]
        [InlineData(5, 3, 6, 3, 5, 2, 42)]
        [InlineData(1.5, 0.99, 6.03, 10, 9, 3, 42)]
        [InlineData(0.99, 0.45, 5, 11, 3, 3, 27.24)]
        public void GetTotalPrice_MultipleProductWithDifferentQuantity_CheckCorrectTotalPrice(
            double price1, double price2, double price3,
            int quantity1, int quantity2, int quantity3, double expectedTotalPrice
            )
        {
            // Arrange
            Product product1 = new Product(1, "Ananas Sweet", price1, Product.VatEnum.FOOD);
            Product product2 = new Product(2, "Ananas Cayenne Lisse", price2, Product.VatEnum.FOOD);
            Product product3 = new Product(3, "Ananas Victoria", price3, Product.VatEnum.FOOD);

            // Action
            _cartService.Add(product1, quantity1);
            _cartService.Add(product2, quantity2);
            _cartService.Add(product3, quantity3);
            double actualTotalPrice = _cartService.GetTotalPrice();

            // Assert
            Assert.Equal(expectedTotalPrice, actualTotalPrice);
        }

        [Theory]
        [InlineData(0.99, 0.94, 6, 6, 12.28)]
        [InlineData(0.24, 0.11, 11, 7, 3.62)]
        [InlineData(22.12, 9.99, 111, 7, 2676.77)]
        public void GetTotalPriceTTC_OnlyFoodProduct_CheckCorrectTotalPrice(double price1, double price2, int qty1, int qty2, double expectedTotalPrice)
        {
            // Arrange
            Product product1 = new Product(1, "Blackberry", price1, Product.VatEnum.FOOD);
            Product product2 = new Product(2, "Yellowberry", price2, Product.VatEnum.FOOD);

            // Action
            _cartService.Add(product1, qty1);
            _cartService.Add(product2, qty2);
            double actual_priceTTC = _cartService.GetTotalPriceTTC();

            // Assert
            Assert.Equal(expectedTotalPrice, actual_priceTTC);
        }

        [Theory]
        [InlineData(0.99, 0.94, 6, 6, 14.02)]
        [InlineData(0.24, 0.11, 11, 7, 4.13)]
        [InlineData(22.12, 9.99, 111, 7, 3055.55)]
        public void GetTotalPriceTTC_OnlyNoFoodProduct_CheckCorrectTotalPrice(double price1, double price2, int qty1, int qty2, double expectedTotalPrice)
        {
            // Arrange
            Product product1 = new Product(1, "tissue box", price1, Product.VatEnum.NO_FOOD);
            Product product2 = new Product(2, "tissue packet", price2, Product.VatEnum.NO_FOOD);

            // Action
            _cartService.Add(product1, qty1);
            _cartService.Add(product2, qty2);
            double actual_priceTTC = _cartService.GetTotalPriceTTC();

            // Assert
            Assert.Equal(expectedTotalPrice, actual_priceTTC);
        }

        public static IEnumerable<object[]> productCartData = [
            [
            new Product(1, "p1", 0.99, Product.VatEnum.NO_FOOD), 6,
            new Product(2, "p2", 0.94, Product.VatEnum.FOOD), 6,
            new Product(3, "p3", 1.01, Product.VatEnum.FOOD), 6,
            19.59
            ],
            [
            new Product(1, "y1", 0.24, Product.VatEnum.NO_FOOD), 11,
            new Product(2, "y2", 0.11, Product.VatEnum.NO_FOOD), 7,
            new Product(3, "y3", 0.09, Product.VatEnum.FOOD), 21,
            6.13
            ],
            [
            new Product(1, "z1", 22.12, Product.VatEnum.FOOD), 111,
            new Product(2, "z2", 9.99, Product.VatEnum.NO_FOOD), 7,
            new Product(3, "z3", 19.96, Product.VatEnum.NO_FOOD), 131,
            5851.11
            ],
            [
            new Product(1, "a1", 0.565, Product.VatEnum.NO_FOOD), 2,
            new Product(2, "a2", 0.625, Product.VatEnum.FOOD), 1,
            new Product(3, "a3", 0.335, Product.VatEnum.NO_FOOD), 3,
            3.26
            ],
        ];

        [Theory]
        [MemberData(nameof(productCartData))]
        public void GetTotalPriceTTC_MixVatValueProduct_CheckCorrectTotalPrice(Product product1, int qty1, Product product2, int qty2, Product product3, int qty3, double expectedTotaPrice)
        {
            // Action
            _cartService.Add(product1, qty1);
            _cartService.Add(product2, qty2);
            _cartService.Add(product3, qty3);
            double actual_priceTTC = _cartService.GetTotalPriceTTC();
            
            // Assert
            Assert.Equal(expectedTotaPrice, actual_priceTTC);
        }
    }
}
