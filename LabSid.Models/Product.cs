using LabSid.Models.Interfaces;

namespace LabSid.Models
{
    public class Product : IProduct
    {
        public Product() { }

        public Product(IProduct product)
        {
            Id = product.Id;
            Name = product.Name;
            Value = product.Value;
            OrderId = product.OrderId;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public long OrderId { get; set; }
    }
}
