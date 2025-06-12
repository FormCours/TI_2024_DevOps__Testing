namespace Demo_Testing.BLL.Models
{
    public class Product
    {
        public enum VatEnum
        {
            FOOD,
            NO_FOOD
        }

        public int Id { get; set; }
        public required string Name { get; set; }
        public required double Price { get; set; }
        public required VatEnum Vat { get; set; }
    }
}
