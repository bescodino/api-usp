using LabSid.Infra.Interfaces;
using LabSid.Models.Interfaces;
using LabSid.Services.Auth;
using LabSid.Services.DTO;
using LabSid.Services.Interfaces;

namespace LabSid.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<UserDto> GetByIdAsync(long id)
        {
            var user = await this._userRepository.GetByIdAsync(id);
            return new UserDto(user);
        }

        public async Task<LoginDto> Login(string email, string password)
        {
            try
            {
                var user = await this._userRepository.GetByEmail(email);

                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                bool matches = BCrypt.Net.BCrypt.Verify(password, user.Password);

                if (!matches)
                {
                    throw new Exception("Password is incorrect.");
                }

                var token = TokenService.GenerateToken(user);

                return new LoginDto()
                {
                    token = token,
                    user = user
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<UserDto> SaveAsync(IUser user)
        {
            return new UserDto(await this._userRepository.SaveAsync(user));
        }
    }
}