using LabSid.Models.Interfaces;
using LabSid.Services.DTO;

namespace LabSid.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> SaveAsync(IUser user);
        Task<UserDto> GetByIdAsync(long id);
        Task<LoginDto> Login(string email, string password);
    }
}
