using LabSid.Models.Interfaces;

namespace LabSid.Infra.Interfaces
{
    public interface IOrderRepository
    {
        Task<IOrder> SaveAsync(IOrder user);
        Task<IOrder?> GetByIdAsync(long id);
        Task<IEnumerable<IOrder>> Get();
    }
}
