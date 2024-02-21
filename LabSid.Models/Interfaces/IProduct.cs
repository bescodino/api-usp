namespace LabSid.Models.Interfaces
{
    public interface IProduct
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public long OrderId { get; set; }
    }
}
