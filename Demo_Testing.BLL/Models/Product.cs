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
        public string Name { get; set; }
        public double Price { get; set; }
        public VatEnum Vat { get; set; }

        public Product(int id, string name, double price, VatEnum vat)
        {
            Id = id;
            Name = name;
            Price = price;
            Vat = vat;
        }
    }
}
