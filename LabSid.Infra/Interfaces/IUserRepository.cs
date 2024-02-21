using LabSid.Models.Interfaces;

namespace LabSid.Infra.Interfaces
{
    public interface IUserRepository
    {
        Task<IUser> SaveAsync(IUser user);
        Task<IUser> GetByIdAsync(long id);
        Task<IEnumerable<IUser>> Get();
        Task<string> Token(string user, string password);
        Task<IUser> GetByEmail(string email);
    }
}
