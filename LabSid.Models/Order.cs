using LabSid.Models.Interfaces;

namespace LabSid.Models
{
    public class Order : IOrder
    {
        public Order() { }

        public Order(IOrder order)
        {
            Id = order.Id;
            Name = order.Name;
            Items = order.Items;
            UserId = order.UserId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<IProduct> Items { get; set; }
        public long UserId { get; set; }
    }
}
