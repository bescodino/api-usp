using LabSid.Models.Interfaces;

namespace LabSid.Infra.Interfaces
{
    public interface IProductRepository
    {
        Task<IProduct> SaveAsync(IProduct user);
        Task<IProduct?> GetByIdAsync(long id);
        Task<IEnumerable<IProduct>> Get();
    }
}
