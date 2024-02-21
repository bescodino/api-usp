namespace LabSid.Models.Interfaces
{
    public interface IOrder
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long UserId { get; set; }
        public List<IProduct> Items { get; set; }
    }
}
