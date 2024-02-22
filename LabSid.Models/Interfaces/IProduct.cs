namespace LabSid.Models.Interfaces
{
    public interface IProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public long OrderId { get; set; }
    }
}
